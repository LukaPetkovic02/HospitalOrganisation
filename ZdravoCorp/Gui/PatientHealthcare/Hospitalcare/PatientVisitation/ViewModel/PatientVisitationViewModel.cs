using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.View;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.Command;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.ViewModel
{
    using Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;
    public class PatientVisitationViewModel : BaseViewModel
    {

        private ObservableCollection<HospitalTreatment> _hospitalTreatments;
        public ObservableCollection<HospitalTreatment> HospitalTreatments
        {
			get { return _hospitalTreatments; }
			set
			{
                _hospitalTreatments = value;
				OnPropertyChanged(nameof(HospitalTreatments));
			}
		}

        public ObservableCollection<string> Patients { get; set; }

        private HospitalTreatment _selectedHospitalTreatment;
        public HospitalTreatment SelectedHospitalTreatment
        {
            get
            {
                return _selectedHospitalTreatment;
            }
            set
            {
                _selectedHospitalTreatment = value;
                OnPropertyChanged(nameof(SelectedHospitalTreatment));
            }
        }

        public ICommand CheckTreatmentCommand { get; set; }
        public HospitalTreatmentService HospitalTreatmentService {get; set; }
        public HospitalTreatmentView HospitalTreatmentView { get; set; }

        public PatientVisitationViewModel()
        {   
            LoadData();
            CheckTreatmentCommand = new CheckTreatmentCommand(this);
            ((CheckTreatmentCommand)CheckTreatmentCommand).TreatmentSelected += OnHospitalTreatmentSelected;
        }

        public void LoadData()
        {
            HospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
            HospitalTreatments = new ObservableCollection<HospitalTreatment>(HospitalTreatmentService.GetAllHospitalTreatments());
        }

        public void OnHospitalTreatmentSelected(object? sender, bool isSelected)    
        {
            if (!isSelected)
            {
                MessageBox.Show("You need to select hospital treatment first! ");
            }
        }

    }
}
