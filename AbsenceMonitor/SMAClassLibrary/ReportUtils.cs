using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class ReportUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        public List<Pupil> ReturnPupilsByClass(int classId)
        {
            List<Pupil> pupils = new List<Pupil>();

            foreach (var pupil in smaDB.Pupils.Where(p => p.ClassId == classId))
            {
                pupils.Add(pupil);
            }

            return pupils;

        }

        public List<object> ReturnPupilByClass(int classId)
        {
            List<object> pupilsAbsencesByClass = new List<object>();
            var absenceList = smaDB.Pupils.Join(smaDB.Classes,
                (pupil => pupil.PupilId),
                (schoolClass => schoolClass.ClassId),
                (pupil, schoolClass) => new {
               pupilName = pupil.GivenName + " "+pupil.Surname,
               className = schoolClass.ClassName,
               schoolClassId = schoolClass.ClassId

            }).ToList();

            foreach (var pupil in absenceList.Where(p=> p.schoolClassId == classId))
            {
                pupilsAbsencesByClass.Add(pupil);
            }


            return pupilsAbsencesByClass;

        }
    }
}
