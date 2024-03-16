using System;
using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.Command
{
    public class SubmitHospitalSurveyCommand : BaseCommand
    {
        private readonly HospitalWorkSurveyViewModel _hospitalWorkSurveyViewModel;
        private HospitalWorkSurveyService _hospitalWorkSurveyService;
        public event EventHandler<bool> SubmitHospitalSurvey;
        public SubmitHospitalSurveyCommand(HospitalWorkSurveyViewModel hospitalWorkSurveyViewModel)
        {
            _hospitalWorkSurveyViewModel = hospitalWorkSurveyViewModel;
            _hospitalWorkSurveyService=new HospitalWorkSurveyService();
        }
        public bool CanExecute(object? parameter)
        {
            if (_hospitalWorkSurveyViewModel.Service=="" || _hospitalWorkSurveyViewModel.Cleanliness == "" || _hospitalWorkSurveyViewModel.Recommendation=="")
                return false;
            return true;
        }
        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                int service = int.Parse(_hospitalWorkSurveyViewModel.Service);
                int cleanliness = int.Parse(_hospitalWorkSurveyViewModel.Cleanliness);
                int recommendation = int.Parse(_hospitalWorkSurveyViewModel.Recommendation);
                _hospitalWorkSurveyService.AddHospitalWorkSurvey(new Core.PatientSatisfaction.HospitalWorkSurvey.Model.HospitalWorkSurvey(service, cleanliness,
                    recommendation, _hospitalWorkSurveyViewModel.Comment));
                MessageBox.Show("Survey successfuly added. Thank you for your feedback!");
                SubmitHospitalSurvey?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("You must give a grade to each category!");
                SubmitHospitalSurvey?.Invoke(this, false);
            }
        }
    }
}
