using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Event_Management.Areas.Venue.Models
{
    public class VenueModel
    {
        public int VenueID { get; set; }
        [Required(ErrorMessage = "Venue Name is Required.")]
        public string VenueName { get; set; }


        [Required(ErrorMessage = "Location is Required.")]
        public string Location { get; set; }


        [Required(ErrorMessage = "You have to enter capacity.")]
        [Range(100,800, ErrorMessage = "Enter range between 100 to 800.")]
        public int Capacity { get; set; }


        [Required(ErrorMessage = "Contact Person is required.")]
        public string ContactPerson { get; set; }


        [Required(ErrorMessage = "Contact Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number must be 10 digits.")]
        public double ContactNumber { get; set; }


        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
