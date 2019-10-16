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

namespace WpfApp8
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var sb = (Storyboard)FindResource("BorderAnimation");
      sb.Stop();

      Ellipse1.AnimationDirection = AnimationLib.AnimationDirection.Stopped;

    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      var sb = (Storyboard)FindResource("BorderAnimation");
      ((DoubleAnimation)sb.Children[0]).From = 20;
      ((DoubleAnimation)sb.Children[0]).To = 0;
      sb.Begin();

      sb = (Storyboard)FindResource("Storyboard1");
      sb.Begin();

      Ellipse1.AnimationDirection = AnimationLib.AnimationDirection.Forward;
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      var sb = (Storyboard)FindResource("BorderAnimation");
      ((DoubleAnimation)sb.Children[0]).From = 0;
      ((DoubleAnimation)sb.Children[0]).To = 20;
      sb.Begin();

      Ellipse1.AnimationDirection = AnimationLib.AnimationDirection.Reverse;

    }
  }
}
