using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class SystemUserUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        // Add a new user to the database
        public int AddSystemUser(SystemUser systemUser)
        {
            // Check to see if the userName exists in the SystemDb first - if not add the new system user.

            var newUser = smaDB.SystemUsers.Where(u => u.Username == systemUser.Username).FirstOrDefault();
            if (newUser !=null)
            {
                return 0;
            }
            else
            {
                // Add the system user to the DataBase
                smaDB.Entry(systemUser).State = System.Data.Entity.EntityState.Added;
                // Save the changes to the database
                return smaDB.SaveChanges();
            }
          

        }

        // Method deletes the selected user from the database
        public void DeleteSystemUser(int systemUserId)
        {
            SystemUser systemUser = smaDB.SystemUsers.FirstOrDefault(s => s.UserId == systemUserId);
            // Remove the system user from the DataBase
            smaDB.Entry(systemUser).State = System.Data.Entity.EntityState.Deleted;
            // Save the changes to the database
            smaDB.SaveChanges();

        }

        // Method updates an existing user on the database.
        public int UpdateUserDetails(SystemUser systemUser)
        {
            SystemUser existingUserDetails = smaDB.SystemUsers.FirstOrDefault(s => s.UserId == systemUser.UserId);
            // Update the user details
            existingUserDetails.GivenName = systemUser.GivenName;
            existingUserDetails.Surname = systemUser.Surname;
            existingUserDetails.Password = systemUser.Password;
            existingUserDetails.AccessLevelId = systemUser.AccessLevelId;
            existingUserDetails.Username = systemUser.Username;

            smaDB.Entry(existingUserDetails).State = System.Data.Entity.EntityState.Modified;
            // Save the changes to the database
            return smaDB.SaveChanges();

        }

        // Method returns the selected user record from the data
        public SystemUser GetUserDetails(int systemUserId)
        {
            SystemUser requestSystemUser = smaDB.SystemUsers.Where(s => s.UserId == systemUserId).FirstOrDefault();

            return requestSystemUser;
        }

    }
}
