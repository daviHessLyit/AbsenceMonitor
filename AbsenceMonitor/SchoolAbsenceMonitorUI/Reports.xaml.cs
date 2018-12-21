using SMAClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolAbsenceMonitorUI
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        public SystemUser systemUser = new SystemUser();
        PupilUtils pupilUtils = new PupilUtils();
        public List<Class> SchoolClasses { get; set; }
        private int selectedClass = 0;

        public Reports()
        {
            InitializeComponent();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateClassList();
        }

        private void PopulateClassList()
        {
            CmbBxClassSelector.Items.Clear();
            SchoolClasses = smaDB.Classes.ToList();
            CmbBxClassSelector.ItemsSource = SchoolClasses;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Visible;
            Stk_AbsenceWarnings.Visibility = Visibility.Hidden;
            Stk_AbsenceBreaches.Visibility = Visibility.Hidden;
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;
            Stk_ClassReport.Visibility = Visibility.Hidden;
            Stk_PupilReport.Visibility = Visibility.Hidden;
            Stk_SchoolReport.Visibility = Visibility.Hidden;
        }        

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemWarnings_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AbsenceWarnings.Visibility = Visibility.Visible;
        }

        private void MenuItemBreaches_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AbsenceBreaches.Visibility = Visibility.Visible;
        }

        private void MenuItemPupil_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
        }

        private void MenuItemSchool_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SchoolReport.Visibility = Visibility.Visible;
        }

        private void MenuItemClass_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_ClassReportMenu.Visibility = Visibility.Visible;
        }

        private void MenuItem_Special_Click(object sender, RoutedEventArgs e)
        {
            Stk_ClassReport.Visibility = Visibility.Visible;
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;
        }
        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Stk_PupilReport.Visibility = Visibility.Visible;
        }

        private void CmbBxClassSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {    
                // Pass the int value of the selected class
                selectedClass = Convert.ToInt16(CmbBxClassSelector.SelectedValue.ToString());

                List<Pupil> pupilList = pupilUtils.GetPupilsByClass(selectedClass);

                //LstVClassAbsence.Items.Clear();
                LstVClassAbsence.ItemsSource = pupilList.OrderByDescending(p=> p.PupilAbsences.Count);
                LstVClassAbsence.Items.Refresh();

                Stk_ClassReport.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                // Display an error on failure
                MessageBox.Show("Error in populating class list", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPupilSelect_Click(object sender, RoutedEventArgs e)
        {
            StkPupilReport.Visibility = Visibility.Visible;
            Stk_ClassReport.Visibility = Visibility.Collapsed;

            var selectedPupil = LstVClassAbsence.SelectedItem;
            Tblk_ReportBlock.Padding = new Thickness(10, 10,10,10);
            Tblk_ReportBlock.FontSize = 14;
            Tblk_ReportBlock.Text = WritePupilAbsenceReport((Pupil)selectedPupil);
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;

        }

        private string WritePupilAbsenceReport(Pupil pupil)
        {
            StringBuilder textBlockText = new StringBuilder();
            textBlockText.AppendLine($"Absence Report as of: {DateTime.Now.ToShortDateString()}");
            textBlockText.AppendLine($"Pupil Name: {pupil.FullName}");
            textBlockText.AppendLine($"Guardian Name: {pupil.Guardian.FullName}");
            textBlockText.AppendLine($"Absence Count: {pupil.PupilAbsences.Count}");

            var absences = from _absence in smaDB.Absences.ToList()
                           join _pupilAbsence in smaDB.PupilAbsences on _absence.AbsenceId equals _pupilAbsence.AbsenceId
                           where _pupilAbsence.PupilId == pupil.PupilId
                           orderby _absence.AbsenceDate
                           select new
                           {
                               AbsenceDate = _absence.AbsenceDate.ToShortDateString(),
                               AbsenceReason = _absence.AbsenceType.AbsenceType1
                           };
            int count = 0;
            foreach (var absence in absences)
            {
                count++;
                textBlockText.AppendLine("");
                textBlockText.AppendLine("Absence Number: "+count);
                textBlockText.AppendLine($"Absence Date: {absence.AbsenceDate}");
                textBlockText.AppendLine($"Absence Reason: {absence.AbsenceReason}");
            }


            return textBlockText.ToString();
        }

        private void BtnResetForm_Click(object sender, RoutedEventArgs e)
        {            
            StkPupilReport.Visibility = Visibility.Collapsed;
            Stk_ClassReport.Visibility = Visibility.Collapsed;
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;
            Stk_MenuPanel.Visibility = Visibility.Visible;
            Tblk_ReportBlock.Text = "";
            selectedClass = 0;
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            StkPupilReport.Visibility = Visibility.Collapsed;
            Stk_ClassReport.Visibility = Visibility.Visible ;
            Tblk_ReportBlock.Text = "";
        }
    }
}
