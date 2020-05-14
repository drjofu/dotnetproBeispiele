using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace MVVMUtilities
{
  /// <summary>
  /// Hilfsklasse für INotifyDataErrorInfo-Implementierung
  /// </summary>
  public class ValidationBase : NotificationObject, INotifyDataErrorInfo
  {
    /// <summary>
    /// Liste der Validator-Objekte
    /// </summary>
    public List<Validator> Validators { get; private set; }

    /// <summary>
    /// ctor
    /// </summary>
    public ValidationBase()
    {
      Validators = new List<Validator>();

      var propsToValidate = this.GetType().GetProperties().Where(p => p.DeclaringType != typeof(ValidationBase));
      foreach (var prop in propsToValidate)
      {
        var attrs = prop.GetCustomAttributes(typeof(ValidationAttribute));
        foreach (ValidationAttribute attr in attrs)
          Validators.Add(new AttributeValidator(this, prop.Name, attr));

      }

    }

    /// <summary>
    /// Validierung durchführen
    /// </summary>
    public void Validate()
    {
      // Liste der Validator-Objekte durchlaufen
      foreach (var validator in Validators)
      {
        // liegt ein Fehler vor?
        bool hasError = validator.CheckForError();

        // Wenn sich der Zustand geändert hat, ErrorsChanged feuern
        if (hasError != validator.LastStatus)
        {
          validator.LastStatus = hasError;
          if (ErrorsChanged != null)
            ErrorsChanged(this, new DataErrorsChangedEventArgs(validator.PropertyName));
        }

      }


    }

    #region INotifyDataErrorInfo-Implementierung

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    /// <summary>
    /// Liste der Fehlermeldungen für die angegebene Eigenschaft ermitteln
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public System.Collections.IEnumerable GetErrors(string propertyName)
    {
      var errors = Validators.Where(v => v.PropertyName == propertyName && v.LastStatus).Select(v => v.ErrorMessage);
      return errors;
    }

    /// <summary>
    /// Prüfen, ob Fehler vorliegen
    /// </summary>
    public bool HasErrors
    {
      get { return !Validators.TrueForAll(v => !v.LastStatus); }
    }

    #endregion
  }

  /// <summary>
  /// Hilfsklasse für Validierungen
  /// </summary>
  public class Validator
  {
    /// <summary>
    /// Name der Eigenschaft
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// Methode, die im Fehlerfall true zurück gibt
    /// </summary>
    public Func<bool> CheckForError { get; set; }

    /// <summary>
    /// Die Fehlermeldung
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Letzter Zustand. True bei Fehler
    /// </summary>
    public bool LastStatus { get; set; }
  }

  public class AttributeValidator : Validator
  {
    public ValidationAttribute ValidationAttribute { get; set; }

    public AttributeValidator(object objectToCheck, string propertyName, ValidationAttribute validationAttribute)
    {
      this.ValidationAttribute = validationAttribute;
      this.ErrorMessage = validationAttribute.ErrorMessage;
      this.CheckForError = () => !ValidationAttribute.IsValid(
        objectToCheck.GetType()
        .GetProperty(propertyName)
        .GetValue(objectToCheck));
      this.PropertyName = propertyName;
    }
  }
}
