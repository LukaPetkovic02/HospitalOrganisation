using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.VacationRequest.ViewModel.ApproveVacation
{
    public class VacationRequestDataViewModel: BaseViewModel
    {
        public Core.VacationRequest.Model.VacationRequest vacationRequest;

        private string doctorJmbg;
        public string DoctorJmbg
        {
            get { return doctorJmbg; }
            set
            {
                if (doctorJmbg != value)
                {
                    doctorJmbg = value;
                    OnPropertyChanged(nameof(DoctorJmbg));
                }
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string reason;
        public string Reason
        {
            get { return reason; }
            set
            {
                if (reason != value)
                {
                    reason = value;
                    OnPropertyChanged(nameof(Reason));
                }
            }
        }

        private string start;
        public string Start
        {
            get { return start; }
            set
            {
                if (start != value)
                {
                    start = value;
                    OnPropertyChanged(nameof(Start));
                }
            }
        }

        private string end;
        public string End
        {
            get { return end; }
            set
            {
                if (end != value)
                {
                    end = value;
                    OnPropertyChanged(nameof(End));
                }
            }
        }

        public VacationRequestDataViewModel(Core.VacationRequest.Model.VacationRequest vacationRequest)
        {
            this.vacationRequest = vacationRequest;
            doctorJmbg = vacationRequest.DoctorJmbg;
            status = vacationRequest.Status.ToString();
            id = vacationRequest.Id;
            reason = vacationRequest.Reason;
            start = vacationRequest.Start.ToString("d");
            end = vacationRequest.End.ToString("d");
        }
    }
}
