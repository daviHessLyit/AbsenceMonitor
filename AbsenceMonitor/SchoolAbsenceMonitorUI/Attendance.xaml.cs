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
    /// Interaction logic for Attendance.xaml
    /// </summary>
    public partial class Attendance : Page
    {
        public Attendance()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkPresent_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void chkAbsent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstClassList_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            
        }

        private void DatePAbsenceDate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            stk_MenuPanel.Visibility = Visibility.Visible;
            stk_ClassList.Visibility = Visibility.Hidden;
            StkConfirmationPanel.Visibility = Visibility.Hidden;
            BtnSubmit.Visibility = Visibility.Visible;
            BtnConfirm.Visibility = Visibility.Hidden;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            BtnSubmit.Visibility = Visibility.Hidden;
            BtnConfirm.Visibility = Visibility.Visible;
            StkConfirmationPanel.Visibility = Visibility.Visible;
        }
    }
}
