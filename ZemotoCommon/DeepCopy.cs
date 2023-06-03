﻿// Based on https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net/11308879#11308879
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZemotoCommon;

public static class DeepCopy
{
   private static readonly MethodInfo CloneMethod = typeof( object ).GetMethod( "MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance );

   private static bool IsPrimitive( Type type ) => type == typeof( string ) || ( type.IsValueType && type.IsPrimitive );

   private static object InternalCopy( object originalObject, IDictionary<object, object> visited )
   {
      if ( originalObject == null )
      {
         return null;
      }

      var typeToReflect = originalObject.GetType();
      if ( IsPrimitive( typeToReflect ) )
      {
         return originalObject;
      }

      if ( visited.TryGetValue( originalObject, out var value ) )
      {
         return value;
      }

      if ( typeof( Delegate ).IsAssignableFrom( typeToReflect ) )
      {
         return null;
      }

      var cloneObject = CloneMethod.Invoke( originalObject, null );
      if ( typeToReflect.IsArray )
      {
         var arrayType = typeToReflect.GetElementType();
         if ( !IsPrimitive( arrayType ) )
         {
            Array clonedArray = (Array)cloneObject;
            clonedArray.ForEach( ( array, indices ) => array.SetValue( InternalCopy( clonedArray.GetValue( indices ), visited ), indices ) );
         }
      }

      visited.Add( originalObject, cloneObject );
      CopyFields( originalObject, visited, cloneObject, typeToReflect );
      RecursiveCopyBaseTypePrivateFields( originalObject, visited, cloneObject, typeToReflect );

      return cloneObject;
   }

   private static void RecursiveCopyBaseTypePrivateFields( object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect )
   {
      if ( typeToReflect.BaseType != null )
      {
         RecursiveCopyBaseTypePrivateFields( originalObject, visited, cloneObject, typeToReflect.BaseType );
         CopyFields( originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate );
      }
   }

   private static void CopyFields( object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null )
   {
      foreach ( FieldInfo fieldInfo in typeToReflect.GetFields( bindingFlags ) )
      {
         if ( filter != null && !filter( fieldInfo ) )
         {
            continue;
         }

         if ( IsPrimitive( fieldInfo.FieldType ) )
         {
            continue;
         }

         var originalFieldValue = fieldInfo.GetValue( originalObject );
         var clonedFieldValue = InternalCopy( originalFieldValue, visited );
         fieldInfo.SetValue( cloneObject, clonedFieldValue );
      }
   }

   public static T Copy<T>( T original ) => (T)Copy( (object)original );

   private static object Copy( object originalObject ) => InternalCopy( originalObject, new Dictionary<object, object>( new ReferenceEqualityComparer() ) );
}
