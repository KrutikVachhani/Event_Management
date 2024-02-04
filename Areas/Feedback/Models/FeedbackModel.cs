namespace Event_Management.Areas.Feedback.Models
{
    public class FeedbackModel
    {
        public int FeedbackID { get; set; }

        public int EventID { get; set; }

        public int Rating { get; set; }

        public string Comments { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
