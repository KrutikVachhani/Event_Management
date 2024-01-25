namespace Event_Management.Areas.Schedule.Models
{
    public class ScheduleModel
    {
        public int ScheduleID { get; set; }

        public int EventID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Activity { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
