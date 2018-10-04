using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null) {
            if (value.Equals(backingField))
                return false;
            backingField = value;

            NotifyPropertyChanged(propertyName);

            return true;
        }

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (!String.IsNullOrEmpty(propertyName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
