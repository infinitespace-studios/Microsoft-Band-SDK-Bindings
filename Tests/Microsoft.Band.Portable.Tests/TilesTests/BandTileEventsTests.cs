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
    public class BandTileEventsTests
    {
        [Fact]
        public void NewBandTileOpenedEventArgsSetsProperties()
        {
            // values
            var tileId = Guid.NewGuid();

            // setup
            var args = new BandTileOpenedEventArgs(tileId);

            // test
            Assert.Equal(tileId, args.TileId);
            Assert.Equal(TileActionType.TileOpened, args.ActionType);
        }

        [Fact]
        public void NewBandTileClosedEventArgsSetsProperties()
        {
            // values
            var tileId = Guid.NewGuid();

            // setup
            var args = new BandTileClosedEventArgs(tileId);

            // test
            Assert.Equal(tileId, args.TileId);
            Assert.Equal(TileActionType.TileClosed, args.ActionType);
        }

        [Fact]
        public void NewBandTileButtonPressedEventArgsSetsProperties()
        {
            // values
            var tileId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var buttonId = 12;

            // setup
            var args = new BandTileButtonPressedEventArgs(buttonId, pageId, tileId);

            // test
            Assert.Equal(tileId, args.TileId);
            Assert.Equal(pageId, args.PageId);
            Assert.Equal(buttonId, args.ElementId);
            Assert.Equal(TileActionType.ButtonPressed, args.ActionType);
        }
    }
}
