using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAClassLibrary
{
    public class ValidationUtils
    {

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

            try
            {

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
            }
            catch (Exception)
            {

               
            }


            return validData;
        }
    }

    
}
