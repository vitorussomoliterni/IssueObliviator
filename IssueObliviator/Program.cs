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

        private static List<string> GetFiles()
        {
            var files = from file in Directory.EnumerateFiles("", "*.*", SearchOption.TopDirectoryOnly) // Gets a list of all files
                        select new
                        {
                            File = file
                        };

            List<string> fileList = new List<string>();

            foreach (var f in files)
            {
                if (f.File.ToLower().Trim().EndsWith("pdf") || f.File.ToLower().Trim().EndsWith("dwg")) // Checks if files are pdf or dwg
                {
                    fileList.Add(f.File); // Adds pdfs and dwgs to list
                }
            }
            return fileList;
        }
    }
}
