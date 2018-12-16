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
    /// Interaction logic for TeacherAdmin.xaml
    /// </summary>
    public partial class TeacherAdmin : Page
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        List<Teacher> teachers = new List<Teacher>();
        TeacherUtils teacherUtils = new TeacherUtils();
        SystemEventUtils systemEventUtils = new SystemEventUtils();
        public SystemUser systemUser = new SystemUser();
        public TeacherAdmin()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LstTeacherSearch.ItemsSource = teachers;
            foreach (var teacher in smaDB.Teachers)
            {
                teachers.Add(teacher);
            }

            PopulateCombo();
        }

        public List<Class> ClassList { get; set; }

        private void PopulateCombo()
        {
            ClassList = smaDB.Classes.ToList();
            DataContext = ClassList;
        }

        private void RefreshTeacherList()
        {
            teachers.Clear();
            LstTeacherSearch.ItemsSource = teachers;
            foreach (var teacher in smaDB.Teachers)
            {
                teachers.Add(teacher);
            }

            LstTeacherSearch.Items.Refresh();
        }

        private void BtnAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int teacherAddedSuccess = teacherUtils.AddTeacher(new Teacher
                {
                    GivenName = TbxTeacherGiven.Text.ToString(),
                    Surname = TbxTeacherSurname.Text.ToString(),
                    ClassId = Convert.ToUInt16(CmbBxClassSelector.SelectedValue.ToString())

                });

                if (teacherAddedSuccess == 1)
                {
                    BtnAddTeacher.Visibility = Visibility.Collapsed;
                    BtnAddCancel.Visibility = Visibility.Collapsed;
                    TbxTeacherGiven.IsEnabled = false;
                    TbxTeacherSurname.IsEnabled = false;
                    Lbl_TeacherLabel.Content = "New Teacher Added Successfully";
                    BtnAddReturn.Visibility = Visibility.Visible;

                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 3,
                            EventDateTime = DateTime.Now,
                            EventData = $"New Teacher record added at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    BtnAddTeacher.Visibility = Visibility.Collapsed;
                    BtnAddCancel.Visibility = Visibility.Collapsed;
                    BtnAddReturn.Visibility = Visibility.Visible;
                    Lbl_TeacherErrorLabel.Visibility = Visibility.Visible;

                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 1006,
                            EventDateTime = DateTime.Now,
                            EventData = $"Problem adding Teacher record at { DateTime.Now} , by {systemUser.Username}"
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

        private void BtnAddCancel_Click(object sender, RoutedEventArgs e)
        {
            Stk_AddTeacher.Visibility = Visibility.Hidden;
            BtnAddTeacher.Visibility = Visibility.Visible;
            BtnAddCancel.Visibility = Visibility.Visible;            
            TbxTeacherGiven.Text = "";
            TbxTeacherSurname.Text = "";
            RefreshTeacherList();
            Stk_SearchTeacher.Visibility = Visibility.Visible;
        }

        private void BtnAddReturn_Click(object sender, RoutedEventArgs e)
        {
            Stk_AddTeacher.Visibility = Visibility.Hidden;
            BtnAddTeacher.Visibility = Visibility.Visible;
            BtnAddCancel.Visibility = Visibility.Visible;
            BtnAddReturn.Visibility = Visibility.Visible;
            TbxTeacherGiven.IsEnabled = true;
            TbxTeacherSurname.IsEnabled = true;
            TbxTeacherGiven.Text = "";
            TbxTeacherSurname.Text = "";
            RefreshTeacherList();
            Stk_SearchTeacher.Visibility = Visibility.Visible;
        }

        private void BtnDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            // Warn the user that the delete cannot be undone
            bool confirmDelete = MessageBox.Show("This action cannot be undone", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK;

            // Deletion confirmed
            if (confirmDelete)
            {
                try
                {
                    // Delete the selected record from the system database
                    int teacherDeleted = teacherUtils.DeleteTeacher(Convert.ToInt16(TbxDeleteTeacherID.Text.ToString()));

                    if (teacherDeleted == 1)
                    {
                        // Deletion successful, display success message, update the system logs
                        BtnDeleteTeacher.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        Lbl_DeleteTeacherLabel.Content = "Teacher Successfully Deleted";
                        BtnDeleteReturn.Visibility = Visibility.Visible;
                        try
                        {
                            systemEventUtils.AddSystemEvent(new SystemEvent
                            {
                                UserId = systemUser.UserId,
                                EventTypeId = 6,
                                EventDateTime = DateTime.Now,
                                EventData = $"Teacher record deleted at { DateTime.Now} , by {systemUser.Username}"
                            });
                        }
                        catch (EntityException)
                        {
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Deletion failed, show the errror and reset the form, try to log the event to the system logs
                        BtnDeleteTeacher.Visibility = Visibility.Collapsed;
                        BtnDeleteCancel.Visibility = Visibility.Collapsed;
                        Lbl_TeacherDeleteErrorLabel.Visibility = Visibility.Visible;
                        BtnDeleteReturn.Visibility = Visibility.Visible;
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
                catch (Exception)
                {
                    MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

               

            }
            else
            {
                // Disable button options - make return option available
                BtnDeleteTeacher.Visibility = Visibility.Collapsed;
                BtnDeleteCancel.Visibility = Visibility.Collapsed;
                BtnDeleteReturn.Visibility = Visibility.Visible;
            }
        }

        private void BtnDeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            Stk_DeleteTeacher.Visibility = Visibility.Hidden;
            TbxDeleteClassID.Text = "";
            TbxDeleteTeacherGiven.Text = "";
            TbxDeleteTeacherSurname.Text = "";
            TbxDeleteTeacherID.Text = "";
            RefreshTeacherList();
            Stk_SearchTeacher.Visibility = Visibility.Visible;
        }

        private void BtnDeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            Stk_DeleteTeacher.Visibility = Visibility.Hidden;
            BtnDeleteCancel.Visibility = Visibility.Visible;
            BtnDeleteTeacher.Visibility = Visibility.Visible;
            BtnDeleteReturn.Visibility = Visibility.Collapsed;
            TbxDeleteClassID.Text = "";
            TbxDeleteTeacherGiven.Text = "";
            TbxDeleteTeacherSurname.Text = "";
            TbxDeleteTeacherID.Text = "";
            RefreshTeacherList();
            Stk_SearchTeacher.Visibility = Visibility.Visible;
        }

        private void MnuIDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            Stk_DeleteTeacher.Visibility = Visibility.Visible;
            var selectedTeacher = teacherUtils.GetTeacher(Convert.ToInt16(LstTeacherSearch.SelectedValue.ToString()));
            TbxDeleteClassID.Text = selectedTeacher.ClassId.ToString();
            TbxDeleteTeacherGiven.Text = selectedTeacher.GivenName.ToString();
            TbxDeleteTeacherSurname.Text = selectedTeacher.Surname.ToString();
            TbxDeleteTeacherID.Text = selectedTeacher.TeacherId.ToString();
            Stk_SearchTeacher.Visibility = Visibility.Hidden;
        }

        private void MnuIUpdateTeacher_Click(object sender, RoutedEventArgs e)
        {
            Stk_UpdateTeacher.Visibility = Visibility.Visible;
            var selectedTeacher = teacherUtils.GetTeacher(Convert.ToInt16(LstTeacherSearch.SelectedValue.ToString()));
            TbxUpdateTeacherGiven.Text= selectedTeacher.GivenName.ToString();
            TbxUpdateTeacherSurname.Text = selectedTeacher.Surname.ToString();
            TbxUpdateTeacherID.Text = selectedTeacher.TeacherId.ToString();
            Stk_SearchTeacher.Visibility = Visibility.Hidden;
        }

        private void BtnUpdateTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int teacherUpdated = teacherUtils.UpdateTeacher(new Teacher
                {
                    TeacherId = Convert.ToInt16(TbxUpdateTeacherID.Text.ToString()),
                    GivenName = TbxUpdateTeacherGiven.Text.ToString(),
                    Surname = TbxUpdateTeacherSurname.Text.ToString(),
                    ClassId = Convert.ToInt16(CmbBxClassUpdateSelector.SelectedValue.ToString())
                });

                if (teacherUpdated == 1)
                {
                    BtnUpdateCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateTeacher.Visibility = Visibility.Collapsed;
                    BtnUpdateReturn.Visibility = Visibility.Visible;
                    TbxUpdateTeacherGiven.IsReadOnly = true;
                    TbxUpdateTeacherSurname.IsReadOnly = true;
                    TbxUpdateTeacherID.IsReadOnly = true;
                    CmbBxClassUpdateSelector.IsReadOnly = true;
                    Lbl_UpdateTeacherLabel.Content = "Teacher Successfully Updated";


                    try
                    {
                        systemEventUtils.AddSystemEvent(new SystemEvent
                        {
                            UserId = systemUser.UserId,
                            EventTypeId = 4,
                            EventDateTime = DateTime.Now,
                            EventData = $"Teacher record updated at { DateTime.Now} , by {systemUser.Username}"
                        });
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    BtnUpdateCancel.Visibility = Visibility.Collapsed;
                    BtnUpdateTeacher.Visibility = Visibility.Collapsed;
                    BtnUpdateReturn.Visibility = Visibility.Visible;
                    Lbl_TeacherUpdateErrorLabel.Visibility = Visibility.Visible;

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
        }

        private void BtnUpdateCancel_Click(object sender, RoutedEventArgs e)
        {
            Stk_UpdateTeacher.Visibility = Visibility.Hidden;
            TbxUpdateTeacherGiven.Text = "";
            TbxUpdateTeacherSurname.Text = "";
            TbxUpdateTeacherID.Text = "";
            RefreshTeacherList();
            Stk_SearchTeacher.Visibility = Visibility.Visible;
        }

        private void BtnUpdateReturn_Click(object sender, RoutedEventArgs e)
        {
            Stk_UpdateTeacher.Visibility = Visibility.Hidden;
            TbxUpdateTeacherGiven.Text = "";
            TbxUpdateTeacherSurname.Text = "";
            TbxUpdateTeacherID.Text = "";
            TbxUpdateTeacherGiven.IsReadOnly = false;
            TbxUpdateTeacherSurname.IsReadOnly = false;
            TbxUpdateTeacherID.IsReadOnly = false;
            CmbBxClassUpdateSelector.IsReadOnly = false;
            RefreshTeacherList();
            Stk_SearchTeacher.Visibility = Visibility.Visible;
        }
    }
}
