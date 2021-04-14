using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionBeispiel
{
  public class ServiceLocator
  {
    private Dictionary<Type, Func<object>> container = new();
    private Dictionary<Type, object> singletons = new();

    public static readonly ServiceLocator Current = new ServiceLocator();
    private ServiceLocator(){}

    #region Methoden zum Aufbauen des DI-Containers
    public void AddSingleton<TKey>(object obj)
    {
      // Prüfung des Typs notwendig
      container.Add(typeof(TKey), () => obj);
    }

    public void AddSingleton<TValue>()
    {
      AddSingleton<TValue, TValue>();
    }

    public void AddSingleton<TKey, TValue>() where TValue:TKey
    { 
      var tKey = typeof(TKey);
      container.Add(tKey, () =>
      {
        if (singletons.ContainsKey(tKey))
          return singletons[tKey];
        var obj = CreateInstance<TValue>();
        singletons[tKey] = obj;
        return obj;
      });
    }

    public void AddTransient<TKey, TValue>() where TValue : TKey
    { 
      container.Add(typeof(TKey), () => CreateInstance<TValue>());
    }
    #endregion

    #region Infrastruktur
    private TValue CreateInstance<TValue>()
    {
      var t = typeof(TValue);
      var ctor = t.GetConstructors().Single();
      var parameters = ctor.GetParameters();
      var paramValues = new object[parameters.Length];
      for (int i = 0; i < parameters.Length; i++)
      {
        paramValues[i] = GetInstance(parameters[i].ParameterType);
      }

      return (TValue) ctor.Invoke(paramValues);
    }

    #endregion

    #region Methoden zum Abrufen der Instanzen
    public T GetInstance<T>()
    {
      return (T)container[typeof(T)]();
    }

    public object GetInstance(Type t)
    {
      return container[t]();
    }
    #endregion
  }
}
