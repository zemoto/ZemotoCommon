using System;
using System.Runtime.InteropServices;

namespace ZemotoCommon;

internal static class NativeMethods
{
   public const int DWMWA_NCRENDERING_POLICY = 2;
   public const int DWMNCRP_ENABLED = 2;

   [Flags]
   public enum JOBOBJECTLIMIT : uint
   {
      None = 0,
      JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000
   }

   public enum JobObjectInfoType
   {
      ExtendedLimitInformation = 9,
   }

   [StructLayout( LayoutKind.Sequential )]
   public struct Margins( int left, int right, int top, int bottom )
   {
      public int Left = left;
      public int Right = right;
      public int Top = top;
      public int Bottom = bottom;
   }

   [StructLayout( LayoutKind.Sequential )]
   public struct JOBOBJECT_BASIC_LIMIT_INFORMATION
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

   [StructLayout( LayoutKind.Sequential )]
   public struct IO_COUNTERS
   {
      public ulong ReadOperationCount;
      public ulong WriteOperationCount;
      public ulong OtherOperationCount;
      public ulong ReadTransferCount;
      public ulong WriteTransferCount;
      public ulong OtherTransferCount;
   }

   [StructLayout( LayoutKind.Sequential )]
   public struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
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
   public static extern IntPtr CreateJobObject( IntPtr lpJobAttributes, string name );

   [DllImport( "kernel32.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static extern bool SetInformationJobObject( IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength );

   [DllImport( "kernel32.dll", SetLastError = true )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static extern bool AssignProcessToJobObject( IntPtr job, IntPtr process );

   [DllImport( "shlwapi.dll", CharSet = CharSet.Unicode )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static extern int AssocQueryString(
      int flags,
      int assocStr,
      [MarshalAs( UnmanagedType.LPWStr )] string pszAssoc,
      [MarshalAs( UnmanagedType.LPWStr )] string pszExtra,
      [MarshalAs( UnmanagedType.LPArray )][Out] char[] pszOut,
      ref int pcchOut );

   [DllImport( "dwmapi.dll", PreserveSig = true )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static extern int DwmSetWindowAttribute( IntPtr hwnd, int attr, ref int attrValue, int attrSize );

   [DllImport( "dwmapi.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static extern int DwmExtendFrameIntoClientArea( IntPtr hWnd, ref Margins pMarInset );

   [DllImport( "user32.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static extern bool SetForegroundWindow( IntPtr hWnd );
}