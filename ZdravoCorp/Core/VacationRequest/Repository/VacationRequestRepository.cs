using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;
using Formatting = Newtonsoft.Json.Formatting;

namespace ZdravoCorp.Core.VacationRequest.Repository
{
    public class VacationRequestRepository : IVacationRequestRepository
    {
        public List<Model.VacationRequest> VacationRequests { get; set; }

        public VacationRequestRepository()
        {
            GetAllVacationRequests();
        }

        public void CreateVacationRequest(string doctorJmbg, string reason, DateTime start, DateTime end)
        {
            VacationRequests.Add(new Model.VacationRequest(doctorJmbg, GenerateVacationRequestId(), reason, start, end));
            Save();

        }

        public string GenerateVacationRequestId()
        {
            while (true)
            {
                string vacationRequestId = Generator.GenerateRandomId();

                if (!(FindVacationRequestById(vacationRequestId) == null))
                {
                    continue;
                }
                return vacationRequestId;
            }
        }

        public bool IsAvailable(string doctorJmbg,DateTime startDate,DateTime endDate)
        {
            return VacationRequests.Where(request => request.DoctorJmbg == doctorJmbg).All(request => startDate > request.End || request.Start > endDate);
        }

        public bool IsOnVacation(string doctorJmbg)
        {
            return VacationRequests.Any(vacationRequest => vacationRequest.DoctorJmbg == doctorJmbg && vacationRequest.Status == Model.VacationRequest.VacationStatus.ACCEPTED && vacationRequest.Start <= DateTime.Now && DateTime.Now <= vacationRequest.End);
        }

        public Model.VacationRequest? FindVacationRequestById(string id)
        {
            return VacationRequests.FirstOrDefault(vacationRequest => vacationRequest.Id == id);
        }

        public void GetAllVacationRequests()
        {
            VacationRequests = JsonConvert.DeserializeObject<List<Model.VacationRequest>>(File.ReadAllText("../../../Data/vacationRequests.json"));
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/vacationRequests.json", JsonConvert.SerializeObject(VacationRequests, Formatting.Indented));
        }
    }
}
