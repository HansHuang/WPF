using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MVVMPratice
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Title (INotifyPropertyChanged Property)
        private string _title = "Hello, world. I'm a programer";
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != null && _title.Equals(value)) return;
                _title = value;
                RaisePropertyChanged("Title");
            }
        }
        #endregion

        #region IsEditMode (INotifyPropertyChanged Property)

        private bool _isEditMode;

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode.Equals(value)) return;
                _isEditMode = value;
                RaisePropertyChanged("IsEditMode");
            }
        }

        #endregion

        #region RelayCommand ToggleEditModeCmd

        private RelayCommand _toggleEditModeCmd;

        public ICommand ToggleEditModeCmd
        {
            get
            {
                return _toggleEditModeCmd ?? (_toggleEditModeCmd = new RelayCommand(param => ToggleEditModeExeture()));
            }
        }

        private void ToggleEditModeExeture() 
        {
            IsEditMode = !IsEditMode;
        }

        #endregion

        #region RelayCommand CloseWindowCmd

        private RelayCommand _closeWindowCmd;
        public ICommand CloseWindowCmd
        {
            get { return _closeWindowCmd ?? (_closeWindowCmd = new RelayCommand(param => CloseWindow())); }
        }

        private void CloseWindow()
        {
            Window.Close();
        }
        #endregion

        #region RelayCommand MoveWindowCmd

        private RelayCommand _moveWindowCmd;

        public ICommand MoveWindowCmd
        {
            get { return _moveWindowCmd ?? (_moveWindowCmd = new RelayCommand(param => MoveWindow())); }
        }

        private void MoveWindow()
        {
            Window.DragMove();
        }

        #endregion

        protected MainWindow Window;

        public MainViewModel(MainWindow window) 
        {
            Window = window;
        }


    }

    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }
}
