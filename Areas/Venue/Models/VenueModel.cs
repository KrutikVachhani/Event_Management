using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Event_Management.Areas.Venue.Models
{
    public class VenueModel
    {
        public int VenueID { get; set; }
        [Required]
        public string VenueName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        [Required]
        public double ContactNumber { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
