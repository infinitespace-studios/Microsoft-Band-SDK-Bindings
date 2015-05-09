using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Band.Portable.Personalization;

using Xunit;

namespace Microsoft.Band.Portable.Tests.PersonalizationTests
{
    public class BandColorTests
    {
        [Fact]
        public void NewBandColorIsEmpty()
        {
            // setup
            var color = new BandColor();

            // test
            Assert.True(color.IsEmpty);
        }

        [Fact]
        public void NewBandColorEqualsEmpty()
        {
            // setup
            var color = new BandColor();

            // test
            Assert.Equal(BandColor.Empty, color);
            Assert.True(color.Equals(BandColor.Empty));
            Assert.True(BandColor.Empty == color);
            Assert.False(BandColor.Empty != color);
        }

        [Theory]
        [InlineData(000, 000, 000)] // empty
        [InlineData(127, 000, 000)] // 1 color
        [InlineData(000, 127, 000)] // 1 color
        [InlineData(000, 000, 127)] // 1 color
        [InlineData(127, 127, 000)] // 2 colors
        [InlineData(000, 127, 127)] // 2 colors
        [InlineData(127, 000, 127)] // 2 colors
        [InlineData(127, 127, 127)] // 3 colors
        [InlineData(255, 255, 255)] // full
        public void NewBandColorSetsProperties(byte r, byte g, byte b)
        {
            // setup
            var color = new BandColor(r, g, b);

            // test
            Assert.Equal(r, color.R);
            Assert.Equal(g, color.G);
            Assert.Equal(b, color.B);
        }

        [Theory]
        [InlineData(000, 000, 000, "#000000")] // empty
        [InlineData(127, 000, 000, "#7F0000")] // 1 color
        [InlineData(000, 127, 000, "#007F00")] // 1 color
        [InlineData(000, 000, 127, "#00007F")] // 1 color
        [InlineData(127, 127, 000, "#7F7F00")] // 2 colors
        [InlineData(000, 127, 127, "#007F7F")] // 2 colors
        [InlineData(127, 000, 127, "#7F007F")] // 2 colors
        [InlineData(127, 127, 127, "#7F7F7F")] // 3 colors
        [InlineData(255, 255, 255, "#FFFFFF")] // full
        public void BandColorConvertsToHexCorrectly(byte r, byte g, byte b, string hex)
        {
            // setup
            var color = new BandColor(r, g, b);

            // test
            Assert.Equal(hex, color.Hex);
        }

        [Theory]
        [InlineData(000, 000, 000, "#000000")] // empty
        [InlineData(127, 000, 000, "#7F0000")] // 1 color
        [InlineData(000, 127, 000, "#007F00")] // 1 color
        [InlineData(000, 000, 127, "#00007F")] // 1 color
        [InlineData(127, 127, 000, "#7F7F00")] // 2 colors
        [InlineData(000, 127, 127, "#007F7F")] // 2 colors
        [InlineData(127, 000, 127, "#7F007F")] // 2 colors
        [InlineData(127, 127, 127, "#7F7F7F")] // 3 colors
        [InlineData(255, 255, 255, "#FFFFFF")] // full
        public void BandColorCreatesFromHexCorrectly(byte r, byte g, byte b, string hex)
        {
            // setup
            var color = BandColor.FromHex(hex);

            // test
            Assert.Equal(r, color.R);
            Assert.Equal(g, color.G);
            Assert.Equal(b, color.B);
        }
    }
}
