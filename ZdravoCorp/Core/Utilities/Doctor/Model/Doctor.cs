using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Utilities.Doctor.Model
{
    public class Doctor : Person.Model.Person
    {
        public enum Specialty
        {
            GENERAL,
            CARDIOLOGIST,
            DERMATOLOGIST,
            NEUROLOGIST,
            SURGEON
        }

        [JsonProperty("Specialization")]
        public Specialty Specialization { get; set; }
        [JsonProperty("Ratings")]
        public List<int> Ratings;
        [JsonProperty("AverageScore")]
        public double AverageScore;
        [JsonConstructor]
        public Doctor(string jmbg, string name, string lastName, string password, DateTime birthDate, Specialty specialization, List<int> ratings, double averageScore) : base(jmbg, name, lastName, password, birthDate)
        {
            Specialization = specialization;
            Ratings = ratings;
            AverageScore = averageScore;
        }

        public Doctor(){}
    };

}

