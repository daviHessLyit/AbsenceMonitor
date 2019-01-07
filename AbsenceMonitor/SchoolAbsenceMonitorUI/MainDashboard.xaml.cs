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
using System.Windows.Shapes;

namespace SchoolAbsenceMonitorUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainDashboard : Window
    {
        public SystemUser systemUser = new SystemUser();

        public MainDashboard()
        {
            InitializeComponent();
        }

        private void BtnExitApp_Click(object sender, RoutedEventArgs e)
        {
            // Close the application 
            Close();
            Environment.Exit(0);
        }

        private void Tbi_Attendance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Initialise the attedance page and open in the form
            Attendance attendance = new Attendance
            {
                systemUser = this.systemUser
            };
            frmMain.Navigate(attendance);
        }

        private void Tbi_Reporting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Initialise the reports page and open in the form
            Reports reports = new Reports
            {
                systemUser = this.systemUser
            };
            frmMain.Navigate(reports);
        }

        private void Tbi_ApplicationLogs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Initialise the applicationLogs page and open in the form
            ApplicationLogs applicationLogs = new ApplicationLogs();
            frmMain.Navigate(applicationLogs);
        }

        /// <summary>
        /// Method checks the access level for the system user
        /// </summary>
        /// <param name="user">
        /// System user
        /// </param>
        private void CheckSystemUserAccess(SystemUser user)
        {
            // Show application options based on user access
            if(user.AccessLevelId == 1)
            {
                Tbi_Reporting.Visibility = Visibility.Visible;
                Tbi_Admin.Visibility = Visibility.Visible;
                Tbi_ApplicationLogs.Visibility = Visibility.Visible;
            }
            else if (user.AccessLevelId == 2)
            {
                Tbi_Reporting.Visibility = Visibility.Visible;
                Tbi_Admin.Visibility = Visibility.Collapsed;
                Tbi_ApplicationLogs.Visibility = Visibility.Collapsed;
            }
            else
            {
                Tbi_Reporting.Visibility = Visibility.Collapsed;
                Tbi_Admin.Visibility = Visibility.Collapsed;
                Tbi_ApplicationLogs.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Check the user access
            CheckSystemUserAccess(systemUser);
        }

        private void BtnUserAdd_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the user add form and display
            Admin admin = new Admin();
            admin.Stk_AddUser.Visibility = Visibility.Visible;
            admin.systemUser = this.systemUser;
            frmMain.Navigate(admin);
        }

        private void BtnUserUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the user search form and display
            Admin admin = new Admin();
            admin.Stk_SearchUser.Visibility = Visibility.Visible;
            admin.systemUser = this.systemUser;
            frmMain.Navigate(admin);
        }

        private void BtnGuardAdd_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the guardian add form and display
            GuardianAdmin guardianAdmin = new GuardianAdmin();
            guardianAdmin.Stk_AddGuardian.Visibility = Visibility.Visible;
            guardianAdmin.systemUser = this.systemUser;
            frmMain.Navigate(guardianAdmin);
        }

        private void BtnGuardUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the guardian search form and display
            GuardianAdmin guardianAdmin = new GuardianAdmin();
            guardianAdmin.Stk_SearchGuardian.Visibility = Visibility.Visible;
            guardianAdmin.systemUser = this.systemUser;
            frmMain.Navigate(guardianAdmin);
        }

        private void BtnPupilAdd_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the pupil add form and display
            PupilAdmin pupilAdmin = new PupilAdmin();
            pupilAdmin.Stk_AddPupil.Visibility = Visibility.Visible;
            pupilAdmin.systemUser = this.systemUser;
            frmMain.Navigate(pupilAdmin);
        }

        private void BtnPupilUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the pupil search form and display
            PupilAdmin pupilAdmin = new PupilAdmin();
            pupilAdmin.Stk_SearchPupil.Visibility = Visibility.Visible;
            pupilAdmin.systemUser = this.systemUser;
            frmMain.Navigate(pupilAdmin);
        }

        private void BtnTeacherAdd_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the teacher add form and display
            TeacherAdmin teacherAdmin = new TeacherAdmin();
            teacherAdmin.Stk_AddTeacher.Visibility = Visibility.Visible;
            teacherAdmin.systemUser = this.systemUser;
            frmMain.Navigate(teacherAdmin);
        }

        private void BtnTeacherUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the teacher search form and display
            TeacherAdmin teacherAdmin = new TeacherAdmin();
            teacherAdmin.Stk_SearchTeacher.Visibility = Visibility.Visible;
            teacherAdmin.systemUser = this.systemUser;
            frmMain.Navigate(teacherAdmin);
        }

        private void BtnAbsenceAdd_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the AbsenceType add form and display
            AbsenceTypeAdmin absenceTypeAdmin = new AbsenceTypeAdmin();
            absenceTypeAdmin.Stk_AddAbsence.Visibility = Visibility.Visible;
            absenceTypeAdmin.systemUser = this.systemUser;
            frmMain.Navigate(absenceTypeAdmin);
        }

        private void BtnAbsenceUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the AbsenceType search form and display
            AbsenceTypeAdmin absenceTypeAdmin = new AbsenceTypeAdmin();
            absenceTypeAdmin.Stk_SearchAbsenceType.Visibility = Visibility.Visible;
            absenceTypeAdmin.systemUser = this.systemUser;
            frmMain.Navigate(absenceTypeAdmin);
        }

        private void BtnClassAdd_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the Class add form and display
            ClassAdmin classAdmin = new ClassAdmin();
            classAdmin.Stk_AddClass.Visibility = Visibility.Visible;
            classAdmin.systemUser = this.systemUser;
            frmMain.Navigate(classAdmin);
        }

        private void BtnClassUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Initialise the Class search form and display
            ClassAdmin classAdmin = new ClassAdmin();
            classAdmin.Stk_SearchClass.Visibility = Visibility.Visible;
            classAdmin.systemUser = this.systemUser;
            frmMain.Navigate(classAdmin);
        }

    }
}
