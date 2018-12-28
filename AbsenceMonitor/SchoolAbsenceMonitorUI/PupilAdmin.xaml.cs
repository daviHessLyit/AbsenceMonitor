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
    /// Interaction logic for PupilAdmin.xaml
    /// </summary>
    public partial class PupilAdmin : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        public SystemUser systemUser = new SystemUser();
        List<Pupil> pupils = new List<Pupil>();
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        ValidationUtils validationUtils = new ValidationUtils();
        PupilUtils pupilUtils = new PupilUtils();
        Absence pupilAbsence = new Absence();
        public List<AbsenceType> AbsenceTypes { get; set; }
        public List<Class> SchoolClasses { get; set; }


        public PupilAdmin()
        {
            InitializeComponent();
        }

        private void BtnAddPupilCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form elements and hide the error label.
            TbxPupilGiven.Text = "";
            TbxPupilSurname.Text = "";
            TbxPupilGuardianID.Text = "";
            LblPupilErrorLabel.Visibility = Visibility.Collapsed;
        }

        private void BtnAddPupilReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form elements and return to the main search page.
            Stk_AddPupil.Visibility = Visibility.Hidden;
            TbxPupilGiven.Text = "";
            TbxPupilSurname.Text = "";
            TbxPupilGuardianID.Text = "";
            BtnAddPupil.Visibility = Visibility.Visible;
            BtnAddPupilCancel.Visibility = Visibility.Visible;
            BtnAddPupilReturn.Visibility = Visibility.Collapsed;
            LblPupilErrorLabel.Visibility = Visibility.Collapsed;
            RefreshPupilList();
            Stk_SearchPupil.Visibility = Visibility.Visible;

        }

        private void BtnAddPupil_Click(object sender, RoutedEventArgs e)
        {
            // Verify the form data
            bool validFormData = validationUtils.VerifyPupilFormData(
                TbxPupilGiven.Text.Trim(),
                TbxPupilSurname.Text.Trim(),
                TbxPupilGuardianID.Text.ToString().Trim(),
                CmbBxClassSelector.SelectedValue.ToString());
            // Continue if the data is valid
            if (validFormData)
            {
                // Hide the error label if previously displayed
                LblPupilErrorLabel.Visibility = Visibility.Collapsed;
                try
                {
                    // Add the pupil record to the database
                    int pupilAdded = pupilUtils.AddPupil(new Pupil
                    {
                        GivenName = TbxPupilGiven.Text.Trim(),
                        Surname = TbxPupilSurname.Text.Trim(),
                        GuardianId = Convert.ToInt16(TbxPupilGuardianID.Text.ToString().Trim()),
                        ClassId = Convert.ToInt16(CmbBxClassSelector.SelectedValue.ToString())
                    });

                    // If sucessfully added, display a success message and make the return button available.
                    if (pupilAdded == 1)
                    {
                        TbxPupilGiven.IsReadOnly = true;
                        TbxPupilSurname.IsReadOnly = true;
                        TbxPupilGuardianID.IsReadOnly = true;
                        BtnAddPupil.Visibility = Visibility.Collapsed;
                        BtnAddPupilCancel.Visibility = Visibility.Collapsed;
                        BtnAddPupilReturn.Visibility = Visibility.Visible;
                        Lbl_PupilLabel.Content = "Pupil Details Successfully Added";

                        // Update the system logs if the record was added
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 3,
                                EventDateTime = DateTime.Now,
                                EventData = $"New Pupil record added at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show an error message on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Display an error message if the record wasn't added to the database
                        TbxPupilGiven.Text = "";
                        TbxPupilSurname.Text = "";
                        TbxPupilGuardianID.Text = "";
                        BtnAddPupil.Visibility = Visibility.Collapsed;
                        BtnAddPupilCancel.Visibility = Visibility.Collapsed;
                        BtnAddPupilReturn.Visibility = Visibility.Visible;
                        LblPupilErrorLabel.Visibility = Visibility.Visible;

                        // Update the system logs if the record wasn't added
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Problem adding Pupil record at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        { 
                            // Show an error message on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (EntityException)
                {
                    // Show an error message on failure
                    MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    // Show an error message on failure
                    MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            



            }
            else
            {
                // Display an error if the form data is invalid
                LblPupilErrorLabel.Content = "Invalid form data, please correct and resubmit";
                LblPupilErrorLabel.Visibility = Visibility.Visible;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate a list of pupils on page load
           var pupilList = from _pupil in smaDB.Pupils.ToList()
                            join _schoolClass in smaDB.Classes on _pupil.ClassId equals _schoolClass.ClassId
                            join _guardian in smaDB.Guardians on _pupil.GuardianId equals _guardian.GuardianId
                            orderby _pupil.GuardianId
                            select new
                            {
                                PupilId = _pupil.PupilId,
                                PupilName = _pupil.FullName,
                                SchoolClass =_schoolClass.ClassName,
                                GuardianFullName =_guardian.FullName,
                                GuardianAddress = _guardian.Address
                            };

            LstPupilSearch.ItemsSource = pupilList;
            // Populate the list of absence types
            PopulateComboBox();
            PopulateClassList();
        }

        private void PopulateClassList()
        {
            // Populate the dropdown menu
            SchoolClasses = smaDB.Classes.ToList();
            CmbBxClassSelector.ItemsSource = SchoolClasses;
        }

        private void PopulateComboBox()
        {
            // Populate the list of absence types
            AbsenceTypes = smaDB.AbsenceTypes.ToList();
            DataContext = AbsenceTypes;
        }

        private void RefreshPupilList()
        {            
            // Repopulate the pupil list
            var pupilList = from _pupil in smaDB.Pupils.ToList()
                            join _schoolClass in smaDB.Classes on _pupil.ClassId equals _schoolClass.ClassId
                            join _guardian in smaDB.Guardians on _pupil.GuardianId equals _guardian.GuardianId
                            orderby _pupil.GuardianId
                            select new
                            {
                                PupilId = _pupil.PupilId,
                                PupilName = _pupil.FullName,
                                SchoolClass = _schoolClass.ClassName,
                                GuardianFullName = _guardian.FullName,
                                GuardianAddress = _guardian.Address
                            };
            // Redisplay the pupilList in the list view
            LstPupilSearch.ItemsSource = pupilList;
            LstPupilSearch.Items.Refresh();

        }

        private void MnuIUpdatePupil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected pupil details from the database
                var selectedPupil = pupilUtils.GetPupilDetails(Convert.ToInt16(LstPupilSearch.SelectedValue.ToString()));
                // Populate the update form with the selected pupil's details
                TbxUpdatePupilGiven.Text = selectedPupil.GivenName.ToString();
                TbxUpdatePupilSurname.Text = selectedPupil.Surname.ToString();
                TbxUpdatePupilClassID.Text = selectedPupil.ClassId.ToString();
                TbxUpdatePupilID.Text = selectedPupil.PupilId.ToString();
                TbxUpdatePupilGuardianID.Text = selectedPupil.GuardianId.ToString();
                Stk_SearchPupil.Visibility = Visibility.Hidden;
                Stk_UpdatePupilForm.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdatePupil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update the pupil record on the database
                int pupilUpdated = pupilUtils.UpdatePupil(new Pupil
                {
                    GivenName = TbxUpdatePupilGiven.Text.Trim(),
                    Surname = TbxUpdatePupilSurname.Text.Trim(),
                    ClassId = Convert.ToInt16(TbxUpdatePupilClassID.Text.ToString()),
                    GuardianId = Convert.ToInt16(TbxUpdatePupilGuardianID.Text.ToString()),
                    PupilId = Convert.ToInt16(TbxUpdatePupilID.Text.ToString())

                });

                if (pupilUpdated == 1)
                {
                    // If the update was successful display a success message and make the return button available
                    BtnUpdatePupil.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilCancel.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilReturn.Visibility = Visibility.Visible;
                    TbxUpdatePupilGiven.IsReadOnly = true;
                    TbxUpdatePupilSurname.IsReadOnly = true;
                    TbxUpdatePupilClassID.IsReadOnly = true;
                    TbxUpdatePupilID.IsReadOnly = true;
                    TbxUpdatePupilGuardianID.IsReadOnly = true;
                    Lbl_UpdatePupilLabel.Content = "Pupil details Successfully Updated";

                    // Update the system logs if the pupil record was updated successfully
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 4,
                            EventDateTime = DateTime.Now,
                            EventData = $"Pupil record updated at { DateTime.Now} , by {systemUser.Username}"
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
                    // Display an error if the update was unsuccessful
                    BtnUpdatePupil.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilCancel.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilReturn.Visibility = Visibility.Visible;
                    Lbl_PupilUpdateErrorLabel.Visibility = Visibility.Visible;

                    // Try to update the system logs if the update was unsuccessful
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1004,
                            EventDateTime = DateTime.Now,
                            EventData = $"Pupil record update error at { DateTime.Now} , by {systemUser.Username}"
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
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Error, Please contact the System Administrator", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdatePupilCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset form data and return to the search view
            RefreshPupilList();
            TbxUpdatePupilGiven.Text = "";
            TbxUpdatePupilClassID.Text = "";
            TbxUpdatePupilSurname.Text = "";
            TbxUpdatePupilGuardianID.Text = "";
            TbxUpdatePupilID.Text = "";
            Stk_UpdatePupilForm.Visibility = Visibility.Hidden;          
            Stk_SearchPupil.Visibility = Visibility.Visible;
        }

        private void BtnUpdatePupilReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset update form data and return to the search view
            RefreshPupilList();
            Stk_UpdatePupilForm.Visibility = Visibility.Hidden;
            BtnUpdatePupil.Visibility = Visibility.Visible;
            BtnUpdatePupilCancel.Visibility = Visibility.Visible;
            BtnUpdatePupilReturn.Visibility = Visibility.Hidden;
            TbxUpdatePupilGiven.Text = "";
            TbxUpdatePupilClassID.Text = "";
            TbxUpdatePupilSurname.Text = "";
            TbxUpdatePupilGuardianID.Text = "";
            TbxUpdatePupilID.Text = "";
            TbxUpdatePupilGiven.IsReadOnly = false;
            TbxUpdatePupilSurname.IsReadOnly = false;
            TbxUpdatePupilClassID.IsReadOnly = false;
            TbxUpdatePupilID.IsReadOnly = false;
            TbxUpdatePupilGuardianID.IsReadOnly = false;            
            Stk_SearchPupil.Visibility = Visibility.Visible;

        }

        private void MnuIUpdatePupilAbsence_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                // Populate the form with the selected pupil's details and make the form visible
                var selectedPupil = pupilUtils.GetPupilDetails(Convert.ToInt16(LstPupilSearch.SelectedValue.ToString()));

                TbxUpdatePupilAbsenceGiven.Text = selectedPupil.GivenName;
                TbxUpdatePupilAbsenceSurname.Text = selectedPupil.Surname;
                TbxUpdatePupilAbsenceClassID.Text = selectedPupil.Class.ClassName;
                TbxUpdatePupilAbsenceID.Text = selectedPupil.PupilId.ToString();

                Stk_SearchPupil.Visibility = Visibility.Hidden;
                Stk_AddPupilAbsenceForm.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                // Show an erro on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdatePupilAbsence_Click(object sender, RoutedEventArgs e)
        {
            // Initialise variables
            int selectedAbsenceType = 0;
            DateTime selectedDate;

            try
            {
                 // Get the seleted absence type
                 selectedAbsenceType = Convert.ToUInt16(CmbBxAbsenceTypeSelector.SelectedValue.ToString());
            }
            catch (Exception)
            {
                // Show an error if no absence type has been selected
                MessageBox.Show("Select an AbsenceType from the dropdown", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                // Get the selected date
                selectedDate = DatePAbsenceDate.SelectedDate.Value;
                // Validate the selected form data
                if (validationUtils.VerifyAbsenceFormData(selectedDate, selectedAbsenceType, Convert.ToInt16(TbxUpdatePupilAbsenceID.Text.ToString())))
                {

                    try
                    {
                        // Add the absence to the system database
                       int absenceAdded =  pupilUtils.AddPupilAbsence(new Absence
                                            {
                                                AbsenceDate = selectedDate.Date,
                                                AbsenceTypeId = selectedAbsenceType
                                            }, Convert.ToInt16(TbxUpdatePupilAbsenceID.Text.ToString()));

                        if (absenceAdded == 1)
                        {
                            // Display a success message if absence successfully added and make the return button available
                            BtnUpdatePupilAbsence.Visibility = Visibility.Collapsed;
                            BtnUpdatePupilAbsenceCancel.Visibility = Visibility.Collapsed;
                            BtnUpdatePupilAbsenceReturn.Visibility = Visibility.Visible;
                            Lbl_UpdatePupilAbsenceLabel.Content = "Pupil Absence Successfully Added";
                            CmbBxAbsenceTypeSelector.IsReadOnly = true;
                            DatePAbsenceDate.Text = "";

                            // Update the system logs if the absence was successfully added.
                            try
                            {
                                systemEventUtils.AddSystemEvent(new SystemEvent
                                {
                                    UserId = systemUser.UserId,
                                    EventTypeId = 3,
                                    EventDateTime = DateTime.Now,
                                    EventData = $"New Pupil absence record added at { DateTime.Now} , by {systemUser.Username}"
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
                            // Show an error message if the absence wasn't added
                            BtnUpdatePupilAbsence.Visibility = Visibility.Collapsed;
                            BtnUpdatePupilAbsenceCancel.Visibility = Visibility.Collapsed;
                            BtnUpdatePupilAbsenceReturn.Visibility = Visibility.Visible;
                            Lbl_PupilAddAbsenceErrorLabel.Visibility = Visibility.Visible;

                            // Update the systems logs if the absence wasn't added
                            try
                            {
                                systemEventUtils.AddSystemEvent(new SystemEvent
                                {
                                    UserId = systemUser.UserId,
                                    EventTypeId = 1006,
                                    EventDateTime = DateTime.Now,
                                    EventData = $"Problem adding Pupil absence record at { DateTime.Now} , by {systemUser.Username}"
                                });
                            }
                            catch (EntityException)
                            {
                                // Show an error on failure
                                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                 
                }
                else
                {
                    // Show an error message if validation failed
                    MessageBox.Show("Invalid data, please check the form", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception)
            {
                // Display an error if no date has been selected.
                MessageBox.Show("Select an a date from the dropdown", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnUpdatePupilAbsenceCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            TbxUpdatePupilAbsenceGiven.Text = "";
            TbxUpdatePupilAbsenceSurname.Text = "";
            TbxUpdatePupilAbsenceClassID.Text = "";
            TbxUpdatePupilAbsenceID.Text = "";

            Stk_SearchPupil.Visibility = Visibility.Visible;
            Stk_AddPupilAbsenceForm.Visibility = Visibility.Hidden;
        }

        private void BtnUpdatePupilAbsenceReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            TbxUpdatePupilAbsenceGiven.Text = "";
            TbxUpdatePupilAbsenceSurname.Text = "";
            TbxUpdatePupilAbsenceClassID.Text = "";
            TbxUpdatePupilAbsenceID.Text = "";
            CmbBxAbsenceTypeSelector.IsReadOnly = false;
            BtnUpdatePupilAbsence.Visibility = Visibility.Visible;
            BtnUpdatePupilAbsenceCancel.Visibility = Visibility.Visible;
            BtnUpdatePupilAbsenceReturn.Visibility = Visibility.Collapsed;
            Stk_SearchPupil.Visibility = Visibility.Visible;
            Stk_AddPupilAbsenceForm.Visibility = Visibility.Hidden;
        }

        private void MnuIDeletePupil_Click(object sender, RoutedEventArgs e)
        {
            // Populate the form with the selected record's details
            try
            {
                var selectedPupil = pupilUtils.GetPupilDetails(Convert.ToInt16(LstPupilSearch.SelectedValue.ToString()));

                TbxDeletePupilGiven.Text = selectedPupil.GivenName.ToString();
                TbxDeletePupilSurname.Text = selectedPupil.Surname.ToString();
                TbxDeletePupilID.Text = selectedPupil.PupilId.ToString();
                Stk_SearchPupil.Visibility = Visibility.Hidden;
                Stk_DeletePupilForm.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            RefreshPupilList();
            TbxDeletePupilGiven.Text = "";
            TbxDeletePupilSurname.Text = "";
            TbxDeletePupilID.Text = "";
            Stk_SearchPupil.Visibility = Visibility.Visible;
            Stk_DeletePupilForm.Visibility = Visibility.Hidden;
        }

        private void BtnDeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            RefreshPupilList();
            TbxDeletePupilGiven.Text = "";
            TbxDeletePupilSurname.Text = "";
            TbxDeletePupilID.Text = "";
            Lbl_PupilDeleteErrorLabel.Visibility = Visibility.Collapsed;
            BtnDeleteCancel.Visibility = Visibility.Visible;
            BtnDeletePupil.Visibility = Visibility.Visible;
            BtnDeleteReturn.Visibility = Visibility.Collapsed;
            Stk_SearchPupil.Visibility = Visibility.Visible;
            Stk_DeletePupilForm.Visibility = Visibility.Hidden;
        }

        private void BtnDeletePupil_Click(object sender, RoutedEventArgs e)
        {
            // Warn the user that the delete cannot be undone
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            // Deletion confirmed
            if (confirmDelete)
            {
                try
                {
                    // Delete the selected record from the system database
                    int pupilDeleted = pupilUtils.DeletePupil(Convert.ToInt16(TbxDeletePupilID.Text.ToString()));

                    if (pupilDeleted == 1)
                    {
                        // Deletion successful, display success message, update the system logs
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        BtnDeletePupil.Visibility = Visibility.Collapsed;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        Lbl_DeletePupilLabel.Content = "Pupil Successfully Deleted";

                        // Update the system logs if the record was successfully deleted
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 6,
                                EventDateTime = DateTime.Now,
                                EventData = $"Pupil record deleted at { DateTime.Now} , by {systemUser.Username}"
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
                        // Deletion failed, show the errror and reset the form, try to log the event to the system logs
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        BtnDeletePupil.Visibility = Visibility.Collapsed;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        Lbl_PupilDeleteErrorLabel.Visibility = Visibility.Visible;

                        // Update the system logs if the record wasn't successfully deleted
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Error deleting Pupil record { DateTime.Now} , by {systemUser.Username}"
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
                    MessageBox.Show($"System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex1)
                {
                    // Show an error on failure
                    MessageBox.Show($"Unknown Error, Please contact the System Administrator{ex1.ToString()}", "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                // Disable button options - make return option available
                BtnDeleteCancel.Visibility = Visibility.Collapsed;
                BtnDeletePupil.Visibility = Visibility.Collapsed;
                BtnDeleteReturn.Visibility = Visibility.Visible;
            }
        }

        private void MnuIDeletePupilAbsence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate the form with the selected pupil's details and make the form visible
                var selectedPupil = pupilUtils.GetPupilDetails(Convert.ToInt16(LstPupilSearch.SelectedValue.ToString()));
                // Use the selected AbsenceId to populate the absence related information on the form
                int selectedAbsenceId = Convert.ToInt16(LstPupilAbsenceSearch.SelectedValue.ToString());
                pupilAbsence = smaDB.Absences.Where(a => a.AbsenceId == selectedAbsenceId).FirstOrDefault();
                TbxDeletePupilFullName.Text = selectedPupil.FullName;
                TbxPupilAbsenceClassID.Text = selectedPupil.Class.ClassName;
                TbxPupilAbsenceID.Text = selectedPupil.PupilId.ToString();
                TbxDeleteAbsenceReason.Text = pupilAbsence.AbsenceType.AbsenceType1;
                TbxDeleteAbsenceDate.Text = pupilAbsence.AbsenceDate.ToShortDateString();
                TbxAbsenceID.Text = LstPupilAbsenceSearch.SelectedValue.ToString();
                Stk_SearchPupilAbsence.Visibility = Visibility.Hidden;
                Stk_SearchPupil.Visibility = Visibility.Hidden;
                Stk_DeletePupilAbsenceForm.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                // Show an erro on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeletePupilAbsenceCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data and return to the search view
            Stk_DeletePupilAbsenceForm.Visibility = Visibility.Hidden;
            TbxDeletePupilFullName.Clear();
            TbxDeleteAbsenceReason.Clear();
            TbxPupilAbsenceClassID.Clear();
            TbxPupilAbsenceID.Clear();
            Stk_SearchPupil.Visibility = Visibility.Visible;
        }

        private void MnuViewPupilAbsence_Click(object sender, RoutedEventArgs e)
        {
            // Use the selected pupilId to return a list of absences for that pupil
            int pupilId = Convert.ToInt16(LstPupilSearch.SelectedValue.ToString());

            if (pupilUtils.CountPupilAbsences(pupilId) > 0)
            {
                var pupilAbsences = from _pupilAbsence in smaDB.PupilAbsences.Where(p => p.PupilId == pupilId).ToList()
                                    join _absence in smaDB.Absences on _pupilAbsence.AbsenceId equals _absence.AbsenceId
                                    join _absenceReason in smaDB.AbsenceTypes on _absence.AbsenceTypeId equals _absenceReason.AbsenceTypeId
                                    select new
                                    {
                                        AbsenceID = _pupilAbsence.AbsenceId,
                                        AbsenceDate = _absence.AbsenceDate.ToShortDateString(),
                                        AbsenceReason = _absenceReason.AbsenceType1
                                    };               

                LstPupilAbsenceSearch.ItemsSource = pupilAbsences;
                LstPupilAbsenceSearch.Items.Refresh();
                Stk_SearchPupilAbsence.Visibility = Visibility.Visible;
                Stk_SearchPupil.Visibility = Visibility.Hidden;
            }
            else
            {
                // Show a slection error if the pupil has no associated absences
                MessageBox.Show("There are no absences for the selected pupil", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        /// <summary>
        /// Method Refreshes the PupilAbsences List
        /// </summary>
        private void RefreshPupilAbsences()
        {
            // Use the selected pupilId to return a list of absences for that pupil
            int pupilId = Convert.ToInt16(LstPupilSearch.SelectedValue.ToString());
            if (pupilUtils.CountPupilAbsences(pupilId) > 0)
            {
                var pupilAbsences = from _pupilAbsence in smaDB.PupilAbsences.Where(p => p.PupilId == pupilId).ToList()
                                    join _absence in smaDB.Absences on _pupilAbsence.AbsenceId equals _absence.AbsenceId
                                    join _absenceReason in smaDB.AbsenceTypes on _absence.AbsenceTypeId equals _absenceReason.AbsenceTypeId
                                    select new
                                    {
                                        AbsenceID = _pupilAbsence.AbsenceId,
                                        AbsenceDate = _absence.AbsenceDate.ToShortDateString(),
                                        AbsenceReason = _absenceReason.AbsenceType1
                                    };

                LstPupilAbsenceSearch.ItemsSource = pupilAbsences;
                LstPupilAbsenceSearch.Items.Refresh();
            }
            else
            {
                // Show a slection error if the pupil has no associated absences left to view
                MessageBox.Show("There are no absences to view for the selected pupil", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDeletePupilAbsence_Click(object sender, RoutedEventArgs e)
        {
            // Warn the user that the delete cannot be undone
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            // Deletion confirmed
            if (confirmDelete)
            {
                try
                {
                    int absenceDeleted = pupilUtils.DeletePupilAbsence(Convert.ToInt16(LstPupilAbsenceSearch.SelectedValue.ToString()), Convert.ToInt16(LstPupilSearch.SelectedValue.ToString()), pupilAbsence.AbsenceDate);

                    if (absenceDeleted == 1)
                    {
                        Lbl_PupilDeleteAbsenceSuccessLabel.Visibility = Visibility.Visible;
                        BtnDeletePupilAbsenceReturn.Visibility = Visibility.Visible;
                        BtnDeletePupilAbsence.Visibility = Visibility.Collapsed;
                        BtnDeletePupilAbsenceCancel.Visibility = Visibility.Collapsed;
                        // Update the system logs if the record was successfully deleted
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 6,
                                EventDateTime = DateTime.Now,
                                EventData = $"PupilAbsence record deleted at { DateTime.Now} , by {systemUser.Username}"
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
                        // Deletion failed, show the errror and reset the form, try to log the event to the system logs
                        Lbl_PupilDeleteAbsenceErrorLabel.Visibility = Visibility.Visible;
                        BtnDeletePupilAbsenceReturn.Visibility = Visibility.Visible;

                        // Update the system logs if the record wasn't successfully deleted
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Error deleting PupilAbsence record { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            // Show an error on failure
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                }
                catch (Exception)
                {
                    // Show an error on failure
                    MessageBox.Show($"System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Reset the delete absence form and return to the main pupil search view
                TbxDeletePupilFullName.Clear();
                TbxPupilAbsenceClassID.Clear();
                TbxPupilAbsenceID.Clear();
                TbxDeleteAbsenceReason.Clear();
                TbxDeleteAbsenceDate.Clear();
                TbxAbsenceID.Clear();
                Stk_SearchPupilAbsence.Visibility = Visibility.Hidden;               
                Stk_DeletePupilAbsenceForm.Visibility = Visibility.Hidden;
                Stk_SearchPupil.Visibility = Visibility.Visible;
            }
        }

        private void BtnDeletePupilAbsenceReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the delete absence form and return to the pupil absence search view
            TbxDeletePupilFullName.Clear();
            TbxPupilAbsenceClassID.Clear();
            TbxPupilAbsenceID.Clear();
            TbxDeleteAbsenceReason.Clear();
            TbxDeleteAbsenceDate.Clear();
            TbxAbsenceID.Clear();
            // Refresh the listview
            RefreshPupilAbsences();
            Stk_SearchPupilAbsence.Visibility = Visibility.Visible;
            Stk_DeletePupilAbsenceForm.Visibility = Visibility.Hidden;


        }

        private void MnuIUpdatePupilAbsence_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Populate the form with the selected pupil's details and make the form visible
                var selectedPupil = pupilUtils.GetPupilDetails(Convert.ToInt16(LstPupilSearch.SelectedValue.ToString()));
                // Use the selected AbsenceId to populate the absence related information on the form
                int selectedAbsenceId = Convert.ToInt16(LstPupilAbsenceSearch.SelectedValue.ToString());
                pupilAbsence = smaDB.Absences.Where(a => a.AbsenceId == selectedAbsenceId).FirstOrDefault();
                TbxPupilFullName.Text = selectedPupil.FullName;
                TbxAbsenceClassID.Text = selectedPupil.Class.ClassName;
                TbxUpdatePAID.Text = selectedPupil.PupilId.ToString();
                TbxDeleteAbsenceReason.Text = pupilAbsence.AbsenceType.AbsenceType1;
                TbxAbsenceDate.Text = pupilAbsence.AbsenceDate.ToShortDateString();
                TbxUpdateAbsenceID.Text = LstPupilAbsenceSearch.SelectedValue.ToString();
                Stk_SearchPupilAbsence.Visibility = Visibility.Hidden;
                Stk_SearchPupil.Visibility = Visibility.Hidden;
                Stk_UpdatePupilAbsenceForm.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                // Show an erro on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void BtnPupilAbsenceCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the main pupil search view
            TbxPupilFullName.Clear();
            TbxAbsenceClassID.Clear();
            TbxUpdatePAID.Clear();
            TbxDeleteAbsenceReason.Clear();
            TbxAbsenceDate.Clear();
            TbxUpdateAbsenceID.Clear();
            Stk_SearchPupil.Visibility = Visibility.Visible;
            Stk_UpdatePupilAbsenceForm.Visibility = Visibility.Hidden;
        }

        private void BtnPupilAbsenceReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form and return to the main pupil search view
            TbxPupilFullName.Clear();
            TbxAbsenceClassID.Clear();
            TbxUpdatePAID.Clear();
            TbxDeleteAbsenceReason.Clear();
            TbxAbsenceDate.Clear();
            TbxUpdateAbsenceID.Clear();
            Stk_UpdatePupilAbsenceForm.Visibility = Visibility.Hidden;
            BtnPupilAbsenceReturn.Visibility = Visibility.Collapsed;
            BtnPupilAbsence.Visibility = Visibility.Visible;
            BtnPupilAbsenceCancel.Visibility = Visibility.Visible;
            Lbl_PupilAbsenceSuccessLabel.Visibility = Visibility.Hidden;
            RefreshPupilAbsences();
            Stk_SearchPupil.Visibility = Visibility.Visible;
        }

        private void BtnPupilAbsence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedAbsenceType = 0;
                try
                {
                    // Get the seleted absence type
                    selectedAbsenceType = Convert.ToUInt16(CmbBxUpdateAbsenceTypeSelector.SelectedValue.ToString());
                }
                catch (Exception)
                {
                    // Show an error if no absence type has been selected
                    MessageBox.Show("Select an AbsenceType from the dropdown", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                int absenceUpdated = pupilUtils.UpdatePupilAbsence(Convert.ToInt16(TbxUpdateAbsenceID.Text.ToString()), selectedAbsenceType);

                if (absenceUpdated == 1)
                {
                    BtnPupilAbsenceReturn.Visibility = Visibility.Visible;
                    BtnPupilAbsence.Visibility = Visibility.Collapsed;
                    BtnPupilAbsenceCancel.Visibility = Visibility.Collapsed;
                    Lbl_PupilAbsenceSuccessLabel.Visibility = Visibility.Visible;

                    // Update the system logs if the pupil record was updated successfully
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 4,
                            EventDateTime = DateTime.Now,
                            EventData = $"Absence record updated at { DateTime.Now} , by {systemUser.Username}"
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
                    // Display an error if the update was unsuccessful
                    BtnPupilAbsenceReturn.Visibility = Visibility.Visible;
                    BtnPupilAbsence.Visibility = Visibility.Collapsed;
                    BtnPupilAbsenceCancel.Visibility = Visibility.Collapsed;
                    Lbl_PupilAbsenceErrorLabel.Visibility = Visibility.Visible;

                    // Try to update the system logs if the update was unsuccessful
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1004,
                            EventDateTime = DateTime.Now,
                            EventData = $"Absence record update error at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        // Show an error on failure
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
