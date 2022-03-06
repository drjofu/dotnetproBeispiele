using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUI3MVVM.Utilities;

/// <summary>
/// Some helpfull extensions
/// </summary>
public static class ExtensionMethods
{
  /// <summary>
  /// Get the DI service provider
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static IServiceProvider GetServiceProvider(this DependencyObject obj)
  {
    return ((App)Application.Current).Services;
  }

  /// <summary>
  /// Setup a ViewModel
  /// </summary>
  /// <typeparam name="TViewModel"></typeparam>
  /// <param name="page">the view</param>
  /// <returns>Initialized ViewModel</returns>
  public static TViewModel SetupViewModel<TViewModel>(this Page page) where TViewModel : ViewModelBase
  {
    var vm = ((App)Application.Current).Services.GetService<TViewModel>();
    page.Loaded += vm.PageLoaded;
    page.Unloaded += vm.PageUnloaded;
    page.DataContext = vm;
    return vm;
  }

  /// <summary>
  /// Get the shell
  /// </summary>
  /// <param name="obj">any kind of object</param>
  /// <returns></returns>
  public static IShell GetShell(this object obj)
  {
    return ((App)Application.Current).Services.GetService<IShell>();
  }

}
