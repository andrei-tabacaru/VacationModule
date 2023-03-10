using Microsoft.AspNetCore.Mvc;
using NationalHolidayModule.Core.DTO;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;

namespace VacationModule.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalHolidaysController : Controller
    {
        private readonly INationalHolidaysService _nationalHolidaysService;

        public NationalHolidaysController(INationalHolidaysService nationalHolidaysService)
        {
            _nationalHolidaysService = nationalHolidaysService;
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NationalHolidayResponse>> Create(NationalHolidayAddRequest nationalHolidayAddRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            NationalHolidayResponse? natioalHolidayResponse = await _nationalHolidaysService
                .AddNationalHolidayAsync(nationalHolidayAddRequest);

            return Ok(natioalHolidayResponse);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NationalHolidayResponse>>> GetNationalHolidays()
        {
            List<NationalHolidayResponse> nationalHolidaysList = await _nationalHolidaysService.GetAllNationalHolidaysAsync();

            return Ok(nationalHolidaysList);
        }

        [HttpPut]
        [Route("[action]/{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NationalHolidayResponse>> Edit(Guid Id, NationalHolidayUpdateRequest nationalHolidayUpdateRequest)
        {
            // The given Id and the nationalHolidayUpdateRequest's Id should be the same
            // otherwise HttpPut will create a new object
            if (nationalHolidayUpdateRequest == null || Id != nationalHolidayUpdateRequest.Id)
            {
                return BadRequest();
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

        [HttpPut]
        [Route("[action]/{year:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<NationalHolidayResponse>>> UpdateNationalHolidaysToYear(int year)
        {

            List<NationalHolidayResponse> nationalHolidaysResponse = await _nationalHolidaysService.GetAllNationalHolidaysAsync();

            if (nationalHolidaysResponse == null)
            {
                return NotFound(nationalHolidaysResponse);
            }

            await _nationalHolidaysService.UpdateYearToAsync(year);

            return NoContent();
        }

        [HttpDelete]
        [Route("[action]/{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteNationalHolidays(Guid? Id)
        {
            if(Id == null)
            {
                return BadRequest();
            }

            NationalHolidayResponse? nationalHolidayGetResponse = await _nationalHolidaysService.GetNationalHolidayByIdAsync(Id);

            if(nationalHolidayGetResponse == null)
            {
                return NotFound();
            }

            await _nationalHolidaysService.DeleteNationalHolidayAsync(nationalHolidayGetResponse.Id);

            return NoContent();   
        }
    }
}
