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
        public Reports()
        {
            InitializeComponent();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Visible;
            Stk_AbsenceWarnings.Visibility = Visibility.Hidden;
            Stk_AbsenceBreaches.Visibility = Visibility.Hidden;
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;
            Stk_ClassReport.Visibility = Visibility.Hidden;
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
           
        }

        private void MenuItemSchool_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemClass_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_ClassReportMenu.Visibility = Visibility.Visible;
        }

        private void menuItem_Special_Click(object sender, RoutedEventArgs e)
        {
            Stk_ClassReport.Visibility = Visibility.Visible;
            Stk_ClassReportMenu.Visibility = Visibility.Hidden;
        }
    }
}
