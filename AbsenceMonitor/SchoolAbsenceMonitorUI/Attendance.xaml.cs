using SMAClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Interaction logic for Attendance.xaml
    /// </summary>
    public partial class Attendance : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        PupilUtils pupilUtils = new PupilUtils();
        public SystemUser systemUser = new SystemUser();
        public ValidationUtils validationUtils = new ValidationUtils();
        public List<AbsenceType> AbsenceTypes { get; set; }
        public List<Class> SchoolClasses { get; set; }
        public List<Pupil> attendingPupils = new List<Pupil>();
        public List<Pupil> absentPupils = new List<Pupil>();
        public int selectedClass = 0;
        public int absenceCount = 0;
        public DateTime selectedDate = new DateTime();
        public string absenceReport = "";
        public string selectedReportingClass = "";


        public Attendance()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate the list of classes in the dropdown on page load.
            PopulateClassList();
        }

        private void PopulateClassList()
        {
            // Populate the list of classes
            SchoolClasses = smaDB.Classes.ToList();
            CmbBxClassSelector.ItemsSource = SchoolClasses;
        }

        private void PopulateAbsenceTypeList()
        {
            // Populate the list of AbsenceTypes
            AbsenceTypes = smaDB.AbsenceTypes.ToList();
            CmbBxAbsenceTypeSelector.ItemsSource = AbsenceTypes;
        }

        private void CmbBxClassSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {        
            try
            {
                LblConfirmation.Visibility = Visibility.Collapsed;
                var reportingClass = (Class)CmbBxClassSelector.SelectedItem;
                selectedReportingClass = reportingClass.ClassName;
                // Pass the int value of the selected class
                selectedClass = Convert.ToInt16(CmbBxClassSelector.SelectedValue.ToString());

                // Create a list of pupil objects
                var pupilList = pupilUtils.GetPupilsByClass(selectedClass);

                // Populate the ListView with pupils from the selected class
                LstPupilAttendance.ItemsSource = pupilList;

                // Set all the rows in the listview to selected by default.
                LstPupilAttendance.SelectAll();
                
                // Empty the lists of attending and absent pupils
                if (attendingPupils.Count >0)
                {
                    attendingPupils.Clear();
                }

                if (absentPupils.Count >0)
                {
                    absentPupils.Clear();
                } 

                // Make the stackpanel containing the list of pupils visible
                Stk_ClassList.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                // Display an error on failure
                MessageBox.Show("Error in populating class list", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelAttendance_Click(object sender, RoutedEventArgs e)
        {
            // Make the intial page available and repopulate the class dropdown
            Stk_ClassList.Visibility = Visibility.Hidden;
            PopulateClassList();
        }

        private void BtnConfirmAttendance_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Initialise the date field, this will be passed in recording both the attendance and absences
                selectedDate = Dp_DateSelector.SelectedDate.Value;
                if (!validationUtils.VerifyDate(selectedDate))
                {
                    // Show an error if the date wasn't valid
                    MessageBox.Show("Select a Valid Date, Date cannot in the future", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    /*
                     * Check if the attendance date and class id already exist on the database
                     * Display an error if they do
                     * This check should also prevent absences being set for the same date incorrectly because an absence can't be added unless passing this check
                     */
                    if (smaDB.Attendances.Any(a => DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(selectedDate) && a.ClassId == selectedClass))
                    {
                        // Show an error if database already has an attendance recorded for that class on the selected date.
                        MessageBox.Show("Attendance already recorded for this class  on the selected date", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        // Valid date selected so attendances and absences can now be recorded

                        // Clear the list of attending pupils
                        attendingPupils.Clear();

                        // Populate the list of absent pupils with all pupils from the selected class
                        absentPupils = pupilUtils.GetPupilsByClass(selectedClass);

                        // Loop through the selected pupils from the list view, add seleted pupils to the attending list and remove from the absent list.
                        foreach (var item in LstPupilAttendance.SelectedItems)
                        {
                            attendingPupils.Add((Pupil)item);
                            absentPupils.Remove((Pupil)item);
                        }

                        absenceCount = absentPupils.Count;
                        // If there are pupils in the attending list create an attendance object and submit the attendances for all pupils to the database
                        if (attendingPupils.Count > 0)
                        {
                            try
                            {
                                int attendanceAdded = pupilUtils.RecordPupilAttendance(selectedDate, selectedClass, attendingPupils);
                                // Check if the amount of records added matches the count of attending pupils
                                if (attendanceAdded == attendingPupils.Count)
                                {
                                    // Update the system logs if the attendances were recorded successfully
                                    try
                                    {
                                        systemEventUtils.AddSystemEvent(new SystemEvent
                                        {
                                            UserId = systemUser.UserId,
                                            EventTypeId = 3,
                                            EventDateTime = DateTime.Now,
                                            EventData = $"Attendances added at { DateTime.Now} , by {systemUser.Username}"
                                        });
                                    }
                                    catch (EntityException)
                                    {
                                        // Show an error on failure
                                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    // Update the system logs if the attendances weren't recorded successfully
                                    try
                                    {
                                        systemEventUtils.AddSystemEvent(new SystemEvent
                                        {
                                            UserId = systemUser.UserId,
                                            EventTypeId = 1006,
                                            EventDateTime = DateTime.Now,
                                            EventData = $"Problem adding Pupil attendance records at { DateTime.Now} , by {systemUser.Username}"
                                        });
                                    }
                                    catch (EntityException)
                                    {
                                        // Show an error on failure
                                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }

                            }
                            catch (Exception)
                            {
                                // Show an error on failure
                                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        /*
                         * If there are absent pupils we need to loop through and create pupilAbsence objects for each one.
                         * Initial create an absence object with the date and absence reason added. Then create the absence reason with the absenceId and pupilId added.
                         */
                        if (absentPupils.Count > 0)
                        {
                            try
                            {
                                // There are pupils absent from the selected class so individual reasons 
                                StkConfirmationPanel.Visibility = Visibility.Visible;
                                Stk_ClassList.Visibility = Visibility.Hidden;
                                Stk_MenuPanel.Visibility = Visibility.Hidden;
                                RefreshAbsentPupilList(absentPupils);                              
                                PopulateAbsenceTypeList();
                                LblPageHeading.Content = "Absence Reporting";
                                Lbl_ComfirmationLbl.Content = $"Absence Report for: {selectedReportingClass}";


                            }
                            catch (Exception)
                            {
                                // Show an error on failure
                                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            // If there are no absences just display a success message for the attendance reporting.
                            Stk_ClassList.Visibility = Visibility.Hidden;
                            Stk_MenuPanel.Visibility = Visibility.Visible;
                            LblConfirmation.Visibility = Visibility.Visible;
                            PopulateClassList();
                        }
                    }
                }

               
            }
            catch (Exception)
            {
                // Show an error on validation failure
                MessageBox.Show("Select a Valid Date, Date cannot be empty", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }

        private void ChkPresent_Checked(object sender, RoutedEventArgs e)
        {
            // Change the checkbox text and colour if checked
            CheckBox cb = (CheckBox)sender;
            // Set the content back to present, assumes a checkbox may have been unchecked accidentally
            cb.Content = "Present";
            cb.Foreground = Brushes.Black;
        }

        private void ChkPresent_Unchecked(object sender, RoutedEventArgs e)
        {
            // Change the checkbox text and colour if unchecked.
            CheckBox cb = (CheckBox)sender;
            // Mark the pupil as absent if checked.
            cb.Content = "Absent";
            cb.Foreground = Brushes.Red;
        }

        private void BtnConfirmAbsence_Click(object sender, RoutedEventArgs e)
        {
          
            // The user can submit absence reports for each of the pupils marked as absent at this point.
            if (absentPupils.Count >0)
            {
                try
                {
                    // The pupil selected from the listview
                    Pupil absentPupil = (Pupil)LstPupilAbsence.SelectedItem;

                    // The absence type selected from the dropdown
                    var currentAbsenceType = (AbsenceType)CmbBxAbsenceTypeSelector.SelectedItem;

                    // Record the absence on the database.
                    int absenceAdded = pupilUtils.AddPupilAbsence(new Absence
                    {
                        AbsenceTypeId = currentAbsenceType.AbsenceTypeId,
                        AbsenceDate = selectedDate.Date
                    }, absentPupil.PupilId);

                    // If the record is sucessfully added record it in the system logs.
                    if (absenceAdded == 1)
                    {
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 3,
                                EventDateTime = DateTime.Now,
                                EventData = $"Absence added at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show an error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // If there was a problem in the save operation try and update the system logs.
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Problem adding Pupil absence record at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show an error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Populate the variable that will be shown back to the user with the absence details being reported.
                    absenceReport = absenceReport + $"{absentPupil.FullName} has been reported absent on {selectedDate.ToShortDateString()}: "
                       + Environment.NewLine + $"Absence Reason: {currentAbsenceType.AbsenceType1}"
                       + Environment.NewLine;

                    // Remove the student from the Listview after their details are recorded
                    absentPupils.Remove(absentPupil);
                    // Refresh the Listview
                    RefreshAbsentPupilList(absentPupils);
                    // Print the selected pupils details to screen 
                    Tblk_ReportBlock.Margin = new Thickness(10);
                    Tblk_ReportBlock.Text = absenceReport;

                }
                catch (Exception)
                {
                    // Show a message to the user to ensure all required fields are selected
                    MessageBox.Show("Make sure you have a pupil and absence reason selected", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                // Display the final report to the user and make the submit report button available
                string finalReport = Environment.NewLine + $"FINAL REPORT for {selectedReportingClass}" + Environment.NewLine + $"{attendingPupils.Count} attending school and {absenceCount} " +
                    $"reported absent on {selectedDate.ToShortDateString()} ";

                Tblk_ReportBlock.Text = absenceReport + finalReport;
                LstPupilAbsence.Visibility = Visibility.Collapsed;
                StkPupilConfirmation.Visibility = Visibility.Collapsed;
                BtnSubmitReport.Visibility = Visibility.Visible;
            }
           
        }

        private void RefreshAbsentPupilList(List<Pupil> absentPupils)
        {
            // Refresh the list 
            LstPupilAbsence.ItemsSource = absentPupils;
            LstPupilAbsence.Items.Refresh();
        }

        private void BtnSubmitReport_Click(object sender, RoutedEventArgs e)
        {
            // Return to the main Attendance view and make the success message visible.
            StkPupilConfirmation.Visibility = Visibility.Visible;
            StkConfirmationPanel.Visibility = Visibility.Hidden;
            Tblk_ReportBlock.Text = "";
            absenceReport = "";
            BtnSubmitReport.Visibility = Visibility.Collapsed;
            LstPupilAbsence.Visibility = Visibility.Visible;
            Stk_MenuPanel.Visibility = Visibility.Visible;            
            LblConfirmation.Visibility = Visibility.Visible;
            PopulateClassList();

        }
    }
}
