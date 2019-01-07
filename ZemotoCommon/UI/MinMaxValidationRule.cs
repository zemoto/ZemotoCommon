using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZemotoCommon.UI
{
   public sealed class IntMinMaxBinding : Binding
   {
      private sealed class IntMinMaxValidationRule : ValidationRule
      {
         private readonly int _min;
         private readonly int _max;

         public IntMinMaxValidationRule( int min, int max )
         {
            _min = min;
            _max = max;
         }

         public override ValidationResult Validate( object value, CultureInfo cultureInfo )
         {
            var text = ((string)value).Trim();

            bool validValue = int.TryParse( text, out int intValue ) && intValue >= _min && intValue <= _max;

            return validValue ? new ValidationResult( true, null ) : new ValidationResult( false, $"Value must be an integer between {_min} and {_max}" );
         }
      }

      public IntMinMaxBinding( string path, int min, int max ) 
         : base( path )
      {
         ValidationRules.Add( new IntMinMaxValidationRule( min, max ) );
         UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
      }
   }

   public sealed class DoubleMinMaxBinding : Binding
   {
      private sealed class DoubleMinMaxValidationRule : ValidationRule
      {
         private readonly double _min;
         private readonly double _max;

         public DoubleMinMaxValidationRule( double min, double max )
         {
            _min = min;
            _max = max;
         }

         public override ValidationResult Validate( object value, CultureInfo cultureInfo )
         {
            var text = ((string)value).Trim();

            var validValue = double.TryParse( text, out double doubleValue ) && doubleValue >= _min && doubleValue <= _max;

            return validValue ? new ValidationResult( true, null ) : new ValidationResult( false, $"Value must be between {_min} and {_max}" );
         }
      }

      public DoubleMinMaxBinding( string path, double min, double max ) 
         : base( path )
      {
         ValidationRules.Add( new DoubleMinMaxValidationRule( min, max ) );
         UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
      }
   }
}
