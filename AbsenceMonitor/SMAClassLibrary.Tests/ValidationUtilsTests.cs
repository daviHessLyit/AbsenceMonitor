using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMAClassLibrary;
using Xunit;

namespace SMAClassLibrary.Tests
{
    public class ValidationUtilsTests
    {
        [Theory]
        [InlineData ("", "", false)]
        [InlineData("q", "q", true)]
        [InlineData("ljljsldfjljlkjsdfljksldjfsdfsdfsdfsdfsdfsd", "fdd", false)]
        [InlineData("dfd33dds", "surname", false)]
        [InlineData("givenname", "sur345name", false)]
        [InlineData("givename", "surname", true)]
        [InlineData("givename", "", false)]
        [InlineData("", "surname", false)]
        [InlineData("givename", "safsdafdsafasdfsfsdfsdfsdfsdfsfsdfsdfsdfsd", false)]
        public void VerfiyFormDataTwoFields_StringsShouldVerify(string givenName, string surname, bool expected)
        {
            
            // Arrange phase
            ValidationUtils validationUtils = new ValidationUtils();

            // Act phase

            bool actual = validationUtils.VerifyFormData(givenName, surname);

            // Assert phase
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "", "", "",false)]
        [InlineData("a", "", "", "", false)]
        [InlineData("", "a", "", "", false)]
        [InlineData("", "", "a", "", false)]
        [InlineData("", "", "", "a", false)]
        [InlineData("a", "a", "", "", false)]
        [InlineData("a", "", "a", "", false)]
        [InlineData("a", "", "", "a", false)]
        [InlineData("", "a", "a", "", false)]
        [InlineData("", "a", "", "a", false)]
        [InlineData("", "", "a", "a", false)]
        [InlineData("a", "a", "a", "", false)]
        [InlineData("a", "a", "", "a", false)]
        [InlineData("a", "", "a", "a", false)]
        [InlineData("", "a", "a", "a", false)]
        [InlineData("a", "a", "a", "a", true)]
        public void VerfiyFormDataFourFields_StringsShouldVerify(string givenName, string surname, string address, string mobileNo, bool expected)
        {
            // Arrange phase
            ValidationUtils validationUtils = new ValidationUtils();

            // Act phase

            bool actual = validationUtils.VerifyFormData(givenName, surname, address, mobileNo);

            // Assert phase
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "", false)]
        [InlineData("q", "q", true)]
        [InlineData("ljljsldfjljlkjsdfljksldjfsdfsdfsdfsdfsdfsd", "fdd", false)]
        [InlineData("dfd33dds", "password", false)]
        [InlineData("userName", "sur345name", true)]
        [InlineData("user9Name", "password", false)]
        [InlineData("userName", "", false)]
        [InlineData("", "password", false)]
        [InlineData("givename", "safsdafdsafasdfsfsdfsdfsdfsdfsfsdfsdfsdfsd", false)]
        public void ValidateUserInputTwoFields_StringsShouldVerify(string userName, string password, bool expected)
        {

            // Arrange phase
            ValidationUtils validationUtils = new ValidationUtils();

            // Act phase

            bool actual = validationUtils.ValidateUserInput(userName, password);

            // Assert phase
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "", "", "", false)]
        [InlineData("a", "", "", "", false)]
        [InlineData("", "a", "", "", false)]
        [InlineData("", "", "a", "", false)]
        [InlineData("", "", "", "a", false)]
        [InlineData("a", "a", "", "", false)]
        [InlineData("a", "", "a", "", false)]
        [InlineData("a", "", "", "a", false)]
        [InlineData("", "a", "a", "", false)]
        [InlineData("", "a", "", "a", false)]
        [InlineData("", "", "a", "a", false)]
        private void VerifyPupilFormData_StringsShouldFail(string givenName, string surname, string guardianId, string classId, bool expected)
        {
            // Arrange phase
            ValidationUtils validationUtils = new ValidationUtils();
            bool actual = validationUtils.VerifyPupilFormData(givenName, surname, guardianId, classId);

            // Assert phase
            Assert.Equal(expected, actual);

            // Assert phase
            Assert.Equal(expected, actual);
        }


    }
}
