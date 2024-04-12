#if ZEMOTOUI
using System.Windows;
using System.Windows.Controls;

namespace ZemotoCommon.UI.Controls;

internal sealed class RatioCanvas : Panel
{
   public static readonly DependencyProperty XRatioProperty = DependencyProperty.RegisterAttached(
     "XRatio", typeof( double ), typeof( RatioCanvas ), new FrameworkPropertyMetadata( double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange ) );

   public static double GetXRatio( UIElement target ) => (double)target.GetValue( XRatioProperty );
   public static void SetXRatio( UIElement target, double value ) => target.SetValue( XRatioProperty, value );

   public static readonly DependencyProperty YRatioProperty = DependencyProperty.RegisterAttached(
     "YRatio", typeof( double ), typeof( RatioCanvas ), new FrameworkPropertyMetadata( double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange ) );

   public static double GetYRatio( UIElement target ) => (double)target.GetValue( YRatioProperty );
   public static void SetYRatio( UIElement target, double value ) => target.SetValue( YRatioProperty, value );

   protected override Size MeasureOverride( Size availableSize ) => availableSize.Width == double.PositiveInfinity || availableSize.Height == double.PositiveInfinity
      ? base.MeasureOverride( availableSize )
      : MeasureAgainstAvailableSpace( availableSize );

   private Size MeasureAgainstAvailableSpace( Size availableSize )
   {
      foreach ( UIElement child in InternalChildren )
      {
         child.Measure( availableSize );
      }

      return availableSize;
   }

   protected override Size ArrangeOverride( Size finalSize )
   {
      foreach ( UIElement child in InternalChildren )
      {
         var childXRatio = GetXRatio( child );
         var childYRatio = GetYRatio( child );

         var childTopLeft = new Point( 0, 0 );
         if ( !double.IsNaN( childXRatio ) )
         {
            childTopLeft.X = childXRatio * finalSize.Width;
         }

         if ( !double.IsNaN( childYRatio ) )
         {
            childTopLeft.Y = childYRatio * finalSize.Height;
         }

         child.Arrange( new Rect( childTopLeft, child.DesiredSize ) );
      }

      return finalSize;
   }
}
#endif