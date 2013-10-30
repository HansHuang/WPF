using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace DragAndDropItemsControl
{
    public class ViewModel : INotifyPropertyChanged
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

        #region Student (INotifyPropertyChanged Property)

        private ObservableCollection<String> _student;

        public ObservableCollection<String> Student
        {
            get { return _student ?? (_student = new ObservableCollection<string>()); }
            set
            {
                if (_student != null && _student.Equals(value)) return;
                _student = value;
                RaisePropertyChanged("Student");
            }
        }

        #endregion

        #region Teacher (INotifyPropertyChanged Property)

        private ObservableCollection<string> _teacher;

        public ObservableCollection<string> Teacher
        {
            get { return _teacher ?? (_teacher = new ObservableCollection<string>()); }
            set
            {
                if (_teacher != null && _teacher.Equals(value)) return;
                _teacher = value;
                RaisePropertyChanged("Teacher");
            }
        }

        #endregion

        #region IsCopyWhenDrag (INotifyPropertyChanged Property)

        private bool _isCopyWhenDrag;

        public bool IsCopyWhenDrag
        {
            get { return _isCopyWhenDrag; }
            set
            {
                if (_isCopyWhenDrag.Equals(value)) return;
                _isCopyWhenDrag = value;
                RaisePropertyChanged("IsCopyWhenDrag");
            }
        }
        #endregion

        public ViewModel()
        {
            for (var i = 0; i < 10; i++)
            {
                Student.Add("Student " + i);
                Teacher.Add("Teacher " + i);
            }
            IsCopyWhenDrag = true;
        }
    }
}
