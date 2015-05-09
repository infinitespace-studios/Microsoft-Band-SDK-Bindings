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
    public class PageLayoutTests
    {
        public static IEnumerable<object[]> GetSerializationData()
        {
            yield return new object[] 
            { 
                "<PageLayout />",
                new PageLayout(),
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel /></PageLayout>",
                new PageLayout { Root = new FlowPanel() }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel Margins=\"1, 2, 3, 4\" /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Margins = new Margins(1, 2, 3, 4) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel Margins=\"1, 2\" /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Margins = new Margins(1, 2, 1, 2) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel Margins=\"1\" /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Margins = new Margins(1, 1, 1, 1) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Margins = new Margins(0, 0, 0, 0) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel Rectangle=\"1, 2, 3, 4\" /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Rectangle = new Rectangle(1, 2, 3, 4) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel Rectangle=\"1, 2, 1, 2\" /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Rectangle = new Rectangle(1, 2, 1, 2) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel Rectangle=\"1, 1, 1, 1\" /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Rectangle = new Rectangle(1, 1, 1, 1) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel /></PageLayout>",
                new PageLayout { Root = new FlowPanel { Rectangle = new Rectangle(0, 0, 0, 0) } }, 
            };
            yield return new object[]
            {
                "<PageLayout><ScrollFlowPanel /></PageLayout>",
                new PageLayout { Root = new ScrollFlowPanel() }, 
            };
            yield return new object[]
            {
                "<PageLayout><ScrollFlowPanel /></PageLayout>",
                new PageLayout { Root = new ScrollFlowPanel { Orientation = Orientation.Vertical } }, 
            };
            yield return new object[]
            {
                "<PageLayout><ScrollFlowPanel Orientation=\"Horizontal\" /></PageLayout>",
                new PageLayout { Root = new ScrollFlowPanel { Orientation = Orientation.Horizontal } }, 
            };
            yield return new object[]
            {
                "<PageLayout><ScrollFlowPanel ScrollbarColorSource=\"BandBase\" Orientation=\"Horizontal\" /></PageLayout>",
                new PageLayout { Root = new ScrollFlowPanel { ScrollbarColorSource = ElementColorSource.BandBase, Orientation = Orientation.Horizontal } }, 
            };
            yield return new object[]
            {
                "<PageLayout><ScrollFlowPanel ScrollbarColor=\"#FF7F00\" ScrollbarColorSource=\"BandBase\" Orientation=\"Horizontal\" /></PageLayout>",
                new PageLayout { Root = new ScrollFlowPanel { ScrollbarColor = new BandColor(255, 127,0), ScrollbarColorSource = ElementColorSource.BandBase, Orientation = Orientation.Horizontal } }, 
            };
            yield return new object[]
            {
                "<PageLayout><ScrollFlowPanel><TextBlock Baseline=\"3\" /><FlowPanel /></ScrollFlowPanel></PageLayout>",
                new PageLayout { Root = new ScrollFlowPanel { Elements = { new TextBlock { Baseline = 3 }, new FlowPanel() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><ScrollFlowPanel><ScrollFlowPanel><ScrollFlowPanel /></ScrollFlowPanel></ScrollFlowPanel></PageLayout>",
                new PageLayout { Root = new ScrollFlowPanel { Elements = { new ScrollFlowPanel { Elements = { new ScrollFlowPanel() } } } } }, 
            };

            yield return new object[]
            {
                "<PageLayout><FlowPanel><Barcode /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new Barcode() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><Barcode BarcodeType=\"Pdf417\" /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new Barcode 
                {
                    BarcodeType = BarcodeType.Pdf417 
                } } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><FilledButton /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new FilledButton() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><FilledButton BackgroundColor=\"#00FF00\" /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new FilledButton 
                {
                    BackgroundColor = new BandColor(0, 255, 0) 
                } } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><FilledPanel /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new FilledPanel() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><FilledPanel BackgroundColor=\"#00FF00\" BackgroundColorSource=\"BandBase\" /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new FilledPanel 
                {
                    BackgroundColor = new BandColor(0, 255, 0), 
                    BackgroundColorSource = ElementColorSource.BandBase 
                } } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><Image /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new Image() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><Image Color=\"#00FF00\" ColorSource=\"BandBase\" /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new Image 
                {
                    Color = new BandColor(0, 255, 0), 
                    ColorSource = ElementColorSource.BandBase 
                } } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><TextBlock /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new TextBlock() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><TextBlock AutoWidth=\"false\" Baseline=\"1\" BaselineAlignment=\"Relative\" Font=\"Medium\" TextColor=\"#00FF00\" TextColorSource=\"BandBase\" /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new TextBlock 
                {
                    AutoWidth = false,
                    Baseline = 1,
                    BaselineAlignment = TextBlockBaselineAlignment.Relative,
                    TextColor = new BandColor(0, 255, 0), 
                    TextColorSource = ElementColorSource.BandBase,
                    Font = TextBlockFont.Medium
                } } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><TextButton /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new TextButton() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><TextButton PressedColor=\"#00FF00\" /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new TextButton 
                {
                    PressedColor = new BandColor(0, 255, 0), 
                } } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><WrappedTextBlock /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new WrappedTextBlock() } } }, 
            };
            yield return new object[]
            {
                "<PageLayout><FlowPanel><WrappedTextBlock AutoHeight=\"false\" Font=\"Medium\" TextColor=\"#00FF00\" TextColorSource=\"BandBase\" /></FlowPanel></PageLayout>",
                new PageLayout { Root = new FlowPanel { Elements = { new WrappedTextBlock 
                {
                    AutoHeight = false,
                    TextColor = new BandColor(0, 255, 0), 
                    TextColorSource = ElementColorSource.BandBase,
                    Font = WrappedTextBlockFont.Medium
                } } } }, 
            };
        }

        [Theory]
        [MemberData("GetSerializationData")]
        public void PageLayoutDeserializesCorrectly(string actualXml, PageLayout expectedLayout)
        {
            var compareLogic = new CompareLogic(new ComparisonConfig());

            var deserialized = PageLayout.FromXml(XElement.Parse(actualXml));
            var result = compareLogic.Compare(deserialized, expectedLayout);

            if (!result.AreEqual)
            {
                throw new AssertActualExpectedException(expectedLayout, deserialized, result.DifferencesString);
            }
        }

        [Theory]
        [MemberData("GetSerializationData")]
        public void PageLayoutSerializesCorrectly(string expectedXml, PageLayout actualLayout)
        {
            var expectedXmlTree = XDocument.Parse(expectedXml).Root.ToString(SaveOptions.DisableFormatting);

            var xElement = actualLayout.AsXml();
            var actualXmlTree = xElement.ToString(SaveOptions.DisableFormatting);

            Assert.Equal(expectedXmlTree, actualXmlTree);
        }

        //[Fact]
        //public void PageLayoutWithCyclicReferencesThrows()
        //{
        //    var panel = new FlowPanel();
        //    panel.Elements.Add(panel);
        //    var layout = new PageLayout { Root = panel };

        //    var stringBuilder = new StringBuilder();
        //    using (var textWriter = new StringWriter(stringBuilder))
        //    using (var xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
        //    {
        //        Assert.Throws<StackOverflowException>(() => layout.WriteXml(xmlWriter));
        //    }
        //}
    }
}
