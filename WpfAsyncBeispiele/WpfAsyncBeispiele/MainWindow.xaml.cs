using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAsyncBeispiele
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

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      BTN.IsEnabled = false;
      await Task.Run(Inkrementieren);
      BTN.IsEnabled = true;
    }

    private void Inkrementieren()
    {
      for (int i = 0; i <= 10; i++)
      {
        Thread.Sleep(1000);
        Dispatcher.BeginInvoke(new Action<int>(k => LBL.Content = k), i);
      }
    }
  }
}
