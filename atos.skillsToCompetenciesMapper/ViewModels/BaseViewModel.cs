using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.ViewModels
{
   abstract class BaseViewModel : INotifyPropertyChanged
    {
        public bool SetValue<T>(ref T backingField, T value, [CallerMemberName]string propertyName = null) {
            if (Object.Equals(backingField, value))
                return false;
            backingField = value;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChanged != null && propertyName != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
