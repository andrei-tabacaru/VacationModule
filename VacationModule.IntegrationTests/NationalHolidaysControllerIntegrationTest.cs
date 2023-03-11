using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationModule.IntegrationTests
{
    public class NationalHolidaysControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>  // will be used to create a new object of the WebApplicationFactory
    {
        private readonly HttpClient _client;

        public NationalHolidaysControllerIntegrationTest(CustomWebApplicationFactory factory) 
        {
            // Store the WebApplicationFactory HttpClient object in _client 
            _client = factory.CreateClient();
            // It will call the overriden ConfigureWebHost method in CustomWebApplicationFactory
            // which will execute the complete code of Program.cs file and then the code we provided,
            // so it will remove the DbContext and add the In Memory one
        }

        #region Create
        [Fact]
        public async Task GetNationalHolidays_ShouldReturnAddedObject()
        {
            // Arrange

            // Act
            HttpResponseMessage response = await _client.GetAsync("api/NationalHolidays/GetNationalHolidays");

            // Assert
            response.EnsureSuccessStatusCode(); // check if status code is 2xx
        }
        #endregion
    }
}
