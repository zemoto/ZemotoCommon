#if ZEMOTOUI
using System.Windows;
using System.Windows.Threading;

namespace ZemotoCommon.UI;

internal abstract class CommonApp : Application, IDisposable
{
   private readonly SingleInstance _singleInstance;

   protected CommonApp( string instanceName, bool listenForOtherInstances )
   {
      DispatcherUnhandledException += OnUnhandledException;
      _singleInstance = new SingleInstance( instanceName, listenForOtherInstances );
      if ( !_singleInstance.Claim() )
      {
         // Exit immediately so no more app code runs. Running instance has already been pinged.
         Environment.Exit( 0 );
      }

      if ( listenForOtherInstances )
      {
         _singleInstance.PingedByOtherProcess += OnPingedByOtherProcess;
      }

#if DEBUG
      _ = new Debugging.BindingErrorListener();
#endif
   }

   public virtual void Dispose() => _singleInstance.Dispose();

   private void OnPingedByOtherProcess( object? sender, EventArgs e ) => OnPingedByOtherProcess();

   protected override void OnExit( ExitEventArgs e ) => Dispose();

   private void OnUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e ) => _ = new SystemFile( "crash.txt" ).WriteAllText( e.Exception.ToString() );

   protected virtual void OnPingedByOtherProcess() { }
}
#endif