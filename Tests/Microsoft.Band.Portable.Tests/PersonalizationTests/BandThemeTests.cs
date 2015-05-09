using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Band.Portable.Personalization;

using Xunit;

namespace Microsoft.Band.Portable.Tests.PersonalizationTests
{
    public class BandThemeTests
    {
        [Fact]
        public void NewBandThemeIsEmpty()
        {
            // setup
            var theme = new BandTheme();

            // test
            Assert.True(theme.IsEmpty);
        }

        [Fact]
        public void NewBandThemeEqualsEmpty()
        {
            // setup
            var theme = new BandTheme();

            // test
            Assert.Equal(BandTheme.Empty, theme);
            Assert.True(theme.Equals(BandTheme.Empty));
            Assert.True(BandTheme.Empty == theme);
            Assert.False(BandTheme.Empty != theme);
        }
    }
}
