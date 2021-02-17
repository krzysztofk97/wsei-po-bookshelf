using System;
using System.Collections.Generic;

#nullable disable

namespace BookshelfLib.Models
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int GenereId { get; set; }
        public int ReadCount { get; set; }
        public int ShelfId { get; set; }

        public virtual Genere Genere { get; set; }
        public virtual Shelf Shelf { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
