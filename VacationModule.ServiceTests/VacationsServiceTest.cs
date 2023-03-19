using Moq;
using VacationModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.Domain.RepositoryContracts;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;
using VacationModule.Core.Services;

namespace VacationModule.ServiceTests
{
    public class VacationsServiceTest
    {
        private readonly IVacationsService _vacationsService;

        // Used to mock the methods of IVacationRepository
        private readonly Mock<IVacationRepository> _vacationRepositoryMock;
        private readonly Mock<INationalHolidayRepository> _nationalHolidayRepositoryMock;
        private readonly Mock<INationalHolidayUpdateRepository> _nationalHolidayUpdateRepositoryMock;
        // Represents the mocked object that was created by Mock<T>
        private readonly IVacationRepository _vacationRepository;
        private readonly INationalHolidayRepository _nationalHolidayRepository;
        private readonly INationalHolidayUpdateRepository _nationalHolidayUpdateRepository;

        public VacationsServiceTest()
        {
            _vacationRepositoryMock = new Mock<IVacationRepository>();
            // Create a false VacationRepository object that will change the repository's
            // methods to those defined by the Mock repository
            _vacationRepository = _vacationRepositoryMock.Object;

            _nationalHolidayRepositoryMock = new Mock<INationalHolidayRepository>();
            // Create a false NationalHolidayRepository object that will change the repository's
            // methods to those defined by the Mock repository
            _nationalHolidayRepository = _nationalHolidayRepositoryMock.Object;
            
            _nationalHolidayUpdateRepositoryMock = new Mock<INationalHolidayUpdateRepository>();
            // Create a false NationalHolidayRepository object that will change the repository's
            // methods to those defined by the Mock repository
            _nationalHolidayUpdateRepository = _nationalHolidayUpdateRepositoryMock.Object;

            // Create the service based on mocked repository object
            // This will allow to call mocked repository methods when the service will be used
            _vacationsService = new VacationsService(_vacationRepository, _nationalHolidayRepository, _nationalHolidayUpdateRepository);
        }

        #region AddVacation

