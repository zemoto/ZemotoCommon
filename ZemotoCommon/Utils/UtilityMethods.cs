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
            catch { /*ignored*/ }
         }
      }

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
