using System;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.VacationRequest.Command;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.VacationRequest.ViewModel
{
    public class VacationRequestViewModel : BaseViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }


        public ICommand RequestVacationCommand { get; }

        public void OnVacationRequested(object? sender, bool isRequested)
        {
            if (isRequested)
            {
                MessageBox.Show("Successfully requested vacation! ");
            }
            else
            {
                MessageBox.Show("Insert correct request data (at least 2 days before start date)! ");
            }
        }

        public VacationRequestViewModel(Core.Utilities.Doctor.Model.Doctor doctor)
        {
            RequestVacationCommand = new RequestVacationCommand(this,doctor);
            ((RequestVacationCommand)RequestVacationCommand).VacationRequested += OnVacationRequested;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

    }
}
