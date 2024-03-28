namespace Event_Management.Areas.Service.Models
{
    public class ServiceModel
    {
        public int ServiceID { get; set; }

        public string ServiceName { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
