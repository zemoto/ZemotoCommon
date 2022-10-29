using System;
using System.Threading.Tasks;

namespace ZemotoCommon
{
   internal static class SafeMethod
   {
      public static async Task InvokeSafely( Func<Task> action )
      {
         try
         {
            await action();
         }
         catch { /*ignored*/ }
      }
   }
}
