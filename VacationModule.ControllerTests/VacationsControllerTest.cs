using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VacationModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.API.Controllers;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;
using System.Security.Claims;

namespace VacationModule.ControllerTests
{
    public class VacationsControllerTest
    {
        // Represents the mocked object that was created by Mock<T>
        private readonly IVacationsService _vacationService;

        // Used to mock the methods of IVacationService
        private readonly Mock<IVacationsService> _vacationsServiceMock;

        public VacationsControllerTest()
        {
            // Create 
            _vacationsServiceMock = new Mock<IVacationsService>();
            // Create a false VacationService object that will change the service's
            // methods to those defined by the Mock service
            _vacationService = _vacationsServiceMock.Object;
        }

        #region Create

        [Fact]
        public async Task Create_ReturnStatusCode201CreatedWithCreatedVacationResponse()
        {
            // Arrange
            // Dummy add request
            VacationAddRequest vacationAddRequest = new VacationAddRequest()
            {
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023")
            };
            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                           new Claim[] {
                                                         new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                         new Claim(ClaimTypes.Name, "test")
                                           },
                                           "TestAuthentication"));

            // Dummy response
            VacationResponse vacationResponse = vacationAddRequest.toVacation()
                .toVacationResponse();

            // Set dummy user id
            vacationResponse.ApplicationUserId = userId;

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);

            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Mock the service
            // For any call of AddVacationAsync
            _vacationsServiceMock.Setup(temp => temp.AddVacationAsync(
                It.IsAny<VacationAddRequest>(), It.IsAny<Guid>()))
                // return vacationResponse
                .ReturnsAsync(vacationResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.Create(vacationAddRequest))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the responseFromController.Value is equal to vacationResponse
            Assert.Equal(responseFromController!.Value, vacationResponse);
            // Check if the status code is 200
            Assert.Equal(201, responseFromController.StatusCode);
        }


        [Fact]
        public async Task Create_InvalidEndDate_ReturnStatusCode400BadRequest()
        {
            // Arrange
            // Dummy add request with null EndDate
            VacationAddRequest vacationAddRequest = new VacationAddRequest()
            {
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = null
            };

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                               new Claim[] {
                                                         new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                         new Claim(ClaimTypes.Name, "test")
                                               },
                                               "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);

            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // No need to mock here, it will return bad request before accessing the service

            // Act
            var responseFromController = (await
            // call the controller's create method
            vacationsController.Create(vacationAddRequest))
            // get the result
            .Result
            // as ObjectResult type
            as ObjectResult;

            // Assert
            // Check if the status code is 400
            Assert.Equal(400, responseFromController!.StatusCode);
        }

        #endregion

        #region GetVacations

        [Fact]
        public async Task GetVacations_ReturnStatusCode200OKWithVacationListResponse()
        {
            // Arrange
            // Dummy national holidays list
            List<Vacation> vacationsList = new List<Vacation>()
            {
                new Vacation() {
                    Id = Guid.NewGuid(),
                    StartDate = DateOnly.Parse("1/1/2023"),
                    EndDate = DateOnly.Parse("1/10/2023"),
                    ApplicationUserId = Guid.NewGuid()
                },
                new Vacation() {
                    Id = Guid.NewGuid(),
                    StartDate = DateOnly.Parse("1/11/2023"),
                    EndDate = DateOnly.Parse("1/20/2023"),
                    ApplicationUserId = Guid.NewGuid()
                }
            };
            // Dummy response list
            List<VacationResponse> vacationsListResponse = vacationsList
                .Select(temp => temp.toVacationResponse()).ToList();

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Mock the service
            // For any call of GetAllVacationsAsync
            _vacationsServiceMock.Setup(temp => 
                temp.GetAllVacationsAsync(It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<Guid>()))
                // return vacationsListResponse
                .ReturnsAsync(vacationsListResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.GetVacations())
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the responseFromController.Value is equal to vacationsListResponse
            Assert.Equal(vacationsListResponse, responseFromController!.Value);
            // Check if the status code is 200
            Assert.Equal(200, responseFromController.StatusCode);
        }
        
        #endregion

        #region Edit

        [Fact]
        public async Task Edit_NotMatchingId_ReturnStatusCode400BadRequest()
        {
            // Arrange
            // Valid VacationUpdateRequest
            VacationUpdateRequest vacationUpdateRequest = new VacationUpdateRequest()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023")
            };

            // Id
            Guid id = Guid.NewGuid();

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // No need to mock the service

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.Edit(id, vacationUpdateRequest))
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
            // Null VacationUpdateRequest
            VacationUpdateRequest? vacationUpdateRequest = null;

            // Id
            Guid id = Guid.NewGuid();

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // No need to mock the service

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.Edit(id, vacationUpdateRequest))
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
            // Valid VacationUpdateRequest
            VacationUpdateRequest vacationUpdateRequest = new VacationUpdateRequest()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023")
            };

            // Matching Id
            Guid id = vacationUpdateRequest.Id;

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Mock the service
            // Null VacationRepsonse
            VacationResponse? vacationNullResponse = null;

            // For any call of GetVacationByIdAsync
            _vacationsServiceMock.Setup(temp =>
            // For any Id given as parameter for GetVacationByIdAsync
            temp.GetVacationByIdAsync(It.IsAny<Guid>()))
                // return null national holiday object
                .ReturnsAsync(vacationNullResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.Edit(id, vacationUpdateRequest))
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
            // Valid VacationUpdateRequest
            VacationUpdateRequest vacationUpdateRequest = new VacationUpdateRequest()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023")
            };

            // Matching Id
            Guid id = vacationUpdateRequest.Id;

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Mock the service
            // Convert vacationUpdateRequest to vacation 
            Vacation vacation = vacationUpdateRequest.toVacation();
            // set the user id
            vacation.ApplicationUserId = userId;
            // set the response
            VacationResponse? vacationResponse = vacation.toVacationResponse();

            // For any call of GetVacationByIdAsync
            _vacationsServiceMock.Setup(temp =>
            // For any Id given as parameter for GetVacationByIdAsync
            temp.GetVacationByIdAsync(It.IsAny<Guid>()))
                // return the same national holiday object
                .ReturnsAsync(vacationResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.Edit(id, vacationUpdateRequest))
                // get the result
                .Result
                // as NoContentResult type
                as NoContentResult;

            // Assert
            // Check if the status code is 204
            Assert.Equal(204, responseFromController!.StatusCode);
        }

        #endregion

        #region DeleteVacation

        [Fact]
        public async Task DeleteVacation_NullId_ReturnStatusCode400BadRequest()
        {
            // Arrange
            // null Id
            Guid? id = null;

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // No need to mock the service

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.DeleteVacation(id))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 400
            Assert.Equal(400, responseFromController!.StatusCode);
        }
        
        [Fact]
        public async Task DeleteVacation_IdNotFound_ReturnStatusCode404NotFound()
        {
            // Arrange
            VacationResponse? vacationResponseNull = null;

            // Valid Id
            Guid id = Guid.NewGuid();

            // Dummy user id
            var userId = Guid.NewGuid();

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Mock 
            _vacationsServiceMock.Setup(temp =>
            // For any call of GetVacationByIdAsync
            temp.GetVacationByIdAsync(It.IsAny<Guid>()))
                // return null 
                .ReturnsAsync(vacationResponseNull);

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.DeleteVacation(id))
                // get the result
                .Result
                // as ObjectResult type
                as ObjectResult;

            // Assert
            // Check if the status code is 404
            Assert.Equal(404, responseFromController!.StatusCode);
        }

        
        [Fact]
        public async Task DeleteVacation_ReturnStatusCode204NoContent()
        {
            // Arrange
            VacationResponse vacationResponse = new VacationResponse()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023"),
                ApplicationUserId = Guid.NewGuid()
            };

            // vacationResponse's Id
            Guid id = vacationResponse.Id;

            // Dummy user id that matches the response ApplicationUserId
            var userId = vacationResponse.ApplicationUserId;

            // Dummy user
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                                                new Claim[] {
                                                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                                        new Claim(ClaimTypes.Name, "test")
                                                },
                                                "TestAuthentication"));

            // Controller
            VacationsController vacationsController = new VacationsController(_vacationService);
            // Set the httpContext for the unit test with authenticated dummy user
            vacationsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Mock 
            _vacationsServiceMock.Setup(temp =>
            // For any call of GetVacationByIdAsync
            temp.GetVacationByIdAsync(It.IsAny<Guid>()))
                // return null 
                .ReturnsAsync(vacationResponse);

            // Act
            var responseFromController = (await
                // call the controller's create method
                vacationsController.DeleteVacation(id))
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