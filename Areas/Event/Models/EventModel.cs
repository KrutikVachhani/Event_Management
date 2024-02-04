namespace Event_Management.Areas.Event.Models
{
    public class EventModel
    {
        public int EventID { get; set; }

        public string EventName { get; set; }

        public DateTime EventDateTime { get; set; }

        public bool IsPrivate { get; set; }

        public int VenueID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int CommentID { get; set; }


        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Message { get; set; }
    }
}
