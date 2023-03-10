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
        public ActionResult<NationalHolidayResponse> Create(NationalHolidayAddRequest nationalHolidayAddRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            NationalHolidayResponse? natioalHolidayResponse = _nationalHolidaysService.AddNationalHoliday(nationalHolidayAddRequest);

            return Ok(natioalHolidayResponse);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<NationalHolidayResponse>> GetNationalHolidays()
        {
            List<NationalHolidayResponse> nationalHolidaysList = _nationalHolidaysService.GetAllNationalHolidays();

            return Ok(nationalHolidaysList);
        }

        [HttpPut]
        [Route("[action]/{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<NationalHolidayResponse> Edit(Guid Id, NationalHolidayUpdateRequest nationalHolidayUpdateRequest)
        {
            // The given Id and the nationalHolidayUpdateRequest's Id should be the same
            // otherwise HttpPut will create a new object
            if (nationalHolidayUpdateRequest == null || Id != nationalHolidayUpdateRequest.Id)
            {
                return BadRequest();
            }

            NationalHolidayResponse? nationalHolidayResponse = _nationalHolidaysService
                .GetNationalHolidayById(Id);

            if(nationalHolidayResponse == null)
            {
                return NotFound(nationalHolidayResponse);
            }

            /*NationalHolidayUpdateRequest nationalHolidayUpdateRequest2 = nationalHolidayResponse
                .toNationalHolidayUpdateRequest();*/

            NationalHolidayResponse nationalHolidayUpdateResponse = _nationalHolidaysService.UpdateNationalHoliday(nationalHolidayUpdateRequest);

            return NoContent();
        }

        [HttpDelete]
        [Route("[action]/{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> DeleteNationalHolidays(Guid? Id)
        {
            if(Id == null)
            {
                return BadRequest();
            }

            NationalHolidayResponse? nationalHolidayGetResponse = _nationalHolidaysService.GetNationalHolidayById(Id);

            if(nationalHolidayGetResponse == null)
            {
                return NotFound();
            }

            _nationalHolidaysService.DeleteNationalHoliday(nationalHolidayGetResponse.Id);

            return NoContent();   
        }
    }
}
