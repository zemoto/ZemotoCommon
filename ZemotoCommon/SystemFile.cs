using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZemotoCommon;

internal sealed class SystemFileJsonConverter : JsonConverter<SystemFile>
{
   public override SystemFile Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) => new( reader.GetString() ?? string.Empty );
   public override void Write( Utf8JsonWriter writer, SystemFile value, JsonSerializerOptions options ) => writer.WriteStringValue( value.FullPath );
}

[JsonConverter( typeof( SystemFileJsonConverter ) )]
internal sealed class SystemFile
{
   private static readonly JsonSerializerOptions _serializerOptions = new() { IgnoreReadOnlyProperties = true };

   public SystemFile( string path )
   {
      try
      {
         if ( !Path.IsPathRooted( path ) )
         {
            path = Path.Combine( AppContext.BaseDirectory, path );
         }

         FullPath = path;
         Name = Path.GetFileName( path );
         Directory = Path.GetDirectoryName( path ) ?? string.Empty;
      }
      catch
      {
         // Default to null file on failure to parse path
         FullPath = string.Empty;
         Name = string.Empty;
         Directory = string.Empty;
      }
   }

   public bool ReadAllText( out string contents )
   {
      contents = string.Empty;
      try
      {
         if ( this.Exists() )
         {
            contents = File.ReadAllText( FullPath );
            return true;
         }
      }
      catch
      {
         // Fall through to fail case
      }

      return false;
   }

   public bool WriteAllText( string contents )
   {
      try
      {
         File.WriteAllText( FullPath, contents );
         return true;
      }
      catch
      {
         // Fall through to fail case
      }

      return false;
   }

   public T? DeserializeContents<T>() => ReadAllText( out string contents ) ? JsonSerializer.Deserialize<T>( contents, _serializerOptions ) : default;

   public void SerializeInto<T>( T objectToSerialize )
   {
      string serializedContent = JsonSerializer.Serialize( objectToSerialize, _serializerOptions );
      _ = WriteAllText( serializedContent );
   }

   public bool CopyTo( string targetDir, string fileNameNoExtension, bool overwrite, out SystemFile copiedFile ) => CopyTo( Path.Combine( targetDir, fileNameNoExtension + Extension ), overwrite, out copiedFile );

   public bool CopyTo( string targetFilePath, bool overwrite, out SystemFile copiedFile )
   {
      try
      {
         File.Copy( FullPath, targetFilePath, overwrite );
         copiedFile = targetFilePath;
         return true;
      }
      catch
      {
         copiedFile = string.Empty;
         return false;
      }
   }

   public bool MoveTo( string targetFilePath, bool overwrite, out SystemFile movedFile )
   {
      try
      {
         File.Move( FullPath, targetFilePath, overwrite );
         movedFile = targetFilePath;
         return true;
      }
      catch
      {
         movedFile = string.Empty;
         return false;
      }
   }

   public void Delete()
   {
      try
      {
         File.Delete( FullPath );
      }
      catch
      {
      }
   }

   public string FullPath { get; }
   public string Name { get; }
   public string Directory { get; }

   public string NameNoExtension => field ??= Path.GetFileNameWithoutExtension( FullPath );

   public string Extension => field ??= Path.GetExtension( FullPath );

   public string AbbreviatedPath
   {
      get
      {
         if ( string.IsNullOrEmpty( field ) )
         {
            if ( string.IsNullOrEmpty( FullPath ) )
            {
               field = string.Empty;
            }
            else
            {
               string[] parts = FullPath.Split( '\\' );
               field = parts.Length <= 3 ? FullPath : $@"..\{string.Join( @"\", parts.TakeLast( 2 ) )}";
            }
         }

         return field;
      }
   }

   public static implicit operator string( SystemFile file ) => file.FullPath;

   public static implicit operator SystemFile( string filePath ) => new( filePath );
}

internal static class SystemFileExtensions
{
   /// <summary>
   /// Returns true if the SystemFile represents a file that exists on the system.
   /// Can be used on null SystemFile objects and will return false if null.
   /// </summary>
   public static bool Exists( this SystemFile file ) => file is not null && File.Exists( file.FullPath );
}