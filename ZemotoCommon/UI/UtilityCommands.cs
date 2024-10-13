#if ZEMOTOUI
using CommunityToolkit.Mvvm.Input;

namespace ZemotoCommon.UI;

internal static class UtilityCommands
{
   private static RelayCommand<string> _openInBrowserCommand;
   public static RelayCommand<string> OpenInBrowserCommand => _openInBrowserCommand ??= new RelayCommand<string>( UtilityMethods.OpenInBrowser );
}
#endif