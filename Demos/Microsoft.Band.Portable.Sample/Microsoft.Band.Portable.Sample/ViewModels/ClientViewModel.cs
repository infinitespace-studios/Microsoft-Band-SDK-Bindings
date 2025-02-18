﻿using System;
using System.Threading.Tasks;

using EdSnider.Plugins;

namespace Microsoft.Band.Portable.Sample.ViewModels
{
    public class ClientViewModel : BaseClientViewModel
    {
        private bool isConnecting;

        public ClientViewModel(BandDeviceInfo info)
            : base(info, null)
        {
        }
        
        public string FirmwareVersion { get; private set; }

        public string HardwareVersion { get; private set; }

        public override async Task Prepare()
        {
            if (BandClient == null)
            {
                BandClient = await BandClientManager.Instance.ConnectAsync(BandInfo);

                BandClient.TileManager.TileButtonPressed += (sender, e) =>
                {
                    Notifier.Current.Show(
                        "Tile Button Pressed",
                        string.Format("Button [{0}] pressed on page [{1}] of tile [{2}].", e.ElementId, e.PageId, e.TileId));
                };
                BandClient.TileManager.TileOpened += (sender, e) =>
                {
                    Notifier.Current.Show("Tile Opened", string.Format("Tile [{0}] opened.", e.TileId));
                };
                BandClient.TileManager.TileClosed += (sender, e) =>
                {
                    Notifier.Current.Show("Tile Closed", string.Format("Tile [{0}] closed.", e.TileId));
                };
                await BandClient.TileManager.StartEventListenersAsync();
            }

            await base.Prepare();

            FirmwareVersion = await BandClient.GetFirmwareVersionAsync();
            OnPropertyChanged("FirmwareVersion");

            HardwareVersion = await BandClient.GetHardwareVersionAsync();
            OnPropertyChanged("HardwareVersion");
        }

        public override async Task Destroy()
        {
            await base.Destroy();

            await BandClient.TileManager.StopEventListenersAsync();
            await BandClient.DisconnectAsync();
        }
    }
}
