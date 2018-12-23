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
        ValidationUtils validationUtils = new ValidationUtils();
        public ApplicationLogs()
        {
            InitializeComponent();
        }
       
        private void Btn_Date_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate a list of SystemEvent sorted by date
                List<SystemEvent> sortedEvents = new List<SystemEvent>();
                sortedEvents = systemEventUtils.SortEventsByDate(smaDB.SystemEvents.ToList());
                // Update the listview
                LstSystemEvents.ItemsSource = sortedEvents;
                LstSystemEvents.Items.Refresh();
            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("Error populating list", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Btn_Event_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate a list of SystemEvent sorted by Type
                List<SystemEvent> sortedEvents = new List<SystemEvent>();
                sortedEvents = systemEventUtils.SortEventsByType(smaDB.SystemEvents.ToList());
                // Update the listview
                LstSystemEvents.ItemsSource = sortedEvents;
                LstSystemEvents.Items.Refresh();
            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("Error populating list", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_User_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate a list of SystemEvent sorted by User
                List<SystemEvent> sortedEvents = new List<SystemEvent>();
                sortedEvents = systemEventUtils.SortEventsByUser(smaDB.SystemEvents.ToList());
                // Update the listview
                LstSystemEvents.ItemsSource = sortedEvents;
                LstSystemEvents.Items.Refresh();
            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("Error populating list", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {   
            // Populate the list of system events on page load
            LstSystemEvents.ItemsSource = systemEvents;
            foreach (var eventLog in smaDB.SystemEvents)
            {
                systemEvents.Add(eventLog);
            }
            LstSystemEvents.Items.Refresh();

        }

        private void PageRefresh()
        {
            // Reload the page elements on refresh
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
            // Check the userName has been added
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
                    // Show an error if the user doesn't exist
                    MessageBox.Show("Username doesn't exist, please enter a valid userName", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
              
            }
            else
            {
                // Show an error if the username hasn't been populated
                MessageBox.Show("Username cannot be empty", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }

           
        }

        private void Btn_SelectReset_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data
            Tbx_UserNameSelector.Text = "";
            Btn_SelectReset.Visibility = Visibility.Collapsed;
        }

        private void Btn_DateSelect_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = Dp_DateSelector.SelectedDate.Value;

            try
            {
                // 
                if (validationUtils.VerifyDate(selectedDate))
                {
                    // Populate the list of systemEvents based on the date
                    List<SystemEvent> sortedEvents = new List<SystemEvent>();
                    sortedEvents = systemEventUtils.SortEventsByDate(selectedDate);
                    if (sortedEvents.Count == 0)
                    {
                        // Display an information message if there are no records for the selected date
                        MessageBox.Show("No records for the selected date", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    LstSystemEvents.ItemsSource = sortedEvents;
                    LstSystemEvents.Items.Refresh();
                }
                else
                {
                    // Display an information if the selection is incorrect
                    MessageBox.Show("Date selection cannot be a future date", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                // Show an error message on failure
                MessageBox.Show("Problem with selected date", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Btn_SelectSearch_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected user option
            int selectedSearch = Cmb_SearchSelector.SelectedIndex;
            // Display the selected options
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
                // Display an information message if no option has been selected
                MessageBox.Show("Please select an option from the dropdown", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Btn_ResetForm_Click(object sender, RoutedEventArgs e)
        {
            // Refresh the page on reset
            PageRefresh();
        }
    }
}
