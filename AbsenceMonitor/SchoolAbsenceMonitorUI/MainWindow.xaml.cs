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
        SMADBEntities smaDBEntities = new SMADBEntities();
        SystemEventUtils systemEventUtils = new SystemEventUtils();

        private int loginAttemptCount = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnExitApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Counter to track login attempts
           

            string userId = TbxUsername.Text;
            string userPassword = TbxPassword.Password;

            foreach (var systemUser in smaDBEntities.SystemUsers)
            {
                if ( loginAttemptCount <3 &&  systemUser.Username == userId && systemUser.Password == userPassword)
                {
                    Lbl_ErrorLabel.Visibility = Visibility.Hidden;
                    MainDashboard mainDashboard = new MainDashboard();
                    mainDashboard.Show();
                    systemEventUtils.AddSystemEvent(new SystemEvent { UserId = systemUser.UserId,
                                                                      EventTypeId = 1,
                                                                      EventDateTime = DateTime.Now,
                                                                      EventData = $"UserName { systemUser.Username} successfully logged on to the application at { DateTime.Now}" });
                    
                    Close();
                }
                else
                {
                    loginAttemptCount++;
                    Lbl_ErrorLabel.Visibility = Visibility.Visible;
                    TbxUsername.Clear();
                    TbxPassword.Clear();
                    systemEventUtils.AddSystemEvent(new SystemEvent
                    {
                        UserId = systemUser.UserId,
                        EventTypeId = 2,
                        EventDateTime = DateTime.Now,
                        EventData = $"UserName: { systemUser.Username} had a failed logon to the application at { DateTime.Now}"
                    });

                    if (loginAttemptCount >= 3)
                    {
                        Lbl_ErrorLabel.Content = "3 Unsucessful login attempts,\n" +"Please contact System Administrator";
                        Lbl_ErrorLabel.FontSize = 14;
                    }
                }
            }
        }
    }
}
