using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Patient;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository
{
    public class HospitalTreatmentRepository : IHospitalTreatmentRepository
    {
        public List<Model.HospitalTreatment> HospitalTreatments { get; set; }

        public HospitalTreatmentRepository()
        {
            GetAllHospitalTreatments();
        }

        public Model.HospitalTreatment? FindHospitalTreatmentById(string id)
        {
            return HospitalTreatments.FirstOrDefault(treatmentAccommodation => treatmentAccommodation.Id == id);
        }

        public void FinishHospitalTreatment(Model.HospitalTreatment treatment)
        {
            foreach (var hospitalTreatment in HospitalTreatments.Where(hospitalTreatment => hospitalTreatment.Id == treatment.Id))
            {
                hospitalTreatment.Finish();
                break;
            }
            Save();
        }

        public void ChangeHospitalTreatmentTherapy(Model.HospitalTreatment treatment, string therapy)
        {
            foreach (var hospitalTreatment in HospitalTreatments.Where(hospitalTreatment => treatment.Id == hospitalTreatment.Id))
            {
                hospitalTreatment.Therapy = therapy;
                break;
            }
            Save();
        }

        public void ExtendHospitalTreatment(Model.HospitalTreatment treatment, int days)
        {
            foreach (var hospitalTreatment in HospitalTreatments.Where(hospitalTreatment => hospitalTreatment.Id == treatment.Id))
            {
                hospitalTreatment.End = hospitalTreatment.End.AddDays(days);
                break;
            }
            Save();
        }
      
        public void CreateHospitalTreatment(string referralId, string patientJmbg, DateTime start, DateTime end, string therapy, string roomId)
        {
            HospitalTreatments.Add(new Model.HospitalTreatment(GenerateHospitalTreatmentId(), referralId, patientJmbg, start, end, therapy, roomId));
            Save();
        }

        public string GenerateHospitalTreatmentId()
        {
            while (true)
            {
                string accommodationId = Generator.GenerateRandomId();

                if (!(FindHospitalTreatmentById(accommodationId) == null))
                {
                    continue;
                }
                return accommodationId;
            }
        }

        public List<Model.HospitalTreatment>? GetTreatmentsInGivenRoom(string room)
        {
            return HospitalTreatments.Where(t => t.Status == Model.HospitalTreatment.TreatmentStatus.INPROGRESS && t.RoomId == room).ToList();
        }

        public List<Model.HospitalTreatment>? GetTreatmentsForGivenName(string name)
        {
            PatientService patientService = new PatientService();
            return HospitalTreatments.Where(t => t.Status == Model.HospitalTreatment.TreatmentStatus.INPROGRESS &&
                patientService.FindByJmbg(t.PatientJmbg).Name == name).ToList();
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/hospitaltreatments.json", JsonConvert.SerializeObject(HospitalTreatments, Formatting.Indented));
        }

        public void GetAllHospitalTreatments()
        {
            HospitalTreatments = JsonConvert.DeserializeObject<List<Model.HospitalTreatment>>(File.ReadAllText("../../../Data/hospitaltreatments.json"));
        }
    }
}
