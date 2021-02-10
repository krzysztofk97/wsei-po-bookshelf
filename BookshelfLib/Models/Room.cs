using System;
using System.Collections.Generic;

#nullable disable

namespace BookshelfLib.Models
{
    public partial class Room
    {
        public Room()
        {
            Shelves = new HashSet<Shelf>();
        }

        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int FloorNumber { get; set; }

        public virtual ICollection<Shelf> Shelves { get; set; }
    }
}
