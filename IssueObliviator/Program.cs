using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace IssueObliviator
{
    public class Program
    {
        static string destinationFolder = @"SS\";

        static void Main(string[] args)
        {
            var documents = GetDocuments();
            MoveOlderFiles(documents);
            Console.ReadKey();
        }

        private static void MoveOlderFiles(List<Document> documents)
        {
            var oldFiles = GetOldFilesList(documents).Distinct();

            foreach (var f in oldFiles)
            {
                Console.WriteLine(f.FileName);
            }

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            try
            {
                foreach (var f in oldFiles)
                {
                    var sourceFile = f.FullPath;
                    var destinationFile = Directory.GetCurrentDirectory() + "\\" + destinationFolder + f.FileName;
                    if (File.Exists(destinationFile))
                    {
                        destinationFile += "_copy";
                    }
                    Console.WriteLine(destinationFile);
                    Directory.Move(sourceFile, destinationFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static List<Document> GetOldFilesList(List<Document> documents)
        {
            var filesToMove = new List<Document>();

            var filesToIgnore = documents.FindAll(f => f.IsRevisionCodeAcceptable() == false).ToList();
            
            documents = RemoveFiles(documents, filesToIgnore);

            foreach (var doc in documents)
            {
                var sameCodeGroup = documents.FindAll(d => d.SheetNumber == doc.SheetNumber).ToList();

                sameCodeGroup.OrderBy(d => d.RevisionCode).ToList();

                while (true)
                {
                    var codeGroupStatus = CheckCodeListContent(sameCodeGroup);
                    if (codeGroupStatus == "both")
                    {
                        var numberCodes = sameCodeGroup.FindAll(f => f.IsRevisionCodeANumber()).ToList(); // Selects all the files with numbers as code
                        filesToMove.AddRange(numberCodes); // Adds all the files with numbers as code to the list of the files to move
                        sameCodeGroup = RemoveFiles(sameCodeGroup, numberCodes); // Remove all the files with numbers as code from the current list of files to check
                    }
                    else if (codeGroupStatus == "numbers")
                    {
                        sameCodeGroup = OrderNumbers(sameCodeGroup);
                        sameCodeGroup.Remove(sameCodeGroup.Last());
                        filesToMove.AddRange(sameCodeGroup);
                        break;
                    }

                    else if (codeGroupStatus == "letters")
                    {
                        sameCodeGroup = OrderLetters(sameCodeGroup);
                        sameCodeGroup.Remove(sameCodeGroup.Last());
                        filesToMove.AddRange(sameCodeGroup);
                        break;
                    }
                }
            }

            return filesToMove;
        }

        private static List<Document> OrderNumbers(List<Document> list) // Orders numbers and puts the most recent one at the end
        {
            var lastElement = list.Last();

            foreach (var f in list)
            {
                if (int.Parse(f.RevisionCode) >= int.Parse(lastElement.RevisionCode))
                {
                    lastElement = f;
                }
            }

            list.Remove(lastElement);
            list.Add(lastElement);

            return list;
        }

        private static List<Document> OrderLetters(List<Document> list) // Orders letters and puts the most recent one at the end
        {
            var lastElement = list.Last();

            foreach (var f in list)
            {
                if (f.RevisionCode.Length >= lastElement.RevisionCode.Length)
                {
                    lastElement = f;
                }
            }

            list.Remove(lastElement);
            list.Add(lastElement);

            return list;
        }

        private static List<Document> RemoveFiles(List<Document> files, List<Document> filesToRemove)
        {
            foreach (var f in filesToRemove)
            {
                files.Remove(f);
            }

            return files;
        }

        private static string CheckCodeListContent(List<Document> documents) // Analyses content of a code group
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
