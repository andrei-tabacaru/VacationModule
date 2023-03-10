using NationalHolidayModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;
using VacationModule.Core.Services;

namespace VacationModule.ServiceTests
{
    public class NationalHolidaysServiceTest
    {
        private readonly INationalHolidaysService _nationalHolidaysService;

        public NationalHolidaysServiceTest()
        {
            _nationalHolidaysService = new NationalHolidaysService();
        }

        #region AddNationalHoliday

        // If NationalHolidayAddRequest is null => throw ArgumentNullException
        [Fact]
        public void AddNationalHoliday_NullNationalHoliday()
        {
            // Arrange
            NationalHolidayAddRequest? request = null;

            // Asset
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _nationalHolidaysService.AddNationalHoliday(request);
            });
        }
        // If the HolidayName is null => throw ArgumentException
        [Fact]
        public void AddNationalHoliday_HolidayNameIsNull()
        {
            // Arrange
            NationalHolidayAddRequest? request = new NationalHolidayAddRequest()
            {
                HolidayName = null,
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Asset
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _nationalHolidaysService.AddNationalHoliday(request);
            });
        }

        // If the HolidayDate is null => throw ArgumentException
        [Fact]
        public void AddNationalHoliday_HolidayDateIsNull()
        {
            // Arrange
            NationalHolidayAddRequest? request = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = null
            };

            // Asset
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _nationalHolidaysService.AddNationalHoliday(request);
            });
        }

        // If the HolidayName/HolidayDate are correct => add the national holiday to the existing list of national holidays
        [Fact]
        public void AddNationalHoliday_ProperArgumets()
        {
            // Arrange
            NationalHolidayAddRequest? request = new NationalHolidayAddRequest()
            {
                HolidayName = "name",
                HolidayDate = DateOnly.Parse("1/1/2023")
            };
            
            // Act
            NationalHolidayResponse response = _nationalHolidaysService.AddNationalHoliday(request);

            // Asset
            Assert.True(response.Id != Guid.Empty);
        }
        #endregion

        #region GetAllNationalHolidays

        [Fact]
        public void GetAllNationalHolidays_EmptyList()
        {
            // Act
            List<NationalHolidayResponse> actual_nationalHoliday_response_list = _nationalHolidaysService.GetAllNationalHolidays();

            // Assert
            Assert.Empty(actual_nationalHoliday_response_list);
        }

        [Fact]
        public void GetAllNationalHolidays_AddFewHolidays()
        {
            // Arrange
            List<NationalHolidayAddRequest> nationalHoliday_add_request = new List<NationalHolidayAddRequest>
            { 
                new NationalHolidayAddRequest() { HolidayDate = DateOnly.Parse("1/1/2023"), 
                                                  HolidayName = Guid.NewGuid().ToString()
                                                },
                new NationalHolidayAddRequest() { HolidayDate = DateOnly.Parse("2/1/2023"),
                                                  HolidayName = Guid.NewGuid().ToString()
                                                }
            };

            // Act
            // The AddNationalHoliday method returns a NationalHolidayResponse object if succesful
            // We will store them in a list to check if they are the same as those in the actual_nationalHoliday_response_list
            var nationalHolidays_list_from_add_nationalHoliday = new List<NationalHolidayResponse>();
            foreach(var nationalHoliday_request in nationalHoliday_add_request)
            {
                nationalHolidays_list_from_add_nationalHoliday
                    .Add(_nationalHolidaysService.AddNationalHoliday(nationalHoliday_request));
            }

            var actual_nationalHoliday_response_list = _nationalHolidaysService.GetAllNationalHolidays();

            // check each element from nationalHolidays_list_from_add_nationalHoliday
            foreach(var expected_nationalHoliday in nationalHolidays_list_from_add_nationalHoliday)
            {
                // Assert
                // is the current element from nationalHolidays_list_from_add_nationalHoliday in
                // the actual_nationalHoliday_response_list?
                Assert.Contains(expected_nationalHoliday, actual_nationalHoliday_response_list);
            }
        }

        #endregion

        #region GetNationalHolidayById

        // If id == null => NationalHolidayResponse == null
        [Fact]
        public void GetNationalHolidayById_NullId()
        {
            // Arrange
            Guid? Id = null;

            // Act
            NationalHolidayResponse? nationalHoliday_response_from_get = _nationalHolidaysService
                .GetNationalHolidayById(Id);

            // Assert
            Assert.Null(nationalHoliday_response_from_get);
        }

        // If id is a valid national holiday id =>  return the valid national holiday details as NationalHolidayResponse object
        [Fact]
        public void GetNationalHolidayById_ValidId()
        {
            // Arrange
            // Create a new add request
            NationalHolidayAddRequest nationalHoliday_add_request = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // get the response
            NationalHolidayResponse nationalHoliday_add_response = _nationalHolidaysService
                .AddNationalHoliday(nationalHoliday_add_request);

            // get the id
            Guid Id = nationalHoliday_add_response.Id;

            // Act
            NationalHolidayResponse? nationalHoliday_response_from_get = _nationalHolidaysService
                .GetNationalHolidayById(Id);

            // Assert
            Assert.Equal(nationalHoliday_add_response, nationalHoliday_response_from_get);
        }

        #endregion

        #region UpdateNationalHoliday

        // null PersonUpdateRequest => ArgumentNullException
        [Fact]
        public void UpdateNationalHoliday_NullNationalHoliday()
        {
            // Arrange
            NationalHolidayUpdateRequest? nationalHoliday_update_request = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _nationalHolidaysService.UpdateNationalHoliday(nationalHoliday_update_request);
            });
        }

        // invalid id (the given id does not already exist) => ArgumentExecption
        [Fact]
        public void UpdateNationalHoliday_InvalidId()
        {
            // Arrange 
            NationalHolidayUpdateRequest nationalHoliday_update_request = new NationalHolidayUpdateRequest()
            {
                Id = Guid.NewGuid(), // we generate a new Id so it will not exist in the list of naional holidays
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _nationalHolidaysService.UpdateNationalHoliday(nationalHoliday_update_request);
            });
        }

        // If the HolidayName is null => throw ArgumentException
        [Fact]
        public void UpdateNationalHoliday_HolidayNameIsNull()
        {
            // Arrange
            // Add a national holiday
            NationalHolidayAddRequest nationalHoliday_add_request = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };
            // Get the response
            NationalHolidayResponse nationalHoliday_from_add = _nationalHolidaysService
                .AddNationalHoliday(nationalHoliday_add_request);

            // Valid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest? nationalHoliday_update_request = nationalHoliday_from_add
                .toNationalHolidayUpdateRequest();
            // Make HolidayName invalid
            nationalHoliday_update_request.HolidayName = null;

            // Asset
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _nationalHolidaysService.UpdateNationalHoliday(nationalHoliday_update_request);
            });
        }

        // If the HolidayDate is null => throw ArgumentException
        [Fact]
        public void UpdateNationalHoliday_HolidayDateIsNull()
        {
            // Arrange
            // Add a national holiday
            NationalHolidayAddRequest nationalHoliday_add_request = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };
            // Get the response
            NationalHolidayResponse nationalHoliday_from_add = _nationalHolidaysService
                .AddNationalHoliday(nationalHoliday_add_request);

            // Valid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest? nationalHoliday_update_request = nationalHoliday_from_add
                .toNationalHolidayUpdateRequest();
            // Make HolidayDate invalid
            nationalHoliday_update_request.HolidayDate = null;

            // Asset
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _nationalHolidaysService.UpdateNationalHoliday(nationalHoliday_update_request);
            });
        }

        // If the HolidayName/HolidayDate are correct => update the national holiday in the existing list of national holidays
        [Fact]
        public void UpdateNationalHoliday_ProperArguments()
        {
            // Arrange
            // Add a national holiday
            NationalHolidayAddRequest nationalHoliday_add_request = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };
            // Get the response
            NationalHolidayResponse nationalHoliday_from_add = _nationalHolidaysService
                .AddNationalHoliday(nationalHoliday_add_request);

            // Valid NationalHolidayUpdateRequest
            NationalHolidayUpdateRequest? nationalHoliday_update_request = nationalHoliday_from_add
                .toNationalHolidayUpdateRequest();
            
            // Change data
            nationalHoliday_update_request.HolidayDate = DateOnly.Parse("2/1/2023");
            nationalHoliday_update_request.HolidayName = Guid.NewGuid().ToString();

            // Act
            // save the response in nationalHoliday_response_from_update
            NationalHolidayResponse nationalHoliday_response_from_update = _nationalHolidaysService
                .UpdateNationalHoliday(nationalHoliday_update_request);

            // get the id
            Guid? Id = nationalHoliday_response_from_update.Id;

            // get the national holiday from the list
            NationalHolidayResponse? nationalHoliday_respone_from_get = _nationalHolidaysService
                .GetNationalHolidayById(Id);
            
            // Asset
            //                 from get                              actual
            Assert.Equal(nationalHoliday_respone_from_get, nationalHoliday_response_from_update);
        }
        #endregion

        #region DeleteNationalHoliday

        // Invalid national holiday id =>  false
        [Fact]
        public void DeleteNationalHoliday_InvalidId()
        {
            // Arrange
            Guid Id = Guid.NewGuid(); // random id that does not exist
            
            // Act
            bool isDeleted = _nationalHolidaysService.DeleteNationalHoliday(Id);

            // Assert
            Assert.False(isDeleted);
        }

        // Valid national holiday id => true
        [Fact]
        public void DeleteNationalHoliday_ValidId()
        {
            // Arrange
            // Create a new add request
            NationalHolidayAddRequest nationalHoliday_add_request = new NationalHolidayAddRequest()
            {
                HolidayName = Guid.NewGuid().ToString(),
                HolidayDate = DateOnly.Parse("1/1/2023")
            };

            // Add the object
            NationalHolidayResponse nationalHoliday_add_response = _nationalHolidaysService
                .AddNationalHoliday(nationalHoliday_add_request);

            // Get the id from the response
            Guid? Id = nationalHoliday_add_response.Id;

            // Act
            bool isDeleted = _nationalHolidaysService.DeleteNationalHoliday(Id);

            // Assert
            Assert.True(isDeleted);
        }

        #endregion

        #region GetListToDictionary

        [Fact]
        public void GetListToDictionary_EmptyDictonary()
        {
            // Act
            Dictionary<DateOnly, string?> actual_nationalHoliday_response_dictionary = _nationalHolidaysService
                .GetListToDictionary();

            // Assert
            Assert.Empty(actual_nationalHoliday_response_dictionary);
        }

        [Fact]
        public void GetListToDictionary_AddFewHolidays()
        {
            // Arrange
            List<NationalHolidayAddRequest> nationalHoliday_add_request = new List<NationalHolidayAddRequest>
            {
                new NationalHolidayAddRequest() { HolidayDate = DateOnly.Parse("1/1/2023"),
                                                  HolidayName = Guid.NewGuid().ToString()
                                                },
                new NationalHolidayAddRequest() { HolidayDate = DateOnly.Parse("2/1/2023"),
                                                  HolidayName = Guid.NewGuid().ToString()
                                                }
            };

            // Act
            // Add each nationalHoliday_add_request into the list
            
            foreach (var nationalHoliday_request in nationalHoliday_add_request)
            {
                _nationalHolidaysService.AddNationalHoliday(nationalHoliday_request);
            }


            // Get the list of national holidays
            var actual_nationalHoliday_response_list = _nationalHolidaysService.GetAllNationalHolidays();

            // Convert to dictionary
            var nationalHoliday_dictionary = _nationalHolidaysService.GetListToDictionary();

            // Check each element from actual_nationalHoliday_response_list
            foreach (var expected_nationalHoliday in actual_nationalHoliday_response_list)
            {
                // Assert
                // is the current element from actual_nationalHoliday_response_list's HolidayDate in
                // the nationalHoliday_dictionary?
                Assert.True(nationalHoliday_dictionary.ContainsKey((DateOnly)expected_nationalHoliday.HolidayDate!));
            }
        }

        #endregion

        #region UpdateYearTo

        [Fact]
        public void UpdateYearTo_InvalidYear()
        {
            // Arrange
            // the year can't be a negative number
            int inputYear = -10;

            // Assert 
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _nationalHolidaysService.UpdateYearTo(inputYear);
            });
        }

        [Fact]
        public void UpdateYearTo_ProperArgument()
        {
            // Arrange 
            // Add few national holidays
            List<NationalHolidayAddRequest> nationalHoliday_add_request = new List<NationalHolidayAddRequest>
            {
                new NationalHolidayAddRequest() { HolidayDate = DateOnly.Parse("1/1/2023"),
                                                  HolidayName = Guid.NewGuid().ToString()
                                                },
                new NationalHolidayAddRequest() { HolidayDate = DateOnly.Parse("2/1/2023"),
                                                  HolidayName = Guid.NewGuid().ToString()
                                                }
            };

            // year to update to
            int inputYear = 2024;

            // Act
            // Add each nationalHoliday_add_request into the list
            foreach (var nationalHoliday_request in nationalHoliday_add_request)
            {
                _nationalHolidaysService.AddNationalHoliday(nationalHoliday_request);
            }



            // Update the year to inputYear
            _nationalHolidaysService.UpdateYearTo(inputYear);

            // Get the list of updated national holidays
            var actual_nationalHoliday_response_list = _nationalHolidaysService.GetAllNationalHolidays();

            // Check each element from actual_nationalHoliday_response_list
            foreach (var nationalHoliday in actual_nationalHoliday_response_list)
            {
                // Assert
                // is the current element from actual_nationalHoliday_response_list's HolidayDate's year 
                // equal to inputYear?
                Assert.True(nationalHoliday.HolidayDate!.Value.Year == inputYear);
            }
        }

        #endregion
    }
}
