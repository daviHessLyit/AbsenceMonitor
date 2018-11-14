using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAbsenceMonitorUI
{
    class Guardian
    {
        public Guardian(string givenName, string surname, string mobileNo)
        {
            this.GivenName = givenName;
            this.Surname = surname;
            this.MobileNo = mobileNo;
        }

        public Guardian(string givenName, string surname, string mobileNo, string emergencyNo, string address )
        {
            this.GivenName = givenName;
            this.Surname = surname;
            this.MobileNo = mobileNo;
        }

        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string EmergencyNo { get; set; }
        
    }
}
