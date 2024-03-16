using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.Command
{
    using Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;
    public class SearchPatientsVisitCommand : BaseCommand
    {
        private readonly PatientVisitViewModel _viewModel;
        public HospitalTreatmentService _hospitalTreatmentService;
        public event EventHandler<bool> SearchPatientsVisit;

        public SearchPatientsVisitCommand(PatientVisitViewModel viewModel)
        {
            _viewModel = viewModel;
            _hospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
        }

        public bool CanExecute(object? parameter)
        {
            return (_viewModel.SelectedSearchType == "room" && Regex.IsMatch(_viewModel.SearchInput, @"^\d+$") ||
                _viewModel.SelectedSearchType == "name" && Regex.IsMatch(_viewModel.SearchInput, @"^[a-zA-Z]+$"));
        }

        public override void Execute(object parameter)
        {
            _hospitalTreatmentService = new HospitalTreatmentService(new HospitalTreatmentRepository());
            if (CanExecute(parameter))
            {
                if(_viewModel.SelectedSearchType == "room")
                {
                    _viewModel.PatientsToVisit = new ObservableCollection<HospitalTreatment>(_hospitalTreatmentService.GetTreatmentsInGivenRoom(_viewModel.SearchInput));
                }
                else
                {
                    _viewModel.PatientsToVisit = new ObservableCollection<HospitalTreatment>(_hospitalTreatmentService.GetTreatmentsForGivenName(_viewModel.SearchInput));
                }
                SearchPatientsVisit?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("Wrong input.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SearchPatientsVisit?.Invoke(this, false);
            }

        }
    }
}
