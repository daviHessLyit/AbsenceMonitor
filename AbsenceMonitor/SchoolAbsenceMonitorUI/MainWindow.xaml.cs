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

        private void BtnExitApp_Click(object sender, RoutedEventArgs e)
        {
            // Exit the application
            Close();
            Environment.Exit(0);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Create local variables
            SystemUser validatedUser = null; 
            MainDashboard mainDashboard;
            bool userValidated = false;
            string userName = TbxUsername.Text.Trim();
            string userPassword = TbxPassword.Password.Trim();

            // Loop through all the system users available in the System Database

            if (ValidateInput(userName, userPassword))
            {
                var systemUser = smaDB.SystemUsers.Where(s => s.Username == userName && s.Password == userPassword);

                if (systemUser.UserId > 0)
                {
                    validatedUser = systemUser;
                    userValidated = true;
                }
                else
                {
                    // Credentials don't match anyone in the db.
                }
            }
            else
            {
                // Problem with userName and password.
            }
           




            //foreach (var systemUser in smaDB.SystemUsers)
            //{
            //    // Validate the user if the count is less than 3.
            //    if ( loginAttemptCount <3 &&  systemUser.Username == userName && systemUser.Password == userPassword)
            //    {
            //        Lbl_ErrorLabel.Visibility = Visibility.Hidden;
            //        validatedUser = new SystemUser();
            //        validatedUser = systemUser;
            //        userValidated = true;
            //        break;
            //    }
            //    // Record the user login attempt if the user name is on the System Database and if the count is less than 3.
            //    else if (loginAttemptCount < 3 && systemUser.Username == userName)
            //    {
            //        loginAttemptCount++;
            //        Lbl_ErrorLabel.Content = "User Password incorrect";
            //        Lbl_ErrorLabel.Visibility = Visibility.Visible;                    
            //        TbxPassword.Clear();
            //        systemEventUtils.AddSystemEvent(new SystemEvent
            //        {
            //            UserId = systemUser.UserId,
            //            EventTypeId = 2,
            //            EventDateTime = DateTime.Now,
            //            EventData = $"UserName: { systemUser.Username} had a failed logon to the application at { DateTime.Now}, using Password: {userPassword}"
            //        });

            //        if (loginAttemptCount >= 3)
            //        {
            //            Lbl_ErrorLabel.Content = "3 Unsucessful login attempts,\n" +"Please contact System Administrator";
            //            Lbl_ErrorLabel.FontSize = 14;


            //            systemEventUtils.AddSystemEvent(new SystemEvent
            //            {
            //                UserId = systemUser.UserId,
            //                EventTypeId = 1003,
            //                EventDateTime = DateTime.Now,
            //                EventData = $"System locked after 3 failed attempts for UserName: { systemUser.Username} at { DateTime.Now}, using Password: {userPassword}"
            //            });
            //        }
            //    }
            //    // Record the login attempt if the user name is not on the System Database and if the count is less than 3.
            //    else if (loginAttemptCount < 3 && systemUser.Username != userName)
            //    {
            //        loginAttemptCount++;
            //        Lbl_ErrorLabel.Visibility = Visibility.Visible;
            //        systemEventUtils.AddSystemEvent(new SystemEvent
            //        {
            //            UserId = 1002,
            //            EventTypeId = 1002,
            //            EventDateTime = DateTime.Now,
            //            EventData = $"Unknown user login attempt at {DateTime.Now}, using {userName} / {userPassword} combination"
            //        });

            //        if (loginAttemptCount >= 3)
            //        {
            //            Lbl_ErrorLabel.Content = "3 Unsucessful login attempts,\n" + "Please contact System Administrator";
            //            Lbl_ErrorLabel.FontSize = 14;

            //            systemEventUtils.AddSystemEvent(new SystemEvent
            //            {
            //                UserId = 1002,
            //                EventTypeId = 1003,
            //                EventDateTime = DateTime.Now,
            //                EventData = $"System locked after 3 failed attempts for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
            //            });
            //        }
            //    }
            //}

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
                if (ch >= '0' && ch >= '9')
                {
                    validData = false;
                    break;
                }
            }

            if (password.Length == 0 || password.Length > 30)
            {
                validData = false;
            }
            return validData;
        }





        // Allow the system user to reset the username and password text boxes.
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TbxPassword.Clear();
            TbxUsername.Clear();
        }
    }
}
