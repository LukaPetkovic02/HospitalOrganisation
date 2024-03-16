using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Utilities.Doctor.Repository
{
    public class DoctorRepository
    {
        public List<Model.Doctor> Doctors { get; set; }

        public DoctorRepository()
        {
           GetAllDoctors();
        }
        public Model.Doctor? FindByJmbg(string jmbg)
        {
            return Doctors.FirstOrDefault(doctor => doctor.Jmbg == jmbg);
        }

        public Model.Doctor? FindBySpecialization(Model.Doctor.Specialty specialization,Model.Doctor doctor)
        {
            return Doctors.FirstOrDefault(doc => doc.Jmbg != doctor.Jmbg && doc.Specialization == specialization);
        }

        public List<Model.Doctor> GetDoctorsBySpecialization(List<Model.Doctor> doctors, Model.Doctor.Specialty specialization)
        {
            return doctors.Where(doctor => doctor.Specialization == specialization).ToList();
        }
        public List<Model.Doctor> GetDoctorsByNameSubstring(List<Model.Doctor> doctors, string substring)
        {
            return doctors.Where(doctor => doctor.Name.ToUpper().Contains(substring.ToUpper(), StringComparison.Ordinal)).ToList();
        }
        public List<Model.Doctor> GetDoctorsByLastNameSubstring(List<Model.Doctor> doctors, string substring)
        {
            return doctors.Where(doctor => doctor.LastName.ToUpper().Contains(substring.ToUpper(), StringComparison.Ordinal)).ToList();
        }
        public void GetAllDoctors()
        {
            Doctors = JsonConvert.DeserializeObject<List<Model.Doctor>>(File.ReadAllText("../../../Data/doctors.json"));
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/doctors.json", JsonConvert.SerializeObject(Doctors,Formatting.Indented));
        }



    }
}
