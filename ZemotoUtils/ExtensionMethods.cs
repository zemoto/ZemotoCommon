using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ZemotoUtils
{
   public static class ExtensionMethods
   {
      public static void StartAsChildProcess( this Process process )
      {
         process.Start();
         ChildProcessWatcher.AddProcess( process );
      }

      public static void ForEach<T>( this IEnumerable<T> collection, Action<T> action )
      {
         collection.ToList().ForEach( action );
      }

      public static T GetAttribute<T>( this Enum enumValue ) where T : Attribute
      {
         var enumValueInfo = enumValue.GetType().GetMember( enumValue.ToString() ).First();
         return GetAttribute<T>( enumValueInfo );
      }

      public static T GetAttribute<T>( this ICustomAttributeProvider property ) where T : Attribute
      {
         var attribute = property.GetCustomAttributes( typeof( T ), false ).FirstOrDefault();
         return attribute as T;
      }

      public static IEnumerable<T> GetAttributes<T>( this Enum enumValue ) where T : Attribute
      {
         var enumValueInfo = enumValue.GetType().GetMember( enumValue.ToString() ).First();
         return GetAttributes<T>( enumValueInfo );
      }

      public static IEnumerable<T> GetAttributes<T>( this ICustomAttributeProvider property ) where T : Attribute
      {
         return property.GetCustomAttributes( typeof( T ), false ).Cast<T>();
      }

      public static void ForEach( this Array array, Action<Array, int[]> action )
      {
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
         public int[] Position { get; private set; }
         private int[] _maxLengths;

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
}