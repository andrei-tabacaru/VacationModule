using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NationalHolidayModule.Core.DTO;
using System.Security.Claims;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VacationModule.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/vacations")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VacationsController : ControllerBase
    {
        private readonly IVacationsService _vacationsService;

        public VacationsController(IVacationsService vacationsService)
        {
            _vacationsService = vacationsService;
        }

        /// <summary>
        /// Methd to get the current user's id
        /// </summary>
        /// <returns>Authenticated user's id, or null if there is no user authenticated</returns>
        private Guid? GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity?)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return null;

            Guid currentUserId = Guid.Parse(claim.Value);

            return currentUserId;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VacationResponse>> Create(VacationAddRequest vacationAddRequest)
        {
            if (vacationAddRequest.StartDate.Equals(null)
                || vacationAddRequest.EndDate.Equals(null))
            {
                return BadRequest(ModelState);
            }

            // Get user's Id
            Guid? currentUserId = GetUserId();

            try
            {
                VacationResponse? vacationResponse = await _vacationsService
                    .AddVacationAsync(vacationAddRequest, currentUserId);

                return CreatedAtRoute("GetById", new { id = vacationResponse.Id }, vacationResponse);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet(template: "get-by-id/{id}", Name = "GetById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VacationResponse>> GetVacationById(Guid? id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }


            VacationResponse? vacationResponse = await _vacationsService
                .GetVacationByIdAsync(id);

            if (vacationResponse == null)
            {
                return NotFound(vacationResponse);
            }

            Guid? currentUserId = GetUserId();

            if (!User.IsInRole("Admin") && vacationResponse.ApplicationUserId != currentUserId)
            {
                return Unauthorized(ModelState);
            }

            return Ok(vacationResponse);
        }


        [HttpGet(template: "all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<VacationResponse>>> GetVacations()
        {
            Guid? currentUserId = GetUserId();

            List<VacationResponse> vacationsList = await _vacationsService.GetAllVacationsAsync(true, true, currentUserId);
            return Ok(vacationsList);
        }

        [HttpGet(template: "history")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<VacationResponse>>> GetPastVacations()
        {
            Guid? currentUserId = GetUserId();

            List<VacationResponse> vacationsList = await _vacationsService.GetAllVacationsAsync(false, true, currentUserId);
            return Ok(vacationsList);
        }

        [HttpGet(template: "current")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<VacationResponse>>> GetCurrentVacations()
        {
            Guid? currentUserId = GetUserId();

            List<VacationResponse> vacationsList = await _vacationsService.GetAllVacationsAsync(true, false, currentUserId);
            return Ok(vacationsList);
        }

        [HttpGet(template: "/api/v{version:apiVersion}/admin/vacations")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<VacationResponse>>> GetAllCurrentVacations()
        {
            List<VacationResponse> vacationsList = await _vacationsService.GetAllVacationsAsync(true, false);
            return Ok(vacationsList);
        }

        [HttpGet(template: "/api/v{version:apiVersion}/admin/vacations/history")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<VacationResponse>>> GetVacationsHistory()
        {
            List<VacationResponse> vacationsList = await _vacationsService.GetAllVacationsAsync(false, true);
            return Ok(vacationsList);
        }

        [HttpGet(template: "/api/v{version:apiVersion}/admin/vacations/users/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<VacationResponse>>> GetUserVacations(Guid? userId)
        {
            Guid? currentUserId = GetUserId();

            List<VacationResponse> vacationsList = await _vacationsService.GetAllVacationsAsync(true, false, userId);
            return Ok(vacationsList);
        }

        [HttpPut("{updateId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<VacationResponse>> Edit(Guid updateId, VacationUpdateRequest? vacationUpdateRequest)
        {
            // The given Id and the vacationUpdateRequest's Id should be the same
            // otherwise HttpPut will create a new object
            if (vacationUpdateRequest == null || updateId != vacationUpdateRequest.Id)
            {
                return BadRequest(ModelState);
            }

            VacationResponse? vacationResponse = await _vacationsService
                .GetVacationByIdAsync(updateId);

            if (vacationResponse == null)
            {
                return NotFound(vacationResponse);
            }

            Guid? currentUserId = GetUserId();
            if (vacationResponse.ApplicationUserId != currentUserId)
            {
                return Unauthorized(ModelState);
            }

            try
            {
                await _vacationsService.UpdateVacationAsync(vacationUpdateRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{deleteId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> DeleteVacation(Guid? deleteId)
        {
            if (deleteId == null)
            {
                return BadRequest(ModelState);
            }

            VacationResponse? vacationGetResponse = await _vacationsService.GetVacationByIdAsync(deleteId);

            if (vacationGetResponse == null)
            {
                return NotFound(ModelState);
            }

            Guid? currentUserId = GetUserId();
            if (vacationGetResponse.ApplicationUserId != currentUserId)
            {
                return Unauthorized(ModelState);
            }

            await _vacationsService.DeleteVacationAsync(vacationGetResponse.Id);

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [Route("available-days")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> GetAvailableDaysNumberForYear(int inputYear)
        {
            Guid? currentUserId = GetUserId();
            int remainingDays = 0;
            try
            {
                remainingDays = await _vacationsService.GetRemainingVacationDaysAsync(currentUserId, inputYear);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(remainingDays);
        }
    }
}
