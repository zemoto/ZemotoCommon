﻿#if ZEMOTOUI
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;

namespace ZemotoCommon.UI;

internal sealed class BoundEnumMember
{
   public BoundEnumMember( object value )
   {
      if ( value is null )
      {
         throw new ArgumentNullException( nameof( value ) );
      }

      Display = GetDescription( value );
      Value = value;
   }

   public string Display { get; }
   public object Value { get; }

   private static string GetDescription( object value )
   {
      var enumMember = value.GetType().GetMember( value.ToString() ).FirstOrDefault();
      if ( enumMember != null )
      {
         var description = enumMember.GetAttribute<DescriptionAttribute>()?.Description;
         if ( !string.IsNullOrEmpty( description ) )
         {
            return description;
         }
      }
      return value.ToString();
   }
}

internal sealed class GetEnumValuesExtension : MarkupExtension
{
   private readonly Type _type;
   public GetEnumValuesExtension( Type type )
   {
      _type = type;
   }

   public override object ProvideValue( IServiceProvider serviceProvider )
   {
      if ( serviceProvider?.GetService( typeof( IProvideValueTarget ) ) is IProvideValueTarget target && target.TargetObject is ComboBox box )
      {
         box.SelectedValuePath = "Value";
         box.DisplayMemberPath = "Display";
      }

      var enumValues = Enum.GetValues( _type );

      return ( from object enumValue in enumValues
               select new BoundEnumMember( enumValue ) ).ToArray();
   }
}
#endif