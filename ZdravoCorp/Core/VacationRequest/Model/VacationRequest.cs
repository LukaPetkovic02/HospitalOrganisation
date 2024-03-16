using System;

namespace ZdravoCorp.Core.VacationRequest.Model
{
    public class VacationRequest
    {
        public enum VacationStatus {ACCEPTED,DECLINED,FINISHED,WAITING}

        public String DoctorJmbg;
        public VacationStatus Status;
        public String Id;
        public String Reason;
        public DateTime Start;
        public DateTime End;

        public VacationRequest(string doctorJmbg, string id, string reason, DateTime start, DateTime end)
        {
            DoctorJmbg = doctorJmbg;
            Id = id;
            Reason = reason; 
            Start = start;
            End = end;
            Status = VacationStatus.WAITING;
        }


    }
}
