using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionBeispiel
{
  public class ServiceLocator
  {
    // Key: registrierter Typ, Value: Factory-Function zum Anlegen eines Objekts
    private Dictionary<Type, Func<object>> container = new();

    // Liste der bereits instanzierten Singletons
    private Dictionary<Type, object> singletons = new();

    // Singleton-Konstruktion der ServiceLocator-Klasse
    public static readonly ServiceLocator Current = new ServiceLocator();
    private ServiceLocator() { }

    #region Methoden zum Aufbauen des DI-Containers

    /// <summary>
    /// Hinzufügen eines vorhandenen Objekts als Singleton
    /// </summary>
    /// <param name="obj">Das bereits instanzierte Objekt. Der Typ wird als Schlüssel verwendet</param>
    public void AddSingleton(object obj)
    {
      container.Add(obj.GetType(), () => obj);
    }

    /// <summary>
    /// Hinzufügen eines vorhandenen Objekts als Singleton
    /// </summary>
    /// <typeparam name="TKey">Typ, für den das Objekt später abgerufen werden soll.</typeparam>
    /// <param name="obj">Das zu registrierende Objekt. Muss TKey implementieren</param>
    public void AddSingleton<TKey>(object obj)
    {
      // hier sollte noch geprüft werden, ob obj wirklich TKey implementiert
      container.Add(typeof(TKey), () => obj);
    }


    /// <summary>
    /// Registrieren eines Typs als Singleton
    /// </summary>
    /// <typeparam name="TValue">Der zu registrierende Typ. Dieser wird auch als Key verwendet</typeparam>
    public void AddSingleton<TValue>()
    {
      AddSingleton<TValue, TValue>();
    }

    /// <summary>
    /// Registrieren eines Typs als Singleton
    /// </summary>
    /// <typeparam name="TKey">Schlüsseltyp für den späteren Abruf</typeparam>
    /// <typeparam name="TValue">Zu instanzierender Typ (muss TKey implementieren)</typeparam>
    public void AddSingleton<TKey, TValue>() where TValue : TKey
    {
      var tKey = typeof(TKey);
      container.Add(tKey, () =>
      {
        // Objekt schon vorhanden?
        if (singletons.ContainsKey(tKey))
          return singletons[tKey];

        // Neue Instanz anlegen und speichern
        var obj = CreateInstance<TValue>();
        singletons.Add(tKey, obj);
        return obj;
      });
    }

    /// <summary>
    /// Registrieren eines Typs als Transient
    /// </summary>
    /// <typeparam name="TKey">Schlüsseltyp für den späteren Abruf</typeparam>
    /// <typeparam name="TValue">Zu instanzierender Typ (muss TKey implementieren)</typeparam>
    public void AddTransient<TKey, TValue>() where TValue : TKey
    {
      container.Add(typeof(TKey), () => CreateInstance<TValue>());
    }

    /// <summary>
    /// Registrieren eines Typs als Transient
    /// </summary>
    /// <typeparam name="TValue">Der zu registrierende Typ. Dieser wird auch als Key verwendet</typeparam>
    public void AddTransient<TValue>()
    {
      AddTransient<TValue, TValue>();
    }
    #endregion

    #region Infrastruktur
    private TValue CreateInstance<TValue>()
    {
      // zu instanzierender Typ
      var t = typeof(TValue);

      // es muss genau einen Konstruktor geben
      var ctor = t.GetConstructors().Single();

      // Parameter dieses Konstruktors ermitteln und Werte hierfür holen
      var parameters = ctor.GetParameters();
      var paramValues = new object[parameters.Length];
      for (int i = 0; i < parameters.Length; i++)
      {
        paramValues[i] = GetInstance(parameters[i].ParameterType);
      }

      // Instanz erzeugen und zurückgeben
      return (TValue)ctor.Invoke(paramValues);
    }

    #endregion

    #region Methoden zum Abrufen der Instanzen

    /// <summary>
    /// Abrufen eines Objekts
    /// </summary>
    /// <typeparam name="T">Schlüsseltyp</typeparam>
    /// <returns>Das (generierte) Objekt</returns>
    public T GetInstance<T>()
    {
      // Abruf der Factory-Methode aus dem Dictionary und ausführen derselben
      return (T)container[typeof(T)]();
    }

    /// <summary>
    /// Abrufen eines Objekts
    /// </summary>
    /// <param name="t">Schlüsseltyp</param>
    /// <returns>Das (generierte) Objekt</returns>
    public object GetInstance(Type t)
    {
      // Abruf der Factory-Methode aus dem Dictionary und ausführen derselben
      return container[t]();
    }
    #endregion
  }
}
