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
            this.Close();
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

        private void Tbi_Admin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Admin admin = new Admin();
            frmMain.Navigate(admin);
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
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckSystemUserAccess(systemUser);
        }
    }
}
