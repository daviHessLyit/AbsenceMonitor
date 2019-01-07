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
    /// Interaction logic for AbsenceTypeAdmin.xaml
    /// </summary>
    public partial class AbsenceTypeAdmin : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        List<AbsenceType> absenceTypes = new List<AbsenceType>();
        AbsenceTypeUtils absenceTypeUtils = new AbsenceTypeUtils();
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        public SystemUser systemUser = new SystemUser();

        public AbsenceTypeAdmin()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate a list of Absence types on page load
            LstAbsenceTypeSearch.ItemsSource = absenceTypes;
            foreach (var absenceType in smaDB.AbsenceTypes)
            {
                absenceTypes.Add(absenceType);
            }
            LstAbsenceTypeSearch.Items.Refresh();
        }

        private void RefreshAbsenceTypeList()
        {
            // Refresh the list of absenceTypes
            absenceTypes.Clear();
            LstAbsenceTypeSearch.ItemsSource = absenceTypes;
            foreach (var absenceType in smaDB.AbsenceTypes)
            {               
                absenceTypes.Add(absenceType);
            }

            LstAbsenceTypeSearch.Items.Refresh();

        }

        private void BtnAddAbsence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Add the AbsenceType to the database
                int absenceAddedSuccess = absenceTypeUtils.AddAbsenceType(new AbsenceType
                {
                    AbsenceType1 = TbxAbsenceTypeDesc.Text.ToString()
                });

                if (absenceAddedSuccess == 1)
                {
                    // Display a success message if the absenceType was added successfully
                    BtnAddAbsence.Visibility = Visibility.Collapsed;
                    BtnAddAbsenceCancel.Visibility = Visibility.Collapsed;
                    BtnAddAbsenceReturn.Visibility = Visibility.Visible;
                    TbxAbsenceTypeDesc.IsEnabled = false;
                    Lbl_AbsenceLabel.Content = "New AbsenceType Added Successfully";
                    Lbl_AbsenceLabel.Foreground = Brushes.BlueViolet;

                    // Update the system logs if the absenceType was added successfully
                    try
                    {
                         systemEventUtils.AddSystemEvent(new SystemEvent
                                            {
                                                UserId = systemUser.UserId,
                                                EventTypeId = 3,
                                                EventDateTime = DateTime.Now,
                                                EventData = $"New AbsenceType record added at { DateTime.Now} , by {systemUser.Username}"
                                            });
                    }
                    catch (EntityException)
                    {
                        // Show an error on failure
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                   
                }
                else
                {
                    // Show an error message if absenceType wasn't successfully added.
                    BtnAddAbsence.Visibility = Visibility.Collapsed;
                    BtnAddAbsenceCancel.Visibility = Visibility.Collapsed;
                    BtnAddAbsenceReturn.Visibility = Visibility.Visible;
                    Lbl_AbsenceErrorLabel.Visibility = Visibility.Visible;
                    // Update the system logs if the absenceType wasn't added successfully
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1006,
                            EventDateTime = DateTime.Now,
                            EventData = $"Problem adding AbsenceType record at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        // Show an error on failure
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (EntityException)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddAbsenceCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            TbxAbsenceTypeDesc.Text = "";
            Stk_AddAbsence.Visibility = Visibility.Hidden;
            RefreshAbsenceTypeList();
            Stk_SearchAbsenceType.Visibility = Visibility.Visible;
        }

        private void BtnAddAbsenceReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the search view
            TbxAbsenceTypeDesc.Text = "";
            TbxAbsenceTypeDesc.IsEnabled = true;
            BtnAddAbsence.Visibility = Visibility.Visible;
            BtnAddAbsenceCancel.Visibility = Visibility.Visible;
            BtnAddAbsenceReturn.Visibility = Visibility.Collapsed;
            Stk_AddAbsence.Visibility = Visibility.Hidden;
            RefreshAbsenceTypeList();
            Stk_SearchAbsenceType.Visibility = Visibility.Visible;            
        }

        private void BtnDeleteAbsence_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to confirm before continuing
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;
            // Delete the record if confirmed
            if (confirmDelete)
            {
                try
                {
                    //Delete the selected AbsenceType
                    int absenceDeleted = absenceTypeUtils.DeleteAbsenceType(Convert.ToInt16(TbxDeleteAbsenceID.Text.ToString()));

                    if (absenceDeleted == 1)
                    {
                        // Display a success message if the record was deleted and make the return button available
                        BtnDeleteAbsence.Visibility = Visibility.Collapsed;
                        BtnDeleteAbsenceCancel.Visibility = Visibility.Collapsed;
                        Lbl_DeleteAbsenceLabel.Visibility = Visibility.Visible;
                        Lbl_DeleteAbsenceLabel.Content = "The selected AbsenceType details have been deleted";
                        BtnDeleteAbsenceReturn.Visibility = Visibility.Visible;

                        // Update the system logs if the deletinon was successful
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 6,
                                EventDateTime = DateTime.Now,
                                EventData = $"AbsenceType record deleted at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show an error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Display an error message if the deletion wasn't successful
                        BtnDeleteAbsence.Visibility = Visibility.Collapsed;
                        BtnDeleteAbsenceCancel.Visibility = Visibility.Collapsed;
                        Lbl_AbsenceDeleteErrorLabel.Visibility = Visibility.Visible;
                        BtnDeleteAbsenceReturn.Visibility = Visibility.Visible;

                        // Update the system logs if the deletinon wasn't successful
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Error deleting AbsenceType record { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show an error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (EntityException)
                {
                    // Show an error on failure
                    MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Display a message if the deletion was cancelled and make the return button available
                BtnDeleteAbsence.Visibility = Visibility.Collapsed;
                BtnDeleteAbsenceCancel.Visibility = Visibility.Collapsed;
                Lbl_DeleteAbsenceLabel.Visibility = Visibility.Visible;
                Lbl_DeleteAbsenceLabel.Content = "Delete Operation Cancelled";
                BtnDeleteAbsenceReturn.Visibility = Visibility.Visible;
            }
        }

        private void BtnDeleteAbsenceCance_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the search view
            Stk_DeleteAbsence.Visibility = Visibility.Hidden;
            TbxDeleteAbsenceID.Text = "";
            TbxDeleteAbsenceType.Text = "";
            RefreshAbsenceTypeList();
            Stk_SearchAbsenceType.Visibility = Visibility.Visible;
        }

        private void BtnDeleteAbsenceReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the search view
            Stk_DeleteAbsence.Visibility = Visibility.Hidden;
            BtnDeleteAbsenceCancel.Visibility = Visibility.Visible;
            BtnDeleteAbsence.Visibility = Visibility.Visible;
            BtnDeleteAbsenceReturn.Visibility = Visibility.Collapsed;
            TbxDeleteAbsenceID.Text = "";
            TbxDeleteAbsenceType.Text = "";
            RefreshAbsenceTypeList();
            Stk_SearchAbsenceType.Visibility = Visibility.Visible;
        }

        private void MnuIDeleteAbsenceType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate the form with the selected AbsenceType details
                Stk_SearchAbsenceType.Visibility = Visibility.Hidden;
                var selectedAbsenceType = absenceTypeUtils.GetAbsenceType(Convert.ToInt16(LstAbsenceTypeSearch.SelectedValue.ToString()));
                TbxDeleteAbsenceID.Text = selectedAbsenceType.AbsenceTypeId.ToString();
                TbxDeleteAbsenceType.Text = selectedAbsenceType.AbsenceType1.ToString();
                Stk_DeleteAbsence.Visibility = Visibility.Visible;
            }
            catch (EntityException)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MnuIUpdateAbsenceType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate the form with the selected AbsenceType details
                Stk_SearchAbsenceType.Visibility = Visibility.Hidden;
                var selectedAbsenceType = absenceTypeUtils.GetAbsenceType(Convert.ToInt16(LstAbsenceTypeSearch.SelectedValue.ToString()));
                TbxUpdateAbsenceAbsenceID.Text = selectedAbsenceType.AbsenceTypeId.ToString();
                TbxUpdateAbsenceDescription.Text = selectedAbsenceType.AbsenceType1.ToString();
                Stk_UpdateAbsence.Visibility = Visibility.Visible;
            }
            catch (EntityException)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdateAbsence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update the AbsenceType on the database
                int absenceUpdatedSuccess = absenceTypeUtils.UpdateAbsenceType(new AbsenceType
                {
                    AbsenceType1 = TbxUpdateAbsenceDescription.Text.ToString(),
                    AbsenceTypeId = Convert.ToInt16(TbxUpdateAbsenceAbsenceID.Text.ToString())
                });

                if (absenceUpdatedSuccess == 1)
                {
                    // Display a success message if update was sucessful and make the return button available
                    BtnUpdateAbsence.Visibility = Visibility.Collapsed;
                    BtnUpdateAbsenceCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateAbsenceReturn.Visibility = Visibility.Visible;
                    Lbl_UpdateAbsenceLabel.Content = "AbsenceType Record Successfully Updated";

                    // Update the system logs if the absenceType was updated successfully
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 4,
                            EventDateTime = DateTime.Now,
                            EventData = $"AbsenceType record updated at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        // Show an error on failure
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    // Show an error message if the update wasn't successful

                    BtnUpdateAbsence.Visibility = Visibility.Collapsed;
                    BtnUpdateAbsenceCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateAbsenceReturn.Visibility = Visibility.Visible;
                    Lbl_AbsenceUpdateErrorLabel.Visibility = Visibility.Visible;

                    // Update the system logs if the absenceType wasn't updated successfully
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1004,
                            EventDateTime = DateTime.Now,
                            EventData = $"AbsenceType record update error at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        // Show an error on failure
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (EntityException)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdateAbsenceCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the search view
            TbxUpdateAbsenceAbsenceID.Text = "";
            TbxUpdateAbsenceDescription.Text = "";
            Stk_UpdateAbsence.Visibility = Visibility.Hidden;
            RefreshAbsenceTypeList();
            Stk_SearchAbsenceType.Visibility = Visibility.Visible;
        }

        private void BtnUpdateAbsenceReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the search view
            BtnUpdateAbsence.Visibility = Visibility.Visible;
            BtnUpdateAbsenceCancel.Visibility = Visibility.Visible;
            BtnUpdateAbsenceReturn.Visibility = Visibility.Collapsed;
            Stk_UpdateAbsence.Visibility = Visibility.Hidden;
            RefreshAbsenceTypeList();
            Stk_SearchAbsenceType.Visibility = Visibility.Visible;
        }
    }
}
