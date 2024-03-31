using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZemotoCommon;

internal sealed class SystemFileJsonConverter : JsonConverter<SystemFile>
{
   public override SystemFile Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) => new( reader.GetString() );
   public override void Write( Utf8JsonWriter writer, SystemFile value, JsonSerializerOptions options ) => writer.WriteStringValue( value.FullPath );
}

[JsonConverter( typeof( SystemFileJsonConverter ) )]
internal sealed class SystemFile
{
   public SystemFile( string path )
   {
      try
      {
         if ( !Path.IsPathRooted( path ) )
         {
            path = Path.GetFullPath( path );
         }

         FullPath = path;
         FileName = Path.GetFileName( path );
         Directory = Path.GetDirectoryName( path );
      }
      catch
      {
      }
   }

   private string GetAbbreviatedPath()
   {
      if ( string.IsNullOrEmpty( FullPath ) )
      {
         return string.Empty;
      }

      var parts = FullPath.Split( '\\' );
      if ( parts.Length <= 3 )
      {
         return FullPath;
      }

      return $@"..\{string.Join( @"\", parts.TakeLast( 2 ) )}";
   }

   public T DeserializeContents<T>() => Exists ? JsonSerializer.Deserialize<T>( File.ReadAllText( FullPath ) ) : default;

   public string FullPath { get; }
   public string FileName { get; }
   public string Directory { get; }

   public bool Exists => File.Exists( FullPath );

   private string _fileNameNoExtension;
   public string FileNameNoExtension => _fileNameNoExtension ??= string.IsNullOrEmpty( FileName ) ? string.Empty : FileName.Split( '.' ).First();

   private string _abbreviatedPath;
   public string AbbreviatedPath => _abbreviatedPath ??= GetAbbreviatedPath();

   public static implicit operator string( SystemFile file ) => file.FullPath;
   public static implicit operator SystemFile( string filePath ) => new SystemFile( filePath );
}