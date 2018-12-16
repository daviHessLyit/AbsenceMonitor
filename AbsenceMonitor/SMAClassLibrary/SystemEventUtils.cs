using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

        /// <summary>
        /// Method to sort SystemEvents by Date
        /// </summary>
        /// <param name="systemEvents">
        /// List<SystemEvent>
        /// </param>
        /// <returns>
        /// List<SystemEvent>
        /// </returns>
        public List<SystemEvent> SortEventsByDate(List<SystemEvent> systemEvents)
        {
           var sortedEvents = systemEvents.OrderByDescending(e => e.EventDateTime.Date)
                                           .OrderByDescending(e => e.EventDateTime.Year);

            return sortedEvents.AsEnumerable().ToList();
        }

        /// <summary>
        /// Method to sort SystemEvents by Type and Date Descending
        /// </summary>
        /// <param name="systemEvents">
        /// List<SystemEvent>
        /// </param>
        /// <returns>
        /// List<SystemEvent> sortedEvents
        /// </returns>
        public List<SystemEvent> SortEventsByType(List<SystemEvent> systemEvents)
        {
            var sortedEvents = systemEvents.OrderBy(e => e.EventTypeId)
                                           .OrderByDescending(e => e.EventDateTime.Date)
                                           .OrderByDescending(e => e.EventDateTime.Year);

            return sortedEvents.AsEnumerable().ToList();
        }

        /// <summary>
        /// Method to sort SystemEvents by User and Date Descending
        /// </summary>
        /// <param name="systemEvents">
        /// List<SystemEvent>
        /// </param>
        /// <returns>
        /// List<SystemEvent> sortedEvents
        /// </returns>
        public List<SystemEvent> SortEventsByUser(List<SystemEvent> systemEvents)
        {
            var sortedEvents = systemEvents.OrderBy(e => e.UserId)
                                           .OrderByDescending(e => e.EventTypeId);
            return sortedEvents.AsEnumerable().ToList();
        }

        /// <summary>
        /// Method to return events by userName
        /// </summary>
        /// <param name="systemEvents">
        /// List<SystemEvent>
        /// </param>
        /// <returns>
        /// List<SystemEvent> sortedEvents
        /// </returns>
        public List<SystemEvent> SortEventsByUsername(List<SystemEvent> systemEvents, string userName)
        {
            List<SystemEvent> sortedEvents = new List<SystemEvent>();

            try
            {
                int selectedUserId = smaDB.SystemUsers.Where(u => u.Username == userName).FirstOrDefault().UserId;

                foreach (var item in systemEvents)
                {
                    if (selectedUserId == item.UserId)
                    {
                        sortedEvents.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            } 

            return sortedEvents.AsEnumerable().ToList();
        }

        /// <summary>
        /// Method to return events from the selected date.
        /// </summary>
        /// <param name="systemEvents">
        /// DateTime selectedDate
        /// </param>
        /// <returns>
        /// List<SystemEvent> sortedEvents
        /// </returns>
        public List<SystemEvent> SortEventsByDate( DateTime selectedDate)
        {           
            try
            {
                List<SystemEvent> sortedEvents = smaDB.SystemEvents.Where(e => DbFunctions.TruncateTime( e.EventDateTime) == DbFunctions.TruncateTime(selectedDate)).ToList();
                return sortedEvents;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
