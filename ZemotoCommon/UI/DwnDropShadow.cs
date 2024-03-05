#if ZEMOTOUI
using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ZemotoCommon.UI;

internal static class DwmDropShadow
{
   private const int DWMWA_NCRENDERING_POLICY = 2;
   private const int DWMNCRP_ENABLED = 2;

   [DllImport( "dwmapi.dll", PreserveSig = true )]
   private static extern int DwmSetWindowAttribute( IntPtr hwnd, int attr, ref int attrValue, int attrSize );

   [DllImport( "dwmapi.dll" )]
   private static extern int DwmExtendFrameIntoClientArea( IntPtr hWnd, ref Margins pMarInset );

   public static void AddDropShadowToWindow( Window window )
   {
      if ( !DropShadow( window ) )
      {
         window.SourceInitialized += OnWindowSourceInitialized;
      }
   }

   private static void OnWindowSourceInitialized( object sender, EventArgs e )
   {
      var window = (Window)sender;
      _ = DropShadow( window );
      window.SourceInitialized -= OnWindowSourceInitialized;
   }

   private static bool DropShadow( Window window )
   {
      try
      {
         var windowHandle = new WindowInteropHelper( window ).Handle;
         int val = DWMNCRP_ENABLED;
         if ( DwmSetWindowAttribute( windowHandle, DWMWA_NCRENDERING_POLICY, ref val, sizeof( int ) ) == 0 )
         {
            var m = new Margins( 0, 0, 0, 0 );
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