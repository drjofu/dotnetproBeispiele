using Microsoft.Xaml.Behaviors;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AnimationLib
{
  public class ShapeAnimationBehavior : Behavior<Shape>
  {
    private Storyboard storyboard;
    private DoubleAnimation animation;

    public ShapeAnimationBehavior()
    {
      storyboard = new Storyboard();
      animation = new DoubleAnimation();
      animation.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(1));
      animation.RepeatBehavior = RepeatBehavior.Forever;
      animation.By = 1;
      Storyboard.SetTargetProperty(animation,new PropertyPath( Shape.StrokeDashOffsetProperty));

      storyboard.Children.Add(animation);
    }
    protected override void OnAttached()
    {
      base.OnAttached();
      RunAnimation();
    }

    private void RunAnimation()
    {
      if (AssociatedObject == null) return;
      animation.SetValue(Storyboard.TargetProperty, AssociatedObject);

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

  }
}
