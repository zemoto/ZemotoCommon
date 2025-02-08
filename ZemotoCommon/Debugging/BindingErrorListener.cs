#if (DEBUG && ZEMOTOUI)
using System.Diagnostics;
using System.Windows;

namespace ZemotoCommon.Debugging;

/// <summary>
/// When debugging, this class listens for binding errors and shows them as a Debug.Fail
/// </summary>
internal sealed class BindingErrorListener : TraceListener
{
   public BindingErrorListener() => PresentationTraceSources.DataBindingSource.Listeners.Add( this );

   public override void Write( string message )
   {
   }

   public override void WriteLine( string message )
   {
      if ( !string.IsNullOrEmpty( message ) )
      {
         Application.Current.Dispatcher.Invoke( () => Debug.Fail( message ) );
      }
   }
}
#endif