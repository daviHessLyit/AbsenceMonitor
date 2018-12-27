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
    /// Interaction logic for ClassAdmin.xaml
    /// </summary>
    public partial class ClassAdmin : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        List<Class> classes = new List<Class>();
        SchoolClassUtils schoolClassUtils = new SchoolClassUtils();
        public SystemUser systemUser = new SystemUser();
        SystemEventUtils systemEventUtils = new SystemEventUtils();

        public ClassAdmin()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate the list of classes and add to the list view on page load
            LstClassSearch.ItemsSource = classes;
            foreach (var schoolClass in smaDB.Classes)
            {
                classes.Add(schoolClass);
            }
            LstClassSearch.Items.Refresh();
        }

        private void BtnAddClass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Add the class record to the database
                 int classAdded = schoolClassUtils.AddSchoolClass(new Class
                            {
                                ClassName = TbxClassName.Text.Trim()
                            });

                if (classAdded == 1)
                {
                    // Display a success message if the record was added sucessfully
                    Lbl_ClassAddSuccessLabel.Visibility = Visibility.Visible;
                    TbxClassName.IsEnabled = false;
                    BtnAddClass.Visibility = Visibility.Collapsed;
                    BtnCancel.Visibility = Visibility.Collapsed;
                    BtnReturn.Visibility = Visibility.Visible;

                    // Update the system logs if the record was added sucessfully
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 3,
                            EventDateTime = DateTime.Now,
                            EventData = $"New Class record added at { DateTime.Now} , by {systemUser.Username}"
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
                    // Display a success message if the record wasn't added sucessfully
                    TbxClassName.Clear();
                    Lbl_ClassErrorLabel.Visibility = Visibility.Visible;

                    // Update the system logs if the record wasn't added sucessfully
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1006,
                            EventDateTime = DateTime.Now,
                            EventData = $"Problem adding Class record at { DateTime.Now} , by {systemUser.Username}"
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Clear the form data and reload the search view
            TbxClassName.Text = "";
            Stk_AddClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            // Clear the form data and reload the search view
            Stk_AddClass.Visibility = Visibility.Hidden;
            Lbl_ClassAddSuccessLabel.Visibility = Visibility.Collapsed;
            TbxClassName.Text = "";
            TbxClassName.IsEnabled = true;
            BtnAddClass.Visibility = Visibility.Visible;
            BtnCancel.Visibility = Visibility.Visible;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;
        }

        private void RefreshClassList()
        {
            // Refresh the list of classes and add to the listview
            classes.Clear();
            LstClassSearch.ItemsSource = classes;
            foreach (var schoolClass in smaDB.Classes)
            {
                classes.Add(schoolClass);
            }
            LstClassSearch.Items.Refresh();
        }

        private void MnuIUpdateClass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stk_SearchClass.Visibility = Visibility.Hidden;
                // Populate the form with the selected class details
                var selectedClass = schoolClassUtils.GetSchoolClass(Convert.ToInt16(LstClassSearch.SelectedValue.ToString()));
                TbxUpdateClassID.Text = selectedClass.ClassId.ToString();
                TbxUpdateClassDescription.Text = selectedClass.ClassName.ToString();
                Stk_UpdateClass.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MnuIDeleteClass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stk_SearchClass.Visibility = Visibility.Hidden;
                // Populate the form with the selected class details
                var selectedClass = schoolClassUtils.GetSchoolClass(Convert.ToInt16(LstClassSearch.SelectedValue.ToString()));
                TbxDeleteClassDescription.Text = selectedClass.ClassName.ToString();
                TbxDeleteClassID.Text = selectedClass.ClassId.ToString();
                Stk_DeleteClass.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                // Show an error on failure
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeletClass_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to confirm the deletion of the selected record
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            if (confirmDelete)
            {
                // Delete the record if confirmed by the user
                 try
                 {
                    // Delete the selected record from the database
                    int classDeleted = schoolClassUtils.DeleteSchoolClass(Convert.ToInt16(TbxDeleteClassID.Text.ToString()));
                    if (classDeleted == 1)
                    {
                        // Display a success message if the deletion was successful

                        BtnDeletClass.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        Lbl_DeleteClassLabel.Visibility = Visibility.Visible;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        Lbl_DeleteClassLabel.Content = "The selected Class details have been deleted";

                        // Update the System Events if the deletion is successful
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 6,
                                EventDateTime = DateTime.Now,
                                EventData = $"Class record deleted at { DateTime.Now} , by {systemUser.Username}"
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
                        // Display a error message if the deletion wasn't successful
                        BtnDeletClass.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        Lbl_ClassDeleteErrorLabel.Visibility = Visibility.Visible;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        // Update the system logs if the deletion wasn't successful
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 1006,
                                EventDateTime = DateTime.Now,
                                EventData = $"Error deleting Class record { DateTime.Now} , by {systemUser.Username}"
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
                // Reset the form if the cancel option is selected
                BtnDeletClass.Visibility = Visibility.Collapsed;
                BtnDeleteCancel.Visibility = Visibility.Collapsed;
                Lbl_DeleteClassLabel.Visibility = Visibility.Visible;
                BtnDeleteReturn.Visibility = Visibility.Visible;
                Lbl_DeleteClassLabel.Content = "Delete Operation Cancelled";
            }
           
        }

        private void BtnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data
            TbxDeleteClassDescription.Text = "";
            TbxDeleteClassID.Text = "";
            BtnDeleteReturn.Visibility = Visibility.Collapsed;
            Stk_DeleteClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;
        }

        private void BtnDeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data
            BtnDeletClass.Visibility = Visibility.Visible;
            BtnDeleteCancel.Visibility = Visibility.Visible;
            Lbl_DeleteClassLabel.Visibility = Visibility.Collapsed;
            BtnDeleteReturn.Visibility = Visibility.Collapsed;
            Stk_DeleteClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;
        }

        private void BtnUpdateCancel_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data
            TbxUpdateClassDescription.Text = "";
            TbxUpdateClassID.Text = "";
            Stk_UpdateClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;

        }

        private void BtnUpdateReturn_Click(object sender, RoutedEventArgs e)
        {
            // Reset the form data
            BtnUpdateClass.Visibility = Visibility.Visible;
            BtnUpdateCancel.Visibility = Visibility.Visible;
            BtnUpdateReturn.Visibility = Visibility.Collapsed;
            Stk_UpdateClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;
        }

        private void BtnUpdateClass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update the class record on the database
                int classUpdated = schoolClassUtils.UpdateClass(new Class
                {
                    ClassName = TbxUpdateClassDescription.Text.ToString().Trim(),
                    ClassId = Convert.ToInt16(TbxUpdateClassID.Text.ToString())
                });

                if (classUpdated == 1)
                {
                    // Display a success message if the update was successful
                    BtnUpdateClass.Visibility = Visibility.Collapsed;
                    BtnUpdateCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateReturn.Visibility = Visibility.Visible;
                    Lbl_UpdateClassLabel.Content = "Class Record Successfully Updated";

                    // Update the system logs if the update was successful
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 4,
                            EventDateTime = DateTime.Now,
                            EventData = $"Class record updated at { DateTime.Now} , by {systemUser.Username}"
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
                    // Display a success message if the update wasn't successful
                    BtnUpdateClass.Visibility = Visibility.Collapsed;
                    BtnUpdateCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateReturn.Visibility = Visibility.Visible;
                    Lbl_ClassUpdateErrorLabel.Visibility = Visibility.Visible;

                    // Update the system logs if the update wasn't successful
                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1004,
                            EventDateTime = DateTime.Now,
                            EventData = $"Class record update error at { DateTime.Now} , by {systemUser.Username}"
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
    }
}
