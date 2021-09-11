using System.IO;

namespace ZemotoCommon.Utils
{
   public static class UtilityMethods
   {
      public static void SafeDeleteDirectory( string dirPath )
      {
         if ( Directory.Exists( dirPath ) )
         {
            try
            {
               Directory.Delete( dirPath, true );
            }
            catch { /*ignored*/ }
         }
      }

      public static void CreateDirectory( string dirPath )
      {
         if ( !Directory.Exists( dirPath ) )
         {
            Directory.CreateDirectory( dirPath );
         }
      }

      public static double MapNumberToRange( double value, double oldMin, double oldMax, double newMin, double newMax )
      {
         return ( ( value - oldMin ) / ( oldMax - oldMin ) * ( newMax - newMin ) ) + newMin;
      }
   }
}
