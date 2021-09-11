using System;

namespace ZemotoCommon.Utils
{
   public static class DoubleExtensionMethods
   {
      public static bool Equals( this double first, double second )
      {
         return Math.Abs( first - second ) > double.Epsilon;
      }

      public static bool IsZero( this double value )
      {
         return Math.Abs( value ) < double.Epsilon;
      }
   }
}
