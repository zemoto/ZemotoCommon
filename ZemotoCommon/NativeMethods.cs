using System.Runtime.InteropServices;

namespace ZemotoCommon;

internal static partial class NativeMethods
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

   [LibraryImport( "kernel32.dll", StringMarshalling = StringMarshalling.Utf16 )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static partial IntPtr CreateJobObject( IntPtr lpJobAttributes, string name );

   [LibraryImport( "kernel32.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   [return: MarshalAs( UnmanagedType.Bool )]
   public static partial bool SetInformationJobObject( IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength );

   [LibraryImport( "kernel32.dll", SetLastError = true )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   [return: MarshalAs( UnmanagedType.Bool )]
   public static partial bool AssignProcessToJobObject( IntPtr job, IntPtr process );

   [LibraryImport( "shlwapi.dll", StringMarshalling = StringMarshalling.Utf16 )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static partial int AssocQueryString(
      int flags,
      int assocStr,
      [MarshalAs( UnmanagedType.LPWStr )] string? pszAssoc,
      [MarshalAs( UnmanagedType.LPWStr )] string? pszExtra,
      [MarshalAs( UnmanagedType.LPArray )][Out] char[]? pszOut,
      ref int pcchOut );

   [LibraryImport( "dwmapi.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static partial int DwmSetWindowAttribute( IntPtr hwnd, int attr, ref int attrValue, int attrSize );

   [LibraryImport( "dwmapi.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   public static partial int DwmExtendFrameIntoClientArea( IntPtr hWnd, ref Margins pMarInset );

   [LibraryImport( "user32.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   [return: MarshalAs( UnmanagedType.Bool )]
   public static partial bool SetForegroundWindow( IntPtr hWnd );

   [LibraryImport( "user32.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   [return: MarshalAs( UnmanagedType.Bool )]
   public static partial bool RegisterHotKey( IntPtr hWnd, int id, uint fsModifiers, uint vk );

   [LibraryImport( "user32.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   [return: MarshalAs( UnmanagedType.Bool )]
   public static partial bool UnregisterHotKey( IntPtr hWnd, int id );
}