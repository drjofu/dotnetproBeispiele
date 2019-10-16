using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimationLib
{

 

  /// <summary>
  /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
  ///
  /// Step 1a) Using this custom control in a XAML file that exists in the current project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:AnimationLib"
  ///
  ///
  /// Step 1b) Using this custom control in a XAML file that exists in a different project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:AnimationLib;assembly=AnimationLib"
  ///
  /// You will also need to add a project reference from the project where the XAML file lives
  /// to this project and Rebuild to avoid compilation errors:
  ///
  ///     Right click on the target project in the Solution Explorer and
  ///     "Add Reference"->"Projects"->[Select this project]
  ///
  ///
  /// Step 2)
  /// Go ahead and use your control in the XAML file.
  ///
  ///     <MyNamespace:CustomControl1/>
  ///
  /// </summary>
  public class ShapeAnimation : Control
  {
    static ShapeAnimation()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(ShapeAnimation), new FrameworkPropertyMetadata(typeof(ShapeAnimation)));
    }

    private ContentControl shapeContainer;
    private Storyboard storyboard;
    private DoubleAnimation animation;

    public ShapeAnimation()
    {
      storyboard = new Storyboard();
      animation = new DoubleAnimation();
      animation.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(1));
      animation.RepeatBehavior = RepeatBehavior.Forever;
      animation.By = 1;
      Storyboard.SetTargetProperty(animation, new PropertyPath(Shape.StrokeDashOffsetProperty));

      storyboard.Children.Add(animation);

    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      shapeContainer = (ContentControl)Template.FindName("PART_ShapeContainer", this);

      RunAnimation();
    }

    private void RunAnimation()
    {
      if (shapeContainer == null || Shape == null) return;
      shapeContainer.Content = Shape;
      animation.SetValue(Storyboard.TargetProperty, Shape);

      var dashLength = Shape.StrokeDashArray.Sum() * (Shape.StrokeDashArray.Count % 2 + 1);
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
        DependencyProperty.Register("AnimationDirection", typeof(AnimationDirection), typeof(ShapeAnimation), new FrameworkPropertyMetadata(AnimationDirection.Stopped,  OnAnimationDirectionChanged));

    private static void OnAnimationDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ShapeAnimation)d).RunAnimation();
    }

    public Shape Shape
    {
      get { return (Shape)GetValue(ShapeProperty); }
      set { SetValue(ShapeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Shape.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShapeProperty =
        DependencyProperty.Register("Shape", typeof(Shape), typeof(ShapeAnimation), new FrameworkPropertyMetadata(OnShapeChanged));

    private static void OnShapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ShapeAnimation)d).RunAnimation();

    }
  }
}
