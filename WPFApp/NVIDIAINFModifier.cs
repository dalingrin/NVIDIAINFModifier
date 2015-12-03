using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WPFApp
{
    public class NVIDIAINFModifier
    {
        private string _path;

        public NVIDIAINFModifier(string path, DeviceIDParser idParser)
        {
            _path = path;

            string[] nvidiaINF = File.ReadAllLines(_path);

            string origNameLine = nvidiaINF.Where(l => l.Contains("GeForce GPU")).FirstOrDefault();

            string origID = origNameLine.Split('=')[0].TrimEnd(' ');

            string origDeviceSectionLine = nvidiaINF.Where(l => l.Contains("%" + origID)).FirstOrDefault();

            string nameLine = "NVIDIA_DEV." + idParser.Device + "." + idParser.SubSys1 + "." + idParser.SubSys2 +
                " = \"NVIDIA GeForce GPU\"";

            string deviceSectionLine = "%NVIDIA_DEV." + idParser.Device + "." + idParser.SubSys1 + "." + idParser.SubSys2 +
                "% = Section093, PCI\\VEN_" + idParser.Vendor + "&DEV_" + idParser.Device + "&SUBSYS_" + idParser.SubSys;

            nvidiaINF = nvidiaINF.Select(l => l.Replace(origNameLine, nameLine)).ToArray();
            nvidiaINF = nvidiaINF.Select(l => l.Replace(origDeviceSectionLine, deviceSectionLine)).ToArray();

            File.WriteAllLines(path, nvidiaINF);
        }
    }
}
