using System;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.View;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.ViewModel;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;
using static ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model.HospitalTreatment;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel
{
    using Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;
    public class HospitalTreatmentViewModel : BaseViewModel
    {
        public HospitalTreatment HospitalTreatment;
        public string Id { get; set; }
        public string PatientJmbg { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Therapy { get; set; }
        public string RoomId { get; set; }
        public TreatmentStatus Status { get; set; }

        public ICommand ReleasePatientCommand { get; set; }
        public ICommand ChangeTherapyCommand { get; set; }
        public ICommand ExtendCareCommand { get; set; }

        public HospitalTreatmentService HospitalTreatmentService { get; set; }
        public bool IsChecked { get; set; }
        public string Duration { get; set; }
        public HospitalTreatmentView HospitalTreatmentView;
        public PatientVisitationViewModel PatientVisitationViewModel;

        public HospitalTreatmentViewModel(HospitalTreatment hospitalTreatment, HospitalTreatmentView hospitalTreatmentView,PatientVisitationViewModel patientVisitationViewModel)
        {
            HospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
            HospitalTreatmentView = hospitalTreatmentView;
            PatientVisitationViewModel = patientVisitationViewModel;
            CreateCommands();
            LoadHospitalTreatment(hospitalTreatment);
            PrepareEventHandlers();
        }

        private void CreateCommands()
        {
            ReleasePatientCommand = new ReleasePatientCommand(this);
            ChangeTherapyCommand = new ChangeTherapyCommand(this);
            ExtendCareCommand = new ExtendCareCommand(this);
        }

        private void LoadHospitalTreatment(HospitalTreatment hospitalTreatment)
        {
            HospitalTreatment = hospitalTreatment;
            Id = hospitalTreatment.Id;
            PatientJmbg = hospitalTreatment.PatientJmbg;
            Start = hospitalTreatment.Start.Date;
            End = hospitalTreatment.End.Date;
            Therapy = hospitalTreatment.Therapy;
            RoomId = hospitalTreatment.RoomId;
            Status = hospitalTreatment.Status;
        }

        public void PrepareEventHandlers()
        {
            ((ExtendCareCommand)ExtendCareCommand).ExtendedCare += OnExtendedCare;
            ((ChangeTherapyCommand)ChangeTherapyCommand).TherapyChanged += OnTherapyChanged;
            ((ReleasePatientCommand)ReleasePatientCommand).ReleasedPatient += OnPatientReleased;

        }

        public void OnCommandFinished()
        {
            HospitalTreatmentView.Close();
            PatientVisitationViewModel.LoadData();
        }

        public void OnExtendedCare(object? sender, bool isExtended)
        {
            if (isExtended)
            {
                HospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
                MessageBox.Show("Successfully extended hospital treatment!");
            }
            else
            {
                MessageBox.Show("Please insert valid data! ");
            }
        }

        public void OnTherapyChanged(object? sender, bool isExtended)
        {
            if (isExtended)
            {
                HospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
                MessageBox.Show("Successfully changed therapy!");

            }
            else
            {
                HospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
                MessageBox.Show("Please insert valid therapy! ");
            }
        }

        public void OnPatientReleased(object? sender, bool isReleased)
        {
            if (isReleased)
            {
                MessageBox.Show("Successfully released patient from treatment! ");
            }
            else
            {
                MessageBox.Show("Treatment is already finished!");
            }
        }

    }
}
