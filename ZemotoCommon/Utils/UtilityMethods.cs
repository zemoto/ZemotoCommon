using System.IO;

namespace ZemotoCommon.Utils
{
   public static class UtilityMethods
   {
      public static void SafeDeleteFile( string filePath )
      {
         if ( File.Exists( filePath ) )
         {
            try
            {
               File.Delete( filePath );
            }
            catch { }
         }
      }
   }
}
