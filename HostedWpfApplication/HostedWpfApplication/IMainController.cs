using HostedWpfApplication.Utilities;

namespace HostedWpfApplication
{
  public interface IMainController
  {
    void DisplayViewModel(ViewModelBase viewModel);
    void DisplayViewModel<T>() where T : ViewModelBase;
  }
}