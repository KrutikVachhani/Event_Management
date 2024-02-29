namespace Event_Management.Areas.Users.Models
{
    public class PageModel<T>
    {
        public List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
