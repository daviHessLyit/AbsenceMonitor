using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class AbsenceTypeUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        /// <summary>
        /// Method to add a new AbsenceType record to the system database
        /// </summary>
        /// <param name="newAbsenceType">
        /// AbsenceType newAbsenceType
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddAbsenceType(AbsenceType newAbsenceType)
        {
            // Add the absenceType to the database
            var absenceType = smaDB.AbsenceTypes.Where(a => a.AbsenceType1 == newAbsenceType.AbsenceType1).FirstOrDefault();
            if (absenceType != null)
            {
                return 0;
            }
            else
            {
                smaDB.Entry(newAbsenceType).State = System.Data.Entity.EntityState.Added;
                return smaDB.SaveChanges();
            }

        }

        /// <summary>
        /// Method deletes AbsenceType record from the system database
        /// </summary>
        /// <param name="absenceTypeId">
        /// int absenceTypeId
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int DeleteAbsenceType(int absenceTypeId)
        {
            // Add selected absenceType from the database
            AbsenceType absenceType = new AbsenceType();

            absenceType = smaDB.AbsenceTypes.Where(a=> a.AbsenceTypeId == absenceTypeId).FirstOrDefault();

            smaDB.Entry(absenceType).State = System.Data.Entity.EntityState.Deleted;
            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method updates existing AbsenceType record on the system database
        /// </summary>
        /// <param name="absenceType">
        /// AbsenceType absenceType
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int UpdateAbsenceType(AbsenceType absenceType)
        {
            // Update the absenceType on the database
            AbsenceType existingAbsenceType = new AbsenceType();
            existingAbsenceType = smaDB.AbsenceTypes.Where(a => a.AbsenceTypeId == absenceType.AbsenceTypeId).FirstOrDefault();

            existingAbsenceType.AbsenceType1 = absenceType.AbsenceType1;

            smaDB.Entry(existingAbsenceType).State = System.Data.Entity.EntityState.Modified;

            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method returns the selected AbsenceType record from the system database
        /// </summary>
        /// <param name="absenceTypeId">
        /// int absenceTypeId
        /// </param>
        /// <returns>
        /// AbsenceType object
        /// </returns>
        public AbsenceType GetAbsenceType(int absenceTypeId)
        {
            // Populate and return the selected absenceType
            AbsenceType absenceType = new AbsenceType();

            absenceType = smaDB.AbsenceTypes.Where(a => a.AbsenceTypeId == absenceTypeId).FirstOrDefault();

            return absenceType;
        }

    }
}
