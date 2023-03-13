using Microsoft.AspNetCore.Mvc;
using NationalHolidayModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.DTO;
using VacationModule.Core.Enums;

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

        [Fact]
        public async Task GetNationalHolidays_EmptyDatabase_ShouldReturnEmptyResponseList()
        {
            // Arrange
            // expected response is an empty NationalHolidayResponseList
            List<NationalHolidayResponse> expectedResponseContent = new List<NationalHolidayResponse>();

            // Act
            HttpResponseMessage response = await _client.GetAsync("api/national-holidays");

            // deserialize the json output
            var actualResponseContent = await response.Content.ReadFromJsonAsync<List<NationalHolidayResponse>>();

            // Assert
            //response.EnsureSuccessStatusCode(); // check if status code is 2xx
            Assert.Equal(expectedResponseContent, actualResponseContent);
        }
        [Fact]
        public async Task Create_ValidRequest_ObjectToDatabase()
        {
            // Arrange
            // new user
            var user = new RegisterDTO()
            {
                UserName = "admin",
                Email = "admin@test.com",
                Password = "admin",
                Role = Core.Enums.UserRoleOptions.Admin
            };
            // user to register in json format

            var jsonUser = JsonContent.Create(user);

            // register a new user, it will be logged in automatically
            await _client.PostAsync("api/Account/Register", jsonUser);

            // national holiday to add
            var nationalHolidayAddRequest = new NationalHolidayAddRequest()
            {
                HolidayDate = DateOnly.Parse("1/1/2023"),
                HolidayName = "test"
            };

            // expected data members
            DateOnly? expectedNationalHolidayDate = nationalHolidayAddRequest.HolidayDate;
            string? expectedNationalHolidayName = nationalHolidayAddRequest.HolidayName;

            // Act
            HttpResponseMessage response = await _client
                .PostAsJsonAsync("/api/admin/national-holidays", nationalHolidayAddRequest);

            var responseContent = await response.Content.ReadFromJsonAsync<NationalHolidayResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); // check the status code 
            Assert.Equal(expectedNationalHolidayDate, responseContent?.HolidayDate);
            Assert.Equal(expectedNationalHolidayName, responseContent?.HolidayName);
        }
    }
}
