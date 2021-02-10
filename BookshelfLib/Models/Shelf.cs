using System;
using System.Collections.Generic;

#nullable disable

namespace BookshelfLib.Models
{
    public partial class Shelf
    {
        public Shelf()
        {
            Books = new HashSet<Book>();
        }

        public int ShelfId { get; set; }
        public string ShelfName { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
