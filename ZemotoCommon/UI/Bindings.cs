#if ZEMOTOUI
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZemotoCommon.UI;

internal sealed class IntBinding : Binding
{
   private sealed class IntValidationRule : ValidationRule
   {
      public override ValidationResult Validate( object value, CultureInfo c ) => int.TryParse( (string)value, out _ ) ? new ValidationResult( true, null ) : new ValidationResult( false, "Value must be an integer" );
   }

   private sealed class IntMinMaxValidationRule( int min, int max ) : ValidationRule
   {
      public override ValidationResult Validate( object value, CultureInfo c )
      {
         bool validValue = int.TryParse( (string)value, out int intValue ) && intValue >= min && intValue <= max;
         return validValue ? new ValidationResult( true, null ) : new ValidationResult( false, $"Value must be an integer between {min} and {max}" );
      }
   }

   public IntBinding( string path )
      : base( path )
   {
      ValidationRules.Add( new IntValidationRule() );
      UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
   }

   public IntBinding( string path, int min, int max )
      : base( path )
   {
      ValidationRules.Add( new IntMinMaxValidationRule( min, max ) );
      UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
   }
}

internal sealed class DoubleBinding : Binding
{
   private sealed class DoubleValidationRule : ValidationRule
   {
      public override ValidationResult Validate( object value, CultureInfo c ) => double.TryParse( (string)value, out _ ) ? new ValidationResult( true, null ) : new ValidationResult( false, "Value must be an number" );
   }

   private sealed class DoubleMinMaxValidationRule( double min, double max ) : ValidationRule
   {
      public override ValidationResult Validate( object value, CultureInfo c )
      {
         var validValue = double.TryParse( (string)value, out double doubleValue ) && doubleValue >= min && doubleValue <= max;
         return validValue ? new ValidationResult( true, null ) : new ValidationResult( false, $"Value must be a number between {min} and {max}" );
      }
   }

   public DoubleBinding( string path )
      : base( path )
   {
      ValidationRules.Add( new DoubleValidationRule() );
      UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
   }

   public DoubleBinding( string path, double min, double max )
      : base( path )
   {
      ValidationRules.Add( new DoubleMinMaxValidationRule( min, max ) );
      UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
   }
}
#endif