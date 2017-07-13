        public void ScanDevice()
        {
            File.Delete("scan_temp.jpg");
            DeviceManager DevManager = new DeviceManager();

            Device D;
            for (int i = 1; i <= DevManager.DeviceInfos.Count; i++)
            {
                if (DevManager.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
                {
                    var DeviceName = DevManager.DeviceInfos[i].Connect();
                    D = DeviceName;

                    dynamic item = D.Items[1];
                    int DPI = 150;

                    item.Properties["6147"].Value = DPI; // dpi X
                    item.Properties["6148"].Value = DPI; // dpi Y
                    ImageFile image = (ImageFile)item.Transfer("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}"); // This mean Jpg
                    image.SaveFile("scan_temp.jpg");
                }
            }
        }
