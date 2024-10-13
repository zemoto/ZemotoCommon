using System.Diagnostics;

namespace ZemotoCommon;

internal static class UtilityMethods
{
   public static void OpenInBrowser( string url ) => Process.Start( new ProcessStartInfo( url ) { UseShellExecute = true } );
}
