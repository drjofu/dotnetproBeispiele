using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfGlobalisierung.GlobalizationUtilities
{
  public class SupportedCulture : SelectableItem
  {
    // Alle Informationen zur Kultur
    public CultureInfo CultureInfo { get; set; }

    // Ein Symbol für die grafische Darstellung
    public ImageSource Flag { get; set; }

    // Wert für die Spracheinstellung im XAML-Code
    public XmlLanguage XmlLanguage { get; set; }

    // Textausrichtung
    public FlowDirection FlowDirection { get; set; }


    // Indexer für den Zugriff auf die Texte über eine Id
    public string this[string id]
    {
      get
      {
        return GlobalizationRepository.GetString(CultureInfo.Name,id);
      }
    }

    // Konstruktor zur Initialien Einrichtung der Eigenschaften auf Basis der vorgegebenen Kultur
    public SupportedCulture(string cultureId, EventHandler isSelectedChangedEventHandler)
    {
      CultureInfo = CultureInfo.GetCultureInfo(cultureId);
      Flag = (ImageSource)App.Current.FindResource(cultureId);
      XmlLanguage = XmlLanguage.GetLanguage(CultureInfo.Name);
      FlowDirection = CultureInfo.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
      IsSelectedChanged += isSelectedChangedEventHandler;
    }
  }
}
