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
            int loginAttemptCount = 0;

            string userId = TbxUsername.Text;
            string userPassword = TbxPassword.Password;

            foreach (var systemUser in smaDBEntities.SystemUsers)
            {
                if ( loginAttemptCount <3 &&  systemUser.Username == userId && systemUser.Password == userPassword)
                {
                    MainDashboard mainDashboard = new MainDashboard();
                    mainDashboard.Show();
                    this.Close();
                }
                else
                {
                    loginAttemptCount++;
                }
            }
        }
    }
}