        // If VacationAddRequest is null => throw ArgumentNullException
        [Fact]
        public async Task AddVacation_NullVacation_ToBeArgumentNullException()
        {
            // Arrange
            VacationAddRequest? vacationAddRequest = null;

            // No need to mock here beacause the repository will return null

            // Asset
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _vacationsService.AddVacationAsync(vacationAddRequest, Guid.NewGuid());
            });
        }
        // If the EndDate is null => throw ArgumentException
        [Fact]
        public async Task AddVacation_EndDateIsNull_ToBeArgumentException()
        {
            // Arrange
            VacationAddRequest? vacationAddRequest = new VacationAddRequest()
            {
                EndDate = null,
                StartDate = DateOnly.Parse("1/1/2023")
            };

            // Convert the request DTO to Vacation object
            Vacation vacation = vacationAddRequest.toVacation();

            // If VacationRepository.AddVacationRepository is called, it has to return the given argument object
            _vacationRepositoryMock.Setup(temp =>
            // For the VacationRepository's AddVacationAsync method,
            temp.AddVacationAsync(
                // any Vacation compatible parameters recived
                It.IsAny<Vacation>()))
                // will throw ArgumentException
                .ReturnsAsync(vacation);

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _vacationsService.AddVacationAsync(vacationAddRequest, Guid.NewGuid());
            });
        }

        // If the StartDate is null => throw ArgumentException
        [Fact]
        public async Task AddVacation_StartDateIsNull_ToBeArgumentException()
        {
            // Arrange
            VacationAddRequest? vacationAddRequest = new VacationAddRequest()
            {
                StartDate = null,
                EndDate = DateOnly.Parse("1/10/2023")
            };

            // Convert the request DTO to Vacation object
            Vacation vacation = vacationAddRequest.toVacation();

            // If VacationRepository.AddVacationRepository is called, it has to return the given argument object
            _vacationRepositoryMock.Setup(temp =>
            // For the VacationRepository's AddVacationAsync method,
            temp.AddVacationAsync(
                // any Vacation compatible parameter recived
                It.IsAny<Vacation>()))
                // will throw ArgumentException
                .ReturnsAsync(vacation);

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _vacationsService.AddVacationAsync(vacationAddRequest, Guid.NewGuid());
            });
        }

        // If the StartDate is null => throw ArgumentException
        [Fact]
        public async Task AddVacation_UserIdIsNull_ToBeArgumentNullException()
        {
            // Arrange
            VacationAddRequest? vacationAddRequest = new VacationAddRequest()
            {
                StartDate = DateOnly.Parse("1/5/2023"),
                EndDate = DateOnly.Parse("1/10/2023")
            };

            // Convert the request DTO to Vacation object
            Vacation vacation = vacationAddRequest.toVacation();
            vacation.ApplicationUserId = Guid.NewGuid();

            // Null input user id
            Guid? inputUserId = null;

            // If VacationRepository.AddVacationRepository is called, it has to return the given argument object
            _vacationRepositoryMock.Setup(temp =>
            // For the VacationRepository's AddVacationAsync method,
            temp.AddVacationAsync(
                // any Vacation compatible parameter recived
                It.IsAny<Vacation>()))
                // will throw ArgumentException
                .ReturnsAsync(vacation);

            // Asset
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _vacationsService.AddVacationAsync(vacationAddRequest, inputUserId);
            });
        }

        // If the EndDate/StartDate are correct => add the vacation to the existing list of vacations
        [Fact]
        public async Task AddVacation_FullVacationDetails_ToBeSuccesful()
        {
            // Arrange
            VacationAddRequest? vacationAddRequest = new VacationAddRequest()
            {
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023")
            };

            // Convert the request DTO to Vacation object
            Vacation vacation = vacationAddRequest.toVacation();

            // Dummy User id
            Guid UserId = Guid.NewGuid();
            vacation.ApplicationUserId = UserId;

            // If we supply any argument to the AddVacationsAsync method, then it should return the given argument value
            _vacationRepositoryMock.Setup(temp =>
            // For the VacationRepository's AddVacationAsync method,
            temp.AddVacationAsync(
                // any Vacation compatible parameter recived
                It.IsAny<Vacation>()))
                // will return the vacation object
                .ReturnsAsync(vacation);

            List<Vacation> emptyVacationList = new List<Vacation>();
            _vacationRepositoryMock.Setup(temp => temp.GetAllVacationsAsync()).ReturnsAsync(emptyVacationList);

            // Empty Dictionary
            Dictionary<DateOnly, DateOnly> emptyVacationDictionary = new Dictionary<DateOnly, DateOnly>();

            Dictionary<DateOnly, string?> emptyNationalHolidayDictionary = new Dictionary<DateOnly, string?>();

            _vacationRepositoryMock.Setup(temp => temp.GetVacationsDictionaryAsync(It.IsAny<Guid>())).ReturnsAsync(emptyVacationDictionary);

            _nationalHolidayRepositoryMock.Setup(temp => temp.GetNationalHolidaysDictionaryAsync()).ReturnsAsync(emptyNationalHolidayDictionary);

            _nationalHolidayUpdateRepositoryMock.Setup(temp => temp.GetNationalHolidaysDictionaryYearAsync(It.IsAny<int>())).ReturnsAsync(emptyNationalHolidayDictionary);


            // Act
            VacationResponse vacationResponse = await _vacationsService
                .AddVacationAsync(vacationAddRequest, UserId);

            // Asset
            // If the Id is null, the object was not created
            Assert.True(vacationResponse.Id != Guid.Empty);
        }
        #endregion

        #region GetAllVacations

        [Fact]
        public async Task GetAllVacations_EmptyList_ToBeEmpty()
        {
            // Arrange
            List<Vacation> emptyList = new List<Vacation>();
            // Mock the repository
            // For any call of GetAllVacationsAsync method
            _vacationRepositoryMock.Setup(temp => temp.GetAllVacationsAsync())
                // return an empty list
                .ReturnsAsync(emptyList);

            // Act
            List<VacationResponse> actualVacationResponseList = await _vacationsService.GetAllVacationsAsync(true, true);

            // Assert
            Assert.Empty(actualVacationResponseList);
        }

        [Fact]
        public async Task GetAllVacations_WithFewVacations_ToBeSuccesful()
        {
            // Arrange
            List<Vacation> vacationsList = new List<Vacation>
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

            // Mock the repository
            // For any call of GetAllVacationsAsync method
            _vacationRepositoryMock.Setup(temp => temp.GetAllVacationsAsync())
                // return the same list
                .ReturnsAsync(vacationsList);

            // Act
            List<VacationResponse> actualVacationResponseList = await _vacationsService
                .GetAllVacationsAsync(true, true);

            // Get the expected list
            List<VacationResponse> expectedVacationsList = vacationsList.Select(temp => temp.toVacationResponse()).ToList();

            // check each element from vacations_list_from_add_vacation
            foreach (var expectedVacation in expectedVacationsList)
            {
                // Assert
                // is the current element from vacations_list_from_add_vacation in
                // the actual_vacation_response_list?
                Assert.Contains(expectedVacation, actualVacationResponseList);
            }
        }

        #endregion


        #region GetVacationById

        // If id == null => VacationResponse == null
        [Fact]
        public async Task GetVacationById_NullId_toBeNull()
        {
            // Arrange
            Guid? Id = null;

            // No need to mock the repository here

            // Act
            VacationResponse? vacation_response_from_get = await _vacationsService
                .GetVacationByIdAsync(Id);

            // Assert
            Assert.Null(vacation_response_from_get);
        }

        // If id is a valid vacation id =>  return the valid vacation details as VacationResponse object
        [Fact]
        public async Task GetVacationById_ValidId_ToBeSuccesfull()
        {
            // Arrange
            // Create a new vacation object
            Vacation vacation = new Vacation()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023"),
                ApplicationUserId = Guid.NewGuid()
            };

            // Get the id
            Guid Id = vacation.Id;

            // Convert to response DTO
            VacationResponse vacationResponse = vacation.toVacationResponse();

            // Mock the repository
            _vacationRepositoryMock.Setup(temp =>
            // For any Id given as parameter for GetVacationByIdAsync
            temp.GetVacationByIdAsync(It.IsAny<Guid>()))
                // return the same vacation object
                .ReturnsAsync(vacation);

            // Act
            VacationResponse? vacationResponseFromGet = await _vacationsService
                .GetVacationByIdAsync(Id);

            // Assert
            Assert.Equal(vacationResponse, vacationResponseFromGet);
        }

        #endregion

        #region UpdateVacation

        // null PersonUpdateRequest => ArgumentNullException
        [Fact]
        public async Task UpdateVacation_NullVacation_ToBeArgumentNullException()
        {
            // Arrange
            VacationUpdateRequest? vacationUpdateRequest = null;

            // No need to mock the repository here

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _vacationsService.UpdateVacationAsync(vacationUpdateRequest);
            });
        }

        // invalid id (the given id does not already exist) => ArgumentExecption
        [Fact]
        public async Task UpdateVacation_InvalidId_ToBeArgumentException()
        {
            // Arrange 
            VacationUpdateRequest vacationUpdateRequest = new VacationUpdateRequest()
            {
                Id = Guid.NewGuid(), // we generate a new Id so it will not exist in the list of vacations
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023"),
                //ApplicationUserId = Guid.NewGuid()
            };

            // Assert

            // No need to mock the repository here because it is checked before calling it

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _vacationsService.UpdateVacationAsync(vacationUpdateRequest);
            });
        }

        // If the EndDate is null => throw ArgumentException
        [Fact]
        public async Task UpdateVacation_EndDateIsNull_ToBeArgumentException()
        {
            // Arrange
            // Dummy vacation
            Vacation vacation = new Vacation()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = null,
                ApplicationUserId = Guid.NewGuid()
            };

            // Invalid VacationUpdateRequest
            VacationUpdateRequest? vacationUpdateRequest = vacation
                // Convert the dummy vacation to update request DTO
                .toVacationResponse().toVacationUpdateRequest();

            // No need to mock the repository here because it is checked before calling it

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _vacationsService.UpdateVacationAsync(vacationUpdateRequest);
            });
        }

        // If the StartDate is null => throw ArgumentException
        [Fact]
        public async Task UpdateVacation_StartDateIsNull_ToBeArgumentException()
        {
            // Arrange
            // Dummy vacation
            Vacation vacation = new Vacation()
            {
                Id = Guid.NewGuid(),
                StartDate = null,
                EndDate = DateOnly.Parse("1/10/2023"),
                ApplicationUserId = Guid.NewGuid()
            };

            // Invalid VacationUpdateRequest
            VacationUpdateRequest? vacationUpdateRequest = vacation
                // Convert the dummy vacation to update request DTO
                .toVacationResponse().toVacationUpdateRequest();

            // No need to mock the repository here because it is checked before calling it

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _vacationsService.UpdateVacationAsync(vacationUpdateRequest);
            });
        }

        // If the EndDate/StartDate are correct => update the vacation in the existing list of vacations
        [Fact]
        public async Task UpdateVacation_ProperArguments_ToBeSuccesful()
        {
            // Arrange
            // Dummy vacation
            Vacation vacation = new Vacation()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023"),
                ApplicationUserId = Guid.NewGuid()
            };

            // Get the expected response from the update request
            VacationResponse vacationResponseExpected = vacation.toVacationResponse();

            // Valid VacationUpdateRequest
            VacationUpdateRequest? vacationUpdateRequest = vacation.toVacationResponse()
                .toVacationUpdateRequest();

            // Mock the repository
            // Mock UpdateVacationAsync
            _vacationRepositoryMock.Setup(temp =>
            // For any call of UpdateVacationAsync
            temp.UpdateVacationAsync(It.IsAny<Vacation>()))
                // return the same vacation
                .ReturnsAsync(vacation);

            // Mock GetVacationByIdAsync
            _vacationRepositoryMock.Setup(temp =>
            // For any call of GetVacationByIdAsync with any Id
            temp.GetVacationByIdAsync(It.IsAny<Guid>()))
                // return the same vacation
                .ReturnsAsync(vacation);

            // Empty vacations list
            List<Vacation> emptyVacationList = new List<Vacation>();
            _vacationRepositoryMock.Setup(temp => temp.GetAllVacationsAsync()).ReturnsAsync(emptyVacationList);

            // Empty Dictionary
            Dictionary<DateOnly, DateOnly> emptyVacationDictionary = new Dictionary<DateOnly, DateOnly>();
            Dictionary<DateOnly, string?> emptyNationalHolidayDictionary = new Dictionary<DateOnly, string?>();

            _vacationRepositoryMock.Setup(temp => temp.GetVacationsDictionaryAsync(It.IsAny<Guid>())).ReturnsAsync(emptyVacationDictionary);

            _nationalHolidayRepositoryMock.Setup(temp => temp.GetNationalHolidaysDictionaryAsync()).ReturnsAsync(emptyNationalHolidayDictionary);

            _nationalHolidayUpdateRepositoryMock.Setup(temp => temp.GetNationalHolidaysDictionaryYearAsync(It.IsAny<int>())).ReturnsAsync(emptyNationalHolidayDictionary);

            // Act
            // save the response in vacation_response_from_update
            VacationResponse vacationResponseFromUpdate = await _vacationsService
                .UpdateVacationAsync(vacationUpdateRequest);

            // Asset
            // Check if the dummy vacation has the same id with the updated DTO response
            Assert.Equal(vacationResponseExpected, vacationResponseFromUpdate);
        }
        #endregion


        #region DeleteVacation
        [Fact]
        public async Task DeleteVacation_NullId_ToBeArgumentNullException()
        {
            // Arrange 
            Guid? id = null;

            // No need to mock the repository here beacuse the service will throw the expcetion before using the repository

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _vacationsService.DeleteVacationAsync(id);
            });
        }

        // Invalid vacation id =>  false
        [Fact]
        public async Task DeleteVacation_InvalidId_ToBeFalse()
        {
            // Arrange
            Guid Id = Guid.NewGuid(); // random id that does not exist

            // No need to mock here beacause the id won't be in the database

            // Act
            bool isDeleted = await _vacationsService.DeleteVacationAsync(Id);

            // Assert
            Assert.False(isDeleted);
        }

        // Valid vacation id => true
        [Fact]
        public async Task DeleteVacation_ValidId_ToBeTrue()
        {
            // Arrange
            // Dummy vacation
            Vacation vacation = new Vacation()
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.Parse("1/1/2023"),
                EndDate = DateOnly.Parse("1/10/2023"),
                ApplicationUserId = Guid.NewGuid()
            };

            // Mock the repository
            // For any call of GetVacationByIdAsync
            _vacationRepositoryMock.Setup(temp => temp.GetVacationByIdAsync(It.IsAny<Guid>()))
                // return the same dummy vacation object
                .ReturnsAsync(vacation);

            // For any call of DeleteVacationByIdAsync
            _vacationRepositoryMock.Setup(temp => temp.DeleteVacationByIdAsync(It.IsAny<Guid>()))
                // return true
                .ReturnsAsync(true);

            // Get the id from the dummy vacation
            Guid? Id = vacation.Id;

            // Act
            bool isDeleted = await _vacationsService.DeleteVacationAsync(Id);

            // Assert
            Assert.True(isDeleted);
        }

        #endregion
    }
}
