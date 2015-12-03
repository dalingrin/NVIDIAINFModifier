using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPFApp
{
    public class INFPathFinder
    {
        private string _rootPath;

        public INFPathFinder(string rootDir)
        {
            _rootPath = rootDir;

            var nvdmiFiles = new DirectoryInfo(_rootPath).GetFiles("nvdmi.inf", SearchOption.AllDirectories);

            if (nvdmiFiles != null)
            {
                nvdmi = nvdmiFiles[0].FullName;
            }
        }

        public string nvdmi { get; set; }

    }
}
