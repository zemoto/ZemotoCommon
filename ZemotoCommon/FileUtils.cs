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

   /// <summary>
   /// Gets the default application associated with a given extension
   /// </summary>
   /// <param name="extension">The extension to determine the default application for</param>
   /// <param name="appExe">Gets set to the location of the default application exe</param>
   /// <returns>Whether or not an associated application was found for the given extension</returns>
   public static bool GetDefaultAppForExtension( string extension, out string appExe )
   {
      appExe = null;
      if ( string.IsNullOrEmpty( extension ) )
      {
         return false;
      }

      const int S_OK = 0;
      const int S_FALSE = 1;
      const int AssocFNone = 0;
      const int AssocStrExecutable = 2;

      int length = 0;
      int ret = NativeMethods.AssocQueryString( AssocFNone, AssocStrExecutable, extension, null, null, ref length );
      if ( ret != S_FALSE )
      {
         return false;
      }

      var outVal = new char[length - 1];
      ret = NativeMethods.AssocQueryString( AssocFNone, AssocStrExecutable, extension, null, outVal, ref length );
      if ( ret != S_OK )
      {
         return false;
      }

      appExe = new string( outVal );
      return true;
   }
}
