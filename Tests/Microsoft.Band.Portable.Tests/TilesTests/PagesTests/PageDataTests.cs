using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Band.Portable.Personalization;
using Microsoft.Band.Portable.Tiles;
using Microsoft.Band.Portable.Tiles.Pages;
using Microsoft.Band.Portable.Tiles.Pages.Data;

using KellermanSoftware.CompareNetObjects;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.Band.Portable.Tests.TilesTests.PagesTests
{
    public class PageDataTests
    {
        public static IEnumerable<object[]> GetSerializationData()
        {
            var pageId = Guid.Parse("e78bed32-7b77-40ca-8991-61f6309dd923");

            yield return new object[] 
            { 
                "<PageData />",
                new PageData(),
            };
            yield return new object[] 
            { 
                "<PageData PageId=\"e78bed32-7b77-40ca-8991-61f6309dd923\" PageLayoutIndex=\"1\" />",
                new PageData { PageId = pageId, PageLayoutIndex = 1 },
            };
            yield return new object[] 
            { 
                "<PageData><BarcodeData /></PageData>",
                new PageData { Data = { new BarcodeData() } },
            };
            yield return new object[] 
            { 
                "<PageData><BarcodeData BarcodeType=\"Pdf417\" BarcodeValue=\"value\" ElementId=\"1\" /></PageData>",
                new PageData { Data = { new BarcodeData() { BarcodeType = BarcodeType.Pdf417, BarcodeValue = "value", ElementId = 1 } } },
            };
            yield return new object[] 
            { 
                "<PageData><FilledButtonData /></PageData>",
                new PageData { Data = { new FilledButtonData() } },
            };
            yield return new object[] 
            { 
                "<PageData><FilledButtonData PressedColor=\"#0000FF\" ElementId=\"1\" /></PageData>",
                new PageData { Data = { new FilledButtonData() { PressedColor = new BandColor(0, 0, 255), ElementId = 1 } } },
            };
            yield return new object[] 
            { 
                "<PageData><ImageData /></PageData>",
                new PageData { Data = { new ImageData() } },
            };
            yield return new object[] 
            { 
                "<PageData><ImageData ImageIndex=\"1\" ElementId=\"1\" /></PageData>",
                new PageData { Data = { new ImageData() { ImageIndex = 1, ElementId = 1 } } },
            };
            yield return new object[] 
            { 
                "<PageData><TextBlockData /></PageData>",
                new PageData { Data = { new TextBlockData() } },
            };
            yield return new object[] 
            { 
                "<PageData><TextBlockData Text=\"test\" ElementId=\"1\" /></PageData>",
                new PageData { Data = { new TextBlockData() { Text = "test", ElementId = 1 } } },
            };
            yield return new object[] 
            { 
                "<PageData><TextButtonData /></PageData>",
                new PageData { Data = { new TextButtonData() } },
            };
            yield return new object[] 
            { 
                "<PageData><TextButtonData Text=\"test\" ElementId=\"1\" /></PageData>",
                new PageData { Data = { new TextButtonData() { Text = "test", ElementId = 1 } } },
            };
            yield return new object[] 
            { 
                "<PageData><WrappedTextBlockData /></PageData>",
                new PageData { Data = { new WrappedTextBlockData() } },
            };
            yield return new object[] 
            { 
                "<PageData><WrappedTextBlockData Text=\"test\" ElementId=\"1\" /></PageData>",
                new PageData { Data = { new WrappedTextBlockData() { Text = "test", ElementId = 1 } } },
            };
        }

        [Theory]
        [MemberData("GetSerializationData")]
        public void PageDataDeserializesCorrectly(string actualXml, PageData expectedData)
        {
            var compareLogic = new CompareLogic(new ComparisonConfig());

            var deserialized = PageData.FromXml(XElement.Parse(actualXml));
            var result = compareLogic.Compare(deserialized, expectedData);

            if (!result.AreEqual)
            {
                throw new AssertActualExpectedException(expectedData, deserialized, result.DifferencesString);
            }
        }

        [Theory]
        [MemberData("GetSerializationData")]
        public void PageDataSerializesCorrectly(string expectedXml, PageData actualData)
        {
            var expectedXmlTree = XDocument.Parse(expectedXml).Root.ToString(SaveOptions.DisableFormatting);

            var xElement = actualData.AsXml();
            var actualXmlTree = xElement.ToString(SaveOptions.DisableFormatting);

            Assert.Equal(expectedXmlTree, actualXmlTree);
        }
    }
}
