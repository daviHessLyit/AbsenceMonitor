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
            Stk_PupilReportMenu.Visibility = Visibility.Hidden;
            Stk_PupilList.Visibility = Visibility.Hidden;
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
            Stk_PupilReportMenu.Visibility = Visibility.Visible;
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

        private void MenuItem_PupilSpecial_Click(object sender, RoutedEventArgs e)
        {
            Stk_PupilList.Visibility = Visibility.Visible;
            Stk_PupilReportMenu.Visibility = Visibility.Hidden;
        } 
        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Stk_PupilReport.Visibility = Visibility.Visible;
            Stk_PupilList.Visibility = Visibility.Hidden;
        }
    }
}
