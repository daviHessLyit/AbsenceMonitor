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
            Stk_AddPupil.Visibility = Visibility.Hidden;
            Stk_AddTeacher.Visibility = Visibility.Hidden;
            Stk_AddUser.Visibility = Visibility.Hidden;
            Stk_AddGuardian.Visibility = Visibility.Hidden;
            Stk_AddClass.Visibility = Visibility.Hidden;
            Stk_AddAbsence.Visibility = Visibility.Hidden;
            Stk_UpdateClass.Visibility = Visibility.Hidden;
            Stk_UpdateAbsence.Visibility = Visibility.Hidden;
            Stk_UpdateTeacher.Visibility = Visibility.Hidden;
            Stk_UpdateGuardianForm.Visibility = Visibility.Hidden;
            Stk_UpdateUserForm.Visibility = Visibility.Hidden;
            Stk_UpdatePupilAbsenceForm.Visibility = Visibility.Hidden;
        }

        private void BtnUserDelete_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchUser.Visibility = Visibility.Visible;
        }

        private void BtnUserUpdate_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchUser.Visibility = Visibility.Visible;
        }

        private void BtnUserAdd_Click(object sender, RoutedEventArgs e)
        {
            Stk_AddUser.Visibility = Visibility.Visible;
            Stk_MenuPanel.Visibility = Visibility.Hidden;
        }

        private void BtnGuardDelete_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }

        private void BtnGuardUpdate_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }

        private void BtnGuardAdd_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AddGuardian.Visibility = Visibility.Visible;
        }

        private void BtnPupilAdd_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AddPupil.Visibility = Visibility.Visible;
        }

        private void BtnPupilUpdate_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchPupil.Visibility = Visibility.Visible;
        }

        private void BtnPupilDelete_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchPupil.Visibility = Visibility.Visible;
        }

        private void BtnTeacherAdd_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AddTeacher.Visibility = Visibility.Visible;
        }

        private void BtnTeacherUpdate_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_UpdateTeacher.Visibility = Visibility.Visible;
        }

        private void BtnTeacherDelete_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_DeleteTeacher.Visibility = Visibility.Visible;
        }

        private void BtnClassAdd_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AddClass.Visibility = Visibility.Visible;
        }

        private void BtnAbsenceAdd_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_AddAbsence.Visibility = Visibility.Visible;
        }

        private void BtnAbsenceUpdate_Click(object sender, RoutedEventArgs e)
        {
            Stk_UpdateAbsence.Visibility = Visibility.Visible;
            Stk_MenuPanel.Visibility = Visibility.Hidden;
        }

        private void BtnAbsenceDelete_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_DeleteAbsence.Visibility = Visibility.Visible;
        }

        private void BtnClassUpdate_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_UpdateClass.Visibility = Visibility.Visible;
        }

        private void BtnClassDelete_Click(object sender, RoutedEventArgs e)
        {
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_DeleteClass.Visibility = Visibility.Visible;
        }

        private void MnuI_AbsenceType_Click(object sender, RoutedEventArgs e)
        {
            TbxUpdateAbsenceDescription.Text = MnuI_AbsenceType.Header.ToString();
            //Mnu_ClassSelector.Visibility = Visibility.Hidden;
        }

        private void MnuI_ClassType_Click(object sender, RoutedEventArgs e)
        {
            TbxUpdateClassDescription.Text = MnuI_ClassType.Header.ToString();
        }

        private void MnuI_TeacherType_Click(object sender, RoutedEventArgs e)
        {
            TbxUpdateTeacherGiven.Text = MnuI_TeacherType.Header.ToString();
            TbxUpdateTeacherSurname.Text = MnuI_TeacherType.Header.ToString();
            TbxUpdateTeacherClassID.Text = MnuI_TeacherType.Header.ToString();
            TbxUpdateTeacherID.Text = MnuI_TeacherType.Header.ToString();
        }

        private void MnuIUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchUser.Visibility = Visibility.Hidden;
            Stk_UpdateUserForm.Visibility = Visibility.Visible;
        }

        private void MnuIDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchUser.Visibility = Visibility.Hidden;
            Stk_DeleteUserForm.Visibility = Visibility.Visible;

        }

        private void MnuIUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchGuardian.Visibility = Visibility.Hidden;
            Stk_UpdateGuardianForm.Visibility = Visibility.Visible;
        }

        private void MnuIDeleteGuardian_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchGuardian.Visibility = Visibility.Hidden;
            Stk_DeleteGuardianForm.Visibility = Visibility.Visible;
        }

        private void MnuIUpdatePupil_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchPupil.Visibility = Visibility.Hidden;
            Stk_UpdatePupilForm.Visibility = Visibility.Visible;
        }

        private void MnuIUpdatePupilAbsence_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchPupil.Visibility = Visibility.Hidden;
            Stk_UpdatePupilAbsenceForm.Visibility = Visibility.Visible;
        }

        private void MnuIDeletePupil_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchPupil.Visibility = Visibility.Hidden;
            Stk_DeletePupilForm.Visibility = Visibility.Visible;
        }

        private void BtnDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            Stk_DeleteTeacher.Visibility = Visibility.Hidden;

            string title = "Confirmation Box";
            string messsage = "Confirm Teacher Deletion";
            MessageBox.Show(messsage,title,MessageBoxButton.YesNo);


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
