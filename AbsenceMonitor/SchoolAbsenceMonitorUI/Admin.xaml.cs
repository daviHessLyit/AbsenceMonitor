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
            PopulateComboBox();
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
            MessageBox.Show(messsage, title, MessageBoxButton.YesNo);


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialise a list of Pupils and fill with data if available
            List<Pupil> pupils = new List<Pupil>();
            foreach (var pupil in smaDB.Pupils)
            {
                pupils.Add(pupil);
            }
            int count = pupils.Count;
            // Initialise a list of Teachers and fill with data if available
            List<Teacher> teachers = new List<Teacher>();
            foreach (var teacher in smaDB.Teachers)
            {
                teachers.Add(teacher);
            }
            // Initialise a list of SystemUsers and fill with data if available
            List<SystemUser> systemUsers = new List<SystemUser>();
            foreach (var systemUser in smaDB.SystemUsers)
            {
                systemUsers.Add(systemUser);
            }
            // Initialise a list of Guardians and fill with data if available
            List<Guardian> guardians = new List<Guardian>();
            foreach (var guardian in smaDB.Guardians)
            {
                guardians.Add(guardian);
            }
            int guardiancount = guardians.Count;
            // Initialise a list of School Classes and fill with data if available
            List<Class> classes = new List<Class>();
            foreach (var schoolClass in smaDB.Classes)
            {
                classes.Add(schoolClass);
            }
            // Initialise a list of AbsenceTypes and fill with data if available
            List<AbsenceType> absenceTypes = new List<AbsenceType>();
            foreach (var absenceType in smaDB.AbsenceTypes)
            {
                absenceTypes.Add(absenceType);
            }
            

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
                    TbxGiven.Clear();
                    TbxSurname.Clear();
                    TbxUserId.Clear();
                    TbxPassword.Clear();
                }
               
            }

            
        }
    }
}
