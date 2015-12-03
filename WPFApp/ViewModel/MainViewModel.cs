using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace WPFApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public DeviceIDParser _DeviceIDParser;
        public INFPathFinder _infPathFinder;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            PopulateDevices();
        }

        #region BindableProperties
        /// <summary>
        /// The <see cref="Devices" /> property's name.
        /// </summary>
        public const string DevicesPropertyName = "Devices";

        private Dictionary<string, string> _Devices = null;

        /// <summary>
        /// Sets and gets the Devices property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Dictionary<string, string> Devices
        {
            get
            {
                return _Devices;
            }

            set
            {
                if (_Devices == value)
                {
                    return;
                }

                _Devices = value;
                RaisePropertyChanged(DevicesPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedDevice" /> property's name.
        /// </summary>
        public const string SelectedDevicePropertyName = "SelectedDevice";

        private KeyValuePair<string,string> _SelectedDevice;

        /// <summary>
        /// Sets and gets the SelectedDevice property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public KeyValuePair<string,string> SelectedDevice
        {
            get
            {
                return _SelectedDevice;
            }

            set
            {
                if (_SelectedDevice.Key == value.Key)
                {
                    return;
                }

                _SelectedDevice = value;

                _DeviceIDParser = new DeviceIDParser(_SelectedDevice.Value);
                if (!_DeviceIDParser.IsValid)
                {
                    MessageBox.Show("Please verify the selected device is an NVIDIA GPU", "Invalid device!", MessageBoxButtons.OK);
                    CanModify = false;
                }

                CanModify = isValidState();
                RaisePropertyChanged(SelectedDevicePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="FolderPath" /> property's name.
        /// </summary>
        public const string FolderPathPropertyName = "FolderPath";

        private string _FolderPath = null;

        /// <summary>
        /// Sets and gets the EXEPath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string FolderPath
        {
            get
            {
                return _FolderPath;
            }

            set
            {
                if (_FolderPath == value)
                {
                    return;
                }

                _FolderPath = value;
                CanModify = isValidState();
                RaisePropertyChanged(FolderPathPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CanModify" /> property's name.
        /// </summary>
        public const string CanModifyPropertyName = "CanModify";

        private bool _CanModify = false;

        /// <summary>
        /// Sets and gets the CanModify property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool CanModify
        {
            get
            {
                return _CanModify;
            }

            set
            {
                if (_CanModify == value)
                {
                    return;
                }

                _CanModify = value;
                RaisePropertyChanged(CanModifyPropertyName);
            }
        }
        #endregion

        #region Methods
        public void PopulateDevices()
        {
            Devices = new Dictionary<string, string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
            var test = searcher.Get();

            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["Description"] != null && queryObj["Description"].ToString().ToLower().Contains("nvidia"))
                {
                    if (!Devices.ContainsKey(queryObj["Description"].ToString()))
                        Devices.Add(queryObj["Description"].ToString(), queryObj["DeviceID"].ToString());
                }
            }
        }

        public void BrowseForFolder()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
                FolderPath = dlg.SelectedPath;
            else
                FolderPath = "";
        }

        public void ModifyINFFiles()
        {
            _infPathFinder = new INFPathFinder(FolderPath);

            try
            {
                NVIDIAINFModifier infModifier = new NVIDIAINFModifier(_infPathFinder.nvdmi, _DeviceIDParser);

                MessageBox.Show("INF File successfully modified.\n\n" + 
                    "Install the driver manually using the following file:\n" +
                    _infPathFinder.nvdmi, "Success!", MessageBoxButtons.OK);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Unable to modify inf file!");
            }
        }

        private bool isValidState()
        {
            if (FolderPath != null && Directory.Exists(FolderPath) && _DeviceIDParser.IsValid)
            {
                return SelectedDevice.Key != null;
            }

            return false;
        }
        #endregion
    }
}