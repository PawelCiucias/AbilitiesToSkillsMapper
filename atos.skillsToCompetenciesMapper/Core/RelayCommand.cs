using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace atos.skillsToCompetenciesMapper.Core
{
    class RelayCommand : ICommand
    {
        Action execute;
        Action<object> executeWithParameter;

        public RelayCommand(Action execute) => this.execute = execute;

        public RelayCommand(Action<object> execute) => this.executeWithParameter = execute;
        

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter = null)
        {
            return true;
        }

        public void Execute(object parameter = null)
        {
            if (parameter is null)
                execute();
            else
                executeWithParameter(parameter);
        }
    }
}
