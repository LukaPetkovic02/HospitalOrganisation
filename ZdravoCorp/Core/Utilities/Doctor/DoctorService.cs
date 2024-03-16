using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ZdravoCorp.Core.Utilities.Doctor.Repository;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Core.Utilities.Doctor
{
    public class DoctorService
    {
        public DoctorRepository DoctorRepository;

        public DoctorService()
        {
            DoctorRepository = new DoctorRepository();
        }

        public List<Model.Doctor> Doctors()
        {
            return DoctorRepository.Doctors;
        }

        public Model.Doctor? FindByJmbg(string jmbg)
        {
            return DoctorRepository.FindByJmbg(jmbg);
        }

        public Model.Doctor? FindBySpecialization(Model.Doctor.Specialty specialization,Model.Doctor doctor)
        {
            return DoctorRepository.FindBySpecialization(specialization,doctor);
        }
        public List<Model.Doctor> GetDoctorsBySpecialization(List<Model.Doctor> doctors, Model.Doctor.Specialty specialization)
        {
            return DoctorRepository.GetDoctorsBySpecialization(doctors, specialization);
        }
        public List<Model.Doctor> GetDoctorsByNameSubstring(List<Model.Doctor> doctors, string substring)
        {
            return DoctorRepository.GetDoctorsByNameSubstring(doctors, substring);
        }
        public List<Model.Doctor> GetDoctorsByLastNameSubstring(List<Model.Doctor> doctors, string substring)
        {
            return DoctorRepository.GetDoctorsByLastNameSubstring(doctors, substring);
        }
        public List<Model.Doctor> SortDoctorsByName(List<Model.Doctor> doctors)
        {
            return doctors.OrderBy(doctor => doctor.Name).ToList();
        }
        public List<Model.Doctor> SortDoctorsByLastName(List<Model.Doctor> doctors)
        {
            return doctors.OrderBy(doctor => doctor.LastName).ToList();
        }
        public List<Model.Doctor> SortDoctorsBySpecialty(List<Model.Doctor> doctors)
        {
            return doctors.OrderBy(doctor => doctor.Specialization).ToList();
        }
        public List<Model.Doctor> SortDoctorsByAverageScore(List<Model.Doctor> doctors)
        {
            return doctors.OrderByDescending(doctor => doctor.AverageScore).ToList();
        }

        public List<DoctorDataViewModel> GetDoctorData()
        {
            List < DoctorDataViewModel > doctors = new List<DoctorDataViewModel>();
            foreach (Model.Doctor doctor in DoctorRepository.Doctors)
            {
                doctors.Add(new DoctorDataViewModel(doctor));
            }
            return doctors;
        }

        public List<DoctorDataViewModel> GetBestDoctorsData(int number = 3)
        {
            List<DoctorDataViewModel> doctors = DoctorRepository.Doctors
                .Select(doctor => new DoctorDataViewModel(doctor))
                .OrderByDescending(doctor => doctor.AverageScore)
                .Take(number)
                .ToList();

            return doctors;
        }

        public List<DoctorDataViewModel> GetWorstDoctorsData(int number = 3)
        {
            List<DoctorDataViewModel> doctors = DoctorRepository.Doctors
                .Select(doctor => new DoctorDataViewModel(doctor))
                .OrderBy(doctor => doctor.AverageScore)
                .Take(number)
                .ToList();

            return doctors;
        }
        public void Save()
        {
            DoctorRepository.Save();
        }

    }
}
