using System;
namespace WebAPI.Models
{
	public class GetTripsDTO
	{
		public GetTripsDTO()
		{
		}

		public List<Trip> UserTrips { get; set; } = new List<Trip>();
        public List<Trip> PublicTrips { get; set; } = new List<Trip>();
    }
}

