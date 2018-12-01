using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class SystemEventUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        /// <summary>
        /// Method adds new SystemEvent to the system database
        /// </summary>
        /// <param name="systemEvent">
        /// new SystemEvent object
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddSystemEvent(SystemEvent systemEvent)
        {
            // Add the system event to the DataBase
            smaDB.Entry(systemEvent).State = System.Data.Entity.EntityState.Added;
            
            return smaDB.SaveChanges();

        }





    }
}
