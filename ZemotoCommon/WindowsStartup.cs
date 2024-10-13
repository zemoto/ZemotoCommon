using Microsoft.Win32;
using System;
using System.IO;

namespace ZemotoCommon;

internal static class WindowsStartup
{
   private const string _startupRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
   private const string _startupAllowedRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

   private static string _exeName;
   private static string ExeName => _exeName ??= GetRunningExeName();

   private static string _exeLocation;
   private static string ExeLocation => _exeLocation ??= Path.Combine( AppContext.BaseDirectory, ExeName );

   private static bool? _allowedToStartWithWindows;
   public static bool AllowedToStartWithWindows => _allowedToStartWithWindows ??= CheckIfAllowedToStartWithWindows();

   public static string NotAllowedReason { get; private set; }

   public static bool GetStartupWithWindows()
   {
      try
      {
         var regKey = Registry.CurrentUser.CreateSubKey( _startupRegKey, false );
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
         var regKey = Registry.CurrentUser.CreateSubKey( _startupRegKey, true );
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
      var exeName = AppDomain.CurrentDomain.FriendlyName;
      if ( !exeName.EndsWith( exeExtension ) )
      {
         exeName += exeExtension;
      }

      return exeName;
   }

   private static bool CheckIfAllowedToStartWithWindows()
   {
      try
      {
         var regKey = Registry.CurrentUser.OpenSubKey( _startupAllowedRegKey, true );
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
