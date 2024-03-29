﻿using System;

namespace ZemotoCommon;

internal sealed class ScopeGuard : IDisposable
{
   private readonly Action _disposeAction;

   public ScopeGuard( Action constructionAction, Action disposeAction )
   {
      constructionAction.Invoke();
      _disposeAction = disposeAction;
   }

   public ScopeGuard( Action disposeAction ) => _disposeAction = disposeAction;

   public void Dispose() => _disposeAction?.Invoke();
}