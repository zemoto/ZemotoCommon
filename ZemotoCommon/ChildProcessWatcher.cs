using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ZemotoCommon;

// This class creates a "job" and allows the rest of the app to add child processes to it.
// When the job goes out of scope (program crashes, closes, exits, etc), all added child
// processes are closed with it.
public static class ChildProcessWatcher
{
   private static readonly IntPtr _handle = CreateJobObject( IntPtr.Zero, $"EncoderChildProcessTracker{Environment.ProcessId}" );
   private static bool _initialized;

   public static void Initialize()
   {
      if ( _initialized )
      {
         return;
      }

      var info = new JOBOBJECT_BASIC_LIMIT_INFORMATION
      {
         LimitFlags = JOBOBJECTLIMIT.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
      };

      var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
      {
         BasicLimitInformation = info
      };

      var length = Marshal.SizeOf( typeof( JOBOBJECT_EXTENDED_LIMIT_INFORMATION ) );
      var extendedInfoPtr = Marshal.AllocHGlobal( length );
      Marshal.StructureToPtr( extendedInfo, extendedInfoPtr, false );

      var result = SetInformationJobObject( _handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length );
      Debug.Assert( result );

      Marshal.FreeHGlobal( extendedInfoPtr );

      _initialized = true;
   }

   public static bool AddProcess( Process process )
   {
      if ( process is null )
      {
         throw new ArgumentNullException( nameof( process ) );
      }

      return AssignProcessToJobObject( _handle, process.Handle );
   }

   #region Native Structs and Methods

   private enum JobObjectInfoType
   {
      ExtendedLimitInformation = 9,
   }

   [StructLayout( LayoutKind.Sequential )]
   private struct JOBOBJECT_BASIC_LIMIT_INFORMATION
   {
      public long PerProcessUserTimeLimit;
      public long PerJobUserTimeLimit;
      public JOBOBJECTLIMIT LimitFlags;
      public UIntPtr MinimumWorkingSetSize;
      public UIntPtr MaximumWorkingSetSize;
      public uint ActiveProcessLimit;
      public long Affinity;
      public uint PriorityClass;
      public uint SchedulingClass;
   }

   [Flags]
   private enum JOBOBJECTLIMIT : uint
   {
      None = 0,
      JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000
   }

   [StructLayout( LayoutKind.Sequential )]
   private struct IO_COUNTERS
   {
      public ulong ReadOperationCount;
      public ulong WriteOperationCount;
      public ulong OtherOperationCount;
      public ulong ReadTransferCount;
      public ulong WriteTransferCount;
      public ulong OtherTransferCount;
   }

   [StructLayout( LayoutKind.Sequential )]
   private struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
   {
      public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
      public IO_COUNTERS IoInfo;
      public UIntPtr ProcessMemoryLimit;
      public UIntPtr JobMemoryLimit;
      public UIntPtr PeakProcessMemoryUsed;
      public UIntPtr PeakJobMemoryUsed;
   }

   [DllImport( "kernel32.dll", CharSet = CharSet.Unicode )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   private static extern IntPtr CreateJobObject( IntPtr lpJobAttributes, string name );

   [DllImport( "kernel32.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   private static extern bool SetInformationJobObject( IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength );

   [DllImport( "kernel32.dll", SetLastError = true )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   private static extern bool AssignProcessToJobObject( IntPtr job, IntPtr process );

   #endregion
}
