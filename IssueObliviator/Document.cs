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
        public string Name { get; set; }
        public string FileType { get; set; }
        public string SheetNumber { get; set; }
        public string RevisionNumber { get; set; }

        public Document(string file)
        {
            FullPath = "";
            Name = file;
            FileType = GetFileType();
            SheetNumber = GetSheetNumber();
            RevisionNumber = GetRevisionNumber();
        }

        private string GetRevisionNumber()
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
