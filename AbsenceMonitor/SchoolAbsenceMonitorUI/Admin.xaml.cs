using SMAClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
        List<SystemUser> systemUsers = new List<SystemUser>();
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        ValidationUtils validationUtils = new ValidationUtils();
        public SystemUser systemUser = new SystemUser();


        public Admin()
        {
            InitializeComponent();            
        }

        private void MnuIUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stk_SearchUser.Visibility = Visibility.Hidden;
                // Get the selected user details from the data base
                var selectedUser = userUtils.GetUserDetails(Convert.ToInt16(LstUserSearch.SelectedValue.ToString()));
                // Populate the user update details form.
                TbxUpdateUserGiven.Text = selectedUser.GivenName;
                TbxUpdateUserSurname.Text = selectedUser.Surname;
                TbxUpdateUserName.Text = selectedUser.Username;
                TbxUpdatePassword.Text = selectedUser.Password;
                TbxUpdateUserID.Text = selectedUser.UserId.ToString();
                // Make the form visible to the user.
                Stk_UpdateUserForm.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void UpdateFormReload(int systemUserId)
        {
            try
            {
                var selectedUser = userUtils.GetUserDetails(systemUserId);
                // Populate the user update details form.
                TbxUpdateUserGiven.Text = selectedUser.GivenName;
                TbxUpdateUserGiven.IsReadOnly = true;
                TbxUpdateUserSurname.Text = selectedUser.Surname;
                TbxUpdateUserSurname.IsReadOnly = true;
                TbxUpdateUserName.Text = selectedUser.Username;
                TbxUpdateUserName.IsReadOnly = true;
                TbxUpdatePassword.Text = selectedUser.Password;
                TbxUpdatePassword.IsReadOnly = true;
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
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }

        private void UpdateFormReset()
        {
            // Reset the update form.
            TbxUpdateUserGiven.Clear();
            TbxUpdateUserGiven.IsReadOnly = false;
            TbxUpdateUserSurname.Clear();
            TbxUpdateUserSurname.IsReadOnly = false;
            TbxUpdateUserName.Clear();
            TbxUpdateUserName.IsReadOnly = false;
            TbxUpdatePassword.Clear();
            TbxUpdatePassword.IsReadOnly = false;
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
            TbxDeleteUserGiven.Text = selectedUser.GivenName;
            TbxDeleteUserSurname.Text = selectedUser.Surname;
            TbxDeleteUserName.Text = selectedUser.Username;
            TbxDeleteUserId.Text = selectedUser.UserId.ToString();
            // Make the form visible to the user.
            Stk_DeleteUserForm.Visibility = Visibility.Visible;
        }

        private void RefreshUserDetails()
        {
            // Refresh the SystemUsers listview
            systemUsers.Clear();

            foreach (var systemUser in smaDB.SystemUsers)
            {
                systemUsers.Add(systemUser);
            }
            LstUserSearch.ItemsSource = systemUsers;
            LstUserSearch.Items.Refresh();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Instantiate the listview on page load
            LstUserSearch.ItemsSource = systemUsers;
            foreach (var systemUser in smaDB.SystemUsers)
            {
                systemUsers.Add(systemUser);
            }
            
            // Populate the combo box.
            PopulateComboBox();
        }
        public List<AccessLevel> AccessLevels {get;set;}
        private void PopulateComboBox()
        {
            // Populate the dropdown with access levels
            AccessLevels = smaDB.AccessLevels.ToList();
            DataContext = AccessLevels;

        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            int selectAccessLevel = 0;
            try
            {
                selectAccessLevel = Convert.ToInt16( CmbBxAdminLevel.SelectedValue.ToString());
            }
            catch (Exception)
            {
                // Show an error message if no access level has been selected.
                MessageBox.Show("Select an Access Level", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            if ( validationUtils.ValidateUserAddInput(TbxGiven.Text,TbxSurname.Text,TbxUserId.Text,TbxPassword.Text) && selectAccessLevel >0)
            {
                    // Add the new user details to the system
                    if (userUtils.AddSystemUser(new SystemUser
                    {
                        GivenName = TbxGiven.Text.Trim(),
                        Surname = TbxSurname.Text.Trim(),
                        Username = TbxUserId.Text.Trim(),
                        Password = TbxPassword.Text.Trim(),
                        AccessLevelId = selectAccessLevel
                    }) == 1)
                    {
                        // Display a success message and make the return button visible
                        TbxGiven.IsReadOnly = true;
                        TbxSurname.IsReadOnly = true;
                        TbxUserId.IsReadOnly = true;
                        TbxPassword.IsReadOnly = true;
                        BtnAddUser.Visibility = Visibility.Collapsed;
                        BtnAddUserCancel.Visibility = Visibility.Collapsed;
                        BtnAddUserReturn.Visibility = Visibility.Visible;
                        Lbl_UserAddSuccessLabel.Visibility = Visibility.Visible;

                    // Update the system logs if the record was added successfully
                    try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 3,
                                EventDateTime = DateTime.Now,
                                EventData = $"New SystemUser record added at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else
                    {
                        // Make the ErrorLabel visible  
                        Lbl_UserAddErrorLabel.Visibility = Visibility.Visible;
                        // Clear the details from the form
                        TbxGiven.Clear();
                        TbxSurname.Clear();
                        TbxUserId.Clear();
                        TbxPassword.Clear();

                        // Update the system logs if the record wasn't added successfully
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Problem adding SystemUser record at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }               
               
            }
            else
            {
                // Show an error message if the form data is invalid.
                MessageBox.Show("Invalid form data, please check and resubmit", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
        
            try
            {
                // Update the user details 
                Lbl_UserUpdateErrorLabel.Visibility = Visibility.Hidden;
                int selectAccessLevel = Convert.ToInt16(CmbBxUpdateAdminLevel.SelectedValue.ToString());

                if (validationUtils.ValidateUserAddInput(TbxUpdateUserGiven.Text, TbxUpdateUserSurname.Text, TbxUpdateUserName.Text, TbxUpdatePassword.Text) && selectAccessLevel > 0)
                {
                    int userUpdated = userUtils.UpdateUserDetails(new SystemUser
                    {
                        GivenName = TbxUpdateUserGiven.Text.Trim(),
                        Surname = TbxUpdateUserSurname.Text.Trim(),
                        Username = TbxUpdateUserName.Text.Trim(),
                        Password = TbxUpdatePassword.Text.Trim(),
                        AccessLevelId = selectAccessLevel,
                        UserId = Convert.ToInt16(TbxUpdateUserID.Text)
                    });

                    if (userUpdated == 1)
                    {
                        // Update the form elements
                        UpdateFormReload(Convert.ToInt16(TbxUpdateUserID.Text));

                        // Update the logs if the record was updated
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 4,
                                EventDateTime = DateTime.Now,
                                EventData = $"SystemUser record updated at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        Lbl_UserUpdateErrorLabel.Visibility = Visibility.Visible;
                        BtnReturn.Visibility = Visibility.Visible;
                        BtnUpdateUser.Visibility = Visibility.Collapsed;
                        BtnCancel.Visibility = Visibility.Collapsed;

                        // Update the logs if the record wasn't updated
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1004,
                                EventDateTime = DateTime.Now,
                                EventData = $"SystemUser record update error at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                   
                }
                else
                {
                    // Show an error message if the form data is invalid.
                    MessageBox.Show("Invalid form data, please check and resubmit", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

               
            }
            catch (Exception)
            {
                // Display an error messae if there is an exception
                Lbl_UserUpdateErrorLabel.Visibility = Visibility.Visible;
                Lbl_UserUpdateErrorLabel.Content = "Select an Access Level from the dropdown";
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the search page
            Stk_UpdateUserForm.Visibility = Visibility.Hidden;
            ReloadUserSearch("Update");
            Stk_SearchUser.Visibility = Visibility.Visible;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the search page
            ReloadUserSearch("Update");
        }

        private void DeleteFormReset()
        {
            // Reset the Delete form
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
            // Ask the user to confrim deletion before continuing with the operation.
            bool confirmDelete = MessageBox.Show("This action cannot be undone","Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            if (confirmDelete)
            {
                try
                {
                   int userDeleted = userUtils.DeleteSystemUser(Convert.ToInt16(TbxDeleteUserId.Text.ToString()));

                    if (userDeleted == 1)
                    {
                        BtnDeleteUser.Visibility = Visibility.Collapsed;
                        BtnDeleteUserCancel.Visibility = Visibility.Collapsed;
                        BtnDeleteUserReturn.Visibility = Visibility.Visible;
                        Lbl_UserDeleteSuccessLabel.Visibility = Visibility.Visible;
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 6,
                                EventDateTime = DateTime.Now,
                                EventData = $"SystemUser record deleted at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Error deleting SystemUser record { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }

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
            else if(formName == "Update")
            {
                UpdateFormReset();
            }

            RefreshUserDetails();
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

        private void BtnAddUserReturn_Click(object sender, RoutedEventArgs e)
        {
            Stk_AddUser.Visibility = Visibility.Hidden;
            TbxGiven.IsReadOnly = false;
            TbxSurname.IsReadOnly = false;
            TbxUserId.IsReadOnly = false;
            TbxPassword.IsReadOnly = false;
            BtnAddUser.Visibility = Visibility.Visible;
            BtnAddUserCancel.Visibility = Visibility.Visible;
            BtnAddUserReturn.Visibility = Visibility.Collapsed;
            Lbl_UserAddSuccessLabel.Visibility = Visibility.Collapsed;
            ReloadUserSearch("Update");
            Stk_SearchUser.Visibility = Visibility.Visible;
        }

        private void BtnAddUserCancel_Click(object sender, RoutedEventArgs e)
        {
            Stk_AddUser.Visibility = Visibility.Hidden;
            ReloadUserSearch("Update");
            Stk_SearchUser.Visibility = Visibility.Visible;
        }
    }
}
