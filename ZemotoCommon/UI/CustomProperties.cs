#if ZEMOTOUI
using System.Windows;
using System.Windows.Media;

namespace ZemotoCommon.UI;

internal static class CustomProperties
{
   public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.RegisterAttached( "MouseOverBrush", typeof( SolidColorBrush ), typeof( CustomProperties ), new FrameworkPropertyMetadata( null ) );
   public static SolidColorBrush GetMouseOverBrush( UIElement target ) => (SolidColorBrush)target.GetValue( MouseOverBrushProperty );
   public static void SetMouseOverBrush( UIElement target, SolidColorBrush value ) => target.SetValue( MouseOverBrushProperty, value );

   public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.RegisterAttached( "PressedBrush", typeof( SolidColorBrush ), typeof( CustomProperties ), new FrameworkPropertyMetadata( null ) );
   public static SolidColorBrush GetPressedBrush( UIElement target ) => (SolidColorBrush)target.GetValue( PressedBrushProperty );
   public static void SetPressedBrush( UIElement target, SolidColorBrush value ) => target.SetValue( PressedBrushProperty, value );
}
#endif