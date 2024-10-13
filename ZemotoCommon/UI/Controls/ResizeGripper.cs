#if ZEMOTOUI
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZemotoCommon.UI.Controls;

[Flags]
internal enum ResizeDirection
{
   None = 0,
   Vertical = 1,
   Horizontal = 2,
   Both = Vertical | Horizontal,
}

internal sealed class ResizeGripper : Control
{
   public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
     nameof( Direction ),
     typeof( ResizeDirection ),
     typeof( ResizeGripper ),
     new FrameworkPropertyMetadata( ResizeDirection.Both, OnDirectionChanged ) );
   private static void OnDirectionChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
   {
      var resizeGripper = (ResizeGripper)d;
      resizeGripper.Cursor = e.NewValue switch
      {
         ResizeDirection.Horizontal => Cursors.SizeWE,
         ResizeDirection.Vertical => Cursors.SizeNS,
         ResizeDirection.Both => Cursors.SizeNESW,
         _ => null
      };
   }
   public ResizeDirection Direction
   {
      get => (ResizeDirection)GetValue( DirectionProperty );
      set => SetValue( DirectionProperty, value );
   }

   public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
     nameof( Target ),
     typeof( FrameworkElement ),
     typeof( ResizeGripper ),
     new FrameworkPropertyMetadata( null ) );
   public FrameworkElement Target
   {
      get => (FrameworkElement)GetValue( TargetProperty );
      set => SetValue( TargetProperty, value );
   }

   private Point _initialMousePosition;
   private Size _initialTargetSize;
   private Size _minSize;

   protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e )
   {
      if ( Target is null ||
         ( Direction.HasFlag( ResizeDirection.Horizontal ) && double.IsNaN( Target.Width ) ) ||
         ( Direction.HasFlag( ResizeDirection.Vertical ) && double.IsNaN( Target.Height ) ) )
      {
         return;
      }

      if ( CaptureMouse() )
      {
         _initialMousePosition = Mouse.GetPosition( Target );
         _initialTargetSize = new Size( Target.Width, Target.Height );
         _minSize = new Size( Target.MinWidth, Target.MinHeight );

         if ( double.IsNaN( _minSize.Width ) )
         {
            _minSize.Width = 0;
         }

         if ( double.IsNaN( _minSize.Height ) )
         {
            _minSize.Height = 0;
         }
      }

      base.OnMouseLeftButtonDown( e );
   }

   protected override void OnMouseLeftButtonUp( MouseButtonEventArgs e )
   {
      if ( IsMouseCaptured )
      {
         ReleaseMouseCapture();
      }

      base.OnMouseLeftButtonUp( e );
   }

   protected override void OnMouseMove( MouseEventArgs e )
   {
      if ( IsMouseCaptured )
      {
         var newPosition = Mouse.GetPosition( Target );
         var delta = newPosition - _initialMousePosition;

         if ( Direction.HasFlag( ResizeDirection.Horizontal ) )
         {
            Target.Width = Math.Max( _initialTargetSize.Width + delta.X, _minSize.Width );
         }

         if ( Direction.HasFlag( ResizeDirection.Vertical ) )
         {
            Target.Height = Math.Max( _initialTargetSize.Height + delta.Y, _minSize.Height );
         }
      }

      base.OnMouseMove( e );
   }
}
#endif