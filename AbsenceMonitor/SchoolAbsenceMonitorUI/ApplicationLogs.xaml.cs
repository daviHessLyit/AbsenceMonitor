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

        public ApplicationLogs()
        {
            InitializeComponent();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Date_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Event_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_User_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<SystemEvent> systemEvents = new List<SystemEvent>();

            foreach (var eventLog in smaDB.SystemEvents)
            {
                systemEvents.Add(eventLog);
            }
            int count = systemEvents.Count;
            LstSystemEvents.ItemsSource = systemEvents;
        }
    }
}
