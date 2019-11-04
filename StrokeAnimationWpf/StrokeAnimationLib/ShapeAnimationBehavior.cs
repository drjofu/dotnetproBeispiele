using Microsoft.Xaml.Behaviors;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace StrokeAnimationLib
{


  public class ShapeAnimationBehavior : Behavior<Shape>
  {
    private Storyboard storyboard;
    private DoubleAnimation animation;

    public ShapeAnimationBehavior()
    {
      // Storyboard für Animationen einrichten
      storyboard = new Storyboard();
      animation = new DoubleAnimation();
      animation.Duration = new Duration(TimeSpan.FromSeconds(1));
      animation.RepeatBehavior = RepeatBehavior.Forever;
      animation.By = 1;
      Storyboard.SetTargetProperty(animation, new PropertyPath(Shape.StrokeDashOffsetProperty));

      storyboard.Children.Add(animation);
    }
    protected override void OnAttached()
    {
      base.OnAttached();
      RunAnimation();  // Animation starten, sobald das Behavior einem Control zugeordnet wurde
    }

    private void RunAnimation()
    {
      if (AssociatedObject == null) return;

      // Zielelement für die Animation im Storyboard festlegen
      animation.SetValue(Storyboard.TargetProperty, AssociatedObject);

      // Strichlänge berechnen
      var dashLength = AssociatedObject.StrokeDashArray.Sum() * (AssociatedObject.StrokeDashArray.Count % 2 + 1);
 
      switch (AnimationDirection)
      {
        case AnimationDirection.Stopped:
          storyboard.Stop();
          break;

        case AnimationDirection.Forward:
          animation.From = dashLength;
          animation.To = 0;
          storyboard.Begin();
          break;

        case AnimationDirection.Reverse:
          animation.From = 0;
          animation.To = dashLength;
          storyboard.Begin();
          break;
      }

    }

    public AnimationDirection AnimationDirection
    {
      get { return (AnimationDirection)GetValue(AnimationDirectionProperty); }
      set { SetValue(AnimationDirectionProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AnimationDirection.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AnimationDirectionProperty =
        DependencyProperty.Register("AnimationDirection", typeof(AnimationDirection), typeof(ShapeAnimationBehavior), new FrameworkPropertyMetadata(AnimationDirection.Stopped, OnAnimationDirectionChanged));

    private static void OnAnimationDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ShapeAnimationBehavior)d).RunAnimation();
    }


    public double AnimationSpeed
    {
      get { return (double)GetValue(AnimationSpeedProperty); }
      set { SetValue(AnimationSpeedProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AnimationSpeed.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AnimationSpeedProperty =
        DependencyProperty.Register("AnimationSpeed", typeof(double), typeof(ShapeAnimationBehavior), new FrameworkPropertyMetadata(1.0, OnAnimationSpeedChanged));

    private static void OnAnimationSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ShapeAnimationBehavior behavior = d as ShapeAnimationBehavior;

      // Animationsgeschwindigkeit setzen
      double value = (double)e.NewValue;
      if (value <= 0 || value > 10) throw new ApplicationException("animation speed must be >0 and <=10");
      behavior.animation.Duration =TimeSpan.FromSeconds( 1 / value);

      // Animation mit neuen Werten neu starten
      behavior.storyboard.Stop();
      behavior.storyboard.Begin();
    }
  }
}
