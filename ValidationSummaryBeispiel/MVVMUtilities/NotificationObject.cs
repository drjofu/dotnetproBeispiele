using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVMUtilities
{
  public class NotificationObject:INotifyPropertyChanged
  {
    #region INotifyPropertyChanged

    /// <summary>
    /// Auslösen von PropertyChanged
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression">Lambda-Ausdruck: ()=>Eigenschaft</param>
    protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression");
      MemberExpression body = expression.Body as MemberExpression;
      if (body == null)
        throw new ArgumentException("Ausdruck im Format '()=>Eigenschaft' erwartet");
      RaisePropertyChanged(body.Member.Name);
    }

    /// <summary>
    /// Auslösen von PropertyChanged
    /// </summary>
    /// <param name="propertyName">Eigenschaftsname</param>
    protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}
