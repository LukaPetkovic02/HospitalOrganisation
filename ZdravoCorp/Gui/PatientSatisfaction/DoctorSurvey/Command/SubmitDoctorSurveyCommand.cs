using System;
using System.Linq;
using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.Command
{
    public class SubmitDoctorSurveyCommand : BaseCommand
    {
        private readonly DoctorSurveyViewModel _doctorSurveyViewModel;
        private Core.Utilities.Doctor.Model.Doctor _doctor;
        private DoctorSurveyService _doctorSurveyService;
        public event EventHandler<bool> SubmitDoctorSurvey;
        public SubmitDoctorSurveyCommand(DoctorSurveyViewModel doctorSurveyViewModel, Core.Utilities.Doctor.Model.Doctor doctor)
        {
            _doctorSurveyViewModel = doctorSurveyViewModel;
            _doctor = doctor;
            _doctorSurveyService = new DoctorSurveyService();
        }
        public bool CanExecute(object? parameter)
        {
            if (_doctorSurveyViewModel.Service == "" || _doctorSurveyViewModel.Recommendation == "")
                return false;
            return true;
        }
        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                DoctorService doctorService = new DoctorService();
                _doctor = doctorService.FindByJmbg(_doctor.Jmbg);
                int service = int.Parse(_doctorSurveyViewModel.Service);
                int recommendation = int.Parse(_doctorSurveyViewModel.Recommendation);
                Core.PatientSatisfaction.DoctorSurvey.Model.DoctorSurvey doctorSurvey=new Core.PatientSatisfaction.DoctorSurvey.Model.DoctorSurvey(_doctor.Jmbg, service, recommendation, _doctorSurveyViewModel.Comment);
                _doctorSurveyService.AddDoctorSurvey(doctorSurvey);
                _doctor.Ratings.Add(doctorSurvey.FinalGrade);
                _doctor.AverageScore = _doctor.Ratings.Average();
                doctorService.Save();
                MessageBox.Show("Doctor survey successfuly added. Thank you for your feedback!");
                SubmitDoctorSurvey?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("You must give a grade to each category!");
                SubmitDoctorSurvey?.Invoke(this, false);
            }
        }
    }
}
