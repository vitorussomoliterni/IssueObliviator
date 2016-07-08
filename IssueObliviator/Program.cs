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
            var filesToIgnore = documents.FindAll(f => f.IsRevisionCodeAcceptable() == false).ToList();

            foreach (var f in filesToIgnore)
            {
                documents.Remove(f);
            }

            foreach (var doc in documents)
            {
                var sameCodeGroup = documents.FindAll(d => d.SheetNumber == doc.SheetNumber).ToList();

                sameCodeGroup.OrderBy(d => d.RevisionCode).ToList();

                while (sameCodeGroup.Count > 1)
                {
                    var codeGroupStatus = CheckCodeListContent(sameCodeGroup);
                    if (codeGroupStatus == "both")
                    {
                        // Move all numbers
                    }
                    else if (codeGroupStatus == "numbers")
                    {
                        // Get higher number and move everything else
                    }

                    else if (codeGroupStatus == "letters")
                    {
                        // Get latest file and move everything else
                    }
                }
            }

            return null;
        }

        private static string CheckCodeListContent(List<Document> documents)
        {
            var CodesWithLetters = documents.FindAll(d => d.IsRevisionCodeANumber() == false).ToList();
            var CodesWithNumbers = documents.FindAll(d => d.IsRevisionCodeANumber() == true).ToList();

            string output = "";

            if (CodesWithLetters.Count > 0 && CodesWithNumbers.Count > 0)
            {
                output = "both";
            }

            else if (CodesWithLetters.Count == 0 && CodesWithNumbers.Count > 0)
            {
                output = "numbers";
            }

            else if (CodesWithLetters.Count > 0 && CodesWithNumbers.Count == 0)
            {
                output = "letters";
            }

            return output;
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
