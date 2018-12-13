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
    /// Interaction logic for Attendance.xaml
    /// </summary>
    public partial class Attendance : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        public SystemUser systemUser = new SystemUser();
        public List<Class> SchoolClasses { get; set; }
        public Attendance()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateClassList();
        }

        private void PopulateClassList()
        {
            SchoolClasses = smaDB.Classes.ToList();
            DataContext = SchoolClasses;
        }

        /// <summary>
        /// Method populates the select class pupils into the ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbBxClassSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedClass = 0;

            try
            {
                selectedClass = Convert.ToInt16(CmbBxClassSelector.SelectedValue.ToString());
                var pupilList = smaDB.Pupils.Where(p => p.ClassId == selectedClass).ToList();

                LbCheckList.ItemsSource = pupilList;
                LbCheckList.Items.Refresh();
                Stk_ClassList.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show("Error in populating class list", "Selection Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void BtnCancelAttendance_Click(object sender, RoutedEventArgs e)
        {
            Stk_ClassList.Visibility = Visibility.Hidden;
            PopulateClassList();
        }

        private void BtnConfirmAttendance_Click(object sender, RoutedEventArgs e)
        {
            int count =0;
            //for (int i = 0; i < LstClassList.Items.Count; i++)
            //{
            //    count++;
            //}
            count = LbCheckList.SelectedItems.Count;

            MessageBox.Show($"{count}", "Selection Error", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }

        private void ChkPresent_Checked(object sender, RoutedEventArgs e)
        {
        }
    }
}
