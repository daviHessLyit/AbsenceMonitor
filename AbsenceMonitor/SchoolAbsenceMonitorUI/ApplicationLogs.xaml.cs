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
    /// Interaction logic for ApplicationLogs.xaml
    /// </summary>
    public partial class ApplicationLogs : Page
    {

        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        List<SystemEvent> systemEvents = new List<SystemEvent>();
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        public ApplicationLogs()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button click event filters the Logs by date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Date_Click(object sender, RoutedEventArgs e)
        {
            List<SystemEvent> sortedEvents = new List<SystemEvent>();

            sortedEvents = systemEventUtils.SortEventsByDate(smaDB.SystemEvents.ToList());

            LstSystemEvents.ItemsSource = sortedEvents;
            LstSystemEvents.Items.Refresh();

        }

        private void Btn_Event_Click(object sender, RoutedEventArgs e)
        {
            List<SystemEvent> sortedEvents = new List<SystemEvent>();

            sortedEvents = systemEventUtils.SortEventsByType(smaDB.SystemEvents.ToList());

            LstSystemEvents.ItemsSource = sortedEvents;
            LstSystemEvents.Items.Refresh();
        }

        private void Btn_User_Click(object sender, RoutedEventArgs e)
        {
            List<SystemEvent> sortedEvents = new List<SystemEvent>();

            sortedEvents = systemEventUtils.SortEventsByUser(smaDB.SystemEvents.ToList());

            LstSystemEvents.ItemsSource = sortedEvents;
            LstSystemEvents.Items.Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            LstSystemEvents.ItemsSource = systemEvents;
            foreach (var eventLog in smaDB.SystemEvents)
            {
                systemEvents.Add(eventLog);
            }            
        }

        private void PageRefresh()
        {
            systemEvents.Clear();
            LstSystemEvents.ItemsSource = systemEvents;
            foreach (var eventLog in smaDB.SystemEvents)
            {
                systemEvents.Add(eventLog);
            }

            LstSystemEvents.Items.Refresh();

            Stk_DateSelector.Visibility = Visibility.Collapsed;
            Stk_FilterSelector.Visibility = Visibility.Collapsed;
            Stk_UserSelector.Visibility = Visibility.Collapsed;
            Stk_SearchSelector.Visibility = Visibility.Visible;
            Cmb_SearchSelector.SelectedIndex = 0;
        }

        private void Btn_SelectUser_Click(object sender, RoutedEventArgs e)
        {
           
            string userName = Tbx_UserNameSelector.Text.ToString();
            if (userName.Length >0)
            {
                try
                {
                    List<SystemEvent> sortedEvents = new List<SystemEvent>();
                    sortedEvents = systemEventUtils.SortEventsByUsername(smaDB.SystemEvents.ToList(), userName);
                    LstSystemEvents.ItemsSource = sortedEvents;
                    LstSystemEvents.Items.Refresh();
                    Btn_SelectReset.Visibility = Visibility.Visible;
                }
                catch (Exception)
                {
                    MessageBox.Show("Username doesn't exist, please enter a valid userName", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
              
            }
            else
            {
                MessageBox.Show("Username cannot be empty", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }

           
        }

        private void Btn_SelectReset_Click(object sender, RoutedEventArgs e)
        {
            Tbx_UserNameSelector.Text = "";
            Btn_SelectReset.Visibility = Visibility.Collapsed;
        }

        private void Btn_DateSelect_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = Dp_DateSelector.SelectedDate.Value;

            try
            {
                if (selectedDate <= DateTime.Today)
                {
                    List<SystemEvent> sortedEvents = new List<SystemEvent>();
                    sortedEvents = systemEventUtils.SortEventsByDate(selectedDate);
                    if (sortedEvents.Count == 0)
                    {
                        MessageBox.Show("No records for the selected date", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    LstSystemEvents.ItemsSource = sortedEvents;
                    LstSystemEvents.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Date selection cannot be a future date", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Problem with selected date", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void Btn_SelectSearch_Click(object sender, RoutedEventArgs e)
        {
            int selectedSearch = Cmb_SearchSelector.SelectedIndex;

            if (selectedSearch == 1)
            {
                Stk_FilterSelector.Visibility = Visibility.Visible;
                Stk_SearchSelector.Visibility = Visibility.Collapsed;
            }
            else if(selectedSearch == 2)
            {
                Stk_UserSelector.Visibility = Visibility.Visible;
                Stk_SearchSelector.Visibility = Visibility.Collapsed;
            }
            else if(selectedSearch ==3)
            {
                Stk_DateSelector.Visibility = Visibility.Visible;
                Stk_SearchSelector.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Please select an option from the dropdown", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }

        private void Btn_ResetForm_Click(object sender, RoutedEventArgs e)
        {
            PageRefresh();
        }
    }
}
