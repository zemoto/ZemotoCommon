using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ZemotoCommon.Utils
{
   // This class creates a "job" and allows the rest of the app to add child processes to it.
   // When the job goes out of scope (program crashes, closes, exits, etc), all added child
   // processes are closed with it.
   public static class ChildProcessWatcher
   {
      private static readonly IntPtr Handle;

      public static void Initialize() { /*Ensures the static constructor is called*/ }

      static ChildProcessWatcher()
      {
         Handle = CreateJobObject( IntPtr.Zero, $"EncoderChildProcessTracker{Process.GetCurrentProcess().Id}" );

         var info = new JOBOBJECT_BASIC_LIMIT_INFORMATION
         {
            LimitFlags = JOBOBJECTLIMIT.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
         };

         var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
         {
            BasicLimitInformation = info
         };

         int length = Marshal.SizeOf( typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION) );
         var extendedInfoPtr = Marshal.AllocHGlobal( length );
         Marshal.StructureToPtr( extendedInfo, extendedInfoPtr, false );

         var result = SetInformationJobObject( Handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length );
         Debug.Assert( result );

         Marshal.FreeHGlobal( extendedInfoPtr );
      }

      public static bool AddProcess( Process process )
      {
         return AssignProcessToJobObject( Handle, process.Handle );
      }

      #region Native Structs and Methods

      private enum JobObjectInfoType
      {
         ExtendedLimitInformation = 9,
      }

      [StructLayout( LayoutKind.Sequential )]
      private struct JOBOBJECT_BASIC_LIMIT_INFORMATION
      {
         public Int64 PerProcessUserTimeLimit;
         public Int64 PerJobUserTimeLimit;
         public JOBOBJECTLIMIT LimitFlags;
         public UIntPtr MinimumWorkingSetSize;
         public UIntPtr MaximumWorkingSetSize;
         public UInt32 ActiveProcessLimit;
         public Int64 Affinity;
         public UInt32 PriorityClass;
         public UInt32 SchedulingClass;
      }

      [Flags]
      private enum JOBOBJECTLIMIT : uint
      {
         JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000
      }

      [StructLayout( LayoutKind.Sequential )]
      private struct IO_COUNTERS
      {
         public UInt64 ReadOperationCount;
         public UInt64 WriteOperationCount;
         public UInt64 OtherOperationCount;
         public UInt64 ReadTransferCount;
         public UInt64 WriteTransferCount;
         public UInt64 OtherTransferCount;
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
      private static extern IntPtr CreateJobObject( IntPtr lpJobAttributes, string name );

      [DllImport( "kernel32.dll" )]
      private static extern bool SetInformationJobObject( IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength );

      [DllImport( "kernel32.dll", SetLastError = true )]
      private static extern bool AssignProcessToJobObject( IntPtr job, IntPtr process );

      #endregion
   }
}
