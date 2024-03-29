﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.Utilities.Nurse.Repository
{
    public class NurseRepository
    {
        public List<Model.Nurse> Nurses { get; set; }
        public PatientService patientService;

        public NurseRepository() 
        {
            GetAllNurses();
            patientService = new PatientService();
        }

        public Model.Nurse? FindByJmbg(string jmbg)
        {
            return Nurses.FirstOrDefault(nurse => nurse.Jmbg == jmbg);
        }

        public void GetAllNurses()
        {
            Nurses = JsonConvert.DeserializeObject<List<Model.Nurse>>(File.ReadAllText("../../../Data/nurses.json"));
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/nurses.json", JsonConvert.SerializeObject(Nurses, Formatting.Indented));
        }

        public bool ValidateJmbg(string jmbg)
        {
            return (jmbg.Length >= 9);
        }
        public bool JmbgExists(string jmbg)
        {
            return (patientService.FindByJmbg(jmbg) != null);
        }

        public bool ValidateName(string name)
        {
            return !(name.Any(c => !char.IsLetter(c)) || name.Length < 2);
        }

        public bool ValidateNewPatient(string jmbg, string name, string lastName, string password, string weight, string height, DateTime? birthDate)
        {
            bool valid = ValidatePatientInfo(jmbg, name, lastName, password, birthDate);
            valid = valid && !JmbgExists(jmbg);
            valid = valid && (weight.All(char.IsDigit));
            valid = valid && (height.All(char.IsDigit));

            return valid;
        }

        public bool ValidatePatientInfo(string jmbg, string name, string lastName, string password, DateTime? birthDate)
        {
            bool valid = true;
            valid = valid && ValidateJmbg(jmbg);
            valid = valid && ValidateName(name);
            valid = valid && ValidateName(lastName);
            valid = valid && password.Length > 2;
            valid = valid && birthDate.HasValue;

            return valid;
        }

        public MedicalCard CreateMedicalCard(string weight, string height)
        {
            return new MedicalCard(Convert.ToDouble(weight), Convert.ToDouble(height), new List<MedicalCondition>(), new List<String>());
        }
        public Patient.Model.Patient CreatePatient(string jmbg, string name, string lastName, string password, DateTime birthDate, string weight, string height)
        {
            return new Patient.Model.Patient(jmbg, name, lastName, password, birthDate, CreateMedicalCard(weight, height),TimeSpan.FromMinutes(30));
        }

    }
}
