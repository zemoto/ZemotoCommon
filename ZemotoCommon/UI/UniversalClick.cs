#if ZEMOTOUI
using System.Windows;
using System.Windows.Input;

namespace ZemotoCommon.UI
{
   internal static class UniversalClick
   {
      public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent( "Click", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( UniversalClick ) );

      public static void AddClickHandler( DependencyObject d, RoutedEventHandler handler )
      {
         if ( d is UIElement uie )
         {
            uie.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            uie.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            uie.AddHandler( ClickEvent, handler );
         }
      }

      public static void RemoveClickHandler( DependencyObject d, RoutedEventHandler handler )
      {
         if ( d is UIElement uie )
         {
            uie.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            uie.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            uie.RemoveHandler( ClickEvent, handler );
         }
      }

      private static void OnClick( UIElement uiElement ) => uiElement.RaiseEvent( new RoutedEventArgs( ClickEvent, uiElement ) );

      private static void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
      {
         if ( sender is not UIElement element )
         {
            return;
         }

         if ( e.ButtonState == MouseButtonState.Pressed )
         {
            element.CaptureMouse();
         }
      }

      private static void OnMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
      {
         if ( sender is not UIElement element )
         {
            return;
         }

         if ( element.IsMouseCaptured )
         {
            element.ReleaseMouseCapture();
            if ( element.IsMouseOver )
            {
               OnClick( element );
            }
         }
      }
   }
}
#endif