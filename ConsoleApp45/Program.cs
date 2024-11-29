using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleApp45
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bookManager = new BookManager();
            bool continueProgram = true;

          
            while (continueProgram)
            {
                continueProgram = bookManager.Menu();
            }

            Console.WriteLine("Goodbye!");
        }
    }

    class Book
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public int ReleaseDate { get; set; }

        // Parameterless constructor for JSON deserialization
        public Book() { }

        // Constructor for manual book creation
        public Book(string author, string title, int year)
        {
            Author = author;
            Title = title;
            ReleaseDate = year;
        }

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author}, Release Year: {ReleaseDate}";
        }
    }

    class BookManager
    {
        private List<Book> books = new List<Book>();
        private const string filePath = "C:\\Users\\abela\\source\\repos\\ConsoleApp45\\books.json";  // Path for the JSON file

        public BookManager()
        {
            LoadBooksFromJson();
        }

        public bool Menu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add a new book");
            Console.WriteLine("2. View all books");
            Console.WriteLine("3. Search for a book by title");
            Console.WriteLine("4. Delete a book");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    DisplayBooks();
                    break;
                case "3":
                    SearchBook();
                    break;
                case "4":
                    DeleteBook();
                    break;
                case "5":
                    return false;  // Exit the program
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }

            return true;  // Continue the loop
        }

        // Add a new book to the list
        public void AddBook()
        {
            string title = GetValidTitle();
            string author = GetValidAuthor();
            int releaseYear = GetValidReleaseYear();

            var newBook = new Book(author, title, releaseYear);
            books.Add(newBook);
            Console.WriteLine($"Book with title '{newBook.Title}' added.");
            SaveBooksToJson();
        }

        // Display all books in the library
        public void DisplayBooks()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("There are no books in the library.");
                return;
            }

            Console.WriteLine("All Books in the Library:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        // Search for books by title
        public void SearchBook()
        {
            Console.WriteLine("Enter the title of the book you want to search for:");
            string title = Console.ReadLine().Trim();

            var foundBooks = books.FindAll(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            if (foundBooks.Count == 0)
            {
                Console.WriteLine($"No books found with title containing '{title}'.");
            }
            else
            {
                foreach (var book in foundBooks)
                {
                    Console.WriteLine(book);
                }
            }
        }

        // Delete a book from the library
        public void DeleteBook()
        {
            Console.WriteLine("Enter the title of the book you want to delete:");
            string title = Console.ReadLine().Trim();

            var bookToDelete = books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (bookToDelete != null)
            {
                Console.WriteLine($"Found the book: {bookToDelete}");
                Console.WriteLine("Do you want to delete this book? (yes/no)");

                string confirmation = Console.ReadLine().Trim().ToLower();
                if (confirmation == "yes")
                {
                    books.Remove(bookToDelete);
                    Console.WriteLine($"Book '{bookToDelete.Title}' has been deleted.");
                    SaveBooksToJson();
                }
                else
                {
                    Console.WriteLine("Book deletion cancelled.");
                }
            }
            else
            {
                Console.WriteLine($"No book found with title '{title}'.");
            }
        }

        // Load books from the JSON file
        private void LoadBooksFromJson()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    books = JsonSerializer.Deserialize<List<Book>>(jsonContent) ?? new List<Book>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading books from JSON: {ex.Message}");
                    books = new List<Book>(); // Initialize with an empty list if error occurs
                }
            }
            else
            {
                Console.WriteLine("No previous book data found. Starting with an empty library.");
                books = new List<Book>(); // Start with an empty list
            }
        }

        // Save books to the JSON file
        private void SaveBooksToJson()
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonContent);
                Console.WriteLine("Books have been saved to the JSON file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving books to JSON: {ex.Message}");
            }
        }

        // Helper method to validate the book title
        private string GetValidTitle()
        {
            string title;
            while (true)
            {
                Console.WriteLine("Enter the book Title:");
                title = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(title))
                {
                    Console.WriteLine("Title cannot be empty.");
                }
                else if (!IsValidTitle(title))
                {
                    Console.WriteLine("Title must contain at least one letter or number and cannot be only numbers.");
                }
                else
                {
                    break;  
                }
            }
            return title;
        }

        // Helper method to validate the author
        private string GetValidAuthor()
        {
            string author;
            while (true)
            {
                Console.WriteLine("Enter the book Author:");
                author = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(author))
                {
                    Console.WriteLine("Author cannot be empty.");
                }
                else if (!IsValidAuthor(author))
                {
                    Console.WriteLine("Author should only contain letters and spaces.");
                }
                else
                {
                    break; 
                }
            }
            return author;
        }

 
        private int GetValidReleaseYear()
        {
            int releaseYear;
            Console.WriteLine("Enter the book Release Year (YYYY):");
            while (!int.TryParse(Console.ReadLine().Trim(), out releaseYear) || releaseYear <= 0)
            {
                Console.WriteLine("Please enter a valid year.");
            }
            return releaseYear;
        }

        private bool IsValidTitle(string input)
        {
            bool containsLetterOrDigit = false;
            foreach (var c in input)
            {
                if (char.IsLetterOrDigit(c))
                {
                    containsLetterOrDigit = true;
                    break;
                }
            }

            bool isOnlyDigits = int.TryParse(input, out _);

            return containsLetterOrDigit && !isOnlyDigits;  // Must contain a letter/number but not be all digits
        }


        private bool IsValidAuthor(string input)
        {
            foreach (var c in input)
            {
                if (!char.IsLetter(c) && c != ' ')
                {
                    return false; // Invalid if there's any character that is not a letter or space
                }
            }
            return true; 
        }
    }
}
