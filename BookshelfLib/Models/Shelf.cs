﻿using System;
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

        public string ShelfName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
