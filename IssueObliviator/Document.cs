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
            FileName = GetFileName(this.FullPath);
            FileType = GetFileType();
            SheetNumber = GetSheetNumber();
            RevisionCode = GetRevisionCode();
        }

        private string GetFileName(string path)
        {
            var fileNameStartIndex = path.LastIndexOf("\\") + 1;
            var fileNameEndIndex = path.LastIndexOf(".");
            var fileNameLength = fileNameEndIndex - fileNameStartIndex;
            var fileName = path.Substring(fileNameStartIndex, fileNameLength);
            return fileName;
        }

        private string GetRevisionCode()
        {
            throw new NotImplementedException();
        }

        private string GetSheetNumber()
        {
            throw new NotImplementedException();
        }

        private string GetFileType()
        {
            throw new NotImplementedException();
        }
    }
}
