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
        ValidationUtils validationUtils = new ValidationUtils();
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

        private void UserLogin()
        {
            // Create local variables
            SystemUser validatedUser = null;
            MainDashboard mainDashboard;
            bool userValidated = false;
            string userName = TbxUsername.Text.Trim();
            string userPassword = TbxPassword.Password.Trim();

            if (loginAttemptCount < 3)
            {
                // Loop through all the system users available in the System Database
                loginAttemptCount++;
                if (validationUtils.ValidateUserInput(userName, userPassword))
                {

                    try
                    {
                        var systemUser = smaDB.SystemUsers.FirstOrDefault(s => s.Username == userName && s.Password == userPassword);

                        if (systemUser.UserId > 0)
                        {
                            validatedUser = systemUser;
                            userValidated = true;
                        }

                    }
                    catch (Exception)
                    {
                        if (loginAttemptCount < 3)
                        {
                            Lbl_ErrorLabel.Content = "Username or Password incorrect";
                            Lbl_ErrorLabel.Visibility = Visibility.Visible;
                            TbxPassword.Clear();
                            TbxUsername.Clear();
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = 1002,
                                EventTypeId = 1003,
                                EventDateTime = DateTime.Now,
                                EventData = $"Invalid Login attempt for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
                            });
                        }
                        else
                        {
                            MessageBox.Show("3 Failed Logins System now shutdown, Please contact the System Administrator", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = 1002,
                                EventTypeId = 1003,
                                EventDateTime = DateTime.Now,
                                EventData = $"System locked after 3 failed attempts for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
                            });

                            // Exit the application
                            Close();
                            Environment.Exit(0);
                        }


                    }

                }
                else
                {
                    if (loginAttemptCount < 3)
                    {
                        Lbl_ErrorLabel.Content = "Invalid Credentials";
                        Lbl_ErrorLabel.Visibility = Visibility.Visible;
                        TbxPassword.Clear();
                        TbxUsername.Clear();
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = 1002,
                            EventTypeId = 1003,
                            EventDateTime = DateTime.Now,
                            EventData = $"Invalid Login attempt for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
                        });
                    }
                    else
                    {
                        MessageBox.Show("3 Failed Logins System now shutdown, Please contact the System Administrator", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = 1002,
                            EventTypeId = 1003,
                            EventDateTime = DateTime.Now,
                            EventData = $"System locked after 3 failed attempts for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
                        });

                        // Exit the application
                        Close();
                        Environment.Exit(0);
                    }


                }
            }
            else
            {
                MessageBox.Show("3 Failed Logins System now shutdown, Please contact the System Administrator", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);

                systemEventUtils.AddSystemEvent(new SystemEvent
                {
                    UserId = 1002,
                    EventTypeId = 1003,
                    EventDateTime = DateTime.Now,
                    EventData = $"System locked after 3 failed attempts for unknown user at { DateTime.Now} , using {userName} / {userPassword} combination"
                });

                // Exit the application
                Close();
                Environment.Exit(0);


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
        /// Button click event attempts to login to the system with the details provided
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserLogin();
        }

        /// <summary>
        /// Button click event resets the login form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Lbl_ErrorLabel.Visibility = Visibility.Collapsed;
            TbxPassword.Clear();
            TbxUsername.Clear();
        }

        private void BtnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            UserLogin();
        }
    }
}
