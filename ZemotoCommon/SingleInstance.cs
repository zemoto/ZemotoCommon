using System;
using System.IO.Pipes;
using System.Threading;

namespace ZemotoCommon;

internal sealed class SingleInstance( string instanceName, bool listenForOtherInstances ) : IDisposable
{
   public event EventHandler PingedByOtherProcess;

   private readonly Mutex _instanceMutex = new( true, instanceName );
   private NamedPipeServerStream _server;

   private bool _disposed;

   public void Dispose()
   {
      _disposed = true;
      _instanceMutex.Dispose();
      _server?.Dispose();
   }

   public bool Claim()
   {
      if ( !_instanceMutex.WaitOne( TimeSpan.Zero ) )
      {
         PingSingleInstance();
         return false;
      }

      if ( listenForOtherInstances )
      {
         ListenForOtherProcesses();
      }
      return true;
   }

   private void PingSingleInstance()
   {
      // The act of connecting indicates to the single instance that another process tried to run
      using var client = new NamedPipeClientStream( ".", instanceName, PipeDirection.Out );
      try
      {
         client.Connect( 0 );
      }
      catch
      {
         // ignore
      }
   }

   private void ListenForOtherProcesses()
   {
      _server = new NamedPipeServerStream( instanceName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous );
      _ = _server.BeginWaitForConnection( OnPipeConnection, _server );
   }

   private void OnPipeConnection( IAsyncResult ar )
   {
      using ( var server = (NamedPipeServerStream)ar.AsyncState )
      {
         try
         {
            server.EndWaitForConnection( ar );
         }
         catch
         {
            // ignore
         }
      }

      if ( !_disposed )
      {
         PingedByOtherProcess?.Invoke( null, EventArgs.Empty );
         ListenForOtherProcesses();
      }
   }
}
