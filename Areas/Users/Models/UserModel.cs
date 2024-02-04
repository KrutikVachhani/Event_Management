namespace Event_Management.Areas.Users.Models
{
    public class UserModel
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoPath { get; set; }

        public string EmailAddress { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
