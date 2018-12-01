using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{

    public class SchoolClassUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        /// <summary>
        /// Method to add a new Class record to the system database
        /// </summary>
        /// <param name="newSchoolClass">
        /// Class newSchoolClass
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddSchoolClass(Class newSchoolClass)
        {
            var schoolClass = smaDB.Classes.Where(c => c.ClassName == newSchoolClass.ClassName).FirstOrDefault();
            if (schoolClass !=null)
            {
                return 0;
            }
            else
            {
                smaDB.Entry(newSchoolClass).State = System.Data.Entity.EntityState.Added;
                return smaDB.SaveChanges();
            }
            
        }

        /// <summary>
        /// Method deletes Class record from the system database
        /// </summary>
        /// <param name="classId">
        /// int classId
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int DeleteSchoolClass(int classId)
        {
            Class selectedClass = new Class();

            selectedClass = smaDB.Classes.Where(c => c.ClassId == classId).FirstOrDefault();

            smaDB.Entry(selectedClass).State = System.Data.Entity.EntityState.Deleted;
            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method updates existing Class record on the system database
        /// </summary>
        /// <param name="updateSchoolClass">
        /// Class updatedSchoolClass
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int UpdateClass(Class updateSchoolClass)
        {
            Class existingSchoolClass = new Class();

            existingSchoolClass = smaDB.Classes.Where(c => c.ClassId == updateSchoolClass.ClassId).FirstOrDefault();

            existingSchoolClass.ClassName = updateSchoolClass.ClassName;

            smaDB.Entry(existingSchoolClass).State = System.Data.Entity.EntityState.Modified;

            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method returns the selected Class record from the system database
        /// </summary>
        /// <param name="schoolClassId">
        /// int schooClassId
        /// </param>
        /// <returns>
        /// Class object
        /// </returns>
        public Class GetSchoolClass(int schoolClassId)
        {
            Class schoolClass = new Class();

            schoolClass = smaDB.Classes.Where(c => c.ClassId == schoolClassId).FirstOrDefault();

            return schoolClass;
        }

    }
}
