using System;
using System.Collections.Generic;
using MVVMUtilities;

namespace ValidationSummaryBeispiel
{
  class ViewModel : ValidationBase
  {
    [System.ComponentModel.DataAnnotations.Range(2,6,ErrorMessage="Wert muss zwischen 2 und 6 liegen")]
    public int Minimum { get; set; }
    public int Maximum { get; set; }

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage="Bitte Namen eingeben")]
    public string Name { get; set; }


    private string jaNein;

    public string JaNein
    {
      get { return jaNein; }
      set
      {
        if (!(value.ToLower() == "ja") && !(value.ToLower() == "nein"))
          throw new ApplicationException("Bitte nur 'Ja' oder 'Nein' eingeben");

        jaNein = value;
      }
    }

    public ActionCommand CheckCommand { get; set; }

    public ViewModel()
    {
      CheckCommand = new ActionCommand(Validate);
      Validators.Add(new Validator() { PropertyName = nameof(Minimum), ErrorMessage = "Minimum muss >= 0 sein", CheckForError = () => Minimum < 0 });
      Validators.Add(new Validator() { PropertyName = nameof(Maximum), ErrorMessage = "Maximum muss > 0 sein", CheckForError = () => Maximum <= 0 });
      Validators.Add(new Validator() { PropertyName = nameof(Maximum), ErrorMessage = "Minimum muss kleiner als Maximum sein", CheckForError = () => Minimum >= Maximum });
      Validators.Add(new Validator() { PropertyName = nameof(Maximum), ErrorMessage = "Maximum muss durch 10 teilbar sein", CheckForError = () => (Maximum % 10) != 0 });
      //Validators.Add(new Validator() { PropertyName = "Name", ErrorMessage = "Name darf nicht leer sein", CheckForError = () => string.IsNullOrEmpty(Name) });
    }




  }
}
