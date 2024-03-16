using System.Windows;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.View
{
    public partial class HospitalTreatmentView : Window
    {
        public HospitalTreatmentView(Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model.HospitalTreatment hospitalTreatment, PatientVisitationViewModel patientVisitationViewModel)
        {
            InitializeComponent();
            DataContext = new HospitalTreatmentViewModel(hospitalTreatment,this, patientVisitationViewModel);
        }
    }
}
