using BookshelfLib.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BookshelfLib
{
    /// <summary>
    /// Klasa zawierająca metody używane do zarządzania bazą danych aplikacji.
    /// </summary>
    public class DBConnect
    {
        #region Books Methods
        /// <summary>
        /// Zwraca listę książek, które znajdują się aktualnie w bazie danych.
        /// </summary>
        /// <returns>Lista obiektów klasy <c>Book</c></returns>
        public List<Book> GetBooks()
        {
            using (BookshelfContext context = new BookshelfContext())
            {
                return context.Books.ToList();
            }
        }
        #endregion

        #region Shelfs Methods
        /// <summary>
        /// Zwraca listę półek, które znajdują się aktualnie w bazie danych.
        /// </summary>
        /// <returns>Lista obiektów klasy <c>Shelf</c></returns>
        public List<Shelf> GetShelfs() 
        {
            using(BookshelfContext context = new BookshelfContext())
            {
                return context.Shelfs.ToList();
            }
        }

        /// <summary>
        /// Dodaje nową półkę do bazy danych.
        /// </summary>
        /// <param name="name">Nazwa nowej półki</param>
        public void AddShelf(string name)
        {
            name = name.Trim();

            if (name == null || name == "")
                throw new ArgumentNullException();

            using(BookshelfContext context = new BookshelfContext())
            {
                context.Add(new Shelf() { ShelfName = name });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Usuwa istniejącą w bazie półkę.
        /// </summary>
        /// <param name="shelfToRemove">Półka, która ma zostać usunięta z bazy</param>
        public void RemoveShelf(Shelf shelfToRemove)
        {
            using (BookshelfContext context = new BookshelfContext())
            {
                context.Remove(shelfToRemove);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
