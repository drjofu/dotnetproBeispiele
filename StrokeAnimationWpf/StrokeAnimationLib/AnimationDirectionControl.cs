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

    public static readonly DependencyProperty DirectionProperty =
        DependencyProperty.Register("Direction", typeof(AnimationDirection), typeof(AnimationDirectionControl), new PropertyMetadata(AnimationDirection.Stopped));

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var container = Template.FindName("PART_Buttons", this) as Panel;
      // Eventhandler für alle RadioButtons binden
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
