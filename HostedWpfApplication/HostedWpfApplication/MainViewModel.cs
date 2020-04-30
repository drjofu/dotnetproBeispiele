using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace HostedWpfApplication
{
  public class MainViewModel : INotifyPropertyChanged
  {
    private readonly IConfiguration configuration;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string Title { get; set; }

    public MainViewModel(IConfiguration configuration)
    {
      this.configuration = configuration;

      this.Title = configuration["title"] ?? "nicht konfiguriert";
    }

  }
}
