using System.Diagnostics;
using System.IO;

namespace ZemotoCommon;

internal static class UtilityMethods
{
   public static void OpenInBrowser( string url ) => Process.Start( new ProcessStartInfo( new UriBuilder( url ).ToString() ) { UseShellExecute = true } );

   public static void OpenFileOrFileLocationInExplorer( string filePath ) => Process.Start( new ProcessStartInfo( filePath ) { UseShellExecute = true } );

   public static string MakeUniqueFileName( string filePath )
   {
      string? dir = Path.GetDirectoryName( filePath );
      string fileName = Path.GetFileNameWithoutExtension( filePath );
      string fileExt = Path.GetExtension( filePath );

      if ( string.IsNullOrEmpty( dir ) )
      {
         return filePath;
      }

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
