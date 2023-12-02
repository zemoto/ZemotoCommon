using System;
using System.Threading;

namespace ZemotoCommon;

internal sealed class CancelTokenProvider : IDisposable
{
   private CancellationTokenSource _tokenSource = new();

   public CancellationToken GetToken()
   {
      if ( _tokenSource.Token.IsCancellationRequested )
      {
         _tokenSource.Dispose();
         _tokenSource = new CancellationTokenSource();
      }

      return _tokenSource.Token;
   }

   public void Cancel() => _tokenSource.Cancel();

   public void Dispose() => _tokenSource.Dispose();
}
