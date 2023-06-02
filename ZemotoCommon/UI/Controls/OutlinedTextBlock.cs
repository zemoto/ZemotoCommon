#if ZEMOTOUI
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace ZemotoCommon.UI.Controls;

[ContentProperty( "Text" )]
internal sealed class OutlinedTextBlock : FrameworkElement
{
   public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
     nameof( Fill ),
     typeof( Brush ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender ) );
   public Brush Fill
   {
      get => (Brush)GetValue( FillProperty );
      set => SetValue( FillProperty, value );
   }

   public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
     nameof( Stroke ),
     typeof( Brush ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, StrokePropertyChangedCallback ) );
   public Brush Stroke
   {
      get => (Brush)GetValue( StrokeProperty );
      set => SetValue( StrokeProperty, value );
   }

   public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
     nameof( StrokeThickness ),
     typeof( double ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( 1d, FrameworkPropertyMetadataOptions.AffectsRender, StrokePropertyChangedCallback ) );
   public double StrokeThickness
   {
      get => (double)GetValue( StrokeThicknessProperty );
      set => SetValue( StrokeThicknessProperty, value );
   }

   private static void StrokePropertyChangedCallback( DependencyObject d, DependencyPropertyChangedEventArgs args ) => ( d as OutlinedTextBlock )?.UpdatePen();

   public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner( typeof( OutlinedTextBlock ), new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   public FontFamily FontFamily
   {
      get => (FontFamily)GetValue( FontFamilyProperty );
      set => SetValue( FontFamilyProperty, value );
   }

   public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner( typeof( OutlinedTextBlock ), new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   [TypeConverter( typeof( FontSizeConverter ) )]
   public double FontSize
   {
      get => (double)GetValue( FontSizeProperty );
      set => SetValue( FontSizeProperty, value );
   }

   public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner( typeof( OutlinedTextBlock ), new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   public FontStretch FontStretch
   {
      get => (FontStretch)GetValue( FontStretchProperty );
      set => SetValue( FontStretchProperty, value );
   }

   public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner( typeof( OutlinedTextBlock ), new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   public FontStyle FontStyle
   {
      get => (FontStyle)GetValue( FontStyleProperty );
      set => SetValue( FontStyleProperty, value );
   }

   public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner( typeof( OutlinedTextBlock ), new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   public FontWeight FontWeight
   {
      get => (FontWeight)GetValue( FontWeightProperty );
      set => SetValue( FontWeightProperty, value );
   }

   public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
     nameof( Text ),
     typeof( string ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( string.Empty, OnFormattedTextInvalidated ) );
   public string Text
   {
      get => (string)GetValue( TextProperty );
      set => SetValue( TextProperty, value );
   }

   public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
     nameof( TextAlignment ),
     typeof( TextAlignment ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   public TextAlignment TextAlignment
   {
      get => (TextAlignment)GetValue( TextAlignmentProperty );
      set => SetValue( TextAlignmentProperty, value );
   }

   public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(
     nameof( TextDecorations ),
     typeof( TextDecorationCollection ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   public TextDecorationCollection TextDecorations
   {
      get => (TextDecorationCollection)GetValue( TextDecorationsProperty );
      set => SetValue( TextDecorationsProperty, value );
   }

   public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
     nameof( TextTrimming ),
     typeof( TextTrimming ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( OnFormattedTextUpdated ) );
   public TextTrimming TextTrimming
   {
      get => (TextTrimming)GetValue( TextTrimmingProperty );
      set => SetValue( TextTrimmingProperty, value );
   }

   public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
     nameof( TextWrapping ),
     typeof( TextWrapping ),
     typeof( OutlinedTextBlock ),
     new FrameworkPropertyMetadata( TextWrapping.NoWrap, OnFormattedTextUpdated ) );
   public TextWrapping TextWrapping
   {
      get => (TextWrapping)GetValue( TextWrappingProperty );
      set => SetValue( TextWrappingProperty, value );
   }

   private FormattedText _formattedText;
   private Geometry _textGeometry;
   private Pen _pen;

   public OutlinedTextBlock()
   {
      UpdatePen();
      TextDecorations = new TextDecorationCollection();
   }

   private void UpdatePen()
   {
      _pen = new Pen( Stroke, StrokeThickness )
      {
         DashCap = PenLineCap.Round,
         EndLineCap = PenLineCap.Round,
         LineJoin = PenLineJoin.Round,
         StartLineCap = PenLineCap.Round
      };

      InvalidateVisual();
   }

   protected override void OnRender( DrawingContext drawingContext )
   {
      EnsureGeometry();

      drawingContext.DrawGeometry( null, _pen, _textGeometry );
      drawingContext.DrawGeometry( Fill, null, _textGeometry );
   }

   protected override Size MeasureOverride( Size availableSize )
   {
      EnsureFormattedText();

      _formattedText.MaxTextWidth = Math.Min( 3579139, availableSize.Width );
      _formattedText.MaxTextHeight = Math.Max( 0.0001d, availableSize.Height );

      return new Size( Math.Ceiling( _formattedText.Width ), Math.Ceiling( _formattedText.Height ) );
   }

   protected override Size ArrangeOverride( Size finalSize )
   {
      EnsureFormattedText();

      _formattedText.MaxTextWidth = finalSize.Width;
      _formattedText.MaxTextHeight = Math.Max( 0.0001d, finalSize.Height );
      _textGeometry = null;

      return finalSize;
   }

   private static void OnFormattedTextInvalidated( DependencyObject d, DependencyPropertyChangedEventArgs e )
   {
      var outlinedTextBlock = (OutlinedTextBlock)d;
      outlinedTextBlock._formattedText = null;
      outlinedTextBlock._textGeometry = null;

      outlinedTextBlock.InvalidateMeasure();
      outlinedTextBlock.InvalidateVisual();
   }

   private static void OnFormattedTextUpdated( DependencyObject d, DependencyPropertyChangedEventArgs e )
   {
      var outlinedTextBlock = (OutlinedTextBlock)d;
      outlinedTextBlock.UpdateFormattedText();
      outlinedTextBlock._textGeometry = null;

      outlinedTextBlock.InvalidateMeasure();
      outlinedTextBlock.InvalidateVisual();
   }

   private void EnsureFormattedText()
   {
      if ( _formattedText is not null )
      {
         return;
      }

      _formattedText = new FormattedText(
        Text,
        CultureInfo.CurrentUICulture,
        FlowDirection,
        new Typeface( FontFamily, FontStyle, FontWeight, FontStretch ),
        FontSize,
        Brushes.Black,
        VisualTreeHelper.GetDpi( this ).PixelsPerDip );

      UpdateFormattedText();
   }

   private void UpdateFormattedText()
   {
      if ( _formattedText is null )
      {
         return;
      }

      _formattedText.MaxLineCount = TextWrapping is TextWrapping.NoWrap ? 1 : int.MaxValue;
      _formattedText.TextAlignment = TextAlignment;
      _formattedText.Trimming = TextTrimming;

      _formattedText.SetFontSize( FontSize );
      _formattedText.SetFontStyle( FontStyle );
      _formattedText.SetFontWeight( FontWeight );
      _formattedText.SetFontFamily( FontFamily );
      _formattedText.SetFontStretch( FontStretch );
      _formattedText.SetTextDecorations( TextDecorations );
   }

   private void EnsureGeometry()
   {
      if ( _textGeometry is null )
      {
         EnsureFormattedText();
         _textGeometry = _formattedText.BuildGeometry( new Point( 0, 0 ) );
      }
   }
}
#endif