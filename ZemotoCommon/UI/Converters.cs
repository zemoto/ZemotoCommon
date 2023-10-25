#if ZEMOTOUI
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ZemotoCommon.UI;

internal sealed class BoolVisibilityConverter : IValueConverter
{
   public bool CollapseWhenNotVisible { get; set; } = true;
   public bool Invert { get; set; }

   public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
   {
      bool visible = false;
      if ( value is bool boolValue )
      {
         visible = boolValue;
      }

      if ( Invert )
      {
         visible = !visible;
      }

      return visible
            ? Visibility.Visible
            : ( CollapseWhenNotVisible ? Visibility.Collapsed : Visibility.Hidden );
   }

   public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
}

internal sealed class BoolToObjectConverter : IValueConverter
{
   public object TrueValue { get; set; }
   public object FalseValue { get; set; }

   public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) => value is bool boolValue ? boolValue ? TrueValue : FalseValue : FalseValue;
   public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => value?.Equals( TrueValue ) ?? false;
}

internal static class EqualityLogic
{
   public static bool GetEqualityValue( object value, object parameter, bool invert, CultureInfo culture )
   {
      var sourceType = value.GetType();
      var castedParameter = parameter;
      if ( sourceType != typeof( object ) )
      {
         castedParameter = Convert.ChangeType( parameter, sourceType, culture );
      }

      var equalityFunction = value == null ? new Func<object, bool>( x => x == null ) : value.Equals;
      return invert ? !equalityFunction( castedParameter ) : equalityFunction( castedParameter );
   }
}

internal sealed class EqualityConverter : IValueConverter
{
   public bool Invert { get; set; }

   public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) => EqualityLogic.GetEqualityValue( value, parameter, Invert, culture );

   public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
}

internal sealed class EqualityToObjectConverter : IValueConverter
{
   public object EqualValue { get; set; }
   public object NotEqualValue { get; set; }

   public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
   {
      bool equalityValue = EqualityLogic.GetEqualityValue( value, parameter, false, culture );
      return equalityValue ? EqualValue : NotEqualValue;
   }

   public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
}

internal sealed class MultiBoolToBoolAndConverter : IMultiValueConverter
{
   public static MultiBoolToBoolAndConverter Instance { get; } = new MultiBoolToBoolAndConverter();
   private MultiBoolToBoolAndConverter()
   {
   }

   public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture ) => values.OfType<bool>().Aggregate( ( current, value ) => current && value );

   public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture ) => throw new NotImplementedException();
}

internal sealed class InvertBoolConverter : IValueConverter
{
   public static InvertBoolConverter Instance { get; } = new InvertBoolConverter();
   private InvertBoolConverter()
   {
   }

   public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) => value is bool boolValue ? !boolValue : throw new ArgumentException( "Argument is not a bool", nameof( value ) );

   public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => value is bool boolValue ? !boolValue : throw new ArgumentException( "Argument is not a bool", nameof( value ) );
}

internal sealed class NullVisibilityConverter : IValueConverter
{
   public static NullVisibilityConverter Instance { get; } = new NullVisibilityConverter();
   private NullVisibilityConverter()
   {
   }

   public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
   {
      bool nullOrEmpty = value is string stringValue ? string.IsNullOrEmpty( stringValue ) : value is null;
      return nullOrEmpty ? Visibility.Collapsed : Visibility.Visible;
   }

   public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
}
#endif