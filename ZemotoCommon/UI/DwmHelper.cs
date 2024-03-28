#if ZEMOTOUI
// Based on https://stackoverflow.com/questions/3372303/dropshadow-for-wpf-borderless-window
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ZemotoCommon.UI;

internal static class DwmHelper
{
   private const int DWMWA_NCRENDERING_POLICY = 2;
   private const int DWMNCRP_ENABLED = 2;

   [StructLayout( LayoutKind.Sequential )]
   public struct Margins( int left, int right, int top, int bottom )
   {
      public int Left = left;
      public int Right = right;
      public int Top = top;
      public int Bottom = bottom;
   }

   [DllImport( "dwmapi.dll", PreserveSig = true )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   private static extern int DwmSetWindowAttribute( IntPtr hwnd, int attr, ref int attrValue, int attrSize );

   [DllImport( "dwmapi.dll" )]
   [DefaultDllImportSearchPaths( DllImportSearchPath.System32 )]
   private static extern int DwmExtendFrameIntoClientArea( IntPtr hWnd, ref Margins pMarInset );

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
         int val = DWMNCRP_ENABLED;
         if ( DwmSetWindowAttribute( windowHandle, DWMWA_NCRENDERING_POLICY, ref val, sizeof( int ) ) == 0 )
         {
            var m = new Margins( 0, 0, 0, 1 );
            return DwmExtendFrameIntoClientArea( windowHandle, ref m ) == 0;
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