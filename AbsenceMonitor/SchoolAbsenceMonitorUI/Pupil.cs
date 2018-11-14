using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAbsenceMonitorUI
{
    class Pupil
    {
        public Pupil(string givenName, string surname)
        {
            this.GivenName = givenName;
            this.Surname = surname;
        }

        public Pupil(string givenName, string surname, int guardianID)
        {
            this.GivenName = givenName;
            this.Surname = surname;
            if (guardianID>0)
            {
                this.GuardianID = guardianID;
            }
        }

        public Pupil(string givenName, string surname, int guardianID, int classID)
        {
            this.GivenName = givenName;
            this.Surname = surname;
            if (guardianID > 0)
            {
                this.GuardianID = guardianID;
            }

            if (classID > 0)
            {
                this.ClassID = classID;
            }
        }

        public string GivenName { get; set; }
        public string Surname { get; set; }
        public int GuardianID { get; set; }
        public int ClassID { get; set; }

    }

    
}
