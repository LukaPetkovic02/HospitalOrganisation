using System;
using System.Collections.ObjectModel;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.Commands
{
    public class LoadAvailableAccommodationRoomsCommand : BaseCommand
    {
        private readonly TreatmentAccommodationViewModel _viewModel;
        private TreatmentReferralService _treatmentReferralService;
        private HospitalTreatmentService _hospitalTreatmentService;
        private RoomService _roomService;
        public event EventHandler<bool> LoadAvailableAccommodationRooms;

        public LoadAvailableAccommodationRoomsCommand(TreatmentAccommodationViewModel viewModel)
        {
            _viewModel = viewModel;
            _treatmentReferralService = new TreatmentReferralService();
            _hospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
            _roomService = new RoomService();
        }
        public bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.IdTreatmentReferral) && _treatmentReferralService.FindReferralById(_viewModel.IdTreatmentReferral) != null
                && _treatmentReferralService.FindReferralById(_viewModel.IdTreatmentReferral).IsUsed == false;
        }
        public override void Execute(object parameter)
        {
            _treatmentReferralService = new TreatmentReferralService();
            _hospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
            if (CanExecute(parameter))
            {
                _viewModel.AccommodationRooms = new ObservableCollection<Room>(_roomService.GetAvailableRoomsForHospitalTreatment(
                    DateTime.Now, _hospitalTreatmentService.GetAllHospitalTreatments()));
                LoadAvailableAccommodationRooms?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("Cant intake referral with that id.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadAvailableAccommodationRooms?.Invoke(this, false);
            }

        }
    }

}
