using Microsoft.Win32;
using System.IO;

namespace ZemotoCommon;

internal static class WindowsStartup
{
   private const string _startupRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
   private const string _startupAllowedRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

   private static string ExeName => field ??= GetRunningExeName();

   private static string ExeLocation => field ??= Path.Combine( AppContext.BaseDirectory, ExeName );

   private static bool? _allowedToStartWithWindows;
   public static bool AllowedToStartWithWindows => _allowedToStartWithWindows ??= CheckIfAllowedToStartWithWindows();

   public static string? NotAllowedReason { get; private set; }

   public static bool GetStartupWithWindows()
   {
      try
      {
         RegistryKey regKey = Registry.CurrentUser.CreateSubKey( _startupRegKey, false );
         return ExeLocation.Equals( regKey.GetValue( ExeName ) as string, StringComparison.OrdinalIgnoreCase );
      }
      catch
      {
         return false;
      }
   }

   public static bool SetStartupWithWindows( bool startup )
   {
      try
      {
         RegistryKey regKey = Registry.CurrentUser.CreateSubKey( _startupRegKey, true );
         if ( startup )
         {
            regKey.SetValue( ExeName, ExeLocation );
         }
         else
         {
            regKey.DeleteValue( ExeName );
         }

         return true;
      }
      catch
      {
         return false;
      }
   }

   private static string GetRunningExeName()
   {
      const string exeExtension = ".exe";
      string exeName = AppDomain.CurrentDomain.FriendlyName;
      if ( !exeName.EndsWith( exeExtension, StringComparison.OrdinalIgnoreCase ) )
      {
         exeName += exeExtension;
      }

      return exeName;
   }

   private static bool CheckIfAllowedToStartWithWindows()
   {
      try
      {
         RegistryKey? regKey = Registry.CurrentUser.OpenSubKey( _startupAllowedRegKey, true );
         if ( regKey is null )
         {
            NotAllowedReason = "Insufficient read or write permissions, try running this app as admin.";
            return false;
         }

         bool allowedByWindows = regKey.GetValue( ExeName ) is not byte[] value || ( value.Length > 0 && value[0] == 2 );
         if ( !allowedByWindows )
         {
            NotAllowedReason = "Windows has blocked this app from launching on startup. Enable startup under \"Startup apps\" in Task Manager.";
         }

         return allowedByWindows;
      }
      catch
      {
         NotAllowedReason = "Insufficient read or write permissions, try running this app as admin.";
         return false;
      }
   }
}
