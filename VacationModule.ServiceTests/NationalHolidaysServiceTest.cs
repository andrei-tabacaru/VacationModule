using Moq;
using NationalHolidayModule.Core.DTO;
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
    public class NationalHolidaysServiceTest
    {
        private readonly INationalHolidaysService _nationalHolidaysService;
        
        // Used to mock the methods of INationalHolidayRepository
        private readonly Mock<INationalHolidayRepository> _nationalHolidayRepositoryMock;
        // Represents the mocked object that was created by Mock<T>
        private readonly INationalHolidayRepository _nationalHolidayRepository;

        public NationalHolidaysServiceTest()
        {
            _nationalHolidayRepositoryMock = new Mock<INationalHolidayRepository>();
            // Create a false NationalHolidayRepository object that will change the repository's
            // methods to those defined by the Mock repository
            _nationalHolidayRepository = _nationalHolidayRepositoryMock.Object;

            // Create the service based on mocked repository object
            // This will allow to call mocked repository methods when the service will be used
            _nationalHolidaysService = new NationalHolidaysService(_nationalHolidayRepository);
        }

        #region AddNationalHoliday

        // If NationalHolidayAddRequest is null => throw ArgumentNullException
        [Fact]
        public async Task AddNationalHoliday_NullNationalHoliday_ToBeArgumentNullException()
        {
            // Arrange
            NationalHolidayAddRequest? nationalHolidayAddRequest = null;

            // No need to mock here beacause the repository will return null

            // Asset
            await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                // Act
                await _nationalHolidaysService.AddNationalHolidayAsync(nationalHolidayAddRequest);
            });
        } 
        // If the HolidayName is null => throw ArgumentException
        [Fact]
        public async Task AddNationalHoliday_HolidayNameIsNull_ToBeArgumentException()
        {
            // Arrange
            NationalHolidayAddRequest? nationalHolidayAddRequest = new NationalHolidayAddRequest()
            {
                HolidayName = null,
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Convert the request DTO to NationalHoliday object
            NationalHoliday nationalHoliday = nationalHolidayAddRequest.toNationalHoliday();

            // If NationalHolidayRepository.AddNationalHolidayRepository is called, it has to return the given argument object
            _nationalHolidayRepositoryMock.Setup(temp =>
            // For the NationalHolidayRepository's AddNationalHolidayAsync method,
            temp.AddNationalHolidayAsync(
                // any NationalHoliday compatible parameter recived
                It.IsAny<NationalHoliday>()))
                // will throw ArgumentException
                .ReturnsAsync(nationalHoliday);

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _nationalHolidaysService.AddNationalHolidayAsync(nationalHolidayAddRequest);
            });
        }

        // If the HolidayDate is null => throw ArgumentException
        [Fact]
        public async Task AddNationalHoliday_HolidayDateIsNull_ToBeArgumentException()
        {
            // Arrange
            NationalHolidayAddRequest? nationalHolidayAddRequest = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = null
            };

            // Convert the request DTO to NationalHoliday object
            NationalHoliday nationalHoliday = nationalHolidayAddRequest.toNationalHoliday();

            // If NationalHolidayRepository.AddNationalHolidayRepository is called, it has to return the given argument object
            _nationalHolidayRepositoryMock.Setup(temp =>
            // For the NationalHolidayRepository's AddNationalHolidayAsync method,
            temp.AddNationalHolidayAsync(
                // any NationalHoliday compatible parameter recived
                It.IsAny<NationalHoliday>()))
                // will throw ArgumentException
                .ReturnsAsync(nationalHoliday);

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _nationalHolidaysService.AddNationalHolidayAsync(nationalHolidayAddRequest);
            });
        }

        // If the HolidayName/HolidayDate are correct => add the national holiday to the existing list of national holidays
        [Fact]
        public async Task AddNationalHoliday_FullNationalHolidayDetails_ToBeSuccesful()
        {
            // Arrange
            NationalHolidayAddRequest? nationalHolidayAddRequest = new NationalHolidayAddRequest()
            {
                HolidayName = "name",
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Convert the request DTO to NationalHoliday object
            NationalHoliday nationalHoliday = nationalHolidayAddRequest.toNationalHoliday();
            // Convert the national holiday object into national holiday response DTO
            NationalHolidayResponse nationalHolidayResponseExpected = nationalHoliday.toNationalHolidayResponse();

            // If we supply any argument to the AddNationalHolidaysAsync method, then it should return the given argument value
            _nationalHolidayRepositoryMock.Setup(temp =>
            // For the NationalHolidayRepository's AddNationalHolidayAsync method,
            temp.AddNationalHolidayAsync(
                // any NationalHoliday compatible parameter recived
                It.IsAny<NationalHoliday>()))
                // will return the nationalHoliday object
                .ReturnsAsync(nationalHoliday);

            // Act
            NationalHolidayResponse nationalHolidayResponse = await _nationalHolidaysService
                .AddNationalHolidayAsync(nationalHolidayAddRequest);

            // Asset
            // If the Id is null, the object was not created
            Assert.True(nationalHolidayResponse.Id != Guid.Empty);
            // Check if the actual response is equal to the expected one
            //Assert.Equal(nationalHolidayResponse, nationalHolidayResponseExpected);
        }
        #endregion

        #region GetAllNationalHolidays

        [Fact]
        public async Task GetAllNationalHolidays_EmptyList_ToBeEmpty()
        {
            // Arrange
            List<NationalHoliday> emptyList = new List<NationalHoliday>();
            // Mock the repository
            // For any call of GetAllNationalHolidaysAsync method
            _nationalHolidayRepositoryMock.Setup(temp => temp.GetAllNationalHolidaysAsync())
                // return an empty list
                .ReturnsAsync(emptyList);

            // Act
            List<NationalHolidayResponse> actualNationalHolidayResponseList = await _nationalHolidaysService.GetAllNationalHolidaysAsync();

            // Assert
            Assert.Empty(actualNationalHolidayResponseList);
        }
        
        [Fact]
        public async Task GetAllNationalHolidays_WithFewNationalHolidays_ToBeSuccesful()
        {
            // Arrange
            List<NationalHoliday> nationalHolidaysList = new List<NationalHoliday>
            {
                new NationalHoliday() { Id = Guid.NewGuid(),
                                        HolidayDate = DateOnly.Parse("1/1/2023"),
                                        HolidayName = Guid.NewGuid().ToString()
                                      },
                new NationalHoliday() { Id = Guid.NewGuid(),
                                        HolidayDate = DateOnly.Parse("2/1/2023"),
                                        HolidayName = Guid.NewGuid().ToString()
                                      }
            };

            // Mock the repository
            // For any call of GetAllNationalHolidaysAsync method
            _nationalHolidayRepositoryMock.Setup(temp => temp.GetAllNationalHolidaysAsync())
                // return the same list
                .ReturnsAsync(nationalHolidaysList);

            // Act
            List<NationalHolidayResponse> actualNationalHolidayResponseList = await _nationalHolidaysService
                .GetAllNationalHolidaysAsync();

            // Get the expected list
            List<NationalHolidayResponse> expectedNationalHolidaysList = nationalHolidaysList.Select(temp => temp.toNationalHolidayResponse()).ToList();

            // check each element from nationalHolidays_list_from_add_nationalHoliday
            foreach (var expectedNationalHoliday in expectedNationalHolidaysList)
            {
                // Assert
                // is the current element from nationalHolidays_list_from_add_nationalHoliday in
                // the actual_nationalHoliday_response_list?
                Assert.Contains(expectedNationalHoliday, actualNationalHolidayResponseList);
            }
        }

        #endregion

        
        #region GetNationalHolidayById

        // If id == null => NationalHolidayResponse == null
        [Fact]
        public async Task GetNationalHolidayById_NullId_toBeNull()
        {
            // Arrange
            Guid? Id = null;

            // No need to mock the repository here

            // Act
            NationalHolidayResponse? nationalHoliday_response_from_get = await _nationalHolidaysService
                .GetNationalHolidayByIdAsync(Id);

            // Assert
            Assert.Null(nationalHoliday_response_from_get);
        }

        // If id is a valid national holiday id =>  return the valid national holiday details as NationalHolidayResponse object
        [Fact]
        public async Task GetNationalHolidayById_ValidId_ToBeSuccesfull()
        {
            // Arrange
            // Create a new national holiday object
            NationalHoliday nationalHoliday = new NationalHoliday()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Get the id
            Guid Id = nationalHoliday.Id;

            // Convert to response DTO
            NationalHolidayResponse nationalHolidayResponse = nationalHoliday.toNationalHolidayResponse();

            // Mock the repository
            _nationalHolidayRepositoryMock.Setup(temp =>
            // For any Id given as parameter for GetNationalHolidayByIdAsync
            temp.GetNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return the same national holiday object
                .ReturnsAsync(nationalHoliday);

            // Act
            NationalHolidayResponse? nationalHolidayResponseFromGet = await _nationalHolidaysService
                .GetNationalHolidayByIdAsync(Id);

            // Assert
            Assert.Equal(nationalHolidayResponse, nationalHolidayResponseFromGet);
        }

        #endregion
        
        #region UpdateNationalHoliday

        // null PersonUpdateRequest => ArgumentNullException
        [Fact]
        public async Task UpdateNationalHoliday_NullNationalHoliday_ToBeArgumentNullException()
        {
            // Arrange
            NationalHolidayUpdateRequest? nationalHolidayUpdateRequest = null;

            // No need to mock the repository here

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _nationalHolidaysService.UpdateNationalHolidayAsync(nationalHolidayUpdateRequest);
            });
        }

        // invalid id (the given id does not already exist) => ArgumentExecption
        [Fact]
        public async Task UpdateNationalHoliday_InvalidId_ToBeArgumentException()
        {
            // Arrange 
            NationalHolidayUpdateRequest nationalHolidayUpdateRequest = new NationalHolidayUpdateRequest()
            {
                Id = Guid.NewGuid(), // we generate a new Id so it will not exist in the list of naional holidays
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Assert

            // No need to mock the repository here because it is checked before calling it

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
               await _nationalHolidaysService.UpdateNationalHolidayAsync(nationalHolidayUpdateRequest);
            });
        }

        // If the HolidayName is null => throw ArgumentException
        [Fact]
        public async Task UpdateNationalHoliday_HolidayNameIsNull_ToBeArgumentException()
        {
            // Arrange
            // Dummy national holiday
            NationalHoliday nationalHoliday = new NationalHoliday()
            {
                Id = Guid.NewGuid(),
                HolidayName = null,
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Invalid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest? nationalHolidayUpdateRequest = nationalHoliday
                // Convert the dummy national holiday to update request DTO
                .toNationalHolidayResponse().toNationalHolidayUpdateRequest();

            // No need to mock the repository here because it is checked before calling it

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _nationalHolidaysService.UpdateNationalHolidayAsync(nationalHolidayUpdateRequest);
            });
        }

        // If the HolidayDate is null => throw ArgumentException
        [Fact]
        public async Task UpdateNationalHoliday_HolidayDateIsNull_ToBeArgumentException()
        {
            // Arrange
            // Dummy national holiday
            NationalHoliday nationalHoliday = new NationalHoliday()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = null
            };

            // Invalid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest? nationalHolidayUpdateRequest = nationalHoliday
                // Convert the dummy national holiday to update request DTO
                .toNationalHolidayResponse().toNationalHolidayUpdateRequest();

            // No need to mock the repository here because it is checked before calling it

            // Asset
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _nationalHolidaysService.UpdateNationalHolidayAsync(nationalHolidayUpdateRequest);
            });
        }

        // If the HolidayName/HolidayDate are correct => update the national holiday in the existing list of national holidays
        [Fact]
        public async Task UpdateNationalHoliday_ProperArguments_ToBeSuccesful()
        {
            // Arrange
            // Dummy national holiday
            NationalHoliday nationalHoliday = new NationalHoliday()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Get the expected response from the update request
            NationalHolidayResponse nationalHolidayResponseExpected = nationalHoliday.toNationalHolidayResponse();

            // Valid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest? nationalHolidayUpdateRequest = nationalHoliday.toNationalHolidayResponse()
                .toNationalHolidayUpdateRequest();

            // Mock the repository
            // Mock UpdateNationalHolidayAsync
            _nationalHolidayRepositoryMock.Setup(temp =>
            // For any call of UpdateNationalHolidayAsync
            temp.UpdateNationalHolidayAsync(It.IsAny<NationalHoliday>()))
                // return the same national holiday
                .ReturnsAsync(nationalHoliday);

            // Mock GetNationalHolidayByIdAsync
            _nationalHolidayRepositoryMock.Setup(temp =>
            // For any call of GetNationalHolidayByIdAsync with any Id
            temp.GetNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return the same national holiday
                .ReturnsAsync(nationalHoliday);

            // Act
            // save the response in nationalHoliday_response_from_update
            NationalHolidayResponse nationalHolidayResponseFromUpdate = await _nationalHolidaysService
                .UpdateNationalHolidayAsync(nationalHolidayUpdateRequest);

            // Asset
            // Check if the dummy national holiday has the same id with the updated DTO response
            Assert.Equal(nationalHolidayResponseExpected, nationalHolidayResponseFromUpdate);
        }
        #endregion


        #region DeleteNationalHoliday
        [Fact]
        public async Task DeleteNationalHoliday_NullId_ToBeArgumentNullException()
        {
            // Arrange 
            Guid? id = null;

            // No need to mock the repository here beacuse the service will throw the expcetion before using the repository

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _nationalHolidaysService.DeleteNationalHolidayAsync(id);
            });
        }

        // Invalid national holiday id =>  false
        [Fact]
        public async Task DeleteNationalHoliday_InvalidId_ToBeFalse()
        {
            // Arrange
            Guid Id = Guid.NewGuid(); // random id that does not exist

            // No need to mock here beacause the id won't be in the database

            // Act
            bool isDeleted = await _nationalHolidaysService.DeleteNationalHolidayAsync(Id);

            // Assert
            Assert.False(isDeleted);
        }

        // Valid national holiday id => true
        [Fact]
        public async Task DeleteNationalHoliday_ValidId_ToBeTrue()
        {
            // Arrange
            // Dummy national holiday
            NationalHoliday nationalHoliday = new NationalHoliday()
            {
                Id = Guid.NewGuid(),
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Mock the repository
            // For any call of GetNationalHolidayByIdAsync
            _nationalHolidayRepositoryMock.Setup(temp => temp.GetNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return the same dummy national holiday object
                .ReturnsAsync(nationalHoliday);

            // For any call of DeleteNationalHolidayByIdAsync
            _nationalHolidayRepositoryMock.Setup(temp => temp.DeleteNationalHolidayByIdAsync(It.IsAny<Guid>()))
                // return true
                .ReturnsAsync(true);

            // Get the id from the dummy national holiday
            Guid? Id = nationalHoliday.Id;

            // Act
            bool isDeleted = await _nationalHolidaysService.DeleteNationalHolidayAsync(Id);

            // Assert
            Assert.True(isDeleted);
        }

        #endregion


        #region UpdateYearTo

        [Fact]
        public async Task UpdateYearTo_InvalidYear_ToBeArgumentException()
        {
            // Arrange
            // the year can't be a negative number
            int inputYear = -10;

            // No need to mock the repostory because the exception is thrown before accesing the repository

            // Assert 
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _nationalHolidaysService.UpdateYearToAsync(inputYear);
            });
        }

        [Fact]
        public async Task UpdateYearTo_ProperArgument_ToBeSuccesful()
        {
            // Arrange 
            // Dummy list of national holidays
            List<NationalHoliday> nationalHolidaysList = new List<NationalHoliday>
            {
                new NationalHoliday() { Id = Guid.Parse("d40c8731-1bf5-4729-867b-b9d2ce8f2e97"),
                                        HolidayDate = DateOnly.Parse("1/1/2023"),
                                        HolidayName = "Name 1"
                                      }
            };

            var obj = new NationalHoliday()
            {
                Id = Guid.Parse("d40c8731-1bf5-4729-867b-b9d2ce8f2e97"),
                HolidayDate = DateOnly.Parse("1/1/2023"),
                HolidayName = "Name 1"
            };

            // Mock the repository
            // For any call of GetAllNationalHolidaysAsync method
            _nationalHolidayRepositoryMock.Setup(temp => temp.GetAllNationalHolidaysAsync())
                // return the same list
                .ReturnsAsync(nationalHolidaysList);

            // For any call of UpdateNationalHolidayAsync method
            _nationalHolidayRepositoryMock.Setup(temp => temp.UpdateNationalHolidayAsync(It.IsAny<NationalHoliday>()))
                // return the object from the dummy list
                .ReturnsAsync(obj);

            // year to update to 
            int inputYear = 9999;
            
            // Act
            // Update the year to inputYear
            await _nationalHolidaysService.UpdateYearToAsync(inputYear);

            // Check each element from actual_nationalHoliday_response_list
            foreach (var nationalHoliday in nationalHolidaysList)
            {
                inputYear = 9999;

                // Assert
                // is the current element from actual_nationalHoliday_response_list's HolidayDate's year 
                // equal to inputYear?
                Assert.True(nationalHoliday.HolidayDate!.Value.Year == inputYear);
            }
        }

        #endregion
    }
}
