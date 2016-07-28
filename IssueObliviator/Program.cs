using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IssueObliviator
{
    public class Program
    {
        static string _destinationFolder = @"PREVIOUSLY ISSUED - KEEP\";
        static List<Document> _lockedDocuments = new List<Document>();

        static void Main(string[] args)
        {
            var pdfDocuments = GetDocuments("*.pdf");
            var dwgDocuments = GetDocuments("*.dwg");
            MoveOlderFiles(pdfDocuments);
            MoveOlderFiles(dwgDocuments);
            ShowErrorMessage(_lockedDocuments);
        }

        private static void ShowErrorMessage(List<Document> lockedDocuments)
        {
            var logMessage = "Try to close these files if open and then run the program again:\n\n";
            if (lockedDocuments.Count > 0)
            {
                foreach (var s in lockedDocuments)
                {
                    logMessage += s.FileName + "\n";
                }
                MessageBox.Show(logMessage, "IssueObliviator Error: some files could not be moved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log(logMessage);
            }
        }

        private static void MoveOlderFiles(List<Document> documents)
        {
            var oldFiles = GetOldFilesList(documents).Distinct();

            if (!Directory.Exists(_destinationFolder))
            {
                Directory.CreateDirectory(_destinationFolder);
            }

            try
            {
                foreach (var f in oldFiles)
                {
                    var sourceFile = f.FullPath;
                    var destinationFile = Directory.GetCurrentDirectory() + @"\" + _destinationFolder + f.FileName + "." + f.FileType;

                    if (IsFileLocked(f))
                    {
                        _lockedDocuments.Add(f);
                    }
                    else
                    {
                        if (File.Exists(destinationFile))
                        {
                            destinationFile = RenameExistingDestinationFile(f);
                        }
                        Log("Attempting to copy this file: " + sourceFile + "\nTo this destination path: " + destinationFile);
                        Directory.Move(sourceFile, destinationFile);
                    }
                }
            }
            catch (Exception e)
            {
                var error = "Error while moving files:\n" +
                    e.GetType().Name + "\n" +
                    e.Message + "\n" +
                    e.ToString();
                Log(error);
                MessageBox.Show(error, "Error");
            }
        }

        private static bool IsFileLocked(Document f)
        {
            var file = new FileInfo(f.FullPath);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (Exception e)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }

        private static string RenameExistingDestinationFile(Document document)
        {
            var currentTime = string.Format("{0:yyyy-MM-dd_hh-mm}", DateTime.Now);
            var newDirectory = Directory.GetCurrentDirectory() + @"\" + _destinationFolder + "Files already supereseeded (" + currentTime + ")\\";
            var logMessage = "This file already existed in the " + _destinationFolder + " folder and will be moved instead to " + newDirectory;
            MessageBox.Show(logMessage, "IssueObliviator Error: some files were already copied previously", MessageBoxButtons.OK);
            if(!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }
            var version = 1;
            var textToAdd = " - copy(" + version + ").";
            var file = newDirectory + document.FileName + textToAdd + document.FileType; // Renames the file

            while (File.Exists(file))
            {
                version++;
                textToAdd = " - copy(" + version + ")."; // Increments the new copy version number
                file = newDirectory + document.FileName + textToAdd + document.FileType; // Renames the file with the new copy version
            }
            
            return file;
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

            string output = string.Empty;

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

        private static List<Document> GetDocuments(string searchOption) // Returns a list of .pdf and .dwg documents in current folder
        {
            var files = from file in Directory.GetFiles(Directory.GetCurrentDirectory(), searchOption, SearchOption.TopDirectoryOnly) // Gets a list of all files
                        select new
                        {
                            Path = file
                        };

            var documents = new List<Document>();

            foreach (var f in files)
            {
                var file = f.Path.ToLower().Trim();
                var document = new Document(f.Path);
                documents.Add(document);
            }
            return documents;
        }

        private static void Log(string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(@"IssueObliviatorErrorLog.txt"))
                {
                    w.WriteLine(logMessage);
                    w.WriteLine("Error logged on {0} at {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                    w.WriteLine("-------------------------------");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "IssueObliviator: Error while logging", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
