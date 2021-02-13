using System;
using System.Collections.Generic;

#nullable disable

namespace BookshelfLib.Models
{
    public partial class Genere
    {
        public Genere()
        {
            Books = new HashSet<Book>();
        }

        public string GenereName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
