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


        public PupilAdmin()
        {
            InitializeComponent();
        }

        private void BtnAddPupilCancel_Click(object sender, RoutedEventArgs e)
        {
            TbxPupilGiven.Text = "";
            TbxPupilSurname.Text = "";
            TbxPupilGuardianID.Text = "";
            TbxPupilClassID.Text = "";
            LblPupilErrorLabel.Visibility = Visibility.Collapsed;
        }

        private void BtnAddPupilReturn_Click(object sender, RoutedEventArgs e)
        {
            Stk_AddPupil.Visibility = Visibility.Hidden;
            TbxPupilGiven.Text = "";
            TbxPupilSurname.Text = "";
            TbxPupilGuardianID.Text = "";
            TbxPupilClassID.Text = "";
            BtnAddPupil.Visibility = Visibility.Visible;
            BtnAddPupilCancel.Visibility = Visibility.Visible;
            BtnAddPupilReturn.Visibility = Visibility.Collapsed;
            LblPupilErrorLabel.Visibility = Visibility.Collapsed;
            RefreshPupilList();
            Stk_SearchPupil.Visibility = Visibility.Visible;

        }

        private void BtnAddPupil_Click(object sender, RoutedEventArgs e)
        {
            bool validFormData = validationUtils.VerifyPupilFormData(
                TbxPupilGiven.Text.ToString(),
                TbxPupilSurname.Text.ToString(),
                TbxPupilGuardianID.Text.ToString(),
                TbxPupilClassID.Text.ToString());

            if (validFormData)
            {
                LblPupilErrorLabel.Visibility = Visibility.Collapsed;
                try
                {
                    int pupilAdded = pupilUtils.AddPupil(new Pupil
                    {
                        GivenName = TbxPupilGiven.Text.ToString(),
                        Surname = TbxPupilSurname.Text.ToString(),
                        GuardianId = Convert.ToInt16(TbxPupilGuardianID.Text.ToString()),
                        ClassId = Convert.ToInt16(TbxPupilGuardianID.Text.ToString())
                    });

                    if (pupilAdded == 1)
                    {
                        TbxPupilGiven.IsEnabled = false;
                        TbxPupilSurname.IsEnabled = false;
                        TbxPupilGuardianID.IsEnabled = false;
                        TbxPupilClassID.IsEnabled = false;
                        BtnAddPupil.Visibility = Visibility.Collapsed;
                        BtnAddPupilCancel.Visibility = Visibility.Collapsed;
                        BtnAddPupilReturn.Visibility = Visibility.Visible;
                        Lbl_PupilLabel.Content = "Pupil Details Successfully Added";

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
                            MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        TbxPupilGiven.Text = "";
                        TbxPupilSurname.Text = "";
                        TbxPupilGuardianID.Text = "";
                        TbxPupilClassID.Text = "";
                        BtnAddPupil.Visibility = Visibility.Collapsed;
                        BtnAddPupilCancel.Visibility = Visibility.Collapsed;
                        BtnAddPupilReturn.Visibility = Visibility.Visible;
                        LblPupilErrorLabel.Visibility = Visibility.Visible;

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
                LblPupilErrorLabel.Content = "Invalid form data, please correct and resubmit";
                LblPupilErrorLabel.Visibility = Visibility.Visible;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        { 
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
        }

        private void RefreshPupilList()
        {
            var pupilList = from _pupil in smaDB.Pupils.ToList()
                            join _schoolClass in smaDB.Classes on _pupil.ClassId equals _schoolClass.ClassId
                            join _guardian in smaDB.Guardians on _pupil.GuardianId equals _guardian.GuardianId
                            select new
                            {
                                PupilId = _pupil.PupilId,
                                PupilName = _pupil.FullName,
                                SchoolClass = _schoolClass.ClassName,
                                GuardianFullName = _guardian.FullName
                            };

            LstPupilSearch.ItemsSource = pupilList;
            LstPupilSearch.Items.Refresh();

        }

        private void MnuIUpdatePupil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPupil = pupilUtils.GetPupilDetails(Convert.ToInt16(LstPupilSearch.SelectedValue.ToString()));

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
                MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdatePupil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int pupilUpdated = pupilUtils.UpdatePupil(new Pupil
                {
                    GivenName = TbxUpdatePupilGiven.Text.ToString(),
                    Surname = TbxUpdatePupilSurname.Text.ToString(),
                    ClassId = Convert.ToInt16(TbxUpdatePupilClassID.Text.ToString()),
                    GuardianId = Convert.ToInt16(TbxUpdatePupilGuardianID.Text.ToString()),
                    PupilId = Convert.ToInt16(TbxUpdatePupilID.Text.ToString())

                });

                if (pupilUpdated == 1)
                {
                    BtnUpdatePupil.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilCancel.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilReturn.Visibility = Visibility.Visible;
                    TbxUpdatePupilGiven.IsReadOnly = true;
                    TbxUpdatePupilSurname.IsReadOnly = true;
                    TbxUpdatePupilClassID.IsReadOnly = true;
                    TbxUpdatePupilID.IsReadOnly = true;
                    TbxUpdatePupilGuardianID.IsReadOnly = true;
                    Lbl_UpdatePupilLabel.Content = "Pupil details Successfully Updated";


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
                        MessageBox.Show("System Database Error, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    BtnUpdatePupil.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilCancel.Visibility = Visibility.Collapsed;
                    BtnUpdatePupilReturn.Visibility = Visibility.Visible;
                    Lbl_PupilUpdateErrorLabel.Visibility = Visibility.Visible;

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
    }
}
