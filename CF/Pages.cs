using Microsoft.EntityFrameworkCore;

namespace Event_Management.CF
{
    public class Pages
    {
        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPages { get; set; }

        public int EndPage { get; set; }

        public Pages()
        {

        }

        public Pages(int totalItems, int page, int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 5;

            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if(endPage > totalPages)
            {
                endPage = totalPages;
                if(endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            StartPages = startPage;
            EndPage = endPage;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

    }
}
