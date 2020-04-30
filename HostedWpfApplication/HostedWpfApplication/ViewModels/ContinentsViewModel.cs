using HostedWpfApplication.Models;
using HostedWpfApplication.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WorldLib;

namespace HostedWpfApplication.ViewModels
{
  public class ContinentsViewModel : ViewModelBase
  {
    private readonly World world;
    private readonly IMainController mainController;
    private readonly WorldState worldState;
    private readonly ILogger<ContinentsViewModel> logger;

    public ActionCommand ShowCountriesCommand { get; set; }

    public IEnumerable<Continent> Continents { get; set; }

    public WorldState WorldState => worldState;

    public ContinentsViewModel(World world, IMainController mainController, WorldState worldState, ILogger<ContinentsViewModel> logger)
    {
      logger.LogDebug("CVM angelegt");

      Title = "Continents";
      this.world = world;
      this.mainController = mainController;
      this.worldState = worldState;
      this.logger = logger;
      this.Continents = world.GetContinents();

      ShowCountriesCommand = new ActionCommand(ShowCountries);
    }

    private void ShowCountries()
    {
      mainController.DisplayViewModel<CountriesViewModel>();
    }
  }
}
