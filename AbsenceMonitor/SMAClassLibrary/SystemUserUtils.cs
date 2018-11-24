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

        public void AddSystemUser(SystemUser systemUser)
        {
            // Check to see if the userName exists in the SystemDb first - if not add the new system user.

            var newUser = smaDB.SystemUsers.Where(u => u.Username == systemUser.Username).FirstOrDefault();
            if (newUser !=null)
            {
                throw new Exception("User Exists in the DB");
            }
            else
            {
                // Add the system user to the DataBase
                smaDB.SystemUsers.Add(systemUser);
                smaDB.SaveChanges();
            }
          

        }

        public void DeleteSystemUser(int systemUserId)
        {
            SystemUser systemUser = smaDB.SystemUsers.FirstOrDefault(s => s.UserId == systemUserId);
            // Add the system user to the DataBase
            smaDB.SystemUsers.Remove(systemUser);
            smaDB.SaveChanges();

        }

    }
}
