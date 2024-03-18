#if ZEMOTOUI
using System;
using System.IO;
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
         Shutdown();
      }
   }

   public void Dispose() => _singleInstance.Dispose();

   protected override void OnExit( ExitEventArgs e ) => Dispose();

   private void OnUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e ) => File.WriteAllText( "crash.txt", e.Exception.ToString() );
}
#endif