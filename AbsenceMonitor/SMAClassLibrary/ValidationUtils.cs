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
                // Check if the field meets the length requirements
                if (userName.Length == 0 || userName.Length >30)
                {
                    validData = false;
                }
                // Check if the field contains chars
                foreach (char ch in userName)
                {
                    if (ch >='0' && ch <='9')
                    {
                        validData = false;
                    }
                }
                // Check if the field meets the length requirements
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

            // Check if the field meets the length requirements
            if (givenName.Length == 0 || givenName.Length > 30)
            {
                validData = false;
            }
            else
            {
                // Check if the field contains chars
                foreach (char ch in givenName)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }
            }

            // Check if the field meets the length requirements
            if (surname.Length == 0 || surname.Length > 30)
            {
                validData = false;
            }
            else
            {
                // Check if the field contains chars
                foreach (char ch in surname)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }
            }

            // Check if the field meets the length requirements
            if (address.Length == 0 || address.Length > 100)
            {
                validData = false;
            }

            // Check if the field meets the length requirements
            if (mobileNo.Length == 0 || mobileNo.Length > 30)
            {
                validData = false;
            }

            return validData;
        }

        /// <summary>
        /// Method verifies the date passed by the user is not null and not in the future
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <returns></returns>
        public bool VerifyDate(DateTime selectedDate)
        {
            bool validData = true;
            // Check if the date is null or in the future
            if (selectedDate == null || selectedDate.Date > DateTime.Now)
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
            // Check if the field meets the length requirements
            if (givenName.Length == 0 || givenName.Length > 30)
            {
                validData = false;
            }
            else
            {
                // Check if the field contains chars
                foreach (char ch in givenName)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }
            }

            // Check if the field meets the length requirements
            if (surname.Length == 0 || surname.Length > 30)
            {
                validData = false;
            }
            else
            {
                // Check if the field contains chars
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
                // Check if the guardianId exists in the database
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
                // Check if the class exists in the database
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

            
                // Check if the field meets the length requirements
                if (givenName.Length == 0 || givenName.Length > 30)
                {
                    validData = false;
                }

                
                // Check if the field contains chars
                foreach (char ch in givenName)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }

                // Check if the field meets the length requirements
                if (surname.Length == 0 || surname.Length > 30)
                {
                    validData = false;
                }
                // Check if the field contains chars    
                foreach (char ch in surname)
                {
                    if (ch >= '0' && ch <= '9')
                    {
                        validData = false;
                    }
                }

            return validData;
        }


        /// <summary>
        /// Method verifies form data to add an absence to the system database
        /// </summary>
        /// <param name="absenceDate">
        /// Verify the date selected by the user
        /// </param>
        /// <param name="selectedAbsenceType">
        /// The absence type selected by the user
        /// </param>
        /// <param name="pupilId">
        /// The pupilId selected by the user
        /// </param>
        /// <returns>
        /// bool validData
        /// </returns>
        public bool VerifyAbsenceFormData(DateTime absenceDate, int selectedAbsenceType, int pupilId)
        {
            bool validData = true;

            // User the VerifyDateMethod to verify we have a valid date field
            validData = VerifyDate(absenceDate);

            // Check the absenceType is a positive integer
            if (selectedAbsenceType <=0)
            {
                validData = false;
            }
            // Check the pupilId is a positive integer
            if (pupilId<=0)
            {
                validData = false;
            }
            return validData;
        }
    }

    
}
