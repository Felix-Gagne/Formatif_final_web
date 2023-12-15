using System;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
	public class Trip
	{
		public Trip()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public bool IsPublic { get; set; }
		[JsonIgnore]
		public virtual List<DemoUser> Users { get; set; } = new List<DemoUser>();
	}
}

