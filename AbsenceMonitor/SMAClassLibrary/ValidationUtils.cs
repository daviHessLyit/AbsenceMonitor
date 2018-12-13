using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class ValidationUtils
    {
        SMADBEntities smaDB = new SMADBEntities("metadata = res://*/SchoolAbsenceMonitorModel.csdl|res://*/SchoolAbsenceMonitorModel.ssdl|res://*/SchoolAbsenceMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source=DBSERVER;initial catalog=SMA_DB;persist security info=True;user id=davihess;password=d4vidH355;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        /// <summary>
        /// Validate the user inputs 
        /// </summary>
        /// <param name="userName">
        /// UserName entered by the user
        /// </param>
        /// <param name="password"></param>
        /// <returns>
        /// bool validData
        /// </returns>
        public bool ValidateUserInput(string userName, string password)
        {
            bool validData = true;
            try
            {
                if (userName.Length == 0 || userName.Length >30)
                {
                    validData = false;
                }

                foreach (char ch in userName)
                {
                    if (ch >='0' && ch <='9')
                    {
                        validData = false;
                    }
                }

                if (password.Length == 0 || password.Length > 30)
                {
                    validData = false;
                }

            }
            catch (Exception)
            {

                validData = false;
            }

            return validData;
        }

        /// <summary>
        /// Method to validate form data
        /// </summary>
        /// <param name="givenName">
        /// GivenName entered by the user
        /// </param>
        /// <param name="surname">
        /// Surname entered by the user
        /// </param>
        /// <param name="address">
        /// Address entered by the user
        /// </param>
        /// <param name="mobileNo">
        /// MobileNo entered by the user
        /// </param>
        /// <returns>
        /// bool validData
        /// </returns>
        public bool VerifyFormData(string givenName, string surname, string address, string mobileNo)
        {
            bool validData = true;

            if (givenName.Length == 0 || givenName.Length > 30)
            {
                validData = false;
            }

            if (surname.Length == 0 || surname.Length > 30)
            {
                validData = false;
            }

            if (address.Length == 0 || address.Length > 100)
            {
                validData = false;
            }

            if (mobileNo.Length == 0 || mobileNo.Length > 30)
            {
                validData = false;
            }

            return validData;
        }


        /// <summary>
        /// Method validates inputs from the add pupil form
        /// </summary>
        /// <param name="givenName">
        /// GivenName entered by the user
        /// </param>
        /// <param name="surname">
        /// Surname entered by the user
        /// </param>
        /// <param name="guardianId">
        /// GuardianId entered by the user
        /// </param>
        /// <param name="classId">
        /// ClassId entered by the user
        /// </param>
        /// <returns>
        /// bool validData
        /// </returns>
        public bool VerifyPupilFormData(string givenName, string surname, string guardianId, string classId)
        {
            bool validData = true;

            if (givenName.Length == 0 || givenName.Length > 30)
            {
                validData = false;
            }
            else
            {
                foreach (char ch in givenName)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }
            }
           

            if (surname.Length == 0 || surname.Length > 30)
            {
                validData = false;
            }
            else
            {
                foreach (char ch in surname)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }
            }

            try
            {
                int guardianIdToValidate = Convert.ToInt16(guardianId);

                if (!smaDB.Guardians.Any( g=> g.GuardianId == guardianIdToValidate))
                {
                    validData = false;
                }

            }
            catch (Exception)
            {
                validData = false;
            }

            try
            {
                int classIdtoValidate = Convert.ToInt16(classId);

                if (!smaDB.Classes.Any(c => c.ClassId == classIdtoValidate))
                {
                    validData = false;
                }
            }
            catch (Exception)
            {
                validData = false;
            }
            
            


            return validData;
        }

        /// <summary>
        /// Method to validate form data
        /// </summary>
        /// <param name="givenName">
        /// GivenName entered by the user
        /// </param>
        /// <param name="surname">
        /// Surname entered by the user
        /// </param>
        /// bool validData
        /// </returns>
        public bool VerifyFormData(string givenName, string surname)
        {
            bool validData = true;

            

                if (givenName.Length == 0 || givenName.Length > 30)
                {
                    validData = false;
                }

                foreach (char ch in givenName)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }

                if (surname.Length == 0 || surname.Length > 30)
                {
                    validData = false;
                }

                foreach (char ch in surname)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }


            return validData;
        }
    }

    
}
