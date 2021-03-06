﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace BpOmniaBridgeTest
{
    class TestHelper
    {

        public string TempFileFolder()
        {
            var cmnDocPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            return Path.Combine(cmnDocPath, "BpOmniaBridge", "temp_files");
        }

        public string BpOmniaFolder()
        {
            var cmnDocPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            return Path.Combine(cmnDocPath, "BpOmniaBridge");
        }

        public string PdfFileFolder()
        {
            var cmnDocPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            return Path.Combine(cmnDocPath, "BpOmniaBridge", "pdf_files");
        }

        public string TestProjectFolder()
        {
            List<string> folderArray = new List<string>(Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar));
            var projectFolderIndex = folderArray.IndexOf("BpOmniaBridge");
            var projectFolder = folderArray.GetRange(0, projectFolderIndex + 1);
            projectFolder.Add("BpOmniaBridgeTest");

            return String.Join(@"\", projectFolder);
        }

        public string CopyFileToTest(string fileFrom, string fileTo = "same", string ext = ".out", string destFolder = "tempFileFolder")
        {
            if (fileTo == "same")
                fileTo = fileFrom;
 
            string fileToMove = Path.Combine(TestProjectFolder(), "toTest", fileFrom + ext);
            if (destFolder == "tempFileFolder")
            {
                destFolder = TempFileFolder();
            }

            var destFileName = Path.Combine(destFolder, fileTo + ext); 

            File.Copy(fileToMove, destFileName, true);

            return destFileName;
        }

        public void DeleteTempFileIN(string fileName, string ext = ".in")
        {
            var filePath = Path.Combine(TempFileFolder(), fileName + ext);
            File.Delete(filePath);
        }

        public void SetAppConfigFlag(string key, string value)
        {
            var currentFolder = Directory.GetCurrentDirectory().Replace("\\BpOmniaBridgeTest\\bin\\Debug", "");
            string filePath = Path.Combine(currentFolder, "App.config");
            XDocument xml = XDocument.Load(filePath);
            IEnumerable<XElement> elements = xml.Elements("configuration").Elements("appSettings").Elements();
            foreach(XElement element in elements)
            {
                if(element.FirstAttribute.Value == key)
                {
                    element.SetAttributeValue("value", value);
                }
            }
            xml.Save(filePath);
            // refresh appSettings
            BpOmniaBridge.Utility.RefreshConfig();
        }

        public bool CheckFile(string file_name)
        {
            var file_path = Path.Combine(TempFileFolder(), file_name + ".in");

            return File.Exists(file_path);
        }


    }
}
