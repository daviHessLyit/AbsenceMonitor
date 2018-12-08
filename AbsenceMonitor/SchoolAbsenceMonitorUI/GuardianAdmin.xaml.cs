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

        public GuardianAdmin()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LstGuardianSearch.ItemsSource = guardians;
            foreach (var guardian in smaDB.Guardians)
            {
                guardians.Add(guardian);
            }
        }

        private void RefreshGuardianList()
        {
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
            bool validFormData = VerifyFormData(TbxGuadianGiven.Text.ToString(),
                     TbxGuadianSurname.Text.ToString(),
                     TbkGuardianAddress.Text.ToString(),
                     TbxGuadianMobile.Text.ToString());

            if (validFormData)
            {
                try
            {
                int guardianAdded = guardianUtils.AddGuardian(new Guardian
                {
                    GivenName = TbxGuadianGiven.Text.ToString(),
                    Surname = TbxGuadianSurname.Text.ToString(),
                    MobileNo = TbxGuadianMobile.Text.ToString(),
                    EmergencyNo = TbxGuadianEmergency.Text.ToString(),
                    Address = TbkGuardianAddress.Text.ToString()
                });

                if (guardianAdded ==1)
                {
                    BtnAddGuardian.Visibility = Visibility.Collapsed;
                    BtnAddGuardianCancel.Visibility = Visibility.Collapsed;
                    BtnAddGuardianReturn.Visibility = Visibility.Visible;
                    TbxGuadianGiven.IsEnabled = false;
                    TbxGuadianSurname.IsEnabled = false;
                    TbxGuadianMobile.IsEnabled = false;
                    TbxGuadianEmergency.IsEnabled = false;
                    TbkGuardianAddress.IsEnabled = false;
                    Lbl_GuardianLabel.Content = "Guardian details Successfully Added"; 

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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    BtnAddGuardian.Visibility = Visibility.Collapsed;
                    BtnAddGuardianCancel.Visibility = Visibility.Collapsed;
                    BtnAddGuardianReturn.Visibility = Visibility.Visible;
                    TbxGuadianGiven.Text = "";
                    TbxGuadianSurname.Text = "";
                    TbxGuadianMobile.Text = "";
                    TbxGuadianEmergency.Text = "";
                    TbkGuardianAddress.Text = "";
                    Lbl_GuardianErrorLabel.Visibility = Visibility.Visible;

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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            catch (EntityException)
            {
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            }
            else
            {
                Lbl_GuardianErrorLabel.Content = "Invalid form data, please correct and resubmit";
                Lbl_GuardianErrorLabel.Visibility = Visibility.Visible;
            }
           
        }

        private void BtnAddGuardianCancel_Click(object sender, RoutedEventArgs e)
        {
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
            Stk_AddGuardian.Visibility = Visibility.Hidden;
            BtnAddGuardian.Visibility = Visibility.Visible;
            BtnAddGuardianCancel.Visibility = Visibility.Visible;
            BtnAddGuardianReturn.Visibility = Visibility.Collapsed;
            TbxGuadianGiven.IsEnabled = true;
            TbxGuadianSurname.IsEnabled = true;
            TbxGuadianMobile.IsEnabled = true;
            TbxGuadianEmergency.IsEnabled = true;
            TbkGuardianAddress.IsEnabled = true;
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
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MnuIUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteGuardian_Click(object sender, RoutedEventArgs e)
        {
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            if (confirmDelete)
            {
                try
                {
                    int guardianDeleted = guardianUtils.DeleteGuardian(Convert.ToInt16(TbxDeleteGuardianID.Text.ToString()));

                    if (guardianDeleted == 1)
                    {
                        BtnDeleteGuardian.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        Lbl_DeleteGuardianLabel.Content = "Guardian Details Successfully Deleted";

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
                                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    else
                    {
                        BtnDeleteGuardian.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        Lbl_GuardianDeleteErrorLabel.Visibility = Visibility.Visible;
                        try
                            {
                                systemEventUtils.AddSystemEvent(new SystemEvent
                                {
                                    UserId = systemUser.UserId,
                                    EventTypeId = 1006,
                                    EventDateTime = DateTime.Now,
                                    EventData = $"Error deleting Teacher record { DateTime.Now} , by {systemUser.Username}"
                                });
                            }
                            catch (EntityException)
                            {
                                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            }
                        }
                }
                catch (EntityException)
                {
                    MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                BtnDeleteGuardian.Visibility = Visibility.Collapsed;
                BtnDeleteCancel.Visibility = Visibility.Collapsed;
                BtnDeleteReturn.Visibility = Visibility.Visible;
            }

           
        }

        private void BtnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
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
                int guardianUpated = guardianUtils.UpdateGuardian(new Guardian
                {
                    GivenName = TbxUpdateGuardianGiven.Text.ToString(),
                    Surname = TbxUpdateGuardianSurname.Text.ToString(),
                    MobileNo = TbxUpdateGuardianMobile.Text.ToString(),
                    EmergencyNo = TbxUpdateGuardianEmerg.Text.ToString(),
                    Address = TbxUpdateAddress.Text.ToString(),
                    GuardianId = Convert.ToInt16(TbxUpdateGuardianID.Text.ToString())

                });

                if (guardianUpated == 1)
                {
                    BtnUpdateGuardian.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianReturn.Visibility = Visibility.Visible;
                    TbxUpdateGuardianGiven.IsEnabled = false;
                    TbxUpdateGuardianSurname.IsEnabled = false;
                    TbxUpdateGuardianMobile.IsEnabled = false;
                    TbxUpdateGuardianEmerg.IsEnabled = false;
                    TbxUpdateAddress.IsEnabled = false;
                    Lbl_UpdateGuardianLabel.Content = "Guardian details Successfully Updated";

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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    BtnUpdateGuardian.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateGuardianReturn.Visibility = Visibility.Visible;
                    Lbl_GuardianUpdateErrorLabel.Visibility = Visibility.Visible;

                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1004,
                            EventDateTime = DateTime.Now,
                            EventData = $"Teacher record update error at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdateGuardianCancel_Click(object sender, RoutedEventArgs e)
        {
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
            BtnUpdateGuardian.Visibility = Visibility.Visible;
            BtnUpdateGuardianCancel.Visibility = Visibility.Visible;
            BtnUpdateGuardianReturn.Visibility = Visibility.Collapsed;
            TbxUpdateGuardianGiven.IsEnabled = true;
            TbxUpdateGuardianSurname.IsEnabled = true;
            TbxUpdateGuardianMobile.IsEnabled = true;
            TbxUpdateGuardianEmerg.IsEnabled = true;
            TbxUpdateAddress.IsEnabled = true;
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

        /// <summary>
        /// Method to validate form data for the mandatory fields in the add Guardian form.
        /// </summary>
        /// <param name="givenName">
        /// string givenName
        /// </param>
        /// <param name="surname">
        /// string surname
        /// </param>
        /// <param name="address">
        /// string address
        /// </param>
        /// <param name="mobileNo">
        /// string mobileNo
        /// </param>
        /// <returns>
        /// bool validData
        /// </returns>
        private bool VerifyFormData(string givenName, string surname, string address, string mobileNo )
        {
            bool validData = true;

            if ( givenName.Length == 0 || givenName.Length > 30)
            {
                validData = false;
            }

            if (surname.Length == 0 || surname.Length > 30)
            {
                validData = false;
            }

            if(address.Length == 0 || address.Length > 100)
            {
                validData = false;
            }

            if(mobileNo.Length == 0 || mobileNo.Length > 30)
            {
                validData = false;
            }

            return validData;
        }

    }  
}
