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
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void btnExitApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbi_Attendance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void tbi_Reporting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tbi_Admin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tbi_System_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
