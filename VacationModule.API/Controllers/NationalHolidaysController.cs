using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NationalHolidayModule.Core.DTO;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;

namespace VacationModule.API.Controllers
{
    [Route("api/national-holidays")]
    [ApiController]
    public class NationalHolidaysController : Controller
    {
        private readonly INationalHolidaysService _nationalHolidaysService;

        public NationalHolidaysController(INationalHolidaysService nationalHolidaysService)
        {
            _nationalHolidaysService = nationalHolidaysService;
        }

        [HttpPost("/api/admin/national-holidays")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NationalHolidayResponse>> Create(NationalHolidayAddRequest nationalHolidayAddRequest)
        {
            if (nationalHolidayAddRequest.HolidayDate.Equals(null))
            {
                return BadRequest(ModelState);
            }
            NationalHolidayResponse natioalHolidayResponse = await _nationalHolidaysService
                .AddNationalHolidayAsync(nationalHolidayAddRequest);

            return Ok(natioalHolidayResponse);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NationalHolidayResponse>>> GetNationalHolidays()
        {
            List<NationalHolidayResponse> nationalHolidaysList = await _nationalHolidaysService.GetAllNationalHolidaysAsync();

            return Ok(nationalHolidaysList);
        }

        [HttpPut("/api/admin/national-holidays/{Id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NationalHolidayResponse>> Edit(Guid Id, NationalHolidayUpdateRequest? nationalHolidayUpdateRequest)
        {
            // The given Id and the nationalHolidayUpdateRequest's Id should be the same
            // otherwise HttpPut will create a new object
            if (nationalHolidayUpdateRequest == null || Id != nationalHolidayUpdateRequest.Id)
            {
                return BadRequest(ModelState);
            }

            NationalHolidayResponse? nationalHolidayResponse = await _nationalHolidaysService
                .GetNationalHolidayByIdAsync(Id);

            if(nationalHolidayResponse == null)
            {
                return NotFound(nationalHolidayResponse);
            }

            await _nationalHolidaysService.UpdateNationalHolidayAsync(nationalHolidayUpdateRequest);

            return NoContent();
        }

        [HttpPut("/api/admin/national-holidays/update-to/{year}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<NationalHolidayResponse>>> UpdateNationalHolidaysToYear(int year)
        {

            List<NationalHolidayResponse> nationalHolidaysResponse = await _nationalHolidaysService.GetAllNationalHolidaysAsync();

            if (nationalHolidaysResponse.Count == 0)
            {
                return NotFound(nationalHolidaysResponse);
            }

            await _nationalHolidaysService.UpdateYearToAsync(year);

            return NoContent();
        }

        [HttpDelete("/api/admin/national-holidays/{idToDelete}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteNationalHoliday(Guid? idToDelete)
        {
            if(idToDelete == null)
            {
                return BadRequest(ModelState);
            }

            NationalHolidayResponse? nationalHolidayGetResponse = await _nationalHolidaysService.GetNationalHolidayByIdAsync(idToDelete);

            if(nationalHolidayGetResponse == null)
            {
                return NotFound(ModelState);
            }

            await _nationalHolidaysService.DeleteNationalHolidayAsync(nationalHolidayGetResponse.Id);

            return NoContent();   
        }
    }
}
