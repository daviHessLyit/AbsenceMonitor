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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        SystemEventUtils systemEventUtils = new SystemEventUtils();

        // Counter to track login attempts
        private int loginAttemptCount = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button click exits the application gracefully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExitApp_Click(object sender, RoutedEventArgs e)
        {
            // Exit the application
            Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Button click event attempts to login to the system with the details provided
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Create local variables
            SystemUser validatedUser = null; 
            MainDashboard mainDashboard;
            bool userValidated = false;
            string userName = TbxUsername.Text.Trim();
            string userPassword = TbxPassword.Password.Trim();

            // Loop through all the system users available in the System Database

            if (ValidateInput(userName, userPassword) && loginAttemptCount <4)
            {
                var systemUser = smaDB.SystemUsers.FirstOrDefault(s => s.Username == userName && s.Password == userPassword);

                if (systemUser.UserId > 0)
                {
                    validatedUser = systemUser;
                    userValidated = true;
                }
                else
                {
                    loginAttemptCount++;
                    Lbl_ErrorLabel.Content = "User Credentials Incorrect";
                    Lbl_ErrorLabel.Visibility = Visibility.Visible;
                    TbxPassword.Clear();
                    systemEventUtils.AddSystemEvent(new SystemEvent
                    {
                        UserId = 1002,
                        EventTypeId = 1003,
                        EventDateTime = DateTime.Now,
                        EventData = $"Invalid Login attempt for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
                    });
                }
            }
            else
            {
                loginAttemptCount++;
                Lbl_ErrorLabel.Content = "User Credentials Incorrect";
                Lbl_ErrorLabel.Visibility = Visibility.Visible;
                TbxPassword.Clear();
                systemEventUtils.AddSystemEvent(new SystemEvent
                {
                    UserId = 1002,
                    EventTypeId = 1003,
                    EventDateTime = DateTime.Now,
                    EventData = $"System locked after 3 failed attempts for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
                });
            }

            // If the user has been validated set up the mainDashboard and record the event in the logs
            if (userValidated)
            {
                mainDashboard = new MainDashboard();               

                if (validatedUser != null)
                {
                    mainDashboard.systemUser = validatedUser;
                }               

                systemEventUtils.AddSystemEvent(new SystemEvent
                {
                    UserId = validatedUser.UserId,
                    EventTypeId = 1,
                    EventDateTime = DateTime.Now,
                    EventData = $"UserName { validatedUser.Username} successfully logged on to the application at { DateTime.Now}"
                });
                mainDashboard.Owner = this;
                mainDashboard.ShowDialog();
                this.Hide();
            } 
        }

        /// <summary>
        /// Validates the user inputs to the login screen.
        /// </summary>
        /// <param name="userName">
        /// User name entered by the user.
        /// </param>
        /// <param name="password">Password entered by the user</param>
        /// <returns>
        /// Validated Data
        /// </returns>
        private bool ValidateInput(string userName, string password)
        {
            bool validData = true;

            if (userName.Length == 0 || userName.Length > 30)
            {
                validData = false;
            }

            foreach (char ch in userName)
            {
                if (ch >= '0' && ch <= '9')
                {
                    validData = false;
                    //break;
                }
            }

            if (password.Length == 0 || password.Length > 30)
            {
                validData = false;
            }
            return validData;
        }

        /// <summary>
        /// Button click event resets the login form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TbxPassword.Clear();
            TbxUsername.Clear();
        }
    }
}
