#if ZEMOTOUI
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ZemotoCommon.UI.Controls;

[TemplatePart( Name = "PART_ContentContainer", Type = typeof( Panel ) )]
[TemplatePart( Name = "PART_Content", Type = typeof( FrameworkElement ) )]
internal sealed class AnimatedGroupedExpander : Expander
{
   private static readonly Dictionary<string, List<AnimatedGroupedExpander>> ExpanderGroups = new();
   private static readonly TimeSpan _animationDuration = TimeSpan.FromMilliseconds( 200 );

   public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register( nameof( GroupName ), typeof( string ), typeof( AnimatedGroupedExpander ), new PropertyMetadata( string.Empty, OnGroupNameChanged ) );
   private static void OnGroupNameChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
   {
      if ( sender is not AnimatedGroupedExpander expander )
      {
         return;
      }

      if ( e.OldValue is string oldGroupName && !string.IsNullOrEmpty( oldGroupName ) )
      {
         var group = ExpanderGroups[oldGroupName];
         group.Remove( expander );

         if ( group.Count == 0 )
         {
            ExpanderGroups.Remove( oldGroupName );
         }
      }

      if ( e.NewValue is string newGroupName && !string.IsNullOrEmpty( newGroupName ) )
      {
         if ( !ExpanderGroups.TryGetValue( newGroupName, out List<AnimatedGroupedExpander> value ) )
         {
            ExpanderGroups[newGroupName] = value = new List<AnimatedGroupedExpander>();
         }

         value.Add( expander );
      }
   }

   public string GroupName
   {
      get => (string)GetValue( GroupNameProperty );
      set => SetValue( GroupNameProperty, value );
   }

   private Panel _contentContainer;
   private FrameworkElement _contentControl;

   public AnimatedGroupedExpander()
   {
      Expanded += OnExpanded;
      Collapsed += OnCollapsed;
      IsKeyboardFocusWithinChanged += OnKeyboardFocusWithinChanged;
   }

   private void OnExpanded( object sender, RoutedEventArgs e )
   {
      if ( !string.IsNullOrEmpty( GroupName ) )
      {
         foreach ( var expander in ExpanderGroups[GroupName].Where( expander => expander != this ) )
         {
            expander.IsExpanded = false;
         }
      }

      Animate();
   }

   private void OnCollapsed( object sender, RoutedEventArgs e ) => Animate();

   private void OnKeyboardFocusWithinChanged( object sender, DependencyPropertyChangedEventArgs e )
   {
      if ( IsKeyboardFocusWithin && !IsExpanded )
      {
         IsExpanded = true;
      }
   }

   public override void OnApplyTemplate()
   {
      base.OnApplyTemplate();
      if ( _contentControl is not null )
      {
         _contentControl.SizeChanged -= OnContentControlSizeChanged;
      }
      if ( Template is null )
      {
         return;
      }

      _contentContainer = Template.FindName( "PART_ContentContainer", this ) as Panel;
      _contentControl = Template.FindName( "PART_Content", this ) as FrameworkElement;
      if ( _contentContainer is not null && _contentControl is not null )
      {
         _contentContainer.Height = IsExpanded ? _contentControl.ActualHeight : 0;
         _contentControl.SizeChanged += OnContentControlSizeChanged;
      }
   }

   private void OnContentControlSizeChanged( object sender, SizeChangedEventArgs e )
   {
      if ( IsExpanded )
      {
         // Setting the height directly does not work here for some reason, so use a zero duration animation
         _contentContainer.BeginAnimation( HeightProperty, new DoubleAnimation( _contentControl.ActualHeight, TimeSpan.Zero ) );
      }
   }

   private void Animate()
   {
      if ( _contentContainer is not null && _contentControl is not null )
      {
         var animation = new DoubleAnimation( _contentContainer.Height, IsExpanded ? _contentControl.ActualHeight : 0, _animationDuration );
         _contentContainer.BeginAnimation( HeightProperty, animation );
      }
   }
}
#endif