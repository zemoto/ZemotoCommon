﻿using System.IO;
using System.Linq;

namespace ZemotoCommon;

internal static class FileUtils
{
   public static bool MoveFileToFolder( string filePath, string targetDir, out string newPath )
   {
      try
      {
         var fileName = Path.GetFileName( filePath );
         var target = Path.Combine( targetDir, fileName );

         if ( filePath != target )
         {
            if ( !File.Exists( target ) )
            {
               File.Delete( target );
            }

            File.Move( filePath, target );
         }

         newPath = target;
         return true;
      }
      catch
      {
         newPath = string.Empty;
         return false;
      }
   }

   public static bool CopyFileToFolder( string filePath, string targetDir, out string newPath )
   {
      try
      {
         var fileName = Path.GetFileName( filePath );
         var target = Path.Combine( targetDir, fileName );

         if ( filePath != target )
         {
            if ( File.Exists( target ) )
            {
               File.Delete( target );
            }

            File.Copy( filePath, target );
         }

         newPath = target;
         return true;
      }
      catch
      {
         newPath = string.Empty;
         return false;
      }
   }

   public static bool RenameFile( string filePath, string newName, out string newPath )
   {
      try
      {
         var dir = Path.GetDirectoryName( filePath );
         var newFilePath = Path.Combine( dir, newName ) + Path.GetExtension( filePath );

         if ( filePath != newFilePath )
         {
            if ( File.Exists( newFilePath ) )
            {
               File.Delete( newFilePath );
            }

            File.Move( filePath, newFilePath );
         }

         newPath = newFilePath;
         return true;
      }
      catch
      {
         newPath = string.Empty;
         return false;
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

   public static string AbbreviatePath( string filePath )
   {
      if ( string.IsNullOrEmpty( filePath ) )
      {
         return string.Empty;
      }

      var parts = filePath.Split( '\\' );
      if ( parts.Length <= 3 )
      {
         return filePath;
      }

      return $@"..\{string.Join( @"\", parts.TakeLast( 2 ) )}";
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
