using System;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.VacationRequest;
using ZdravoCorp.Core.VacationRequest.Repository;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.VacationRequest.ViewModel;

namespace ZdravoCorp.Gui.VacationRequest.Command
{
    using Core.Utilities.Doctor.Model;
    public class RequestVacationCommand : BaseCommand
    {
        private readonly VacationRequestViewModel _viewModel;
        private readonly Doctor _doctor;
        private VacationRequestService _vacationRequestService;
        public event EventHandler<bool> VacationRequested;
        public ScheduleService ScheduleService;

        public RequestVacationCommand(VacationRequestViewModel viewModel, Doctor doctor)
        {
            _viewModel = viewModel;
            _doctor = doctor;
            _vacationRequestService = new VacationRequestService(new VacationRequestRepository());
            ScheduleService = new ScheduleService();
        }

        public bool CanExecute(object? parameter)
        {
            return !(ScheduleService.IsAppointmentInRange(_doctor.Jmbg, _viewModel.StartDate, _viewModel.EndDate) || !_vacationRequestService.IsAvailable(_doctor.Jmbg, _viewModel.StartDate.Date, _viewModel.EndDate.Date) || 
                     string.IsNullOrEmpty(_viewModel.Reason) || _viewModel.StartDate.Date < DateTime.Now.AddDays(1) || _viewModel.EndDate < _viewModel.StartDate);
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                ScheduleService = new ScheduleService();
                _vacationRequestService.CreateVacationRequest(_doctor.Jmbg, _viewModel.Reason,
                    _viewModel.StartDate.Date, _viewModel.EndDate.Date);
                
                VacationRequested?.Invoke(this,true);
            }
            else
            {
                
                VacationRequested?.Invoke(this, false);
            }

        }

    }
}
