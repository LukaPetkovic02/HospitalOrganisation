using System;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel;
using Validation = ZdravoCorp.Core.Utilities.Validation;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.Commands
{
    using Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;

    public class ExtendCareCommand : BaseCommand
    {
        private readonly HospitalTreatmentViewModel _viewModel;
        public event EventHandler<bool> ExtendedCare;

        public ExtendCareCommand(HospitalTreatmentViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return Validation.IsStringAPositiveNumber(_viewModel.Duration) && int.Parse(_viewModel.Duration) >= 1
                 && _viewModel.Status == HospitalTreatment.TreatmentStatus.INPROGRESS;
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.HospitalTreatmentService.ExtendHospitalTreatment(_viewModel.HospitalTreatment,int.Parse(_viewModel.Duration));
                ExtendedCare?.Invoke(this, true);
                _viewModel.OnCommandFinished();
            }
            else
            {
                ExtendedCare?.Invoke(this, false);
            }
        }
    }
}
