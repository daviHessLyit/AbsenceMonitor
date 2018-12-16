using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace SMAClassLibrary
{
    public class PupilUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
       
        /// <summary>
        /// Method adds a new pupil record to the system database
        /// </summary>
        /// <param name="addedPupil">
        /// The pupil details added by the user.
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddPupil(Pupil addedPupil)
        {
            // Check the database to ensure the pupil doesn't already exist
            var pupil = smaDB.Pupils.Where(p => p.GivenName == addedPupil.GivenName && p.Surname == addedPupil.Surname && p.GuardianId == addedPupil.GuardianId).FirstOrDefault();

            // If the pupil exists return 0
            if (pupil !=null)
            {
                return 0;
            }
            else
            {
                // Commit the new pupil to the database.
                smaDB.Entry(addedPupil).State = System.Data.Entity.EntityState.Added;
                return smaDB.SaveChanges();
            }
        }

        /// <summary>
        ///  Method updates existing Pupil details on the system database
        /// </summary>
        /// <param name="pupil">
        /// Pupil details entered by the user.
        /// </param>
        /// <returns>
        ///  int signifying success (1) or failure (0) of operation
        /// </returns>
        public int UpdatePupil(Pupil pupil)
        {          

            // Loop through the pupils list and update the pupil matching the pupil id.
            foreach (var existingPupil in smaDB.Pupils.Where(p => p.PupilId == pupil.PupilId))
            {
                existingPupil.GivenName = pupil.GivenName;
                existingPupil.Surname = pupil.Surname;
                existingPupil.ClassId = pupil.ClassId;
                existingPupil.GuardianId = pupil.GuardianId;                
            }
            // Return an int 
            return smaDB.SaveChanges();

        }

        /// <summary>
        /// Method to delete selected pupil details from the system database
        /// </summary>
        /// <param name="pupilId">
        /// The selected pupilId
        /// </param>
        /// <returns>
        ///  int signifying success (1) or failure (0) of operation
        /// </returns>
        public int DeletePupil(int pupilId)
        {
            // Delete the pupil from the database meeting the criteria
            smaDB.Entry(smaDB.Pupils.Where(p => p.PupilId == pupilId)).State = System.Data.Entity.EntityState.Deleted;

            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method to return individual Pupil details that match the pupilId
        /// </summary>
        /// <param name="pupilId">
        /// The selected pupilId
        /// </param>
        /// <returns>
        /// Pupil object
        /// </returns>
        public Pupil GetPupilDetails(int pupilId)
        {
            Pupil pupil = new Pupil();

            // Return a pupil object matching the pupilId passed by the user.
            pupil = smaDB.Pupils.Where(p => p.PupilId == pupilId).FirstOrDefault();

            return pupil;
        }

        /// <summary>
        /// Method returns a list of Pupils for the selected class
        /// </summary>
        /// <param name="classId">
        /// int classId
        /// </param>
        /// <returns>
        /// List<Pupil>
        /// </returns>
        public List<Pupil> GetPupilsByClass(int classId)
        {
            // Return a list of pupils from the selected class.
            var classPupils = smaDB.Pupils.Where(p => p.ClassId == classId);

            return classPupils.ToList();
        }


        public int RecordPupilAttendance(DateTime attendanceDate, List<Pupil> pupils)
        {
            int attendanceId = 0;
            List<PupilAttendance> pupilAttendances = new List<PupilAttendance>();
            try
            {
                /*
                 *Check the database to see if we have an attendance record for the specified date.
                 * Use this record if it exists otherwise create a new attendance record.
                 */

                if (smaDB.Attendances.Any(a => DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(attendanceDate)))
                {
                    var existingAttendance = smaDB.Attendances.Where(a => DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(attendanceDate)).FirstOrDefault();

                    attendanceId = existingAttendance.AttendanceId;
                }
                else
                {
                    // Create a new attendance object with the date passed in by the user.
                    Attendance attendance = new Attendance
                    {
                        AttendanceDate = attendanceDate.Date
                    };

                    // Add the attendance to the database
                    try
                    {
                        smaDB.Entry(attendance).State = System.Data.Entity.EntityState.Added;
                        smaDB.SaveChanges();
                        attendanceId = attendance.AttendanceId;

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Problem adding attendance records, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }            
                
                // Loop through the list of pupils and create a pupilAttendance object for each pupil
                foreach (var pupil in pupils)
                {
                    pupilAttendances.Add(
                    new PupilAttendance
                    {
                        PupilId = pupil.PupilId,
                        AttendanceId = attendanceId
                    });
                }

                // Commit the pupilAttendances to the database.
                if (pupilAttendances.Count >0)
                {
                    try
                    {
                        smaDB.PupilAttendances.AddRange(pupilAttendances);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Problem adding pupil attendance records", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }

                return smaDB.SaveChanges();

            }
            catch (Exception)
            {
                MessageBox.Show("Problem adding attendance records", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }           
        }

        /// <summary>
        /// Method will do simultaneous inserts to the pupilAbsence table and the Absence table
        /// </summary>
        /// <param name="absence">
        /// Absence details
        /// </param>
        /// <param name="pupilId">
        /// PupilId of the selected pupil
        /// </param>
        /// <returns>
        ///  int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddPupilAbsence(Absence absence, int pupilId)
        {
            // Add the absence passed into  the method to the system database
            smaDB.Absences.Add(absence);
            smaDB.SaveChanges();

            // Get the absenceId from the absence
            int absenceId = absence.AbsenceId;

            // Create a new pupilAbsence object and pass in the absenceId and pupilId
            smaDB.PupilAbsences.Add(new PupilAbsence
            {
                PupilId = pupilId,
                AbsenceId = absenceId
            });

            // Save the changes and return an int signifying success or failure.
            return smaDB.SaveChanges();
        }


    }
}
