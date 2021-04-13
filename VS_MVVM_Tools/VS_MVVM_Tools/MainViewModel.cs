using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VS_MVVM_Tools;

namespace VS_MVVM_Tools
{
  class MainViewModel : ViewModelBase
  {

    private string vorname;

    public string Vorname { get => vorname; set => SetProperty(ref vorname, value); }

    private string nachname;

    public string Nachname { get => nachname; set => SetProperty(ref nachname, value); }

    private ICommand openCommand;
    public ICommand OpenCommand => openCommand ??= new ActionCommand(Open);

    private void Open()
    {
    }

    private IEnumerable<ListItem> liste;

    public IEnumerable<ListItem> Liste { get => liste; set => SetProperty(ref liste, value); }

    private object okButtonText;

    public object OkButtonText { get => okButtonText; set => SetProperty(ref okButtonText, value); }

    private ICommand okCommand;
    public ICommand OkCommand => okCommand ??= new ActionCommand(Ok);

    private void Ok()
    {
    }
  }
}
