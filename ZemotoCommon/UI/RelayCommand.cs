using System;
using System.Windows.Input;

namespace ZemotoCommon.UI
{
   public sealed class RelayCommand : ICommand
   {
      readonly Action _execute;
      readonly Func<bool> _canExecute;

      public RelayCommand( Action execute, Func<bool> canExecute = null )
      {
         _execute = execute;
         _canExecute = canExecute;
      }

      public bool CanExecute( object _ ) => _canExecute?.Invoke() ?? true;

      public event EventHandler CanExecuteChanged
      {
         add { if ( _canExecute != null ) CommandManager.RequerySuggested += value; }
         remove { if ( _canExecute != null ) CommandManager.RequerySuggested -= value; }
      }

      public void Execute( object _ )
      {
         _execute();
      }
   }

   public sealed class RelayCommand<T> : ICommand
   {
      readonly Action<T> _execute;
      readonly Func<T, bool> _canExecute;

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
         if ( parameter is T castedParam )
         {
            return _canExecute( castedParam );
         }
         else
         {
            throw new ArgumentException( "Invalid command parameter", nameof( parameter ) );
         }
      }

      public event EventHandler CanExecuteChanged
      {
         add { if ( _canExecute != null ) CommandManager.RequerySuggested += value; }
         remove { if ( _canExecute != null ) CommandManager.RequerySuggested -= value; }
      }

      public void Execute( object parameter )
      {
         if ( parameter is T castedParam )
         {
            _execute( castedParam );
         }
         else
         {
            throw new ArgumentException( "Invalid command parameter", nameof( parameter ) );
         }
      }
   }
}
