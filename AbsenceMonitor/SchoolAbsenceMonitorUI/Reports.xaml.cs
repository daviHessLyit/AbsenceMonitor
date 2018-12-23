using SMAClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
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
        ValidationUtils validationUtils = new ValidationUtils();
        PupilUtils pupilUtils = new PupilUtils();
        SchoolClassUtils classUtils = new SchoolClassUtils();
        public List<Class> SchoolClasses { get; set; }
        private int selectedClass = 0;
        private int selectedReport = 0;
        List<Attendance> schoolAttendances = new List<Attendance>();

        // Enum including report options
        public enum ReportSelect
        {
            Class = 1,
            School = 2,
            ClassDate = 3,
            SchoolDate = 4
        }

        public Reports()
        {
            InitializeComponent();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           // Populate the class list on page loaded
           PopulateClassList();
        }

        private void PopulateClassList()
        {
            // Populate the dropdown menu
            SchoolClasses = smaDB.Classes.ToList();
            CmbBxClassSelector.ItemsSource = SchoolClasses;
        }

        private void MenuItemBreaches_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AbsenceBreaches.Visibility = Visibility.Visible;

            // Populate the listview with a list of pupils who have 10 or more absences.
            try
            {
                var warningPupils = smaDB.Pupils.Where(p => p.PupilAbsences.Count >= 10).ToList();
                LstVAbsenceBreaches.Items.Clear();
                LstVAbsenceBreaches.ItemsSource = warningPupils.OrderByDescending(p => p.PupilAbsences.Count);
                LstVAbsenceBreaches.Items.Refresh();
            }
            catch (Exception)
            {
                // Display an error on failure
                MessageBox.Show("Error in populating class list", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void MenuItemSchool_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SchoolReportSelector.Visibility = Visibility.Visible;           
        }

        private void MenuItemClass_Click(object sender, RoutedEventArgs e)
        {
            // Set up the report menu
            selectedReport = (int) ReportSelect.Class;
            Stk_ClassReportMenu.Visibility = Visibility.Visible;
            CmbBxClassSelector.Visibility = Visibility.Visible;
            LblCmbLabel.Visibility = Visibility.Visible;
            BtnReportGen.Visibility = Visibility.Collapsed;
            Dp_DateSelector.Visibility = Visibility.Hidden;
            Lbl_Date.Visibility = Visibility.Hidden;
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            PopulateClassList();
        }

        private void CmbBxClassSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {    
                // Pass the int value of the selected class
                selectedClass = Convert.ToInt16(CmbBxClassSelector.SelectedValue.ToString());

                // Check which type of report the selectedClass is being used for.
                if (selectedReport == 1)
                {
                    List<Pupil> pupilList = pupilUtils.GetPupilsByClass(selectedClass);
                    // Populate the Listview with the pupilList ordered by the absence count
                    LstVClassAbsence.ItemsSource = pupilList.OrderByDescending(p => p.PupilAbsences.Count);
                    LstVClassAbsence.Items.Refresh();
                    // Make List view visible
                    Stk_ClassReport.Visibility = Visibility.Visible; 
                }

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

            // Pass the selected pupil to the report write method
            var selectedPupil = LstVClassAbsence.SelectedItem;
            Tblk_ReportBlock.Text = WritePupilAbsenceReport((Pupil)selectedPupil);
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// Method writes out an Absence report for the selected pupil
        /// </summary>
        /// <param name="pupil">
        /// Pupil selected by the user
        /// </param>
        /// <returns>
        /// Pupil absence report
        /// </returns>
        private string WritePupilAbsenceReport(Pupil pupil)
        {
            // Initialise the StringBuilder
            StringBuilder textBlockText = new StringBuilder();

            // Write out information related to the pupil and add to the StringBuilder
            textBlockText.AppendLine($"Absence Report as of: {DateTime.Now.ToShortDateString()}");
            textBlockText.AppendLine($"Pupil Name: {pupil.FullName}");
            textBlockText.AppendLine($"Guardian Name: {pupil.Guardian.FullName}");
            textBlockText.AppendLine($"Guardian Address: {pupil.Guardian.Address}");
            textBlockText.AppendLine($"Guardian Address: {pupil.Guardian.MobileNo}");
            textBlockText.AppendLine($"Absence Count: {pupil.PupilAbsences.Count}");

            // Create a list of absences for the pupil ordered by the Absence date
            var absences = from _absence in smaDB.Absences.ToList()
                           join _pupilAbsence in smaDB.PupilAbsences on _absence.AbsenceId equals _pupilAbsence.AbsenceId
                           where _pupilAbsence.PupilId == pupil.PupilId
                           orderby _absence.AbsenceDate
                           select new
                           {
                               AbsenceDate = _absence.AbsenceDate.ToShortDateString(),
                               AbsenceReason = _absence.AbsenceType.AbsenceType1
                           };
            // Initialise the count variable
            int count = 0;
            // Loop through the absence list and add the details to the StringBuilder
            foreach (var absence in absences)
            {
                count++;
                textBlockText.AppendLine("");
                textBlockText.AppendLine("Absence Number: "+count);
                textBlockText.AppendLine($"Absence Date: {absence.AbsenceDate}");
                textBlockText.AppendLine($"Absence Reason: {absence.AbsenceReason}");
            }

            // Return the completed string
            return textBlockText.ToString();
        }

        private void BtnResetForm_Click(object sender, RoutedEventArgs e)
        {
            // Reset various page elements
            Stk_AbsenceBreaches.Visibility = Visibility.Collapsed;
            StkPupilReport.Visibility = Visibility.Collapsed;
            Stk_ClassReport.Visibility = Visibility.Collapsed;
            Stk_SchoolReport.Visibility = Visibility.Collapsed;
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;
            Stk_MenuPanel.Visibility = Visibility.Visible;
            Tblk_ReportBlock.Text = "";
            Tblk_SchoolReportBlock.Text = "";
            Dp_DateSelector.Text = "";
            selectedClass = 0;
            selectedReport = 0;
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            // Make the class list visible again
            StkPupilReport.Visibility = Visibility.Collapsed;
            Stk_ClassReport.Visibility = Visibility.Visible ;
            // Clear up the textBlock text.
            Tblk_ReportBlock.Text = "";
        }

        private void MenuFullSchooDate_Click(object sender, RoutedEventArgs e)
        {
            // Make the school report selection screen visible
            Dp_DateSelector.Text = "";
            Stk_SchoolReportSelector.Visibility = Visibility.Collapsed;
            Stk_ClassReportMenu.Visibility = Visibility.Visible;
            Dp_DateSelector.Visibility = Visibility.Visible;
            Lbl_Date.Visibility = Visibility.Visible;
            CmbBxClassSelector.Visibility = Visibility.Collapsed;
            LblCmbLabel.Visibility = Visibility.Collapsed;
            BtnReportGen.Visibility = Visibility.Visible;
            selectedReport = (int)ReportSelect.SchoolDate;           
        }

        private void MenuClassDate_Click(object sender, RoutedEventArgs e)
        {
            // Make the class report selection screen visible
            Dp_DateSelector.Text = "";
            Stk_SchoolReportSelector.Visibility = Visibility.Collapsed;
            Stk_ClassReportMenu.Visibility = Visibility.Visible;
            CmbBxClassSelector.Visibility = Visibility.Visible;
            PopulateClassList();
            Dp_DateSelector.Visibility = Visibility.Visible;
            Lbl_Date.Visibility = Visibility.Visible;
            BtnReportGen.Visibility = Visibility.Visible;
            selectedReport = (int)ReportSelect.ClassDate;
        }

        private void BtnReportGen_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the count variables and the StringBuilder
            int attending = 0;
            int absent = 0;
            int classCount = 0;
            int totalAttendance = 0;
            int totalAbsence = 0;
            string className = "";
            StringBuilder schoolReportBuilder = new StringBuilder();

            try
            {
                // Generate report based on the selected type
                DateTime selectedDate = new DateTime();
                selectedDate = Dp_DateSelector.SelectedDate.Value;
                if (selectedReport == 3)
                {

                    // For the class report we need both the date and the selected class to be available, send error message if not both selected.
                    if (!validationUtils.VerifyDate(selectedDate))
                    {
                        MessageBox.Show("Select a Valid Date, Date cannot in the future", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (selectedClass == 0)
                    {
                        MessageBox.Show("Select a class from the dropdown", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        // Initialise the StringBuilder
                        schoolReportBuilder = new StringBuilder();

                        // Get the user's class selection
                        var reportingClass = classUtils.GetSchoolClass(selectedClass);

                        // Get the attendance for the selected date
                        var attendance = smaDB.Attendances.Where(a => a.ClassId == reportingClass.ClassId && DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(selectedDate)).FirstOrDefault();

                        // populate the variables
                        classCount = reportingClass.Pupils.Count;
                        attending = attendance.PupilAttendances.Count;
                        absent = classCount - attending;
                        className = reportingClass.ClassName;

                        // Generate the report text
                        schoolReportBuilder.AppendLine($"Attendance Report for: {selectedDate.ToShortDateString()}");
                        schoolReportBuilder.AppendLine("");
                        schoolReportBuilder.AppendLine($"Class Summary for: {className}");
                        schoolReportBuilder.AppendLine($"Pupils attending: {attending}");
                        schoolReportBuilder.AppendLine($"Pupils absent: {absent}");

                        // Make the report visible to the user.
                        Tblk_SchoolReportBlock.Text = schoolReportBuilder.ToString();
                        Stk_SchoolReport.Visibility = Visibility.Visible;

                    }
                }

                if (selectedReport == 4)
                {
                    // Initialise the StringBuilder
                    schoolReportBuilder = new StringBuilder();

                    // Full school report to be generated for the selected date
                    if (!validationUtils.VerifyDate(selectedDate))
                    {
                        MessageBox.Show("Select a Valid Date, Date cannot in the future", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        // Get the list of attendances for the selected date
                        var attendances = smaDB.Attendances.Where(a => DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(selectedDate)).ToList();

                        // Begin populating the report information in the stringBuilder
                        schoolReportBuilder.AppendLine($"Attendance Report for: {selectedDate.ToShortDateString()}");

                        // Loop through the attendances and populate the report information
                        foreach (var attendance in attendances)
                        {
                            attending = attendance.PupilAttendances.Count;
                            classCount = attendance.Class.Pupils.Count;
                            absent = classCount - attending;
                            className = attendance.Class.ClassName;
                            totalAbsence = totalAbsence + absent;
                            totalAttendance = totalAttendance + attending;
                            schoolReportBuilder.AppendLine("");
                            schoolReportBuilder.AppendLine($"Class Summary for: {className}");
                            schoolReportBuilder.AppendLine($"Pupils attending: {attending}");
                            schoolReportBuilder.AppendLine($"Pupils absent: {absent}");
                        }

                        // Add the final summary line
                        schoolReportBuilder.AppendLine("");
                        schoolReportBuilder.AppendLine($"Full school summary for: {selectedDate.ToShortDateString()}");
                        schoolReportBuilder.AppendLine($"Total Pupils attending: {totalAttendance}");
                        schoolReportBuilder.AppendLine($"Total Pupils absent: {totalAbsence}");

                        // Make the report visible to the user.
                        Tblk_SchoolReportBlock.Text = schoolReportBuilder.ToString();
                        Stk_SchoolReport.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Select a Date, date cannot be empty", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
    }
}
