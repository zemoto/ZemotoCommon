using System.IO;

namespace ZemotoCommon.Utils
{
   public static class PathUtils
   {
      public static string MoveFileToFolder( string filePath, string targetDir )
      {
         var fileName = Path.GetFileName( filePath );
         var target = Path.Combine( targetDir, fileName );
         File.Move( filePath, target );
         return target;
      }

      public static string RenameFile( string filePath, string newName )
      {
         var dir = Path.GetDirectoryName( filePath );
         var newFilePath = Path.Combine( dir, newName ) + Path.GetExtension( filePath );

         if ( File.Exists( newFilePath ) )
         {
            File.Delete( newFilePath );
         }

         File.Move( filePath, newFilePath );
         return newFilePath;
      }
   }
}
