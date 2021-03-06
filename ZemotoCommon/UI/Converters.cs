﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ZemotoCommon.UI
{
   public sealed class EqualityToVisibilityConverter : IValueConverter
   {
      public bool Invert { get; set; }

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         var equalityFunction = value == null ? new Func<object, bool>( x => x == null ) : value.Equals;
         return Invert ? equalityFunction( parameter ) ? Visibility.Collapsed : Visibility.Visible 
                       : equalityFunction( parameter ) ? Visibility.Visible : Visibility.Collapsed;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }

   public sealed class MultiBoolToBoolAndConverter : IMultiValueConverter
   {
      public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture ) => values.OfType<bool>().Aggregate( ( current, value ) => current && value );

      public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}
