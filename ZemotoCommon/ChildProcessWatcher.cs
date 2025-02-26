using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ZemotoCommon;

// This class creates a "job" and allows the rest of the app to add child processes to it.
// When the job goes out of scope (program crashes, closes, exits, etc), all added child
// processes are closed with it.
internal static class ChildProcessWatcher
{
   private static readonly IntPtr _handle = NativeMethods.CreateJobObject( IntPtr.Zero, $"EncoderChildProcessTracker{Environment.ProcessId}" );
   private static bool _initialized;

   public static void Initialize()
   {
      if ( _initialized )
      {
         return;
      }

      var info = new NativeMethods.JOBOBJECT_BASIC_LIMIT_INFORMATION
      {
         LimitFlags = NativeMethods.JOBOBJECTLIMIT.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
      };

      var extendedInfo = new NativeMethods.JOBOBJECT_EXTENDED_LIMIT_INFORMATION
      {
         BasicLimitInformation = info
      };

      var length = Marshal.SizeOf<NativeMethods.JOBOBJECT_EXTENDED_LIMIT_INFORMATION>();
      var extendedInfoPtr = Marshal.AllocHGlobal( length );
      Marshal.StructureToPtr( extendedInfo, extendedInfoPtr, false );

      var result = NativeMethods.SetInformationJobObject( _handle, NativeMethods.JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length );
      Debug.Assert( result );

      Marshal.FreeHGlobal( extendedInfoPtr );

      _initialized = true;
   }

   public static bool AddProcess( Process process )
   {
      ArgumentNullException.ThrowIfNull( process );

      return NativeMethods.AssignProcessToJobObject( _handle, process.Handle );
   }
}
