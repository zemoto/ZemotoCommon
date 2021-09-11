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
   }
}
