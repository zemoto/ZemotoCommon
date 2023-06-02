﻿using System;
using System.IO.Pipes;
using System.Threading;

namespace ZemotoCommon;

internal sealed class SingleInstance : IDisposable
{
   public event EventHandler PingedByOtherProcess;

   private readonly Mutex _instanceMutex;
   private readonly string _instanceName;
   private NamedPipeServerStream _server;

   public SingleInstance( string instanceName )
   {
      _instanceMutex = new Mutex( true, instanceName );
      _instanceName = instanceName;
   }

   public void Dispose()
   {
      _instanceMutex.Dispose();

      if ( _server != null )
      {
         _server.Dispose();
      }
   }

   public bool Claim()
   {
      if ( !_instanceMutex.WaitOne( TimeSpan.Zero ) )
      {
         PingSingleInstance();
         return false;
      }

      ListenForOtherProcesses();
      return true;
   }

   private void PingSingleInstance()
   {
      // The act of connecting indicates to the single instance that another process tried to run
      using var client = new NamedPipeClientStream( ".", _instanceName, PipeDirection.Out );
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
      _server = new NamedPipeServerStream( _instanceName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous );
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

      PingedByOtherProcess?.Invoke( null, EventArgs.Empty );

      ListenForOtherProcesses();
   }
}
