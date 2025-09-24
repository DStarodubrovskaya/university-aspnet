using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryWebApplication.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace LibraryWebApplication.Pages
{
    public class LibraryManagementModel : PageModel
    {
        // Cash injection
        private readonly IMemoryCache _cache;
        public LibraryManagementModel(IMemoryCache cache)
        {
            _cache = cache;
        }
        // key used to store/retrieve the library from cache
        public const string cashKey = "MyLibrary";

        // form fields for binding user input
        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Author { get; set; }
        [BindProperty]
        public string Identifier { get; set; }

        public Dictionary<string, Book> My_dict { get; set; }

        // used to show results of form actions
        [TempData]
        public string FormResult { get; set; }
        [BindProperty]
        public List<string> TitlesMatchedStr { get; set; }

        public void OnGet()
        {
            // pulls out a dictionary from the cash (if it exists there)
            My_dict = _cache.Get<Dictionary<string, Book>>(cashKey);

            if (My_dict == null)
            {
                My_dict = new Dictionary<string, Book>();
                My_dict = Actions.ReadCsv(My_dict, "books.csv");
                _cache.Set(cashKey, My_dict);
            } // Populates the dictionary if it haven't been read yet.

            if (_cache.TryGetValue("Titles", out List<string> list))
            {
                TitlesMatchedStr = list;
                _cache.Remove("Titles"); // cleans up after reading
            }
            else { TitlesMatchedStr = new List<string>(); }
        }
        // validates if input ID contains only digits
        public bool IDValidation(string id)
        {
            Regex validateNumberRegex = new Regex("^\\d+$");
            if (validateNumberRegex.IsMatch(id)) 
            { 
                return true; 
            } else
            {
                return false;
            }
        } 
       
        public IActionResult OnPostSearch()
        {
            // input validation
            if (Identifier == null)
            {
                FormResult = $"Error: Wrong input.";
                return RedirectToPage();
            }
                // pulls out a dictionary from the cash (if it exists there)
                My_dict = _cache.Get<Dictionary<string, Book>>(cashKey);

            if (IDValidation(Identifier)) // check if the input was Id or Title
            {
                bool status = Actions.DictSearchID(Identifier, My_dict);
                if (status)
                {
                    FormResult = My_dict[Identifier].ToString();
                }
                else
                {
                    FormResult = $"Action failed: Didn't find any book with this ID {Identifier}.";
                }
            } // searching by ID
            else
            {
                Regex titlePattern = new Regex($@"\b{Identifier}\b", RegexOptions.IgnoreCase);
                List<Book> titlesMatched = Actions.DictSearchRgx(titlePattern, My_dict);

                if (titlesMatched.Count > 0)
                {
                    foreach (Book title in titlesMatched)
                    {
                        TitlesMatchedStr.Add(title.ToString());
                    }
                    _cache.Set("Titles", TitlesMatchedStr);
                    FormResult = $"Here are the books that contain the entered text in their title:";
                }
                else
                {
                    FormResult = $"Action failed: Didn't find any book with this title.";
                }
            } // searching by title using pattern match (case-insensitive)
            return RedirectToPage();
        }
        public IActionResult OnPostInsert()
        {
            // input validation
            if (!IDValidation(Id) || string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Author))
            {
                FormResult = $"Error: Wrong input.";
                return RedirectToPage(); 
            }

            // pulls out a dictionary from the cash (if it exists there)
            My_dict = _cache.Get<Dictionary<string, Book>>(cashKey);

            bool status = Actions.DictInsert(Id, Title, Author, My_dict);
            if (status)
            {
                Actions.WriteCsv(My_dict, "books.csv");
                _cache.Set(cashKey, My_dict);
                FormResult = "The book has been added sucessfully.";

            } // rewrites csv file and cash
            else
            {
                FormResult = $"Action failed: Book with the same ID {Id} already exists in the library.";
            }
            return RedirectToPage();

        }
        public IActionResult OnPostDelete()
        {
            // input validation
            if (!IDValidation(Id)) 
            {
                FormResult = $"Error: Wrong input.";
                return RedirectToPage(); 
            }

            // pulls out a dictionary from the cash (if it exists there)
            My_dict = _cache.Get<Dictionary<string, Book>>(cashKey);

            bool status = Actions.DictDelete(Id, My_dict);

            if (status)
            {
                Actions.WriteCsv(My_dict, "books.csv");
                _cache.Set(cashKey, My_dict);
                FormResult = "The book has been deleted sucessfully.";

            } // rewrites csv file and cash
            else
            {
                FormResult = $"Action failed: Didn't find any book with this ID {Id}.";
            }
            return RedirectToPage();
        }
    }
}
