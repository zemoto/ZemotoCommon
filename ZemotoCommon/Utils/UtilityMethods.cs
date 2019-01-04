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

      public static string MakeUniqueFileName( string filePath )
      {
         string dir = Path.GetDirectoryName( filePath );
         string fileName = Path.GetFileNameWithoutExtension( filePath );
         string fileExt = Path.GetExtension( filePath );

         for ( int i = 1; ; i++ )
         {
            if ( !File.Exists( filePath ) )
            {
               return filePath;
            }
            filePath = Path.Combine( dir, $"{fileName}({i}){fileExt}" );
         }
      }
   }
}
