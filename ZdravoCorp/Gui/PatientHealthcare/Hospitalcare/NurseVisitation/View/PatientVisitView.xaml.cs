using System.Windows.Controls;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.NurseVisitation.View
{
    /// <summary>
    /// Interaction logic for PatientVisitView.xaml
    /// </summary>
    public partial class PatientVisitView : UserControl
    {
        public PatientVisitView()
        {
            InitializeComponent();
            DataContext = new PatientVisitViewModel();
            SearchTypeCB.Items.Add("room");
            SearchTypeCB.Items.Add("name");
            SearchTypeCB.SelectedIndex = 0;
        }
    }
}
