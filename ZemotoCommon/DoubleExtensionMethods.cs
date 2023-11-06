using System;

namespace ZemotoCommon;

internal static class DoubleExtensionMethods
{
   public static bool IsEqualTo( this double first, double second )
   {
      return Math.Abs( first - second ) < double.Epsilon;
   }

   public static bool IsZero( this double value )
   {
      return Math.Abs( value ) < double.Epsilon;
   }

   public static double MapNumberToRange( this double value, double oldMin, double oldMax, double newMin, double newMax )
   {
      return ( ( value - oldMin ) / ( oldMax - oldMin ) * ( newMax - newMin ) ) + newMin;
   }
}
