using System;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.View;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.Command
{
    internal class CheckTreatmentCommand : BaseCommand
    {
        private readonly PatientVisitationViewModel _viewModel;
        public event EventHandler<bool> TreatmentSelected;
        public HospitalTreatmentService HospitalTreatmentService;

        public CheckTreatmentCommand(PatientVisitationViewModel viewModel)
        {
            _viewModel = viewModel;
            HospitalTreatmentService = _viewModel.HospitalTreatmentService;
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedHospitalTreatment != null;
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
              _viewModel.HospitalTreatmentView = new HospitalTreatmentView(_viewModel.SelectedHospitalTreatment,_viewModel);
              _viewModel.HospitalTreatmentView.ShowDialog();
                TreatmentSelected?.Invoke(this,true);
            }
            else
            {
                TreatmentSelected?.Invoke(this, false);
            }

        }

    }
}
