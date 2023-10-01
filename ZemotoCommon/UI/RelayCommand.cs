#if ZEMOTOUI
using System;
using System.Globalization;
using System.Windows.Input;

namespace ZemotoCommon.UI;

public sealed class RelayCommand : ICommand
{
   private readonly Action _execute;
   private readonly Func<bool> _canExecute;

   public RelayCommand( Action execute, Func<bool> canExecute = null )
   {
      _execute = execute;
      _canExecute = canExecute;
   }

   public bool CanExecute( object parameter ) => _canExecute?.Invoke() ?? true;

   public event EventHandler CanExecuteChanged
   {
      add { if ( _canExecute != null ) CommandManager.RequerySuggested += value; }
      remove { if ( _canExecute != null ) CommandManager.RequerySuggested -= value; }
   }

   public void Execute( object parameter )
   {
      _execute();
   }
}

public sealed class RelayCommand<T> : ICommand
{
   private readonly Action<T> _execute;
   private readonly Func<T, bool> _canExecute;

   public RelayCommand( Action<T> execute, Func<T, bool> canExecute = null )
   {
      _execute = execute;
      _canExecute = canExecute;
   }

   public bool CanExecute( object parameter )
   {
      if ( _canExecute == null )
      {
         return true;
      }

      var castedParameter = parameter;
      if ( typeof( T ) != typeof( object ) )
      {
         castedParameter = Convert.ChangeType( parameter, typeof( T ), CultureInfo.InvariantCulture );
      }

      return _canExecute( (T)castedParameter );
   }

   public event EventHandler CanExecuteChanged
   {
      add { if ( _canExecute != null ) CommandManager.RequerySuggested += value; }
      remove { if ( _canExecute != null ) CommandManager.RequerySuggested -= value; }
   }

   public void Execute( object parameter )
   {
      var castedParameter = parameter;
      if ( typeof( T ) != typeof( object ) )
      {
         castedParameter = Convert.ChangeType( parameter, typeof( T ), CultureInfo.InvariantCulture );
      }

      _execute( (T)castedParameter );
   }
}
#endif