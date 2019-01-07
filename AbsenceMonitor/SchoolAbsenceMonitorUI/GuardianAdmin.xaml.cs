using SMAClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for GuardianAdmin.xaml
    /// </summary>
    public partial class GuardianAdmin : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        List<Guardian> guardians = new List<Guardian>();
        public SystemUser systemUser = new SystemUser();
        GuardianUtils guardianUtils = new GuardianUtils();
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        ValidationUtils validationUtils = new ValidationUtils();

        public GuardianAdmin()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate the list of guardians on page load
            LstGuardianSearch.ItemsSource = guardians;
            foreach (var guardian in smaDB.Guardians)
            {
                guardians.Add(guardian);
            }
            LstGuardianSearch.Items.Refresh();
        }

        private void RefreshGuardianList()
        {
            // Populate the list of guardians on page load
            guardians.Clear();
            
            LstGuardianSearch.ItemsSource = guardians;
            foreach (var guardian in smaDB.Guardians)
            {
                guardians.Add(guardian);
            }

            LstGuardianSearch.Items.Refresh();
        }

        private void BtnAddGuardian_Click(object sender, RoutedEventArgs e)
        {
            // Verify the form data
            bool validFormData = validationUtils.VerifyFormData(TbxGuadianGiven.Text.ToString(),
                     TbxGuadianSurname.Text.ToString(),
                     TbkGuardianAddress.Text.ToString(),
                     TbxGuadianMobile.Text.ToString());

            if (validFormData)
            {
                // Form data is valid
                try
                {
                    // Add the guardian record to the database
                    int guardianAdded = guardianUtils.AddGuardian(new Guardian
                    {
                        GivenName = TbxGuadianGiven.Text.Trim(),
                        Surname = TbxGuadianSurname.Text.Trim(),
                        MobileNo = TbxGuadianMobile.Text.Trim(),
                        EmergencyNo = TbxGuadianEmergency.Text.Trim(),
                        Address = TbkGuardianAddress.Text.Trim()
                    });

                    if (guardianAdded ==1)
                    {
                        // Display a sucess message if the record is added successfully
                        BtnAddGuardian.Visibility = Visibility.Collapsed;
                        BtnAddGuardianCancel.Visibility = Visibility.Collapsed;
                        BtnAddGuardianReturn.Visibility = Visibility.Visible;
                        TbxGuadianGiven.IsReadOnly = true;
                        TbxGuadianSurname.IsReadOnly = true;
                        TbxGuadianMobile.IsReadOnly = true;
                        TbxGuadianEmergency.IsReadOnly = true;
                        TbkGuardianAddress.IsReadOnly = true;
                        Lbl_GuardianLabel.Content = "Guardian details Successfully Added"; 

                        // Update the system logs if the record was added successfully
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 3,
                                EventDateTime = DateTime.Now,
                                EventData = $"New Guardian record added at { DateTime.Now} , by {systemUser.Username}"
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
                        // Display an error message if the record wasn't added successfully
                        BtnAddGuardian.Visibility = Visibility.Collapsed;
                        BtnAddGuardianCancel.Visibility = Visibility.Collapsed;
                        BtnAddGuardianReturn.Visibility = Visibility.Visible;
                        TbxGuadianGiven.Text = "";
                        TbxGuadianSurname.Text = "";
                        TbxGuadianMobile.Text = "";
                        TbxGuadianEmergency.Text = "";
                        TbkGuardianAddress.Text = "";
                        Lbl_GuardianErrorLabel.Visibility = Visibility.Visible;

                        // Update the system logs if the record wasn't added successfully
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Problem adding Guardian record at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                catch (EntityException)
                {
                    // Show error on failure
                    MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    // Show error on failure
                    MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                }
            else
            {
                // Validation failed, show an error message
                Lbl_GuardianErrorLabel.Content = "Invalid form data, please correct and resubmit";
                Lbl_GuardianErrorLabel.Visibility = Visibility.Visible;
            }
           
        }

        private void BtnAddGuardianCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form on cancel
            Stk_AddGuardian.Visibility = Visibility.Hidden;
            BtnAddGuardian.Visibility = Visibility.Visible;
            BtnAddGuardianCancel.Visibility = Visibility.Visible;
            BtnAddGuardianReturn.Visibility = Visibility.Collapsed;
            TbxGuadianGiven.Text = "";
            TbxGuadianSurname.Text = "";
            TbxGuadianMobile.Text = "";
            TbxGuadianEmergency.Text = "";
            TbkGuardianAddress.Text = "";
            RefreshGuardianList();
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }

        private void BtnAddGuardianReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data on return and show the search view
            Stk_AddGuardian.Visibility = Visibility.Hidden;
            BtnAddGuardian.Visibility = Visibility.Visible;
            BtnAddGuardianCancel.Visibility = Visibility.Visible;
            BtnAddGuardianReturn.Visibility = Visibility.Collapsed;
            TbxGuadianGiven.IsReadOnly = false;
            TbxGuadianSurname.IsReadOnly = false;
            TbxGuadianMobile.IsReadOnly = false;
            TbxGuadianEmergency.IsReadOnly = false;
            TbkGuardianAddress.IsReadOnly = false;
            TbxGuadianGiven.Text = "";
            TbxGuadianSurname.Text = "";
            TbxGuadianMobile.Text = "";
            TbxGuadianEmergency.Text = "";
            TbkGuardianAddress.Text = "";
            Lbl_GuardianErrorLabel.Visibility = Visibility.Collapsed;
            RefreshGuardianList();
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }

        private void MnuIDeleteGuardian_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate the form with the selected guardian details
                var selectedGuardian = guardianUtils.GetGuardianDetails(Convert.ToInt16(LstGuardianSearch.SelectedValue.ToString()));
                TbxDeleteGuardianGiven.Text = selectedGuardian.GivenName.ToString();
                TbxDeleteGuardianSurname.Text = selectedGuardian.Surname.ToString();
                TbxDeleteGuardianID.Text = selectedGuardian.GuardianId.ToString();
                TbxDeleteMobileNo.Text = selectedGuardian.MobileNo.ToString();
                Stk_DeleteGuardianForm.Visibility = Visibility.Visible;
                Stk_SearchGuardian.Visibility = Visibility.Hidden;

            }
            catch (EntityException)
            {
                // Show error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MnuIUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate the form with the selected guardian details
                var selectedGuardian = guardianUtils.GetGuardianDetails(Convert.ToInt16(LstGuardianSearch.SelectedValue.ToString()));
                TbxUpdateGuardianGiven.Text = selectedGuardian.GivenName.ToString();
                TbxUpdateGuardianSurname.Text = selectedGuardian.Surname.ToString();
                TbxUpdateGuardianID.Text = selectedGuardian.GuardianId.ToString();
                TbxUpdateGuardianMobile.Text = selectedGuardian.MobileNo.ToString();
                TbxUpdateGuardianEmerg.Text = selectedGuardian.EmergencyNo.ToString();
                TbxUpdateAddress.Text = selectedGuardian.Address.ToString();
                Stk_UpdateGuardianForm.Visibility = Visibility.Visible;
                Stk_SearchGuardian.Visibility = Visibility.Hidden;

            }
            catch (EntityException)
            {
                // Show error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteGuardian_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to confirm deletion
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            
            if (confirmDelete)
            {
                // Deletion confirmed
                try
                {
                    // Delete the selected record
                    int guardianDeleted = guardianUtils.DeleteGuardian(Convert.ToInt16(TbxDeleteGuardianID.Text.ToString()));

                    if (guardianDeleted == 1)
                    {
                        // Display a success message if the record was deleted
                        BtnDeleteGuardian.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        Lbl_DeleteGuardianLabel.Content = "Guardian Details Successfully Deleted";

                        // Update the system logs on if the record was deleted successfully
                        try
                            {
                                systemEventUtils.AddSystemEvent(new SystemEvent
                                {
                                    UserId = systemUser.UserId,
                                    EventTypeId = 6,
                                    EventDateTime = DateTime.Now,
                                    EventData = $"Guardian record deleted at { DateTime.Now} , by {systemUser.Username}"
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
                        // Display a error message if the record wasn't deleted
                        BtnDeleteGuardian.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        Lbl_GuardianDeleteErrorLabel.Visibility = Visibility.Visible;

                        // Update the system logs on if the record wasn't deleted successfully
                        try
                        {
                                systemEventUtils.AddSystemEvent(new SystemEvent
                                {
                                    UserId = systemUser.UserId,
                                    EventTypeId = 1006,
                                    EventDateTime = DateTime.Now,
                                    EventData = $"Error deleting Guardian record { DateTime.Now} , by {systemUser.Username}"
                                });
                            }
                            catch (EntityException)
                            {
                                // Show error on failure
                                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                }
                catch (EntityException)
                {
                    // Show error on failure
                    MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    // Show error on failure
                    MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Deletion canceled, make the return button available
                BtnDeleteGuardian.Visibility = Visibility.Collapsed;
                BtnDeleteCancel.Visibility = Visibility.Collapsed;
                BtnDeleteReturn.Visibility = Visibility.Visible;
            }

           
        }

        private void BtnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            TbxDeleteGuardianGiven.Text = "";
            TbxDeleteGuardianSurname.Text = "";
            TbxDeleteGuardianID.Text = "";
            TbxDeleteMobileNo.Text = "";
            Stk_DeleteGuardianForm.Visibility = Visibility.Hidden;
            RefreshGuardianList();
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }

        private void BtnDeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            TbxDeleteGuardianGiven.Text = "";
            TbxDeleteGuardianSurname.Text = "";
            TbxDeleteGuardianID.Text = "";
            TbxDeleteMobileNo.Text = "";
            Lbl_GuardianDeleteErrorLabel.Visibility = Visibility.Collapsed;
            BtnDeleteGuardian.Visibility = Visibility.Visible;
            BtnDeleteCancel.Visibility = Visibility.Visible;
            BtnDeleteReturn.Visibility = Visibility.Collapsed;
            Stk_DeleteGuardianForm.Visibility = Visibility.Hidden;
            RefreshGuardianList();
            Stk_SearchGuardian.Visibility = Visibility.Visible;


        }

        private void BtnUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update the selected user on the database
                int guardianUpated = guardianUtils.UpdateGuardian(new Guardian
                {
                    GivenName = TbxUpdateGuardianGiven.Text.Trim(),
                    Surname = TbxUpdateGuardianSurname.Text.Trim(),
                    MobileNo = TbxUpdateGuardianMobile.Text.Trim(),
                    EmergencyNo = TbxUpdateGuardianEmerg.Text.Trim(),
                    Address = TbxUpdateAddress.Text.Trim(),
                    GuardianId = Convert.ToInt16(TbxUpdateGuardianID.Text.ToString())

                });

                if (guardianUpated == 1)
                {
                    // Display a success message if the guardian has been updated
                    BtnUpdateGuardian.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianReturn.Visibility = Visibility.Visible;
                    TbxUpdateGuardianGiven.IsReadOnly = true;
                    TbxUpdateGuardianSurname.IsReadOnly = true;
                    TbxUpdateGuardianMobile.IsReadOnly = true;
                    TbxUpdateGuardianEmerg.IsReadOnly = true;
                    TbxUpdateAddress.IsReadOnly = true;
                    Lbl_UpdateGuardianLabel.Content = "Guardian details Successfully Updated";

                    // Update the logs if the record was updated
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 4,
                            EventDateTime = DateTime.Now,
                            EventData = $"Guardian record updated at { DateTime.Now} , by {systemUser.Username}"
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
                    // Show an error message if the guardian wasn't updated successfully
                    BtnUpdateGuardian.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianReturn.Visibility = Visibility.Visible;
                    Lbl_GuardianUpdateErrorLabel.Visibility = Visibility.Visible;

                    // Update the logs if the record wasn't updated
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1004,
                            EventDateTime = DateTime.Now,
                            EventData = $"Guardian record update error at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        // Show error on failure
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (EntityException)
            {
                // Show error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                // Show error on failure
                MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdateGuardianCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            TbxUpdateGuardianGiven.Text = "";
            TbxUpdateGuardianSurname.Text = "";
            TbxUpdateGuardianID.Text = "";
            TbxUpdateGuardianMobile.Text = "";
            TbxUpdateGuardianEmerg.Text = "";
            TbxUpdateAddress.Text = "";
            Stk_UpdateGuardianForm.Visibility = Visibility.Hidden;
            RefreshGuardianList();
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }

        private void BtnUpdateGuardianReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            BtnUpdateGuardian.Visibility = Visibility.Visible;
            BtnUpdateGuardianCancel.Visibility = Visibility.Visible;
            BtnUpdateGuardianReturn.Visibility = Visibility.Collapsed;
            TbxUpdateGuardianGiven.IsReadOnly = false;
            TbxUpdateGuardianSurname.IsReadOnly = false;
            TbxUpdateGuardianMobile.IsReadOnly = false;
            TbxUpdateGuardianEmerg.IsReadOnly = false;
            TbxUpdateAddress.IsReadOnly = false;
            Lbl_GuardianUpdateErrorLabel.Visibility = Visibility.Collapsed;
            TbxUpdateGuardianGiven.Text = "";
            TbxUpdateGuardianSurname.Text = "";
            TbxUpdateGuardianID.Text = "";
            TbxUpdateGuardianMobile.Text = "";
            TbxUpdateGuardianEmerg.Text = "";
            TbxUpdateAddress.Text = "";
            Stk_UpdateGuardianForm.Visibility = Visibility.Hidden;
            RefreshGuardianList();
            Stk_SearchGuardian.Visibility = Visibility.Visible;
        }       

    }  
}
