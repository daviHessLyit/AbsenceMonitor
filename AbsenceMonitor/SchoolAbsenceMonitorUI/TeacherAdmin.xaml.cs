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
        }

        private void RefreshTeacherList()
        {
            teachers.Clear();
            LstTeacherSearch.ItemsSource = teachers;
            foreach (var teacher in smaDB.Teachers)
            {
                teachers.Add(teacher);
            }
        }

        private void BtnAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int teacherAddedSuccess = teacherUtils.AddTeacher(new Teacher
                {
                    GivenName = TbxTeacherGiven.Text.ToString(),
                    Surname = TbxTeacherSurname.Text.ToString(),
                    ClassId = Convert.ToUInt16(TbxTeacherClassID.Text.ToString())
                    
                });

                if (teacherAddedSuccess == 1)
                {
                    BtnAddTeacher.Visibility = Visibility.Collapsed;
                    BtnAddCancel.Visibility = Visibility.Collapsed;
                    TbxTeacherGiven.IsEnabled = false;
                    TbxTeacherSurname.IsEnabled = false;
                    TbxTeacherClassID.IsEnabled = false;
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
            TbxTeacherClassID.Text = "";
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
            TbxTeacherClassID.IsEnabled = true;
            TbxTeacherGiven.Text = "";
            TbxTeacherSurname.Text = "";
            TbxTeacherClassID.Text = "";
            RefreshTeacherList();
            Stk_SearchTeacher.Visibility = Visibility.Visible;
        }
    }
}
