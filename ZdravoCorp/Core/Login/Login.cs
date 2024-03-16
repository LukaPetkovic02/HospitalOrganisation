using System.Linq;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Core.Utilities.Nurse.Model;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.Login
{
    public class Login
    {
        public string Jmbg;
        public string Password;
        public DoctorService DoctorService;
        public NurseService NurseService;
        public PatientService PatientService;

        public Login(string jmbg, string password)
        {
            Jmbg = jmbg;
            Password = password;
            DoctorService = new DoctorService();
            NurseService = new NurseService();
            PatientService = new PatientService();
        }

        public Doctor? CheckDoctorLogin()
        {
            return DoctorService.Doctors().FirstOrDefault(doctor => doctor.Jmbg == Jmbg && doctor.Password == Password);
        }

        public Patient? CheckPatientLogin()
        {
            return PatientService.AllPatients().FirstOrDefault(patient => patient.Jmbg == Jmbg && patient.Password == Password);
        }

        public Nurse? CheckNurseLogin()
        {
            return NurseService.Nurses().FirstOrDefault(nurse => nurse.Jmbg == Jmbg && nurse.Password == Password);
        }

    }
}
