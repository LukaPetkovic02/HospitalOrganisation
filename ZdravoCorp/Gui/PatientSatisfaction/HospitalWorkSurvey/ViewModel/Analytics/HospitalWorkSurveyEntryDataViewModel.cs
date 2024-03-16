using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel.Analytics
{
    public class HospitalWorkSurveyEntryDataViewModel : BaseViewModel
    {
        private int _oneStar;

        public int OneStar
        {
            get { return _oneStar; }
            set
            {
                _oneStar = value;
                OnPropertyChanged(nameof(OneStar));
            }
        }

        private int _twoStar;

        public int TwoStar
        {
            get { return _twoStar; }
            set
            {
                _twoStar = value;
                OnPropertyChanged(nameof(TwoStar));
            }
        }
        private int _threeStar;

        public int ThreeStar
        {
            get { return _threeStar; }
            set
            {
                _threeStar = value;
                OnPropertyChanged(nameof(ThreeStar));
            }
        }
        private int _fourStar;

        public int FourStar
        {
            get { return _fourStar; }
            set
            {
                _fourStar = value;
                OnPropertyChanged(nameof(FourStar));
            }
        }
        private int _fiveStar;

        public int FiveStar
        {
            get { return _fiveStar; }
            set
            {
                _fiveStar = value;
                OnPropertyChanged(nameof(FiveStar));
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
        private string _average;

        public string Average
        {
            get { return _average; }
            set
            {
                _average = value;
                OnPropertyChanged(nameof(Average));
            }
        }
        public HospitalWorkSurveyEntryDataViewModel(int oneStar, int twoStar, int threeStar, int fourStar, int fiveStar, string name, double average)
        {
            _oneStar = oneStar;
            _twoStar = twoStar;
            _threeStar = threeStar;
            _fourStar = fourStar;
            _fiveStar = fiveStar;
            _name = name;
            _average = average.ToString("0.00");
        }

    }
}
