using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class SystemEventUtils
    {
        SMADBEntities SMADBEntities = new SMADBEntities();
        public void AddSystemEvent(SystemEvent systemEvent)
    {
            // Add the system event to the DataBase
            SMADBEntities.SystemEvents.Add(systemEvent);
            SMADBEntities.SaveChanges();


    }





    }
}
