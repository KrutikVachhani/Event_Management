using System.ComponentModel.DataAnnotations;

namespace Event_Management.Areas.Users.Models
{
    public class UserModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        public IFormFile? File { get; set; }

        [Display(Name = "Photo Path")]
        public string? PhotoPath { get; set; }


        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string EmailAddress { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
