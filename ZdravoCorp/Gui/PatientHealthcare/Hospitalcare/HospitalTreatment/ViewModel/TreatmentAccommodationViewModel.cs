using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.Commands;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel
{
    public class TreatmentAccommodationViewModel : BaseViewModel
    {

        private ObservableCollection<Room> _accommodationRooms;
        public string IdTreatmentReferral { get; set; }

        public ObservableCollection<Room> AccommodationRooms
        {
            get { return _accommodationRooms; }
            set
            {
                _accommodationRooms = value;
                OnPropertyChanged(nameof(AccommodationRooms));
            }
        }
        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
            }
        }
        public ICommand LoadAvailableAccommodationRoomsCommand { get; }
        public ICommand AccommodatePatientCommand { get; }
        public TreatmentAccommodationViewModel()
        {
            LoadAvailableAccommodationRoomsCommand = new LoadAvailableAccommodationRoomsCommand(this);
            AccommodatePatientCommand = new AccommodatePatientCommand(this);
        }

    }
}
