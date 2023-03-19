using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NationalHolidayModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.API.Controllers;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;

namespace VacationModule.ControllerTests
{
    public class NationalHolidaysControllerTest
    {
        // Represents the mocked object that was created by Mock<T>
        private readonly INationalHolidaysService _nationalHolidayService;

        // Used to mock the methods of INationalHolidayService
        private readonly Mock<INationalHolidaysService> _nationalHolidaysServiceMock;

        public NationalHolidaysControllerTest()
        {
            // Create 
            _nationalHolidaysServiceMock = new Mock<INationalHolidaysService>();
            // Create a false NationalHolidayService object that will change the service's
            // methods to those defined by the Mock service
            _nationalHolidayService = _nationalHolidaysServiceMock.Object;
        }

        #region Create

        [Fact]
        public async Task Create_ReturnStatusCode200OKWithCreatedNationalHolidayResponse()
        {
            // Arrange
            // Dummy add request
            NationalHolidayAddRequest nationalHolidayAddRequest = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Dummy response
            NationalHolidayResponse nationalHolidayResponse = nationalHolidayAddRequest.toNationalHoliday()
                .toNationalHolidayResponse();

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // Mock the service
            // For any call of AddNationalHolidayAsync
            _nationalHolidaysServiceMock.Setup(temp => temp.AddNationalHolidayAsync(
                It.IsAny<NationalHolidayAddRequest>()))
                // return nationalHolidayResponse
                .ReturnsAsync(nationalHolidayResponse);

            // Act
            var responseFromController = (await 
                // call the controller's create method
                nationalHolidaysController.Create(nationalHolidayAddRequest))
                // get the result
                .Result 
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the responseFromController.Value is equal to nationalHolidayResponse
            Assert.Equal(responseFromController!.Value, nationalHolidayResponse);
            // Check if the status code is 200
            Assert.Equal(200, responseFromController.StatusCode);
        }

        [Fact]
        public async Task Create_InvalidHolidayDate_ReturnStatusCode400BadRequest()
        {
            // Arrange
            // Dummy add request with null HolidayDate
            NationalHolidayAddRequest nationalHolidayAddRequest = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = null
            };

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);
            
            // No need to mock here, it will return bad request before accessing the service

            // Act
            var responseFromController = (await
                 // call the controller's create method
                 nationalHolidaysController.Create(nationalHolidayAddRequest))
                 // get the result
                 .Result
                 // as ObjectResult type
                 as ObjectResult;

