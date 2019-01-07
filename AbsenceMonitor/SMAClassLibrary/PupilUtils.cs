using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
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
                smaDB.Entry(addedPupil).State = EntityState.Added;
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
            Pupil pupil = GetPupilDetails(pupilId);
            smaDB.Pupils.Remove(pupil);
            // Return an int signifying successful deletion
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
            // Return the selected pupil
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
            // Return the list of pupils
            return classPupils.ToList();
        }

        /// <summary>
        /// Method adds a list of Attendance records and PupilAttendances to the system database
        /// </summary>
        /// <param name="attendanceDate">
        /// Date selected by the user
        /// </param>
        /// <param name="selectedClassId">
        /// Class selected by the user
        /// </param>
        /// <param name="pupils">
        /// List of pupils from the selected class
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int RecordPupilAttendance(DateTime attendanceDate, int selectedClassId, List<Pupil> pupils)
        {
            // Initialise the attendanceId and a List of PupilAttendances
            int attendanceId = 0;
            List<PupilAttendance> pupilAttendances = new List<PupilAttendance>();
            try
            {
                /*
                 *Check the database to see if we have an attendance record for the specified date.
                 * Use this record if it exists otherwise create a new attendance record.
                 */

                    // Create a new attendance object with the date passed in by the user.
                    Attendance attendance = new Attendance
                    {
                        AttendanceDate = attendanceDate.Date,
                        ClassId = selectedClassId

                    };

                    // Add the attendance to the database
                    try
                    {
                        smaDB.Entry(attendance).State = EntityState.Added;
                        smaDB.SaveChanges();
                        attendanceId = attendance.AttendanceId;

                    }
                    catch (Exception)
                    {
                        // Show an error on failure
                        MessageBox.Show("Problem adding attendance records, Please contact the System Administrator", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        // Show an error on failure
                        MessageBox.Show("Problem adding pupil attendance records", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
                // Return an int signifying success
                return smaDB.SaveChanges();

            }
            catch (Exception)
            {
                // Show an error on failure
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
            // Add the absence passed into the method to the system database
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

        /// <summary>
        /// Method returns a count of pupil absences
        /// </summary>
        /// <param name="pupilId">
        /// pupilId selected by the user
        /// </param>
        /// <returns>
        /// Count of pupil absences
        /// </returns>
        public int CountPupilAbsences(int pupilId)
        {
            // Get a count of absences for the selected pupil
            int absenceCount = smaDB.PupilAbsences.Where(p => p.PupilId == pupilId).ToList().Count();

            // Return the count
            return absenceCount;
        }

        /// <summary>
        /// Method deletes the absence selected by the user and creates an attendance for the pupil for that date
        /// </summary>
        /// <param name="absenceId">
        /// The absenceId of the absence to be selected
        /// </param>
        /// <param name="pupilId">
        /// The selected pupil's pupilId
        /// </param>
        /// <param name="absenceDate">
        /// The date of the absence
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int DeletePupilAbsence(int absenceId, int pupilId, DateTime absenceDate)
        {
            /* Need to do two deletd operations at this point:
             * First delete pupilAbsence
             * Secondly delete the associated absence
             */
            int deletionCompleted = 0;
            try
            {
                PupilAbsence pupilAbsence = smaDB.PupilAbsences.Where(p => p.AbsenceId == absenceId && p.PupilId == pupilId).FirstOrDefault();
                smaDB.PupilAbsences.Remove(pupilAbsence);
                deletionCompleted = smaDB.SaveChanges();

                Absence absence = smaDB.Absences.Where(a => a.AbsenceId == absenceId).FirstOrDefault();
                smaDB.Absences.Remove(absence);
                deletionCompleted = smaDB.SaveChanges();

                // If the absence has been deleted we need to create an attendance for that date for that pupil
                if (deletionCompleted == 1)
                {
                    deletionCompleted = CreatePupilAttendane(pupilId, absenceDate); 
                }
            }
            catch (Exception ex)
            {
                // Show an error on failure
                MessageBox.Show($"System Database Error, Please contact the System Administrator{ex.GetBaseException().ToString()}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return deletionCompleted;
        }

        /// <summary>
        /// Method creates an pupilAttendance record for the selected pupil
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="attendanceDate"></param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int CreatePupilAttendane(int pupilId, DateTime attendanceDate)
        {
            /* Need to check the database to see if an attendance exists for the selected date and is associated to the pupil's class
             * Get the id of the attendance if it exists and create the pupilAttendance 
             * If no attendance exists for the date and class selected we need to create one for that date and then create the pupilAttendance
             */
            Pupil selectedPupil = GetPupilDetails(pupilId);
            int attendanceCreated = 0;
            if (smaDB.Attendances.Any(a=> DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(attendanceDate) && a.ClassId == selectedPupil.ClassId))
            {
                // An attendance exists for the date and class 
                var attendance = smaDB.Attendances.Where(a=> DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(attendanceDate) && a.ClassId == selectedPupil.ClassId).FirstOrDefault();
                smaDB.PupilAttendances.Add(new PupilAttendance
                {
                    AttendanceId = attendance.AttendanceId,
                    PupilId = pupilId
                });
                attendanceCreated = smaDB.SaveChanges();
            }
            else
            {
                // No attendance exists for the date and class 
                smaDB.Attendances.Add(new Attendance
                {
                    AttendanceDate = attendanceDate,
                    ClassId = selectedPupil.ClassId
                });
                smaDB.SaveChanges();

                // Create the pupilAttendance for the selected date
                var attendance = smaDB.Attendances.Where(a => DbFunctions.TruncateTime(a.AttendanceDate) == DbFunctions.TruncateTime(attendanceDate) && a.ClassId == selectedPupil.ClassId).FirstOrDefault();
                smaDB.PupilAttendances.Add(new PupilAttendance
                {
                    AttendanceId = attendance.AttendanceId,
                    PupilId = pupilId
                });
                attendanceCreated = smaDB.SaveChanges();

            }

            return attendanceCreated; 
        }

        /// <summary>
        /// Method updates the selected absence
        /// </summary>
        /// <param name="absenceId">
        /// AbsenceId of the selected absence
        /// </param>
        /// <param name="absenceTypeId">
        /// AbsenceTypeId of the selected AbsenceType
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int UpdatePupilAbsence(int absenceId, int absenceTypeId )
        {
            // Get the absence to be updated
            Absence updatedAbsence = smaDB.Absences.FirstOrDefault(a => a.AbsenceId == absenceId);
            // Update the absenceTypeId with the selected value
            updatedAbsence.AbsenceTypeId = absenceTypeId;
            return smaDB.SaveChanges();
        }
    }
}
