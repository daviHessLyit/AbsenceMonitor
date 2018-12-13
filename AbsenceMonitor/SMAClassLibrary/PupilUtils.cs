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
       
        /// <summary>
        /// Method adds a new pupil record to the system database
        /// </summary>
        /// <param name="addedPupil">
        /// The pupil details added by the user.
        /// </param>
        /// <returns>
        /// int signifying success (1) or failure (0) of operation
        /// </returns>
        public int AddPupil(Pupil addedPupil)
        {
            var pupil = smaDB.Pupils.Where(p => p.GivenName == addedPupil.GivenName && p.Surname == addedPupil.Surname && p.GuardianId == addedPupil.GuardianId).FirstOrDefault();

            if (pupil !=null)
            {
                return 0;
            }
            else
            {
                smaDB.Entry(addedPupil).State = System.Data.Entity.EntityState.Added;
                return smaDB.SaveChanges();
            }
        }

        /// <summary>
        ///  Method updates existing Pupil details on the system database
        /// </summary>
        /// <param name="pupil">
        /// Pupil details entered by the user.
        /// </param>
        /// <returns>
        ///  int signifying success (1) or failure (0) of operation
        /// </returns>
        public int UpdatePupil(Pupil pupil)
        {
            Pupil existingPupil = new Pupil();

            existingPupil = smaDB.Pupils.Where(p => p.PupilId == pupil.PupilId).FirstOrDefault();

            if (existingPupil != null)
            {
                existingPupil.GivenName = pupil.GivenName;
                existingPupil.Surname = pupil.Surname;
                existingPupil.ClassId = pupil.ClassId;
                existingPupil.GuardianId = pupil.GuardianId;

                smaDB.Entry(existingPupil).State = System.Data.Entity.EntityState.Modified;

                return smaDB.SaveChanges();
            }
            else
            {
                return 0;
            }
            
        }

        /// <summary>
        /// Method to delete selected pupil details from the system database
        /// </summary>
        /// <param name="pupilId">
        /// The selected pupilId
        /// </param>
        /// <returns>
        ///  int signifying success (1) or failure (0) of operation
        /// </returns>
        public int DeletePupil(int pupilId)
        {
            Pupil pupil = new Pupil();
            pupil = smaDB.Pupils.Where(p => p.PupilId == pupil.PupilId).FirstOrDefault();

            smaDB.Entry(pupil).State = System.Data.Entity.EntityState.Deleted;

            return smaDB.SaveChanges();
        }

        /// <summary>
        /// Method to return individual Pupil details that match the pupilId
        /// </summary>
        /// <param name="pupilId">
        /// The selected pupilId
        /// </param>
        /// <returns>
        /// Pupil object
        /// </returns>
        public Pupil GetPupilDetails(int pupilId)
        {
            Pupil pupil = new Pupil();

            pupil = smaDB.Pupils.Where(p => p.PupilId == pupil.PupilId).FirstOrDefault();

            return pupil;
        }


    }
}
