#if ZEMOTOUI
using CommunityToolkit.Mvvm.Input;

namespace ZemotoCommon.UI;

internal static class UtilityCommands
{
   public static RelayCommand<string> OpenInBrowserCommand => field ??= new RelayCommand<string>( url =>
   {
      if ( !string.IsNullOrEmpty( url ) )
      {
         UtilityMethods.OpenInBrowser( url );
      }
   } );
}
#endif