﻿using System;
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

   public T DeserializeContents<T>() => this.Exists() ? JsonSerializer.Deserialize<T>( File.ReadAllText( FullPath ) ) : default;

   public string FullPath { get; }
   public string FileName { get; }
   public string Directory { get; }

   private string _fileNameNoExtension;
   public string FileNameNoExtension => _fileNameNoExtension ??= Path.GetFileNameWithoutExtension( FullPath );

   private string _abbreviatedPath;
   public string AbbreviatedPath
   {
      get
      {
         if ( string.IsNullOrEmpty( _abbreviatedPath ) )
         {
            if ( string.IsNullOrEmpty( FullPath ) )
            {
               _abbreviatedPath = string.Empty;
            }
            else
            {
               var parts = FullPath.Split( '\\' );
               _abbreviatedPath = parts.Length <= 3 ? FullPath : $@"..\{string.Join( @"\", parts.TakeLast( 2 ) )}";
            }
         }
         return _abbreviatedPath;
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