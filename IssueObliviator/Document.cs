﻿using System;
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
            SheetNumber = GetSheetNumber(FileName);
            RevisionCode = GetRevisionCode(FileName);
        }

        private string GetFileName(string path) // Extracts file name from the full path
        {
            var fileNameStartIndex = path.LastIndexOf("\\") + 1;
            var fileNameEndIndex = path.LastIndexOf(".");
            var fileNameLength = fileNameEndIndex - fileNameStartIndex;
            var fileName = path.Substring(fileNameStartIndex, fileNameLength);
            return fileName;
        }

        private string GetRevisionCode(string fileName) // Extracts revision code from the file name
        {
            try
            {
                var partialName = fileName.Substring(fileName.IndexOf("_") + 1);
                var revisionCode = partialName.Substring(partialName.IndexOf("_") + 1);
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
                var sheetNumber = partialName.Substring(0, 5);
                return sheetNumber;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private string GetFileType(string path) // Extracts file extension from the full path
        {
            var extensionStartIndex = path.LastIndexOf(".") + 1;
            var fileType = path.Substring(extensionStartIndex);

            return fileType;
        }

        public bool IsRevisionCodeANumber()
        {
            int i;
            if (int.TryParse(this.RevisionCode, out i))
            {
                return true;
            }
            return false;
        }

        public bool IsRevisionCodeAcceptable()
        {

            return true;
        }
    }
}
