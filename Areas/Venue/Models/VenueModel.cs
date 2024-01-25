using System.Data;

namespace Event_Management.Areas.Venue.Models
{
    public class VenueModel
    {
        public int VenueID { get; set; }

        public string VenueName { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public string ContactPerson { get; set; }

        public double ContactNumber { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
