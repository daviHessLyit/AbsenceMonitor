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
            Close();
            Environment.Exit(0);
        }

        private void Tbi_Attendance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Attendance attendance = new Attendance();
            frmMain.Navigate(attendance);
        }

        private void Tbi_Reporting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Reports reports = new Reports();
            frmMain.Navigate(reports);
        }

        private void Tbi_ApplicationLogs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ApplicationLogs applicationLogs = new ApplicationLogs();
            frmMain.Navigate(applicationLogs);
        }

        private void CheckSystemUserAccess(SystemUser user)
        {
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckSystemUserAccess(systemUser);
        }

        private void BtnUserAdd_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = new Admin();
            admin.Stk_AddUser.Visibility = Visibility.Visible;
            frmMain.Navigate(admin);
        }

        private void BtnUserUpdate_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = new Admin();
            admin.Stk_SearchUser.Visibility = Visibility.Visible;
            frmMain.Navigate(admin);
        }

        private void BtnUserDelete_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = new Admin();
            admin.Stk_SearchUser.Visibility = Visibility.Visible;
            frmMain.Navigate(admin);
        }

        private void BtnGuardAdd_Click(object sender, RoutedEventArgs e)
        {
            GuardianAdmin guardianAdmin = new GuardianAdmin();
            guardianAdmin.Stk_AddGuardian.Visibility = Visibility.Visible;
            guardianAdmin.systemUser = this.systemUser;
            frmMain.Navigate(guardianAdmin);
        }

        private void BtnGuardUpdate_Click(object sender, RoutedEventArgs e)
        {
            GuardianAdmin guardianAdmin = new GuardianAdmin();
            guardianAdmin.Stk_SearchGuardian.Visibility = Visibility.Visible;
            guardianAdmin.systemUser = this.systemUser;
            frmMain.Navigate(guardianAdmin);
        }

        private void BtnGuardDelete_Click(object sender, RoutedEventArgs e)
        {
            GuardianAdmin guardianAdmin = new GuardianAdmin();
            guardianAdmin.Stk_SearchGuardian.Visibility = Visibility.Visible;
            guardianAdmin.systemUser = this.systemUser;
            frmMain.Navigate(guardianAdmin);
        }

        private void BtnPupilAdd_Click(object sender, RoutedEventArgs e)
        {
            PupilAdmin pupilAdmin = new PupilAdmin();
            pupilAdmin.Stk_AddPupil.Visibility = Visibility.Visible;
            pupilAdmin.systemUser = this.systemUser;
            frmMain.Navigate(pupilAdmin);
        }

        private void BtnPupilUpdate_Click(object sender, RoutedEventArgs e)
        {
            PupilAdmin pupilAdmin = new PupilAdmin();
            pupilAdmin.Stk_SearchPupil.Visibility = Visibility.Visible;
            pupilAdmin.systemUser = this.systemUser;
            frmMain.Navigate(pupilAdmin);
        }

        private void BtnPupilDelete_Click(object sender, RoutedEventArgs e)
        {
            PupilAdmin pupilAdmin = new PupilAdmin();
            pupilAdmin.Stk_SearchPupil.Visibility = Visibility.Visible;
            pupilAdmin.systemUser = this.systemUser;
            frmMain.Navigate(pupilAdmin);
        }

        private void BtnTeacherAdd_Click(object sender, RoutedEventArgs e)
        {
            TeacherAdmin teacherAdmin = new TeacherAdmin();
            teacherAdmin.Stk_AddTeacher.Visibility = Visibility.Visible;
            teacherAdmin.systemUser = this.systemUser;
            frmMain.Navigate(teacherAdmin);
        }

        private void BtnTeacherUpdate_Click(object sender, RoutedEventArgs e)
        {
            TeacherAdmin teacherAdmin = new TeacherAdmin();
            teacherAdmin.Stk_SearchTeacher.Visibility = Visibility.Visible;
            teacherAdmin.systemUser = this.systemUser;
            frmMain.Navigate(teacherAdmin);
        }

        private void BtnTeacherDelete_Click(object sender, RoutedEventArgs e)
        {
            TeacherAdmin teacherAdmin = new TeacherAdmin();
            teacherAdmin.Stk_SearchTeacher.Visibility = Visibility.Visible;
            teacherAdmin.systemUser = this.systemUser;
            frmMain.Navigate(teacherAdmin);
        }

        private void BtnAbsenceAdd_Click(object sender, RoutedEventArgs e)
        {
            AbsenceTypeAdmin absenceTypeAdmin = new AbsenceTypeAdmin();
            absenceTypeAdmin.Stk_AddAbsence.Visibility = Visibility.Visible;
            absenceTypeAdmin.systemUser = this.systemUser;
            frmMain.Navigate(absenceTypeAdmin);
        }

        private void BtnAbsenceUpdate_Click(object sender, RoutedEventArgs e)
        {
            AbsenceTypeAdmin absenceTypeAdmin = new AbsenceTypeAdmin();
            absenceTypeAdmin.Stk_SearchAbsenceType.Visibility = Visibility.Visible;
            absenceTypeAdmin.systemUser = this.systemUser;
            frmMain.Navigate(absenceTypeAdmin);
        }

        private void BtnAbsenceDelete_Click(object sender, RoutedEventArgs e)
        {
            AbsenceTypeAdmin absenceTypeAdmin = new AbsenceTypeAdmin();
            absenceTypeAdmin.Stk_SearchAbsenceType.Visibility = Visibility.Visible;
            absenceTypeAdmin.systemUser = this.systemUser;
            frmMain.Navigate(absenceTypeAdmin);
        }

        private void BtnClassAdd_Click(object sender, RoutedEventArgs e)
        {
            ClassAdmin classAdmin = new ClassAdmin();
            classAdmin.Stk_AddClass.Visibility = Visibility.Visible;
            classAdmin.systemUser = this.systemUser;
            frmMain.Navigate(classAdmin);
        }

        private void BtnClassUpdate_Click(object sender, RoutedEventArgs e)
        {
            ClassAdmin classAdmin = new ClassAdmin();
            classAdmin.Stk_SearchClass.Visibility = Visibility.Visible;
            classAdmin.systemUser = this.systemUser;
            frmMain.Navigate(classAdmin);
        }

        private void BtnClassDelete_Click(object sender, RoutedEventArgs e)
        {
            ClassAdmin classAdmin = new ClassAdmin();
            classAdmin.Stk_SearchClass.Visibility = Visibility.Visible;
            classAdmin.systemUser = this.systemUser;
            frmMain.Navigate(classAdmin);
        }
    }
}
