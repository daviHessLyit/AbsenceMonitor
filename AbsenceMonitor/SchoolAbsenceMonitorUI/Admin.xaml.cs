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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        SystemUserUtils userUtils = new SystemUserUtils();

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
            PopulateUserDetails();
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchUser.Visibility = Visibility.Visible;
        }

        private void BtnUserUpdate_Click(object sender, RoutedEventArgs e)
        {
            PopulateUserDetails();
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
            PopulateGuardianDetails();
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }

        private void BtnGuardUpdate_Click(object sender, RoutedEventArgs e)
        {
            PopulateGuardianDetails();
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
            PopulatePupilDetails();
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_SearchPupil.Visibility = Visibility.Visible;
        }

        private void BtnPupilDelete_Click(object sender, RoutedEventArgs e)
        {
            PopulatePupilDetails();
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
            PopulateTeacherDetails();
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_UpdateTeacher.Visibility = Visibility.Visible;
        }

        private void BtnTeacherDelete_Click(object sender, RoutedEventArgs e)
        {
            PopulateTeacherDetails();
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
            PopulateAbsenceDetails();
            Stk_UpdateAbsence.Visibility = Visibility.Visible;
            Stk_MenuPanel.Visibility = Visibility.Hidden;
        }

        private void BtnAbsenceDelete_Click(object sender, RoutedEventArgs e)
        {
            PopulateAbsenceDetails();
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_DeleteAbsence.Visibility = Visibility.Visible;
        }

        private void BtnClassUpdate_Click(object sender, RoutedEventArgs e)
        {
            PopulateClassDetails();
            Stk_MenuPanel.Visibility = Visibility.Hidden;
            Stk_UpdateClass.Visibility = Visibility.Visible;
        }

        private void BtnClassDelete_Click(object sender, RoutedEventArgs e)
        {
            PopulateClassDetails();
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
            // Get the selected user details from the data base
            var selectedUser = userUtils.GetUserDetails(Convert.ToInt16(LstUserSearch.SelectedValue.ToString()));
            // Populate the user update details form.
            TbxUpdateUserGiven.Text = selectedUser.GivenName.ToString();
            TbxUpdateUserSurname.Text = selectedUser.Surname.ToString();
            TbxUpdateUserName.Text = selectedUser.Username.ToString();
            TbxUpdatePassword.Text = selectedUser.Password.ToString();
            TbxUpdateUserID.Text = selectedUser.UserId.ToString();
            // Make the form visible to the user.
            Stk_UpdateUserForm.Visibility = Visibility.Visible;
        }

        private void UpdateFormReload(int systemUserId)
        {
            var selectedUser = userUtils.GetUserDetails(systemUserId);
            // Populate the user update details form.
            TbxUpdateUserGiven.Text = selectedUser.GivenName.ToString();
            TbxUpdateUserGiven.IsEnabled = false;
            TbxUpdateUserSurname.Text = selectedUser.Surname.ToString();
            TbxUpdateUserSurname.IsEnabled = false;
            TbxUpdateUserName.Text = selectedUser.Username.ToString();
            TbxUpdateUserName.IsEnabled = false;
            TbxUpdatePassword.Text = selectedUser.Password.ToString();
            TbxUpdatePassword.IsEnabled = false;
            TbxUpdateUserID.Text = selectedUser.UserId.ToString();
            LblAccessLevelDrp.Visibility = Visibility.Collapsed;
            LblAccLvlId.Visibility = Visibility.Visible;
            TbxUpdateAccessLevelId.Text = selectedUser.AccessLevelId.ToString();
            TbxUpdateAccessLevelId.Visibility = Visibility.Visible;
            CmbBxUpdateAdminLevel.Visibility = Visibility.Collapsed;
            Lbl_UserUpdateSuccessLabel.Visibility = Visibility.Visible;
            BtnUpdateUser.Visibility = Visibility.Collapsed;
            BtnCancel.Visibility = Visibility.Collapsed;
            BtnReturn.Visibility = Visibility.Visible;
        }

        private void UpdateFormReset()
        {
            TbxUpdateUserGiven.Clear();
            TbxUpdateUserGiven.IsEnabled = true;
            TbxUpdateUserSurname.Clear();
            TbxUpdateUserSurname.IsEnabled = true;
            TbxUpdateUserName.Clear();
            TbxUpdateUserName.IsEnabled = true;
            TbxUpdatePassword.Clear();
            TbxUpdatePassword.IsEnabled = true;
            TbxUpdateUserID.Clear();

            LblAccLvlId.Visibility = Visibility.Hidden;
            TbxUpdateAccessLevelId.Clear();
            TbxUpdateAccessLevelId.Visibility = Visibility.Hidden;
            LblAccessLevelDrp.Visibility = Visibility.Visible;
            CmbBxUpdateAdminLevel.Visibility = Visibility.Visible;
            Lbl_UserUpdateSuccessLabel.Visibility = Visibility.Hidden;           
            BtnUpdateUser.Visibility = Visibility.Visible;
            BtnCancel.Visibility = Visibility.Visible;
            BtnReturn.Visibility = Visibility.Collapsed;

            Stk_UpdateUserForm.Visibility = Visibility.Hidden;
        }

        private void MnuIDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchUser.Visibility = Visibility.Hidden;
            // Get the selected user details from the data base
            var selectedUser = userUtils.GetUserDetails(Convert.ToInt16(LstUserSearch.SelectedValue.ToString()));
            // Populate the user details in the delete form
            TbxDeleteUserGiven.Text = selectedUser.GivenName.ToString();
            TbxDeleteUserSurname.Text = selectedUser.Surname.ToString();
            TbxDeleteUserName.Text = selectedUser.Username.ToString();
            TbxDeleteUserId.Text = selectedUser.UserId.ToString();
            // Make the form visible to the user.
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
            MessageBox.Show(messsage, title, MessageBoxButton.YesNo);


        }

        private void PopulateUserDetails()
        {
            // Initialise a list of SystemUsers and fill with data if available
            List<SystemUser> systemUsers = new List<SystemUser>();
            LstUserSearch.ItemsSource = systemUsers;
            foreach (var systemUser in smaDB.SystemUsers)
            {
                systemUsers.Add(systemUser);
            }
            PopulateComboBox();
        }

        private void PopulateClassDetails()
        {
            // Initialise a list of School Classes and fill with data if available
            List<Class> classes = new List<Class>();
            foreach (var schoolClass in smaDB.Classes)
            {
                classes.Add(schoolClass);
            }
        }

        private void PopulateGuardianDetails()
        {
            // Initialise a list of Guardians and fill with data if available
            List<Guardian> guardians = new List<Guardian>();
            foreach (var guardian in smaDB.Guardians)
            {
                guardians.Add(guardian);
            }
        }

        private void PopulateTeacherDetails()
        {
            // Initialise a list of Teachers and fill with data if available
            List<Teacher> teachers = new List<Teacher>();
            foreach (var teacher in smaDB.Teachers)
            {
                teachers.Add(teacher);
            }
        }

        private void PopulateAbsenceDetails()
        {
            // Initialise a list of AbsenceTypes and fill with data if available
            List<AbsenceType> absenceTypes = new List<AbsenceType>();
            foreach (var absenceType in smaDB.AbsenceTypes)
            {
                absenceTypes.Add(absenceType);
            }
        }

        private void PopulatePupilDetails()
        {
            // Initialise a list of Pupils and fill with data if available
            List<Pupil> pupils = new List<Pupil>();
            foreach (var pupil in smaDB.Pupils)
            {
                pupils.Add(pupil);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        { 

        }
        public List<AccessLevel> AccessLevels {get;set;}
        private void PopulateComboBox()
        {
            AccessLevels = smaDB.AccessLevels.ToList();
            DataContext = AccessLevels;

        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            int selectAccessLevel = Convert.ToInt16(CmbBxAdminLevel.SelectedValue.ToString());
            if (TbxGiven.Text.Length>0 && TbxSurname.Text.Length>0 && TbxUserId.Text.Length>0 && TbxPassword.Text.Length >0)
            {
                try
                {
                    userUtils.AddSystemUser(new SystemUser
                    {
                        GivenName = TbxGiven.Text,
                        Surname = TbxSurname.Text,
                        Username = TbxUserId.Text,
                        Password = TbxPassword.Text,
                        AccessLevelId = selectAccessLevel
                    });
                    
                }
                catch (Exception)
                {
                    // Make the ErrorLabel visible  
                    Lbl_UserAddErrorLabel.Visibility = Visibility.Visible;
                    // Clear the details from the form
                    TbxGiven.Clear();
                    TbxSurname.Clear();
                    TbxUserId.Clear();
                    TbxPassword.Clear();
                }
               
            }            
        }

        private void BtnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Lbl_UpdateUserErrorLabel.Visibility = Visibility.Hidden;
                int selectAccessLevel = Convert.ToInt16(CmbBxUpdateAdminLevel.SelectedValue.ToString());
                userUtils.UpdateUserDetails(new SystemUser
                {
                    GivenName = TbxUpdateUserGiven.Text,
                    Surname = TbxUpdateUserSurname.Text,
                    Username = TbxUpdateUserName.Text,
                    Password = TbxUpdatePassword.Text,
                    AccessLevelId = selectAccessLevel,
                    UserId = Convert.ToInt16(TbxUpdateUserID.Text)
                });

                UpdateFormReload(Convert.ToInt16(TbxUpdateUserID.Text));
            }
            catch (Exception)
            {
                Lbl_UpdateUserErrorLabel.Visibility = Visibility.Visible;
                Lbl_UpdateUserErrorLabel.Content = "Select an Access Level from the dropdown";
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            Stk_UpdateUserForm.Visibility = Visibility.Hidden;
            ReloadUserSearch("Update");
            Stk_SearchUser.Visibility = Visibility.Visible;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReloadUserSearch("Update");
        }

        private void DeleteFormReset()
        {
            TbxDeleteUserGiven.Clear();
            TbxDeleteUserSurname.Clear();
            TbxDeleteUserName.Clear();
            TbxDeleteUserId.Clear();
            BtnDeleteUser.Visibility = Visibility.Visible;
            BtnDeleteUserCancel.Visibility = Visibility.Visible;
            BtnDeleteUserReturn.Visibility = Visibility.Collapsed;
            Lbl_UserDeleteSuccessLabel.Visibility = Visibility.Collapsed;
            Lbl_UserDeleteErrorLabel.Visibility = Visibility.Collapsed;
            Stk_DeleteUserForm.Visibility = Visibility.Hidden;
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {           

            bool confirmDelete = MessageBox.Show("This action cannot be undone","Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            if (confirmDelete)
            {
                try
                {
                    userUtils.DeleteSystemUser(Convert.ToInt16(TbxDeleteUserId.Text.ToString()));
                    BtnDeleteUser.Visibility = Visibility.Collapsed;
                    BtnDeleteUserCancel.Visibility = Visibility.Collapsed;
                    BtnDeleteUserReturn.Visibility = Visibility.Visible;
                    Lbl_UserDeleteSuccessLabel.Visibility = Visibility.Visible;
                }
                catch (Exception)
                {
                    Lbl_UserDeleteErrorLabel.Visibility = Visibility.Visible;
                }                
            }
            else
            {
                BtnDeleteUser.Visibility = Visibility.Collapsed;
                BtnDeleteUserCancel.Visibility = Visibility.Collapsed;
                BtnDeleteUserReturn.Visibility = Visibility.Visible;
                Lbl_UserDeleteSuccessLabel.Content = "User Deletion Cancelled";
                Lbl_UserDeleteSuccessLabel.Visibility = Visibility.Visible;
            }            
        }

        private void ReloadUserSearch(string formName)
        {
            if (formName == "Delete")
            {
                DeleteFormReset();
            }
            else
            {
                UpdateFormReset();
            }

            PopulateUserDetails();
            Stk_SearchUser.Visibility = Visibility.Visible;
        }

        private void BtnDeleteUserCancel_Click(object sender, RoutedEventArgs e)
        {
            ReloadUserSearch("Delete");
        }

        private void BtnDeleteUserReturn_Click(object sender, RoutedEventArgs e)
        {
            ReloadUserSearch("Delete");
        }
    }
}
