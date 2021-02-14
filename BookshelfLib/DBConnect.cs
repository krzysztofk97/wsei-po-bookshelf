using BookshelfLib.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BookshelfLib
{
    /// <summary>
    /// Klasa zawierająca metody używane do zarządzania bazą danych aplikacji.
    /// </summary>
    public class DBConnect
    {
        #region General Methods
        /// <summary>
        /// Metoda wywołująca skrypt CreateDatabase.sql, odpowiadający za tworzenie bazy danych programu.
        /// </summary>
        public void CreateDatabase()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Database=master";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BookshelfLib.CreateDatabase.sql");

            StreamReader reader = new StreamReader(stream);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using(SqlCommand command = connection.CreateCommand())
                {
                    var script = ParseSQLScript(reader.ReadToEnd());

                    foreach (var query in script)
                    {
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                }
            }  
        }
        #endregion

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

        /// <summary>
        /// Dodaj nową książkę do bazy danych.
        /// </summary>
        /// <param name="title">Tytuł ksiązki</param>
        /// <param name="purchaseDate">Data zakupu ksiązki</param>
        /// <param name="genere">Gatunek</param>
        /// <param name="shelf">Półka, na której znajduje się książka</param>
        public void AddBook(string title, DateTime purchaseDate, Genere genere, Shelf shelf) 
        {
            title = title.Trim();

            if (title == null || title == "" || purchaseDate == null || genere == null || shelf == null)
                throw new ArgumentNullException();

            if (purchaseDate > DateTime.Now)
                throw new ArgumentOutOfRangeException();

            using(BookshelfContext context = new BookshelfContext())
            {
                context.Add(new Book()
                {
                    Title = title,
                    PurchaseDate = purchaseDate,
                    GenereName = genere.GenereName,
                    ShelfName = shelf.ShelfName
                });
                context.SaveChanges();
            }
        }

        public void RemoveBook(Book bookToRemove)
        {
            if (bookToRemove == null)
                throw new ArgumentNullException();

            using(BookshelfContext context = new BookshelfContext())
            {
                context.Remove(bookToRemove);
                context.SaveChanges();
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

        #region Generes Methods
        /// <summary>
        /// Zwraca listę gatunków, które znajdują się aktualnie w bazie danych.
        /// </summary>
        /// <returns>Lista obiektów klasy <c>Genere</c></returns>
        public List<Genere> GetGeneres()
        {
            using (BookshelfContext context = new BookshelfContext())
            {
                return context.Generes.ToList();
            }
        }

        /// <summary>
        /// Dodaje nowy gatunek do bazy danych.
        /// </summary>
        /// <param name="name">Nazwa nowego gatunku</param>
        public void AddGenere(string name)
        {
            name = name.Trim();

            if (name == null || name == "")
                throw new ArgumentNullException();

            using (BookshelfContext context = new BookshelfContext())
            {
                context.Add(new Genere() { GenereName = name });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Usuwa istniejący w bazie gatunek.
        /// </summary>
        /// <param name="genereToRemove">Gatunek, który ma zostać usunięty z bazy</param>
        public void RemoveGenere(Genere genereToRemove)
        {
            using (BookshelfContext context = new BookshelfContext())
            {
                context.Remove(genereToRemove);
                context.SaveChanges();
            }
        }
        #endregion

        #region Other Methods
        private IEnumerable<string> ParseSQLScript(string script)
        {
            var result = Regex.Split(script, @"^GO[\r\n]?$", RegexOptions.Multiline);

            return result.Where(x => !string.IsNullOrWhiteSpace(x));
        }
        #endregion
    }
}
