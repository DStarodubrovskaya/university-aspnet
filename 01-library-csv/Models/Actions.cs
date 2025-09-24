using CsvHelper;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LibraryWebApplication.Models
{
    public class Actions
    {
        // Prints a message when a search operation fails.
        private static void PrintNotFound()
        {
            Console.WriteLine("Didn't find any book.");
        }
        // Reads data from the CSV file and inserts all records into a dictionary.
        public static Dictionary<string, Book> ReadCsv(Dictionary<string, Book> My_dict, string path)
        {
            Console.WriteLine("\n\nInserting Data to the Dictionary ...");
            // Reading csv file (insert all)
            try
            {
                // Creates an object that reads files
                using (StreamReader reader = new StreamReader(path))
                {
                    // Creates an object that use StrReader to read csv files
                    var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        string id = csv.GetField<string>("Id");
                        string title = csv.GetField<string>("Title");
                        string author = csv.GetField<string>("Author");

                        // Avoides duplicate IDs
                        if (!My_dict.ContainsKey(id))
                        {
                            My_dict.Add(id, new Book(id, title, author)); // Add book if ID is unique
                        }
                        else // Error message
                        {
                            Console.WriteLine($"Duplicate ID {id} found. Skipping entry.");
                        }
                    }
                    Console.WriteLine($"Number of objects added to the dictionary: {My_dict.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return My_dict;
        }
        // Searches for a book in the dictionary by its ID and prints the book details.
        public static bool DictSearchID(string id, Dictionary<string, Book> My_dict)
        {
            Console.WriteLine($"\n\nSearching for ID {id} in the Dictionary ...");
            if (My_dict.ContainsKey(id))
            {
                return true;
            }
            else
            {
                PrintNotFound();
                return false;
            } // In case didn't find
        }
        // Searches for books in the dictionary with titles matching a regular expression pattern.
        public static List<Book> DictSearchRgx(Regex rgx, Dictionary<string, Book> My_dict)
        {
            Console.WriteLine($"\n\nSearching in the Dictionary for titles matching the pattern: \"{rgx}\" ...");
            List<Book> matchedTitles = new List<Book>();
            foreach (var book in My_dict)
            {
                if (rgx.IsMatch(book.Value.Title))
                {
                    matchedTitles.Add(book.Value);
                }
            }
           return matchedTitles;
        }
        // Inserts a book in the dictionary.
        public static bool DictInsert(string id, string title, string author, Dictionary<string, Book> My_dict)
        {
            Console.WriteLine($"\n\nInserting the book in the Dictionary ...");
            if (!DictSearchID(id, My_dict))
            {
                My_dict.Add(id,new Book(id,title,author));
                return true;
            }
            else
            {
                Console.WriteLine("This book already exists in the library.");
                return false;
            } // In case didn't find
        }
        // Deletes a book from the dictionary by its ID.
        public static bool DictDelete(string id, Dictionary<string, Book> My_dict)
        {
            Console.WriteLine($"\n\nDeleting the ID {id} from the Dictionary ...");
            if (My_dict.ContainsKey(id))
            {
                My_dict.Remove(id);
                Console.WriteLine("The book was deleted.");
                return true;
            }
            else
            {
                PrintNotFound();
                return false;
            } // In case didn't find
        }
        // Writes all the data back to the CSV file
        public static void WriteCsv(Dictionary<string, Book> My_dict, string path) {
            Console.WriteLine("\n\nWriting Data back to csv-file ...");
            // Reading csv file (insert all)
            try
            {
                // Creates an object that reads files
                using (StreamWriter writer = new StreamWriter(path))
                {
                    // Creates an object that use StrReader to read csv files
                    var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    csv.WriteRecords(My_dict.Values);
                    
                    Console.WriteLine($"The data has been written back to 'books.csv'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
