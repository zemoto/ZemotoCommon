#if ZEMOTOUI
using System;
using System.Globalization;
using System.Windows.Input;

namespace ZemotoCommon.UI;

internal sealed class RelayCommand( Action execute, Func<bool> canExecute = null ) : ICommand
{
   public bool CanExecute( object parameter ) => canExecute?.Invoke() ?? true;

   public event EventHandler CanExecuteChanged
   {
      add { if ( canExecute != null ) CommandManager.RequerySuggested += value; }
      remove { if ( canExecute != null ) CommandManager.RequerySuggested -= value; }
   }

   public void Execute( object parameter )
   {
      execute();
   }
}

internal sealed class RelayCommand<T>( Action<T> execute, Func<T, bool> canExecute = null ) : ICommand
{
   public bool CanExecute( object parameter )
   {
      if ( canExecute == null )
      {
         return true;
      }

      var castedParameter = parameter;
      if ( typeof( T ) != typeof( object ) )
      {
         castedParameter = Convert.ChangeType( parameter, typeof( T ), CultureInfo.InvariantCulture );
      }

      return canExecute( (T)castedParameter );
   }

   public event EventHandler CanExecuteChanged
   {
      add { if ( canExecute != null ) CommandManager.RequerySuggested += value; }
      remove { if ( canExecute != null ) CommandManager.RequerySuggested -= value; }
   }

   public void Execute( object parameter )
   {
      var castedParameter = parameter;
      if ( typeof( T ) != typeof( object ) )
      {
         castedParameter = Convert.ChangeType( parameter, typeof( T ), CultureInfo.InvariantCulture );
      }

      execute( (T)castedParameter );
   }
}
#endif