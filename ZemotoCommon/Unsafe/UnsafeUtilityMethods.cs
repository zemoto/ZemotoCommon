using System.Diagnostics;

namespace ZemotoCommon.Unsafe;

internal static class UnsafeUtilityMethods
{
   public static void StartAsChildProcess( this Process process )
   {
      ArgumentNullException.ThrowIfNull( process );

      _ = process.Start();
      _ = ChildProcessWatcher.AddProcess( process );
   }

   /// <summary>
   /// Gets the default application associated with a given extension
   /// </summary>
   /// <param name="extension">The extension to determine the default application for</param>
   /// <param name="appExe">Gets set to the location of the default application exe</param>
   /// <returns>Whether or not an associated application was found for the given extension</returns>
   public static bool GetDefaultAppForExtension( string extension, out string appExe )
   {
      appExe = string.Empty;
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

      char[] outVal = new char[length - 1];
      ret = NativeMethods.AssocQueryString( AssocFNone, AssocStrExecutable, extension, null, outVal, ref length );
      if ( ret != S_OK )
      {
         return false;
      }

      appExe = new string( outVal );
      return true;
   }
}
