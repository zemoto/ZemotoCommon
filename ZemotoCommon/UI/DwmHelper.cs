#if ZEMOTOUI
// Based on https://stackoverflow.com/questions/3372303/dropshadow-for-wpf-borderless-window
using System;
using System.Windows;
using System.Windows.Interop;

namespace ZemotoCommon.UI;

internal static class DwmHelper
{
   public static void EnableDwmManagementOfWindow( Window window )
   {
      if ( !EnableDwm( window ) )
      {
         window.SourceInitialized += OnWindowSourceInitialized;
      }
   }

   private static void OnWindowSourceInitialized( object sender, EventArgs e )
   {
      var window = (Window)sender;
      _ = EnableDwm( window );
      window.SourceInitialized -= OnWindowSourceInitialized;
   }

   private static bool EnableDwm( Window window )
   {
      try
      {
         var windowHandle = new WindowInteropHelper( window ).Handle;
         int val = NativeMethods.DWMNCRP_ENABLED;
         if ( NativeMethods.DwmSetWindowAttribute( windowHandle, NativeMethods.DWMWA_NCRENDERING_POLICY, ref val, sizeof( int ) ) == 0 )
         {
            var m = new NativeMethods.Margins( 0, 0, 0, 1 );
            return NativeMethods.DwmExtendFrameIntoClientArea( windowHandle, ref m ) == 0;
         }
         else
         {
            return false;
         }
      }
      catch
      {
         return false;
      }
   }
}
#endif