using System;

namespace Bookstore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public bool HasDiscount { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public string DiscountText => HasDiscount ? "Да" : "Нет";
    }
}
