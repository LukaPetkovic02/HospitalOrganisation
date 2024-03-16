using System;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.NurseVisitation;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.NurseVisitation.Repository;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.Command
{
    public class VisitCommand : BaseCommand
    {
        private readonly PatientVisitViewModel _viewModel;
        public HospitalTreatmentService _hospitalTreatmentService;
        public NurseVisitationService _nurseVisitationService;
        public event EventHandler<bool> VisitPatient;

        public VisitCommand(PatientVisitViewModel viewModel) 
        {
            _viewModel = viewModel;
            _hospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
            _nurseVisitationService = new NurseVisitationService(new NurseVisitationRepository());
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedPatient != null;
        }

        public override void Execute(object parameter)
        {

            if (CanExecute(parameter))
            {
                Core.PatientHealtcare.Hospitalcare.NurseVisitation.Model.NurseVisitation.VisitationTime time = _viewModel.Night ? Core.PatientHealtcare.Hospitalcare.NurseVisitation.Model.NurseVisitation.VisitationTime.NIGHT : Core.PatientHealtcare.Hospitalcare.NurseVisitation.Model.NurseVisitation.VisitationTime.MORNING;
                _nurseVisitationService.CreateNurseVisitation(_viewModel.SelectedPatient.PatientJmbg, DateTime.Now, time, _viewModel.Observation);
                _nurseVisitationService.Save();

                MessageBox.Show("Successfully visited.", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                _viewModel.Observation = string.Empty;
                VisitPatient?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("Wrong input.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VisitPatient?.Invoke(this, false);
            }

        }
    }
}
