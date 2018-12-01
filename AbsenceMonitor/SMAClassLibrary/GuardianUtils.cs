using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class GuardianUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        /// <summary>
        /// Method adds new Guardian details to the system database
        /// </summary>
        /// <param name="addedGuardian">
        /// Guardian addedGuardian
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddGuardian(Guardian addedGuardian)
        {
            var guardian = smaDB.Guardians.Where(g => g.GivenName == addedGuardian.GivenName && g.Surname == addedGuardian.Surname && g.MobileNo == addedGuardian.MobileNo).FirstOrDefault();

            if (guardian != null)
            {
                return 0;
            }
            else
            {
                smaDB.Entry(addedGuardian).State = System.Data.Entity.EntityState.Added;
                return smaDB.SaveChanges();
            }            
        }


        /// <summary>
        /// Method updates existing Guardian details on the system database
        /// </summary>
        /// <param name="guardian">
        /// Guardian guardian
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int UpdateGuardian(Guardian guardian)
        {
            Guardian existingGuardian = new Guardian();
            existingGuardian = smaDB.Guardians.Where(g => g.GuardianId == guardian.GuardianId).FirstOrDefault();

            existingGuardian.GivenName = guardian.GivenName;
            existingGuardian.Surname = guardian.Surname;
            existingGuardian.MobileNo = guardian.MobileNo;
            existingGuardian.EmergencyNo = guardian.EmergencyNo;
            existingGuardian.Address = guardian.Address;

            smaDB.Entry(existingGuardian).State = System.Data.Entity.EntityState.Modified;

            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method to delete selected guardian details from the system database
        /// </summary>
        /// <param name="guardianId">
        /// int guardianId
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int DeleteGuardian(int guardianId)
        {

            Guardian guardian = new Guardian();

            guardian = smaDB.Guardians.Where(g => g.GuardianId == guardianId).FirstOrDefault();

            smaDB.Entry(guardian).State = System.Data.Entity.EntityState.Deleted;

            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method to return individual Guardian details that match the guardianId
        /// </summary>
        /// <param name="guardianId">
        /// int GuardianId 
        /// </param>
        /// <returns>
        /// Guardian object
        /// </returns>
        public Guardian GetGuardianDetails(int guardianId)
        {
            Guardian guardian = new Guardian();

            guardian = smaDB.Guardians.Where(g => g.GuardianId == guardianId).FirstOrDefault();

            return guardian;
        }


    }
}
