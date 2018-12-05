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
            LstClassSearch.ItemsSource = classes;
            foreach (var schoolClass in smaDB.Classes)
            {
                classes.Add(schoolClass);
            }

        }

        private void BtnAddClass_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                 int classAdded = schoolClassUtils.AddSchoolClass(new Class
                            {
                                ClassName = TbxClassName.Text
                            });

                if (classAdded == 1)
                {
                    Lbl_ClassAddSuccessLabel.Visibility = Visibility.Visible;
                    TbxClassName.IsEnabled = false;
                    BtnAddClass.Visibility = Visibility.Collapsed;
                    BtnCancel.Visibility = Visibility.Collapsed;
                    BtnReturn.Visibility = Visibility.Visible;

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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }
                else
                {
                    TbxClassName.Clear();
                    Lbl_ClassErrorLabel.Visibility = Visibility.Visible;
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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          

            
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TbxClassName.Text = "";
            Stk_AddClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
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
            Stk_SearchClass.Visibility = Visibility.Hidden;
            var selectedClass = schoolClassUtils.GetSchoolClass(Convert.ToInt16(LstClassSearch.SelectedValue.ToString()));
            TbxUpdateClassID.Text = selectedClass.ClassId.ToString();
            TbxUpdateClassDescription.Text = selectedClass.ClassName.ToString();
            Stk_UpdateClass.Visibility = Visibility.Visible;
        }

        private void MnuIDeleteClass_Click(object sender, RoutedEventArgs e)
        {
            Stk_SearchClass.Visibility = Visibility.Hidden;
            // get the selected class id
            var selectedClass = schoolClassUtils.GetSchoolClass(Convert.ToInt16(LstClassSearch.SelectedValue.ToString()));
            TbxDeleteClassDescription.Text = selectedClass.ClassName.ToString();
            TbxDeleteClassID.Text = selectedClass.ClassId.ToString();
            Stk_DeleteClass.Visibility = Visibility.Visible;
        }

        private void BtnDeletClass_Click(object sender, RoutedEventArgs e)
        {
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            if (confirmDelete)
            {
                 try
                 {
                    int classDeleted = schoolClassUtils.DeleteSchoolClass(Convert.ToInt16(TbxDeleteClassID.Text.ToString()));
                    if (classDeleted == 1)
                    {
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
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        BtnDeletClass.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        Lbl_ClassDeleteErrorLabel.Visibility = Visibility.Visible;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
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
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }


                }
                 catch (EntityException)
                 {
                    MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                 }
            }
            else
            {
                BtnDeletClass.Visibility = Visibility.Collapsed;
                BtnDeleteCancel.Visibility = Visibility.Collapsed;
                Lbl_DeleteClassLabel.Visibility = Visibility.Visible;
                BtnDeleteReturn.Visibility = Visibility.Visible;
                Lbl_DeleteClassLabel.Content = "Delete Operation Cancelled";
            }
           
        }

        private void BtnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            TbxDeleteClassDescription.Text = "";
            TbxDeleteClassID.Text = "";
            BtnDeleteReturn.Visibility = Visibility.Collapsed;
            Stk_DeleteClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;
        }

        private void BtnDeleteReturn_Click(object sender, RoutedEventArgs e)
        {
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
            TbxUpdateClassDescription.Text = "";
            TbxUpdateClassID.Text = "";
            Stk_UpdateClass.Visibility = Visibility.Hidden;
            RefreshClassList();
            Stk_SearchClass.Visibility = Visibility.Visible;

        }

        private void BtnUpdateReturn_Click(object sender, RoutedEventArgs e)
        {
           
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
                int classUpdated = schoolClassUtils.UpdateClass(new Class
                {
                    ClassName = TbxUpdateClassDescription.Text.ToString(),
                    ClassId = Convert.ToInt16(TbxUpdateClassID.Text.ToString())
                });

                if (classUpdated == 1)
                {
                    BtnUpdateClass.Visibility = Visibility.Collapsed;
                    BtnUpdateCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateReturn.Visibility = Visibility.Visible;
                    Lbl_UpdateClassLabel.Content = "Class Record Successfully Updated";

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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    BtnUpdateClass.Visibility = Visibility.Collapsed;
                    BtnUpdateCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateReturn.Visibility = Visibility.Visible;
                    Lbl_ClassUpdateErrorLabel.Visibility = Visibility.Visible;
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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
