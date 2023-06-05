using System;
using System.Threading.Tasks;

namespace ZemotoCommon;

internal static class SafeMethod
{
   public static async Task InvokeSafelyAsync( Task task, Action<Exception> exceptionAction = null )
   {
      try
      {
         await task.ConfigureAwait( false );
      }
      catch ( Exception ex )
      {
         exceptionAction?.Invoke( ex );
      }
   }

   public static async Task<T> InvokeSafelyAsync<T>( Task<T> task, Action<Exception> exceptionAction = null )
   {
      try
      {
         return await task.ConfigureAwait( false );
      }
      catch ( Exception ex )
      {
         exceptionAction?.Invoke( ex );
      }

      return default;
   }
}
