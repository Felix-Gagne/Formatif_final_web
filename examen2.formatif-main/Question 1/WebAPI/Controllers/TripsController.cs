using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TripsController : ControllerBase
    {
        private readonly TripsService _service;

        public virtual string? UserId
        {
            get
            {
                if(User!=null)
                    return User.FindFirstValue(ClaimTypes.NameIdentifier);
                return null;
            }
        }

        public TripsController(TripsService service)
        {
            _service = service;
        }

        // GET: api/Trips
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<GetTripsDTO>> GetTrips()
        {
            var result = new GetTripsDTO();

            if(UserId != null)
            {
                var userTrips = await _service.GetUserTrips(UserId);
                result.UserTrips = userTrips;
            }

            var publicTrips = await _service.GetPublicTrips();
            result.PublicTrips = publicTrips;

            return result;
        }

        // POST: api/Trips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> ShareTrip(int id, ShareTripDTO shareTripDTO)
        {
            //TODO le contrôleur a [Authorize], alors il y aura toujours un UserId ici
            try
            {
                var trip = await _service.ShareTrip(UserId!, id, shareTripDTO.UserEmail);
                return Ok(trip);

            }
            catch(NotUserTripException)
            {
                return BadRequest();
            }
            catch (TripNotFoundException)
            {
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
