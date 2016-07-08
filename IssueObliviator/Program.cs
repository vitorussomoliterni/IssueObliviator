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
            var documents = GetDocuments();
            MoveOlderFiles(documents);
        }

        private static void MoveOlderFiles(List<Document> documents)
        {
            var oldFiles = GetOldFilesList(documents);
        }

        private static List<Document> GetOldFilesList(List<Document> documents)
        {
            foreach (var doc in documents)
            {
                var sameSheet = documents.FindAll(d => d.SheetNumber == doc.SheetNumber).ToList();
                sameSheet.OrderBy(d => d.RevisionCode).ToList();

                while(sameSheet.Count > 1)
                {
                    foreach (var s in sameSheet)
                    {
                        Document t = sameSheet.FirstOrDefault();
                        var currentCode = s.RevisionCode;
                        if (s.IsRevisionCodeANumber())
                        {

                        }
                        var previousCode = t.RevisionCode;
                        if (currentCode.Length < previousCode.Length)
                        {
                            sameSheet.Remove(s);
                        }
                    }
                }
            }

            return null;
        }

        private static List<Document> GetDocuments() // Returns a list of .pdf and .dwg documents in current folder
        {
            var files = from file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*", SearchOption.TopDirectoryOnly) // Gets a list of all files
                        select new
                        {
                            Path = file
                        };

            var documents = new List<Document>();

            foreach (var f in files)
            {
                var file = f.Path.ToLower().Trim();
                if (file.EndsWith("pdf") || file.EndsWith("dwg")) // Checks if files are pdf or dwg
                {
                    var document = new Document(f.Path);
                    documents.Add(document); // Adds pdfs and dwgs to list
                }
            }
            return documents;
        }
    }
}
