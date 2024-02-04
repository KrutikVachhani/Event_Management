namespace Event_Management.Areas.Comment.Models
{
    public class CommentModel
    {
        public int CommentID { get; set; }

        public int EventID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
