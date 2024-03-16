using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics
{
    public class DoctorDataViewModel : BaseViewModel
    {
        public Core.Utilities.Doctor.Model.Doctor Doctor;

        private string _jmbg;
        public string Jmbg
        {
            get { return _jmbg; }
            set
            {
                _jmbg = value;
                OnPropertyChanged(nameof(Jmbg));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _birthDate;
        public string BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        private string _specialization;
        public string Specialization
        {
            get { return _specialization; }
            set
            {
                _specialization = value;
                OnPropertyChanged(nameof(Specialization));
            }
        }

        private double _averageScore;
        public double AverageScore
        {
            get { return _averageScore; }
            set
            {
                _averageScore = value;
                OnPropertyChanged(nameof(AverageScore));
            }
        }

        public DoctorDataViewModel(Core.Utilities.Doctor.Model.Doctor Doctor)
        {
            this.Doctor = Doctor;  
            _jmbg = Doctor.Jmbg;
            _name = Doctor.Name;
            _lastName = Doctor.LastName;
            _birthDate = Doctor.BirthDate.ToString("d");
            _specialization = Doctor.Specialization.ToString();
            _averageScore = Doctor.AverageScore;
        }
    }
}
