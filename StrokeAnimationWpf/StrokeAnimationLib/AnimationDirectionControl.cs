using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StrokeAnimationLib
{
  /// <summary>
  /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
  ///
  /// Step 1a) Using this custom control in a XAML file that exists in the current project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:StrokeAnimationLib"
  ///
  ///
  /// Step 1b) Using this custom control in a XAML file that exists in a different project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:StrokeAnimationLib;assembly=StrokeAnimationLib"
  ///
  /// You will also need to add a project reference from the project where the XAML file lives
  /// to this project and Rebuild to avoid compilation errors:
  ///
  ///     Right click on the target project in the Solution Explorer and
  ///     "Add Reference"->"Projects"->[Browse to and select this project]
  ///
  ///
  /// Step 2)
  /// Go ahead and use your control in the XAML file.
  ///
  ///     <MyNamespace:AnimationDirectionControl/>
  ///
  /// </summary>
  public class AnimationDirectionControl : Control
  {
    static AnimationDirectionControl()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationDirectionControl), new FrameworkPropertyMetadata(typeof(AnimationDirectionControl)));
    }



    public AnimationDirection Direction
    {
      get { return (AnimationDirection)GetValue(DirectionProperty); }
      set { SetValue(DirectionProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DirectionProperty =
        DependencyProperty.Register("Direction", typeof(AnimationDirection), typeof(AnimationDirectionControl), new PropertyMetadata(AnimationDirection.Stopped));


    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var container = Template.FindName("PART_Buttons", this) as Panel;
      foreach (RadioButton rb in container.Children)
      {
        rb.Checked += Rb_Checked;
      }
      Console.WriteLine();
    }

    private void Rb_Checked(object sender, RoutedEventArgs e)
    {
      var rb = sender as RadioButton;
      this.Direction = (AnimationDirection)rb.Tag;
    }
  }
}
