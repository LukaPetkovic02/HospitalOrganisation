using System;

namespace ZdravoCorp.Core.Utilities.Nurse.Model
{
    public class Nurse : Person.Model.Person
    {
        public Nurse(string jmbg, string name, string lastName, string password, DateTime birthDate) : base(jmbg, name, lastName, password, birthDate) { }
    
    }
}
