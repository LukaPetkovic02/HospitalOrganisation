using System.Collections.Generic;
using System.Windows.Input;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.Command.Analytics;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics
{
    public class DoctorAnalyticsViewModel : BaseViewModel
    {
        public DoctorService DoctorService;
        public DoctorSurveyService DoctorSurveyService;

        private List<DoctorDataViewModel> doctorViewModels;
        private DoctorDataViewModel selecteDoctorViewModel;
        public List<DoctorDataViewModel> Doctors
        {
            get { return doctorViewModels; }
            set
            {
                doctorViewModels = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        public DoctorDataViewModel SelectedDoctor
        {
            get { return selecteDoctorViewModel; }
            set
            {
                selecteDoctorViewModel = value;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }

        public ICommand SortBestDoctors { get; }
        public ICommand SortWorstDoctors { get; }
        public ICommand GetAllDoctors { get; }
        public ICommand MoreDetails { get; }

        public DoctorAnalyticsViewModel(DoctorService DoctorService, DoctorSurveyService DoctorSurveyService)
        {
            this.DoctorSurveyService = DoctorSurveyService;
            this.DoctorService = DoctorService;
            doctorViewModels = DoctorService.GetDoctorData();
            selecteDoctorViewModel = null;
            SortBestDoctors = new GetBestDoctorsCommand(this);
            SortWorstDoctors = new GetWorstDoctorsCommand(this);
            MoreDetails = new MoreDoctorDetailsCommand(this);
            GetAllDoctors = new GetAllDoctorsCommand(this);
        }
    }
}
