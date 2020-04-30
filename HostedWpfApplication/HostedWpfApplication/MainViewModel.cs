using HostedWpfApplication.Utilities;
using HostedWpfApplication.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace HostedWpfApplication
{
  public class MainViewModel : NotificationObject, IMainController
  {
   
    public string Title { get; set; }

    private readonly IConfiguration configuration;
    private readonly ILogger<MainViewModel> logger;
    private readonly IServiceProvider services;
    private ViewModelBase selectedViewModel;

    public ViewModelBase SelectedViewModel
    {
      get { return selectedViewModel; }
      set
      {
        selectedViewModel = value;

        OnPropertyChanged();
      }
    }

    public IEnumerable<ActionCommand> Commands { get; set; }

    // Bereitstellen von Services über Constructor-Injection
    public MainViewModel(IConfiguration configuration, ILogger<MainViewModel>logger, IServiceProvider services)
    {
      this.configuration = configuration;
      this.logger = logger;
      this.services = services;

      // Laden des Titels aus Konfigurationsdaten
      this.Title = configuration["title"] ?? "nicht konfiguriert";

      logger.LogDebug("Konstruktor MainViewModel");

      Commands = new List<ActionCommand>
       {
         new ActionCommand(ShowContinents){DisplayText="Continents"}
       };

    }

    private void ShowContinents()
    {
      DisplayViewModel<ContinentsViewModel>();
    }

    // IMainController-Implementierung
    public void DisplayViewModel(ViewModelBase viewModel)
    {
      SelectedViewModel = viewModel;
    }

    public void DisplayViewModel<T>() where T : ViewModelBase
    {
      var t = typeof(T);
      var vm = (T)services.GetService(t);
      DisplayViewModel(vm);
    }


  }
}
