using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Band.Portable.Personalization;
using Microsoft.Band.Portable.Tiles;

using Xunit;

namespace Microsoft.Band.Portable.Tests.TilesTests
{
    public class BandTileTests
    {
        [Fact]
        public void NewBandTileHasListInstances()
        {
            // setup
            var tile = new BandTile(Guid.NewGuid());

            // test
            Assert.NotNull(tile.PageImages);
            Assert.NotNull(tile.PageLayouts);
        }

        [Fact]
        public void NewBandTileSetsPropertiesWithId()
        {
            // values
            var guid = Guid.NewGuid();

            // setup
            var tile = new BandTile(guid);

            // test
            Assert.Equal(guid, tile.Id);
        }

        [Fact]
        public void NewBandTileSetsPropertiesWithIdNameIcon()
        {
            // values
            var guid = Guid.NewGuid();
            var name = "Name";
            var icon = new BandImage();

            // setup
            var tile = new BandTile(guid, name, icon);

            // test
            Assert.Equal(guid, tile.Id);
            Assert.Equal(name, tile.Name);
            Assert.Equal(icon, tile.Icon);
        }

        [Fact]
        public void NewBandTileSetsPropertiesWithIdNameIconBadge()
        {
            // values
            var guid = Guid.NewGuid();
            var name = "Name";
            var icon = new BandImage();
            var badge = new BandImage();

            // setup
            var tile = new BandTile(guid, name, icon, badge);

            // test
            Assert.Equal(guid, tile.Id);
            Assert.Equal(name, tile.Name);
            Assert.Equal(icon, tile.Icon);
            Assert.Equal(badge, tile.SmallIcon);
        }
    }
}
