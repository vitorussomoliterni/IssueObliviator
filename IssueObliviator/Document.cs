using System;

namespace IssueObliviator
{
    public class Document
    {
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string SheetNumber { get; set; }
        public string RevisionCode { get; set; }

        public Document(string file)
        {
            FullPath = file;
            FileName = GetFileName(FullPath);
            FileType = GetFileType(FullPath);
            SheetNumber = GetSheetNumber(FileName);
            RevisionCode = GetRevisionCode(FileName);
        }

        private string GetFileName(string path) // Extracts file name from the full path
        {
            var fileNameStartIndex = path.LastIndexOf("\\") + 1;
            var fileName = path.Substring(fileNameStartIndex);
            return fileName;
        }

        private string GetRevisionCode(string fileName) // Extracts revision code from the file name
        {
            try
            {
                var countUnderscores = fileName.Split('_').Length - 1;
                if (countUnderscores < 4)
                {
                    return null;
                }
                var partialName = fileName.Substring(fileName.LastIndexOf("_") + 1);
                var lastIndexReviosionCode = partialName.LastIndexOf(".");
                var revisionCode = partialName.Substring(0, lastIndexReviosionCode);
                return revisionCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private string GetSheetNumber(string fileName) // Extracts sheet number from the file name
        {
            try
            {
                var partialName = fileName.Substring(fileName.IndexOf("_") + 1);
                var lastIndexSheetNumber = partialName.LastIndexOf("_");
                var sheetNumber = partialName.Substring(0, lastIndexSheetNumber);
                return sheetNumber;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return string.Empty;
        }

        private string GetFileType(string path) // Extracts file extension from the full path
        {
            var extensionStartIndex = path.LastIndexOf(".") + 1;
            var fileType = path.Substring(extensionStartIndex);

            return fileType;
        }

        public bool IsRevisionCodeANumber() // Checks if revision code is a number
        {
            int i;
            var code = this.RevisionCode;
            if (int.TryParse(code, out i))
            {
                return true;
            }
            return false;
        }

        public bool IsRevisionCodeAcceptable() // Checks if revision code format complies to standard
        {
            var code = this.RevisionCode;
            if (code == null)
            {
                return false;
            }
            else if (code.Length == 1 || IsRevisionCodeANumber())
            {
                return true;
            }
            for (int i = 1; i < code.Length; i++)
            {
                if (!code[i].Equals(code[i - 1]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
