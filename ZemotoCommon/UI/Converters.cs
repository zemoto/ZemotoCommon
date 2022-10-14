using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ZemotoCommon.UI
{
   public sealed class BoolVisibilityConverter : IValueConverter
   {
      public bool Invert { get; set; }

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         var boolValue = (bool)value;
         return Invert
            ? boolValue ? Visibility.Collapsed : Visibility.Visible
            : boolValue ? Visibility.Visible : Visibility.Collapsed;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
   }

   public sealed class BoolToObjectConverter : IValueConverter
   {
      public object TrueValue { get; set; }
      public object FalseValue { get; set; }

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) => value is bool boolValue ? boolValue ? TrueValue : FalseValue : FalseValue;
      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => value.Equals( TrueValue );
   }

   public sealed class EqualityConverter : IValueConverter
   {
      public bool Invert { get; set; }
      public Type ComparisonType { get; set; }

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         object castedParameter = parameter;
         if ( ComparisonType is not null )
         {
            castedParameter = System.Convert.ChangeType( parameter, ComparisonType );
         }

         var equalityFunction = value == null ? new Func<object, bool>( x => x == null ) : value.Equals;
         return Invert ? !equalityFunction( castedParameter ) : equalityFunction( castedParameter );
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
   }

   public sealed class EqualityToVisibilityConverter : IValueConverter
   {
      public bool Invert { get; set; }

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         var equalityFunction = value == null ? new Func<object, bool>( x => x == null ) : value.Equals;
         return Invert ? equalityFunction( parameter ) ? Visibility.Collapsed : Visibility.Visible
                       : equalityFunction( parameter ) ? Visibility.Visible : Visibility.Collapsed;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
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
