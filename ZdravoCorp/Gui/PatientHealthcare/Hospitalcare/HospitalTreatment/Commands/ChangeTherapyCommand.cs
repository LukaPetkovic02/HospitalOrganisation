using System;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.Commands
{
    public class ChangeTherapyCommand : BaseCommand
    {
        private readonly HospitalTreatmentViewModel _viewModel;
        public event EventHandler<bool> TherapyChanged;

        public ChangeTherapyCommand(HospitalTreatmentViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Therapy);
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.HospitalTreatmentService.ChangeHospitalTreatmentTherapy(_viewModel.HospitalTreatment,_viewModel.Therapy);
                TherapyChanged?.Invoke(this, true);
                _viewModel.OnCommandFinished();
            }
            else
            {
                TherapyChanged?.Invoke(this, false);
            }
        }

    }
}
