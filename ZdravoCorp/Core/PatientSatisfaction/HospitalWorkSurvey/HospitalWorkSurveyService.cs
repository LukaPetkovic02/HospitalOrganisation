using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey.Repository;
using ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey
{
    public class HospitalWorkSurveyService
    {
        public IHospitalWorkSurveyRepository HospitalWorkSurveyRepository { get; set; }
        public HospitalWorkSurveyService()
        {
            HospitalWorkSurveyRepository = new HospitalWorkSurveyRepository();
        }
        public void GetAllHospitalWorkSurveys()
        {
            HospitalWorkSurveyRepository.GetAllHospitalWorkSurveys();
        }
        public void AddHospitalWorkSurvey(Model.HospitalWorkSurvey hospitalWorkSurvey)
        {
            HospitalWorkSurveyRepository.AddHospitalWorkSurvey(hospitalWorkSurvey);
        }

        public double GetAverageCleanliness()
        {
            double average = 0;
            foreach (Model.HospitalWorkSurvey survey in HospitalWorkSurveyRepository.GetHospitalWorkSurveys())
            {
                average += survey.Cleanliness;
            }
            return average/ HospitalWorkSurveyRepository.GetHospitalWorkSurveys().Count();
        }
        public double GetAverageRecommendation()
        {
            double average = 0;
            foreach (Model.HospitalWorkSurvey survey in HospitalWorkSurveyRepository.GetHospitalWorkSurveys())
            {
                average += survey.Recommendation;
            }

            return average / HospitalWorkSurveyRepository.GetHospitalWorkSurveys().Count();
        }
        public double GetAverageService()
        {
            double average = 0;
            foreach (Model.HospitalWorkSurvey survey in HospitalWorkSurveyRepository.GetHospitalWorkSurveys())
            {
                average += survey.Service;
            }
            return average / HospitalWorkSurveyRepository.GetHospitalWorkSurveys().Count();
        }

        public int GetServiceGradeNumber(int grade)
        {
            int number = 0;
            foreach (Model.HospitalWorkSurvey survey in HospitalWorkSurveyRepository.GetHospitalWorkSurveys())
            {
                if (survey.Service == grade)
                {
                    number++;
                }
            }
            return number;
        }
        public int GetRecommendationGradeNumber(int grade)
        {
            int number = 0;
            foreach (Model.HospitalWorkSurvey survey in HospitalWorkSurveyRepository.GetHospitalWorkSurveys())
            {
                if (survey.Recommendation == grade)
                {
                    number++;
                }
            }
            return number;
        }
        public int GetCleanlinessGradeNumber(int grade)
        {
            int number = 0;
            foreach (Model.HospitalWorkSurvey survey in HospitalWorkSurveyRepository.GetHospitalWorkSurveys())
            {
                if (survey.Cleanliness == grade)
                {
                    number++;
                }
            }
            return number;
        }

        public List<HospitalWorkSurveyDataViewModel> GetHospitalWorkSurveyData()
        {
            List<HospitalWorkSurveyDataViewModel> surveys = new List<HospitalWorkSurveyDataViewModel>();
            foreach (Model.HospitalWorkSurvey survey in HospitalWorkSurveyRepository.GetHospitalWorkSurveys())
            {
                surveys.Add(new HospitalWorkSurveyDataViewModel(survey));
            }
            return surveys;
        }

        public List<HospitalWorkSurveyEntryDataViewModel> GetEntryDataViewModels()
        {
            List<HospitalWorkSurveyEntryDataViewModel> entries = new List<HospitalWorkSurveyEntryDataViewModel>
            {
                GetCleanlinessEntryDataViewModel(),
                GetRecommendationEntryDataViewModel(),
                GetServiceEntryDataViewModel()
            };

            return entries;
        }

        public HospitalWorkSurveyEntryDataViewModel GetCleanlinessEntryDataViewModel()
        {
            return new HospitalWorkSurveyEntryDataViewModel(GetCleanlinessGradeNumber(1), GetCleanlinessGradeNumber(2),
                GetCleanlinessGradeNumber(3), GetCleanlinessGradeNumber(4), GetCleanlinessGradeNumber(5),
                "Cleanliness", GetAverageCleanliness());
        }

        public HospitalWorkSurveyEntryDataViewModel GetRecommendationEntryDataViewModel()
        {
            return new HospitalWorkSurveyEntryDataViewModel(GetRecommendationGradeNumber(1), GetRecommendationGradeNumber(2),
                GetRecommendationGradeNumber(3), GetRecommendationGradeNumber(4), GetRecommendationGradeNumber(5),
                "Recommendation", GetAverageRecommendation());
        }

        public HospitalWorkSurveyEntryDataViewModel GetServiceEntryDataViewModel()
        {
            return new HospitalWorkSurveyEntryDataViewModel(GetServiceGradeNumber(1), GetServiceGradeNumber(2),
                GetServiceGradeNumber(3), GetServiceGradeNumber(4), GetServiceGradeNumber(5),
                "Service.cs", GetAverageService());
        }

        public void Save()
        {
            HospitalWorkSurveyRepository.Save();
        }
    }
}
