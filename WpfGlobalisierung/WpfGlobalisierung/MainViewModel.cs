using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfGlobalisierung.MVVMUtilities;

namespace WpfGlobalisierung
{
  public class MainViewModel : ViewModelBase
  {
    public ActionCommand InfosAnzeigenCommand{get;set;}

    private DateTime aktuellesDatum;

    public DateTime AktuellesDatum
    {
      get { return aktuellesDatum; }
      set { aktuellesDatum = value; OnPropertyChanged(); }
    }

    private string datumAlsText;

    public string DatumAlsText
    {
      get { return datumAlsText; }
      set { datumAlsText = value; OnPropertyChanged(); }
    }

    private string begrüßungAusVM;

    public string BegrüßungAusVM
    {
      get { return begrüßungAusVM; }
      set { begrüßungAusVM = value; OnPropertyChanged(); }
    }
    public MainViewModel()
    {
      InfosAnzeigenCommand = new ActionCommand(InfosAnzeigen);
    }

    private void InfosAnzeigen()
    {
      AktuellesDatum = DateTime.Now;
      EigenschaftenAktualisieren();
    }
    protected override void OnCurrentCultureChanged()
    {
      EigenschaftenAktualisieren();
    }

    private void EigenschaftenAktualisieren()
    {
      DatumAlsText = aktuellesDatum.ToString("G");
      BegrüßungAusVM = CurrentCulture["BegrüßungAusVM"];
    }

  }
}