            // Assert
            // Check if the status code is 400
            Assert.Equal(400, responseFromController!.StatusCode);
        }

        #endregion

        #region GetNationalHolidays

        [Fact]
        public async Task GetNationalHolidays_ReturnStatusCode200OKWithNationalHolidayListResponse()
        {
            // Arrange
            // Dummy national holidays list
           List<NationalHoliday> nationalHolidaysList = new List<NationalHoliday>()
            {
               {
                   new NationalHoliday()
                   {
                       Id = Guid.NewGuid(),
                       HolidayName = Guid.NewGuid().ToString(),
                       HolidayDate = DateOnly.Parse("1/1/2023")
                   }
               },
               {
                   new NationalHoliday()
                   {
                       Id = Guid.NewGuid(),
                       HolidayName = Guid.NewGuid().ToString(),
                       HolidayDate = DateOnly.Parse("1/1/2023").AddDays(1)
                   }
               }
            };

            // Dummy response list
            List<NationalHolidayResponse> nationalHolidaysListResponse = nationalHolidaysList
                .Select(temp => temp.toNationalHolidayResponse()).ToList();

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // Mock the service
            // For any call of GetAllNationalHolidaysAsync
            _nationalHolidaysServiceMock.Setup(temp => temp.GetAllNationalHolidaysAsync())
                // return nationalHolidaysListResponse
                .ReturnsAsync(nationalHolidaysListResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.GetNationalHolidays())
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the responseFromController.Value is equal to nationalHolidaysListResponse
            Assert.Equal(nationalHolidaysListResponse, responseFromController!.Value);
            // Check if the status code is 200
            Assert.Equal(200, responseFromController.StatusCode);
        }

        #endregion

        #region Edit

        [Fact]
        public async Task Edit_NotMatchingId_ReturnStatusCode400BadRequest()
        {
            // Arrange
            // Valid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest nationalHolidayUpdateRequest = new NationalHolidayUpdateRequest()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Id
            Guid id = Guid.NewGuid();

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // No need to mock the service

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.Edit(id, nationalHolidayUpdateRequest))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 400
            Assert.Equal(400, responseFromController!.StatusCode);
        }

        [Fact]
        public async Task Edit_InvalidUpdateRequestObject_ReturnStatusCode400BadRequest()
        {
            // Arrange
            // Null NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest? nationalHolidayUpdateRequest = null;

            // Id
            Guid id = Guid.NewGuid();

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // No need to mock the service

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.Edit(id, nationalHolidayUpdateRequest))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 400
            Assert.Equal(400, responseFromController!.StatusCode);
        }
        
        [Fact]
        public async Task Edit_IdNotFound_ReturnStatusCode404NotFound()
        {
            // Arrange
            // Valid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest nationalHolidayUpdateRequest = new NationalHolidayUpdateRequest()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Matching Id
            Guid id = nationalHolidayUpdateRequest.Id;

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // Mock the service
            // Null NationalHolidayRepsonse
            NationalHolidayResponse? nationalHolidayNullResponse = null;

            // For any call of GetNationalHolidayByIdAsync
            _nationalHolidaysServiceMock.Setup(temp =>
            // For any Id given as parameter for GetNationalHolidayByIdAsync
            temp.GetNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return null national holiday object
                .ReturnsAsync(nationalHolidayNullResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.Edit(id, nationalHolidayUpdateRequest))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 404
            Assert.Equal(404, responseFromController!.StatusCode);
        }

        [Fact]
        public async Task Edit_ReturnStatusCode204NoContent() // => update succesful
        {
            // Arrange
            // Valid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest nationalHolidayUpdateRequest = new NationalHolidayUpdateRequest()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Matching Id
            Guid id = nationalHolidayUpdateRequest.Id;

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // Mock the service
            // Convert nationalHolidayUpdateRequest to Response DTO
            NationalHolidayResponse? nationalHolidayResponse = nationalHolidayUpdateRequest
                .toNationalHoliday().toNationalHolidayResponse();

            // For any call of GetNationalHolidayByIdAsync
            _nationalHolidaysServiceMock.Setup(temp =>
            // For any Id given as parameter for GetNationalHolidayByIdAsync
            temp.GetNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return the same national holiday object
                .ReturnsAsync(nationalHolidayResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.Edit(id, nationalHolidayUpdateRequest))
                // get the result
                .Result
                // as NoContentResult type
                as NoContentResult;

            // Assert
            // Check if the status code is 204
            Assert.Equal(204, responseFromController!.StatusCode);
        }

        #endregion

        #region UpdateNationalHolidaysToYear

        [Fact] 
        public async Task UpdateNationalHolidaysToYear_EmptyTable_ReturnStatusCode404NotFound()
        {
            // Arrange
            // Null NationalHolidayRepsonse
            List<NationalHolidayResponse> nationalHolidayEmptyListResponse = new List<NationalHolidayResponse>();

            // Mock the service
            _nationalHolidaysServiceMock.Setup(temp =>
            // For any call of GetAllNationalHolidaysAsync
            temp.GetAllNationalHolidaysAsync())
                // return empty national holiday response list DTO
                .ReturnsAsync(nationalHolidayEmptyListResponse);

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // input year
            int inputYear = 2010;

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.UpdateNationalHolidaysToYear(inputYear))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 404
            Assert.Equal(404, responseFromController!.StatusCode);
        }
        
        [Fact] 
        public async Task UpdateNationalHolidaysToYear_ReturnStatusCode204NoContent()
        {
            // Arrange
            // Valid NationalHolidayRepsonse list
            List<NationalHolidayResponse> nationalHolidayListResponse = new List<NationalHolidayResponse>()
            {
               {
                   new NationalHolidayResponse()
                   {
                       Id = Guid.NewGuid(),
                       HolidayName = Guid.NewGuid().ToString(),
                       HolidayDate = DateOnly.Parse("1/1/2023")
                   }
               },
               {
                   new NationalHolidayResponse()
                   {
                       Id = Guid.NewGuid(),
                       HolidayName = Guid.NewGuid().ToString(),
                       HolidayDate = DateOnly.Parse("1/1/2023").AddDays(1)
                   }
               }
            };

            // Mock the service
            _nationalHolidaysServiceMock.Setup(temp =>
            // For any call of GetAllNationalHolidaysAsync
            temp.GetAllNationalHolidaysAsync())
                // return empty national holiday response list DTO
                .ReturnsAsync(nationalHolidayListResponse);

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // input year
            int inputYear = 2010;

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.UpdateNationalHolidaysToYear(inputYear))
                // get the result
                .Result
                // as NoContentResult type
                as NoContentResult;

            // Assert
            // Check if the status code is 204
            Assert.Equal(204, responseFromController!.StatusCode);
        }

        #endregion

        #region DeleteNationalHoliday

        [Fact]
        public async Task DeleteNationalHoliday_NullId_ReturnStatusCode400BadRequest()
        {
            // Arrange
            // null Id
            Guid? id = null;

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // No need to mock the service

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.DeleteNationalHoliday(id))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 400
            Assert.Equal(400, responseFromController!.StatusCode);
        }
        
        [Fact]
        public async Task DeleteNationalHoliday_IdNotFound_ReturnStatusCode404NotFound()
        {
            // Arrange
            NationalHolidayResponse? nationalHolidayResponseNull = null;
            
            // Valid Id
            Guid id = Guid.NewGuid();

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // Mock 
            _nationalHolidaysServiceMock.Setup(temp =>
            // For any call of GetNationalHolidayByIdAsync
            temp.GetNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return null 
                .ReturnsAsync(nationalHolidayResponseNull);

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.DeleteNationalHoliday(id))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 404
            Assert.Equal(404, responseFromController!.StatusCode);
        }
        
        [Fact]
        public async Task DeleteNationalHoliday_ReturnStatusCode204NoContent()
        {
            // Arrange
            NationalHolidayResponse nationalHolidayResponse = new NationalHolidayResponse()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // nationalHolidayResponse's Id
            Guid id = nationalHolidayResponse.Id;

            // Controller
            NationalHolidaysController nationalHolidaysController = new NationalHolidaysController(_nationalHolidayService);

            // Mock 
            _nationalHolidaysServiceMock.Setup(temp =>
            // For any call of GetNationalHolidayByIdAsync
            temp.GetNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return null 
                .ReturnsAsync(nationalHolidayResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                nationalHolidaysController.DeleteNationalHoliday(id))
                // get the result
                .Result
                // as ObjectResult type
                as NoContentResult;

            // Assert
            // Check if the status code is 204
            Assert.Equal(204, responseFromController!.StatusCode);
        }

        #endregion
    }
}
