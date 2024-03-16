using System;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Model;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.Commands
{
    public class AccommodatePatientCommand : BaseCommand
    {

        private readonly TreatmentAccommodationViewModel _viewModel;
        public event EventHandler<bool> AccommodatePatient;
        private TreatmentReferralService _treatmentReferralService;
        private HospitalTreatmentService _hospitalTreatmentService;
        public AccommodatePatientCommand(TreatmentAccommodationViewModel viewModel)
        {
            _viewModel = viewModel;
            _treatmentReferralService = new TreatmentReferralService();
            _hospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedRoom != null;
        }

        public void CreateHospitalTreatment()
        {
            TreatmentReferral referral = _treatmentReferralService.FindReferralById(_viewModel.IdTreatmentReferral);

            _hospitalTreatmentService.CreateHospitalTreatment(
                referral.Id, referral.PatientJmbg, DateTime.Now, DateTime.Now.AddDays(referral.Duration), referral.Therapy, _viewModel.SelectedRoom.Id);
            _hospitalTreatmentService.Save();
        }

        public void UpdateReferral()
        {
            TreatmentReferral referral = _treatmentReferralService.FindReferralById(_viewModel.IdTreatmentReferral);
            referral.IsUsed = true;
            _treatmentReferralService.Save();
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                CreateHospitalTreatment();


                UpdateReferral();

                _viewModel.SelectedRoom = null;
                _viewModel.AccommodationRooms.Clear();

                MessageBox.Show("Successfully created hospital treatment.", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                AccommodatePatient?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AccommodatePatient?.Invoke(this, false);
            }

        }
    }
}
