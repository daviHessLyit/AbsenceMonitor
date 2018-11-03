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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Visible;
            Stk_UserPanel.Visibility = Visibility.Hidden;
            Stk_TeacherPanel.Visibility = Visibility.Hidden;
            Stk_PupilPanel.Visibility = Visibility.Hidden;
            Stk_GuardianPanel.Visibility = Visibility.Hidden;
            Stk_AbsencePanel.Visibility = Visibility.Hidden;
            Stk_ClassPanel.Visibility = Visibility.Hidden;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemUser_Click(object sender, RoutedEventArgs e)
        {
            Stk_UserPanel.Visibility = Visibility.Visible;
            Stk_MenuPanel.Visibility = Visibility.Hidden;
        }

        private void MenuItemGuardian_Click(object sender, RoutedEventArgs e)
        {
            Stk_GuardianPanel.Visibility = Visibility.Visible;
            Stk_MenuPanel.Visibility = Visibility.Hidden;
        }

        private void MenuItemPupil_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_PupilPanel.Visibility = Visibility.Visible;
        }

        private void MenuItemAbsence_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AbsencePanel.Visibility = Visibility.Visible;
        }

        private void MenuItemClass_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_ClassPanel.Visibility = Visibility.Visible;
        }

        private void MenuItemTeacher_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_TeacherPanel.Visibility = Visibility.Visible;
        }

        private void Chk_AddUser_Checked(object sender, RoutedEventArgs e)
        {
            Chk_UpdateUser.IsEnabled = false;
            Chk_DeleteUser.IsEnabled = false;
        }

        private void Chk_UpdateUser_Checked(object sender, RoutedEventArgs e)
        {
            Chk_DeleteUser.IsEnabled = false;
            Chk_AddUser.IsEnabled = false;
        }

        private void Chk_DeleteUser_Checked(object sender, RoutedEventArgs e)
        {
            Chk_UpdateUser.IsEnabled = false;
            Chk_AddUser.IsEnabled = false;
        }

        private void Chk_AddUser_Unchecked(object sender, RoutedEventArgs e)
        {
            Chk_DeleteUser.IsEnabled = true;
            Chk_UpdateUser.IsEnabled = true;
        }

        private void Chk_UpdateUser_Unchecked(object sender, RoutedEventArgs e)
        {
            Chk_DeleteUser.IsEnabled = true;
            Chk_AddUser.IsEnabled = true;
        }

        private void Chk_DeleteUser_Unchecked(object sender, RoutedEventArgs e)
        {
            Chk_UpdateUser.IsEnabled = true;
            Chk_AddUser.IsEnabled = true;
        }
    }
}
