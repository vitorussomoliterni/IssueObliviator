using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            SheetNumber = GetSheetNumber();
            RevisionCode = GetRevisionCode();
        }

        private string GetFileName(string path) // Extracts file name from the full path
        {
            var fileNameStartIndex = path.LastIndexOf("\\") + 1;
            var fileNameEndIndex = path.LastIndexOf(".");
            var fileNameLength = fileNameEndIndex - fileNameStartIndex;
            var fileName = path.Substring(fileNameStartIndex, fileNameLength);
            return fileName;
        }

        private string GetRevisionCode() // Extracts revision code from the file name
        {
            throw new NotImplementedException();
        }

        private string GetSheetNumber() // Extracts sheet number from the file name
        {
            throw new NotImplementedException();
        }

        private string GetFileType(string path) // Extracts file extension from the full path
        {
            var extensionStartIndex = path.LastIndexOf(".") + 1;
            var fileType = path.Substring(extensionStartIndex);

            return fileType;
        }
    }
}
