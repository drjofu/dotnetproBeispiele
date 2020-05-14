using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;
using System.Windows.Documents;

namespace MVVMUtilities
{
  /// <summary>
  /// Control zur Anzeige von Validierungsmeldungen
  /// </summary>
  [TemplatePart(Name = "PART_ErrorList", Type = typeof(ItemsControl))]
  public class ValidationSummary : Control
  {
    // Das Control zur Anzeige der Meldungen
    private ItemsControl errorList;

    /// <summary>
    /// Liste der aktuell anstehenden Meldungen
    /// </summary>
    public ObservableCollection<ActionValueCommand<ValidationError>> ActiveErrors { get; set; }

    // Einrichten der Control-Klasse
    static ValidationSummary()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(ValidationSummary), new FrameworkPropertyMetadata(typeof(ValidationSummary)));
      WidthProperty.OverrideMetadata(typeof(ValidationSummary), new FrameworkPropertyMetadata(300.0));
    }

    /// <summary>
    /// Einrichten des templatebasierten Teils
    /// </summary>
    public override void OnApplyTemplate()
    {
      // Das gültige Template laden
      errorList = GetTemplateChild<ItemsControl>("PART_ErrorList");

      // Fehlerliste einrichten und Listencontrol daran binden
      ActiveErrors = new ObservableCollection<ActionValueCommand<ValidationError>>();
      var binding = new Binding("ActiveErrors") { Source = this };
      errorList.SetBinding(ItemsControl.ItemsSourceProperty, binding);

    }

    /// <summary>
    /// Gültigkeitsbereich, der für Fehlermeldungen erfasst werden soll
    /// </summary>
    public FrameworkElement ValidationScope
    {
      get { return (FrameworkElement)GetValue(ValidationScopeProperty); }
      set { SetValue(ValidationScopeProperty, value); }
    }

    /// <summary>
    /// DepProp für ValidationScope
    /// </summary>
    public static readonly DependencyProperty ValidationScopeProperty =
        DependencyProperty.Register("ValidationScope", typeof(FrameworkElement), typeof(ValidationSummary), 
        new FrameworkPropertyMetadata(OnValidationScopeChanged));


    private static void OnValidationScopeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      // Das auslösende Control
      ValidationSummary ctrl = d as ValidationSummary;

      // ggfs. alten Handler entfernen
      if (e.OldValue != null)
        Validation.RemoveErrorHandler((DependencyObject)e.OldValue, ctrl.OnValidationError);

      if (e.NewValue != null)
      {
        // Control zunächst nicht darstellen
        ctrl.Visibility = Visibility.Collapsed;

        // Handler an Validation-Error-Event anhängen
        Validation.AddErrorHandler((DependencyObject)e.NewValue, ctrl.OnValidationError);
      }
    }

    // Wird aufgerufen, wenn eine Meldung gegangen oder hinzugekommen ist
    private void OnValidationError(object sender, ValidationErrorEventArgs e)
    {
      // Meldung in Liste aufnehmen bzw. wieder löschen
      if (e.Action == ValidationErrorEventAction.Added)
        ActiveErrors.Add(new ActionValueCommand<ValidationError>(MoveFocus, e.Error));
      else
        ActiveErrors.Remove(ActiveErrors.First(er => er.Value == e.Error));

      // Control soll nur sichtbar sein, wenn Fehler vorliegen
      if (ActiveErrors.Count > 0)
        this.Visibility = System.Windows.Visibility.Visible;
      else
        this.Visibility = System.Windows.Visibility.Collapsed;
    }


    private void MoveFocus(ValidationError validationError)
    {
      // Aus der Bindung das betreffende Control ermitteln und den Fokus setzen
      var ctrlWithError = ((BindingExpression)validationError.BindingInError).Target as IInputElement;
      if (ctrlWithError != null)
      {
        ctrlWithError.Focus();
      }

    }



    private T GetTemplateChild<T>(string name) where T : class
    {
      // Template-Part suchen
      object obj = GetTemplateChild(name);

      if (obj == null)
        return null; // nicht vorhanden

      if (obj is T)
        return (T)obj; // Typ ist korrekt

      // Vorhanden, aber falscher Datentyp
      throw new ApplicationException("Falscher Typ für Template-Part! Erwartet: " + typeof(T).Name + ", gefunden: " + obj.GetType().Name);
    }

  }
}
