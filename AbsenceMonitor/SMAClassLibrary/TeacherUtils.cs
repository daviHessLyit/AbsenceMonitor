using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class TeacherUtils
    {

        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        /// <summary>
        /// Method to add a new Teacher to the system database
        /// </summary>
        /// <param name="newTeacher">
        /// new Teacher object
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddTeacher(Teacher newTeacher)
        {
            // Add the new teacher record to the system database
            var teacher = smaDB.Teachers.Where(t => t.GivenName == newTeacher.GivenName && t.Surname == newTeacher.Surname && t.ClassId == newTeacher.ClassId).FirstOrDefault();

            if (teacher != null)
            {
                return 0;
            }
            else
            {
                smaDB.Entry(newTeacher).State = System.Data.Entity.EntityState.Added;
                return smaDB.SaveChanges();
            }         

        }
        /// <summary>
        /// Method to delete a Teacher record from the system database
        /// </summary>
        /// <param name="teacherId">
        /// int teacherId
        /// </param>
        /// <returns>
        ///  int signifying success (1) or failure (0) of operation
        /// </returns>
        public int DeleteTeacher(int teacherId)
        {
            // Delete the selected teacher record from the system database
            Teacher teacher = new Teacher();
            teacher = smaDB.Teachers.Where(t => t.TeacherId == teacherId).FirstOrDefault();
            smaDB.Entry(teacher).State = System.Data.Entity.EntityState.Deleted;
            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method returns the selected Teacher record from the system database
        /// </summary>
        /// <param name="teacherId">
        /// int teacherId
        /// </param>
        /// <returns>
        /// Teacher object
        /// </returns>
        public Teacher GetTeacher(int teacherId)
        {
            // Return the selected teacher record from the system database
            Teacher requestedTeacher = new Teacher();
            requestedTeacher = smaDB.Teachers.Where(t => t.TeacherId == teacherId).FirstOrDefault();
            return requestedTeacher;
        }

        /// <summary>
        /// Method updates the selected Teacher record on the system database
        /// </summary>
        /// <param name="updateTeacher">
        /// Teacher object
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int UpdateTeacher(Teacher updateTeacher)
        {
            // Update the selected teacher record on the system database.
            Teacher existingTeacher = new Teacher();

            existingTeacher = smaDB.Teachers.Where(t => t.TeacherId == updateTeacher.TeacherId).FirstOrDefault();

            existingTeacher.GivenName = updateTeacher.GivenName;
            existingTeacher.Surname = updateTeacher.Surname;
            existingTeacher.ClassId = updateTeacher.ClassId;

            smaDB.Entry(existingTeacher).State = System.Data.Entity.EntityState.Modified;

            return smaDB.SaveChanges();
        }

    }
}
