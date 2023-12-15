using System;
namespace WebAPI.Models
{
	public class GetTripsDTO
	{
		public GetTripsDTO()
		{
		}

		public virtual List<Trip> UserTrips { get; set; } = new List<Trip>();
        public virtual List<Trip> PublicTrips { get; set; } = new List<Trip>();
    }
}

