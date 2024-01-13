using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ZemotoCommon;

internal static class ExtensionMethods
{
   public static void StartAsChildProcess( this Process process )
   {
      ArgumentNullException.ThrowIfNull( process );

      process.Start();
      ChildProcessWatcher.AddProcess( process );
   }

   public static void ForEach<T>( this IEnumerable<T> collection, Action<T> action )
   {
      collection.ToList().ForEach( action );
   }

   public static T GetAttribute<T>( this Enum enumValue ) where T : Attribute
   {
      ArgumentNullException.ThrowIfNull( enumValue );

      var enumValueInfo = enumValue.GetType().GetMember( enumValue.ToString() )[0];
      return GetAttribute<T>( enumValueInfo );
   }

   public static T GetAttribute<T>( this ICustomAttributeProvider property ) where T : Attribute
   {
      ArgumentNullException.ThrowIfNull( property );

      var attribute = property.GetCustomAttributes( typeof( T ), false ).FirstOrDefault();
      return attribute as T;
   }

   public static IEnumerable<T> GetAttributes<T>( this Enum enumValue ) where T : Attribute
   {
      ArgumentNullException.ThrowIfNull( enumValue );

      var enumValueInfo = enumValue.GetType().GetMember( enumValue.ToString() )[0];
      return GetAttributes<T>( enumValueInfo );
   }

   public static IEnumerable<T> GetAttributes<T>( this ICustomAttributeProvider property ) where T : Attribute
   {
      ArgumentNullException.ThrowIfNull( property );

      return property.GetCustomAttributes( typeof( T ), false ).Cast<T>();
   }

   public static void ForEach( this Array array, Action<Array, int[]> action )
   {
      ArgumentNullException.ThrowIfNull( array );
      ArgumentNullException.ThrowIfNull( action );

      if ( array.LongLength == 0 )
      {
         return;
      }

      var walker = new ArrayTraverse( array );
      do
      {
         action( array, walker.Position );
      }
      while ( walker.Step() );
   }

   internal sealed class ArrayTraverse
   {
      public int[] Position { get; }
      private readonly int[] _maxLengths;

      public ArrayTraverse( Array array )
      {
         _maxLengths = new int[array.Rank];
         for ( int i = 0; i < array.Rank; ++i )
         {
            _maxLengths[i] = array.GetLength( i ) - 1;
         }
         Position = new int[array.Rank];
      }

      public bool Step()
      {
         for ( int i = 0; i < Position.Length; ++i )
         {
            if ( Position[i] < _maxLengths[i] )
            {
               Position[i]++;
               for ( int j = 0; j < i; j++ )
               {
                  Position[j] = 0;
               }
               return true;
            }
         }
         return false;
      }
   }
}