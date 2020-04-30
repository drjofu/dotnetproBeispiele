using HostedWpfApplication.Models;
using HostedWpfApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using WorldLib;

namespace HostedWpfApplication.ViewModels
{
  public class CountriesViewModel : ViewModelBase
  {
    private readonly World world;
    private readonly WorldState worldState;

    private IEnumerable<Country> countries;

    public IEnumerable<Country> Countries
    {
      get { return countries; }
      set { countries = value; OnPropertyChanged(); }
    }


    public CountriesViewModel(World world, WorldState worldState)
    {
      Title = "Countries";
      this.world = world;
      this.worldState = worldState;
      GetCountries();
      this.worldState.PropertyChanged += WorldState_PropertyChanged;
    }

    private void WorldState_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      GetCountries();
    }

    private void GetCountries()
    {
      Countries = world.GetCountriesOnContinent(worldState.SelectedContinent.ShortName);
    }




  }
}
