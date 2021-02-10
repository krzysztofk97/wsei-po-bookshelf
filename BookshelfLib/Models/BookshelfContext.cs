using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BookshelfLib.Models
{
    public partial class BookshelfContext : DbContext
    {
        public BookshelfContext()
        {
        }

        public BookshelfContext(DbContextOptions<BookshelfContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<Genere> Generes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Shelf> Shelfs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Database=Bookshelf");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.FirstName).HasMaxLength(32);

                entity.Property(e => e.LastName).HasMaxLength(32);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.Genere)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.Property(e => e.ShelfId).HasColumnName("ShelfID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.GenereNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.Genere)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Books_Generes");

                entity.HasOne(d => d.Shelf)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.ShelfId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Books_Shelfs");
            });

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.AuthorId })
                    .HasName("PK__BookAuth__6AED6DE69F047E1E");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookAuthors_Authors");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookAuthors_Books");
            });

            modelBuilder.Entity<Genere>(entity =>
            {
                entity.HasKey(e => e.Genere1)
                    .HasName("PK__Generes__8D08175417769D7E");

                entity.Property(e => e.Genere1)
                    .HasMaxLength(32)
                    .HasColumnName("Genere");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Shelf>(entity =>
            {
                entity.Property(e => e.ShelfId).HasColumnName("ShelfID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.ShelfName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Shelves)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shelfs_Rooms");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
