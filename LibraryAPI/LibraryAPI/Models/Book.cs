using System;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; } 
        public double Price { get; set; } 
    }
}
