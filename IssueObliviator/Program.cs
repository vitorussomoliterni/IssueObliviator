using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueObliviator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var files = GetFiles();
        }

        private static List<Document> GetFiles()
        {
            var files = from file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*", SearchOption.TopDirectoryOnly) // Gets a list of all files
                        select new
                        {
                            File = file
                        };

            var documents = new List<Document>();

            foreach (var f in files)
            {
                var file = f.File.ToLower().Trim();
                if (file.EndsWith("pdf") || file.EndsWith("dwg")) // Checks if files are pdf or dwg
                {
                    var document = new Document(f.File);
                    documents.Add(document); // Adds pdfs and dwgs to list
                }
            }
            return documents;
        }
    }
}
