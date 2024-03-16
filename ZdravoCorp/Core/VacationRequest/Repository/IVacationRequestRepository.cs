using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.VacationRequest.Repository
{
    public interface IVacationRequestRepository
    {
        public List<Model.VacationRequest> VacationRequests { get; set; }

        public void CreateVacationRequest(string doctorJmbg, string reason, DateTime start, DateTime end);

        public string GenerateVacationRequestId();

        public bool IsAvailable(string doctorJmbg, DateTime startDate, DateTime endDate);

        public bool IsOnVacation(string doctorJmbg);

        public Model.VacationRequest? FindVacationRequestById(string id);

        public void GetAllVacationRequests();

        public void Save();

    }
}
