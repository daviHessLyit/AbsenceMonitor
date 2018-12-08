using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    class PupilUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        public SystemUser systemUser = new SystemUser();
        SystemEventUtils systemEventUtils = new SystemEventUtils();

        private void PopulatePupilInfo(int pupilId)
        {
            var pupilInfo = from _pupil in smaDB.Pupils.Where(p => p.PupilId == pupilId)
                            join _guardian in smaDB.Guardians on _pupil.GuardianId equals _guardian.GuardianId
                            join _schoolClass in smaDB.Classes on _pupil.ClassId equals _schoolClass.ClassId
                            join _teacher in smaDB.Teachers on _pupil.ClassId equals _teacher.ClassId
                            join _pupilAbsence in smaDB.PupilAbsences on _pupil.PupilId equals _pupilAbsence.PupilId
                            join _absence in smaDB.Absences on _pupilAbsence.AbsenceId equals _absence.AbsenceId
                            select new
                            {
                                PupilName = _pupil.FullName,
                                _schoolClass.ClassName,
                                GuardianName = _guardian.FullName,
                                TeacherName = _teacher.FullName
                            };
        }

        private void PopulatePupilInfoListByClass(int classId)
        {

        }




    }
}
