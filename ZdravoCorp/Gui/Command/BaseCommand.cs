using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZdravoCorp.Gui.Commands
{
    public class BaseCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter){}

        public event EventHandler? CanExecuteChanged;

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
