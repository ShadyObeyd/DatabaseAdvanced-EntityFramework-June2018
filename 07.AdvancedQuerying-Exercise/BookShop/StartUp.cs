namespace BookShop
{
    using BookShop.Data;
    using System;
    using System.Linq;
    using System.Text;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            //string input = Console.ReadLine();

            using (BookShopContext context = new BookShopContext())
            {
                
            }
        }

        // 15.Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            int booksDeleted = books.Count();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return booksDeleted;
        }

        // 14.Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        // 13. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecentBooks = c.CategoryBooks.Select(cb => cb.Book).OrderByDescending(b => b.ReleaseDate).Take(3)
                })
                .OrderBy(c => c.Name)
                .ToArray();


            StringBuilder builder = new StringBuilder();

            foreach (var category in categories)
            {
                builder.AppendLine($"--{category.Name}");

                foreach (var book in category.MostRecentBooks)
                {
                    builder.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return builder.ToString().Trim();
        }

        // 12. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (var category in categories)
            {
                builder.AppendLine($"{category.Name} ${category.TotalProfit:f2}");
            }

            return builder.ToString().Trim();
        }

        // 11.Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    BookCopiesCount = a.Books.Select(b => b.Copies).Sum()
                })
                .OrderByDescending(a => a.BookCopiesCount)
                .ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (var author in authors)
            {
                builder.AppendLine($"{author.AuthorName} - {author.BookCopiesCount}");
            }

            return builder.ToString().Trim();
        }

        // 10.Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck).ToArray();

            return books.Count();
        }

        // 09.Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Include(b => b.Author)
                .Where(b => EF.Functions.Like(b.Author.LastName.ToLower(), $"{input.ToLower()}%"))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}"
                });

            StringBuilder builder = new StringBuilder();

            foreach (var book in books)
            {
                builder.AppendLine($"{book.Title} ({book.AuthorName})");
            }

            return builder.ToString().Trim();
        }

        // 08.Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] bookTitles = context.Books
                .Where(b => EF.Functions.Like(b.Title.ToLower(), $"%{input.ToLower()}%"))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (string title in bookTitles)
            {
                builder.AppendLine(title);
            }

            return builder.ToString().Trim();
        }

        // 07.Author Search 
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => EF.Functions.Like(a.FirstName, $"%{input}")).ToArray()
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                }).OrderBy(a => a.FullName);

            StringBuilder builder = new StringBuilder();

            foreach (var author in authors)
            {
                builder.AppendLine(author.FullName);
            }

            return builder.ToString().Trim();
        }

        // 06.Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    b.EditionType,
                    b.Price
                }).ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (var book in books)
            {
                builder.AppendLine($"{book.BookTitle} - {book.EditionType.ToString()} - ${book.Price:f2}");
            }

            return builder.ToString().Trim();
        }

        // 05.Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();

            var books = context.Books
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .OrderBy(b => b.Title);

            StringBuilder builder = new StringBuilder();

            foreach (var book in books)
            {
                var bookCategories = book.BookCategories.Select(bc => bc.Category.Name.ToLower()).ToArray();

                foreach (string category in categories)
                {
                    if (bookCategories.Contains(category.ToLower()))
                    {
                        builder.AppendLine(book.Title);
                    }
                }
            }

            return builder.ToString().Trim();
        }

        // 04.Not Released In
        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            string[] bookTitles = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (string title in bookTitles)
            {
                builder.AppendLine(title);
            }

            return builder.ToString().Trim();
        }

        // 03.Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                }).ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (var book in books)
            {
                builder.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return builder.ToString().Trim();
        }

        // 02.Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var editionType = (EditionType)Enum.Parse(typeof(EditionType), "Gold");

            string[] bookTitles = context.Books
                .Where(b => b.EditionType == editionType && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (string title in bookTitles)
            {
                builder.AppendLine(title);
            }

            return builder.ToString().Trim();
        }

        // 01.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            string[] bookTitles = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            StringBuilder builder = new StringBuilder();

            foreach (string title in bookTitles)
            {
                builder.AppendLine(title);
            }

            return builder.ToString().Trim();
        }
    }
}