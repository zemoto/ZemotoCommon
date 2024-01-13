using System;

namespace ZemotoCommon;

internal sealed class ScopeGuard( Action disposeAction ) : IDisposable
{
   public void Dispose() => disposeAction?.Invoke();
}