using System;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Exceptions;
using WebAPI.Models;

namespace WebAPI.Services
{
	public class TripsService
    {
        private readonly WebAPIContext? _context;

        public TripsService()
        {
            
        }

        public TripsService(WebAPIContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Trip>> GetUserTrips(string userid)
        {
            var user = await _context!.Users.SingleOrDefaultAsync(u => u.Id == userid);
            if(user == null)
            {
                throw new UserNotFoundException();
            }
            return user.Trips.ToList();
        }

        public virtual async Task<List<Trip>> GetPublicTrips()
        {
            var trips = _context!.Trips.Where(t => t.IsPublic == true);
            return await trips.ToListAsync();
        }

        public virtual async Task<Trip?> Get(int id)
        {
            var trip = await _context!.Trips.FindAsync(id);

            if (trip == null)
            {
                throw new TripNotFoundException();
            }

            return trip;
        }

        public virtual async Task<Trip> ShareTrip(string ownerid, int tripid, string sharetoemail)
        {
            var trip = await Get(tripid);
            if(trip == null)
            {
                throw new TripNotFoundException();
            }

            var isUsertrip = (trip.Users.SingleOrDefault(u => u.Id == ownerid) != null);
            if (!isUsertrip)
            {
                throw new NotUserTripException();
            }

            var newOwner = _context!.Users.SingleOrDefault(u => u.Email == sharetoemail);
            if(newOwner == null)
            {
                throw new UserNotFoundException();
            }

            trip.Users.Add(newOwner);
            _context.SaveChanges();

            return trip;
        }
    }
}

