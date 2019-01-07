using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ZemotoCommon.Utils
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
   }
}