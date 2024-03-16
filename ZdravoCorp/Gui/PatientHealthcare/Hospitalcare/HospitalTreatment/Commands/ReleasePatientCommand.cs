using System;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.Commands
{   
    using Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;
    public class ReleasePatientCommand : BaseCommand
    {
        private readonly HospitalTreatmentViewModel _viewModel;
        public event EventHandler<bool> ReleasedPatient;
        public HospitalTreatmentService HospitalTreatmentService;
        public PatientService PatientService;
        public ScheduleService ScheduleService;

        public ReleasePatientCommand(HospitalTreatmentViewModel viewModel)
        {
            _viewModel = viewModel;
            HospitalTreatmentService = _viewModel.HospitalTreatmentService;
            PatientService = new PatientService();
            ScheduleService = new ScheduleService();
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.Status != HospitalTreatment.TreatmentStatus.FINISHED;
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                if (_viewModel.IsChecked && !ScheduleService.CreateAppointmentIn10Days(MainWindow.LoginDoctor,Generator.GenerateDateForAppointment(), _viewModel.PatientJmbg)) 
                { ReleasedPatient?.Invoke(this, false); }

                _viewModel.HospitalTreatmentService.FinishHospitalTreatment(_viewModel.HospitalTreatment);
                ReleasedPatient?.Invoke(this, true);
                _viewModel.OnCommandFinished();
            }
            else
            {
                ReleasedPatient?.Invoke(this, false);
            }
        }
    }

}

