using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Event_Management.Areas.SEC_User.Models
{
    public class SEC_UserModel
    {

        public int UserID { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string PhotoPath { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }
}
