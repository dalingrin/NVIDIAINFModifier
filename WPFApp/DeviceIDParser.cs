using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFApp
{
    public class DeviceIDParser
    {
        private string rawID;

        public DeviceIDParser(string deviceID)
        {
            rawID = deviceID;
            Parse();
        }

        private void Parse()
        {
            string tmp = rawID.ToLower();
            tmp = tmp.Replace("pci\\", "");

            var strings = tmp.Split('&');

            foreach (var substring in strings)
            {
                if (substring.StartsWith("ven_"))
                    Vendor = substring.Substring(4);
                else if (substring.StartsWith("dev_"))
                    Device = substring.Substring(4);
                else if (substring.StartsWith("subsys_"))
                    SubSys = substring.Substring(7);
            }
        }

        public string Vendor { get; set; }
        public string Device { get; set; }
        public string SubSys { get; set; }

        public string SubSys1
        {
            get
            {
                return SubSys.Substring(0, 4);
            }
        }
        public string SubSys2
        {
            get
            {
                return SubSys.Substring(4, 4);
            }
        }

        public bool IsValid
        {
            get
            {
                return (Vendor != null && Device != null && SubSys != null);
            }
        }

    }
}
