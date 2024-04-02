using System.IO;

namespace ZemotoCommon;

internal static class FileUtils
{
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
         _ = Directory.CreateDirectory( dirPath );
      }
   }
}
