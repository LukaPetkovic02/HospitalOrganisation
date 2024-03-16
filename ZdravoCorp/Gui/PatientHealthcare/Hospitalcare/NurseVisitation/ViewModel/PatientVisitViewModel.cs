using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.Command;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.ViewModel
{
    using Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;
    public class PatientVisitViewModel : BaseViewModel
    {

        private ObservableCollection<HospitalTreatment> _patientsToVisit;
        public ObservableCollection<HospitalTreatment> PatientsToVisit
        {
            get { return _patientsToVisit; }
            set
            {
                _patientsToVisit = value;
                OnPropertyChanged(nameof(PatientsToVisit));
            }
        }
        private string _selectedSearchType;
        public string SelectedSearchType
        {
            get { return _selectedSearchType; }
            set
            {
                _selectedSearchType = value;
                OnPropertyChanged(nameof(SelectedSearchType));
            }
        }

        private HospitalTreatment _selectedPatient;
        public HospitalTreatment SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        private bool _morning;
        public bool Morning
        {
            get { return _morning; }
            set
            {
                _morning = value;
                OnPropertyChanged(nameof(Morning));
            }
        }

        private bool _night;
        public bool Night
        {
            get { return _night; }
            set
            {
                _night = value;
                OnPropertyChanged(nameof(Night));
            }
        }

        public string SearchInput {get; set;}
        public string Observation { get; set;}

        

        public ICommand SearchPatientsVisitCommand { get; }
        public ICommand VisitCommand { get; }


        public PatientVisitViewModel() 
        {
            Morning = true;
            SearchPatientsVisitCommand = new SearchPatientsVisitCommand(this);
            VisitCommand = new VisitCommand(this); 
        }
    }
}
