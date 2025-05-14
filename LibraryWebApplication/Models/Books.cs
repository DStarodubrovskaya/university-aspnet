using CsvHelper;
using System.Globalization;
namespace LibraryWebApplication.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Book(string id, string title, string author)
        {
            Id = id;
            Title = title;
            Author = author;
        }
        public override string ToString()
        {
            string info = "ID: " + Id + "   -   '" + Title + "' by " + Author + ".";
            return info;
        }// Overriding ToString to display book information
    }
}
