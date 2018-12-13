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
        public void ValidateFormData_StringsShouldVerify(string givenName, string surname, bool expected)
        {
            
            // Arrange phase
            ValidationUtils validationUtils = new ValidationUtils();

            // Act phase

            bool actual = validationUtils.VerifyFormData(givenName, surname);

            // Assert phase
            Assert.Equal(expected, actual);
        }

    }
}
