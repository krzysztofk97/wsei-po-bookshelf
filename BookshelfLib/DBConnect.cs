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
using Microsoft.EntityFrameworkCore;

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

                using (SqlCommand command = connection.CreateCommand())
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
                return context.Books
                    .Include("Genere")
                    .Include("Shelf")
                    .ToList();
            }
        }

        /// <summary>
        /// Dodaj nową książkę do bazy danych.
        /// </summary>
        /// <param name="title">Tytuł ksiązki</param>
        /// <param name="purchaseDate">Data zakupu ksiązki</param>
        /// <param name="genere">Gatunek</param>
        /// <param name="shelf">Półka, na której znajduje się książka</param>
        /// <param name="authors">Lista autorów książki</param>
        public void AddBook(string title, DateTime purchaseDate, Genere genere, Shelf shelf, List<Author> authors)
        {
            title = title.Trim();

            if (title == null || title == "" || purchaseDate == null || genere == null || shelf == null || authors == null || authors.Count == 0)
                throw new ArgumentNullException();

            if (purchaseDate > DateTime.Now)
                throw new ArgumentOutOfRangeException();

            Book bookToAdd = new Book()
            {
                Title = title,
                PurchaseDate = purchaseDate,
                GenereId = genere.GenereId,
                ShelfId = shelf.ShelfId
            };

            using (BookshelfContext context = new BookshelfContext())
            {
                context.Add(bookToAdd);
                context.SaveChanges();                    
            }

            foreach (Author author in authors)
                AddBookAuthor(bookToAdd, author);
        }

        /// <summary>
        /// Modyfikuje istniejącą w bazie książkę.
        /// </summary>
        /// <param name="bookToModify">Książka, która ma zostać zmodyfikowana</param>
        /// <param name="title">Nowy tytuł</param>
        /// <param name="purchaseDate">Nowa data zakupu</param>
        /// <param name="genere">Nowy gatunek</param>
        /// <param name="shelf">Nowa półka</param>
        public void ModifyBook(Book bookToModify, string title, DateTime purchaseDate, Genere genere, Shelf shelf, List<Author> authors)
        {
            title = title.Trim();

            if (title == null || title == "" || purchaseDate == null || genere == null || shelf == null || authors == null || authors.Count == 0)
                throw new ArgumentNullException();

            if (purchaseDate > DateTime.Now)
                throw new ArgumentOutOfRangeException();


            bookToModify.Title = title;
            bookToModify.PurchaseDate = purchaseDate;
            bookToModify.Genere = genere;
            bookToModify.Shelf = shelf;

            using (BookshelfContext context = new BookshelfContext())
            {
                context.Update(bookToModify);
                context.SaveChanges();
            }

            List<Author> currentBookAuthors = GetBookAuthors(bookToModify);

            foreach(Author authorToRemove in currentBookAuthors)
            {
                if (!authors.Contains(authorToRemove))
                    RemoveAuthorFromBook(bookToModify, authorToRemove);
            }

            foreach(Author authorToAdd in authors)
            {
                if (!currentBookAuthors.Contains(authorToAdd))
                    AddBookAuthor(bookToModify, authorToAdd);
            }
        }

        /// <summary>
        /// Zwiększa licznik przeczytań książki o 1.
        /// </summary>
        /// <param name="bookRead">Przeczytana książka</param>
        public void IncrementBookReadCount(Book bookRead)
        {
            if (bookRead == null)
                throw new ArgumentNullException();

            using (BookshelfContext context = new BookshelfContext())
            {
                bookRead.ReadCount++;
                context.Update(bookRead);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Resetuje licznik przeczytań książki
        /// </summary>
        /// <param name="book">Książka, której licznik ma zostać zresetowany</param>
        public void ResetBookReadCount(Book book)
        {
            if (book == null)
                throw new ArgumentNullException();

            using (BookshelfContext context = new BookshelfContext())
            {
                book.ReadCount = 0;
                context.Update(book);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Usuwa istniejącą w bazie książkę.
        /// </summary>
        /// <param name="bookToRemove">Książka, która ma zostać usunieta</param>
        public void RemoveBook(Book bookToRemove)
        {
            if (bookToRemove == null)
                throw new ArgumentNullException();

            foreach (Author bookAuthor in GetBookAuthors(bookToRemove))
                RemoveAuthorFromBook(bookToRemove, bookAuthor);

            using (BookshelfContext context = new BookshelfContext())
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
            using (BookshelfContext context = new BookshelfContext())
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

            using (BookshelfContext context = new BookshelfContext())
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

        /// <summary>
        /// Modyfikuje istniejącą w bazie półkę.
        /// </summary>
        /// <param name="shelfToModify">Pólka, która ma zostać zmodyfikowana</param>
        /// <param name="name">Nowa nazwa półki</param>
        public void ModifyShelf(Shelf shelfToModify, string name)
        {
            name = name.Trim();

            if (shelfToModify == null || name == null || name == "")
                throw new ArgumentNullException();

            shelfToModify.ShelfName = name;

            using (BookshelfContext context = new BookshelfContext())
            {
                context.Update(shelfToModify);
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

        /// <summary>
        /// Modyfikuje istniejący w bazie gatunek.
        /// </summary>
        /// <param name="genereToModify">Autor, który ma zostać zmodyfikowany</param>
        /// <param name="name">Nowa nazwa gatunku</param>
        public void ModifyGenere(Genere genereToModify, string name)
        {
            name = name.Trim();

            if (genereToModify == null || name == null || name == "")
                throw new ArgumentNullException();

            genereToModify.GenereName = name;

            using (BookshelfContext context = new BookshelfContext())
            {
                context.Update(genereToModify);
                context.SaveChanges();
            }
        }
        #endregion

        #region Authors Methods
        /// <summary>
        /// Zwraca listę autorów, którzy znajdują się aktualnie w bazie danych.
        /// </summary>
        /// <returns>Lista obiektów klasy <c>Author</c></returns>
        public List<Author> GetAuthors()
        {
            using (BookshelfContext context = new BookshelfContext())
            {
                return context.Authors.ToList();
            }
        }

        /// <summary>
        /// Dodaje nowego autora do bazy danych.
        /// </summary>
        /// <param name="firstName">Imię autora</param>
        /// <param name="lastName">Nazwisko autora</param>
        public void AddAuthor(string firstName, string lastName)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();

            if (firstName == "" || firstName == null || lastName == "" || lastName == null)
                throw new ArgumentNullException();

            using (BookshelfContext context = new BookshelfContext())
            {
                context.Authors.Add(new Author()
                {
                    FirstName = firstName,
                    LastName = lastName
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Usuwa istniejącego w bazie autora.
        /// </summary>
        /// <param name="authorToRemove">Autor, który ma zostać usunięty z bazy</param>
        public void RemoveAuthor(Author authorToRemove)
        {
            using (BookshelfContext context = new BookshelfContext())
            {
                context.Remove(authorToRemove);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Modyfikuje dane instniejącego w bazie autora.
        /// </summary>
        /// <param name="authorToModify">Autor, który ma zostać zmodyfikowany</param>
        /// <param name="firstName">Nowe imię autora</param>
        /// <param name="lastName">Nowe nazwisko autora</param>
        public void ModifyAuthor(Author authorToModify, string firstName, string lastName)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();

            if (authorToModify == null || firstName == "" || firstName == null || lastName == "" || lastName == null)
                throw new ArgumentNullException();

            authorToModify.FirstName = firstName;
            authorToModify.LastName = lastName;

            using(BookshelfContext context = new BookshelfContext())
            {
                context.Update(authorToModify);
                context.SaveChanges();
            }
        }
        #endregion

        #region BookAuthors Methods
        /// <summary>
        /// Zwraca listę autorów danej książki.
        /// </summary>
        /// <param name="book">Książka, której autorzy mają zostać zwróceni</param>
        /// <returns>Lista obiektów klasy <c>Author</c></returns>
        public List<Author> GetBookAuthors(Book book)
        {
            using(BookshelfContext context = new BookshelfContext())
            {
                return context.BookAuthors
                    .Where(x => x.BookId == book.BookId)
                    .Include("Author")
                    .Select(x => x.Author).ToList();
            }
        }

        /// <summary>
        /// Dodaje istniejącego w bazie autora do istniejącej bazie ksiązki.
        /// </summary>
        /// <param name="book">Ksiązka, do której ma zostać dodany autor</param>
        /// <param name="author">Autor, który ma zostać przypisany do danej książki</param>
        public void AddBookAuthor(Book book, Author author) 
        {
            if (book == null || author == null)
                throw new ArgumentNullException();

            using(BookshelfContext context = new BookshelfContext())
            {
                context.Add(new BookAuthor()
                {
                    BookId = book.BookId,
                    AuthorId = author.AuthorId
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Usuwa autora z danej książki.
        /// </summary>
        /// <param name="book">Książka, z której ma zostać usunięty dany autor</param>
        /// <param name="author">Autor, który ma zostać usunięty z danej książki</param>
        public void RemoveAuthorFromBook(Book book, Author author)
        {
            if (book == null || author == null)
                throw new ArgumentNullException();

            using (BookshelfContext context = new BookshelfContext())
            {
                context.Remove(new BookAuthor()
                {
                    BookId = book.BookId,
                    AuthorId = author.AuthorId
                });
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