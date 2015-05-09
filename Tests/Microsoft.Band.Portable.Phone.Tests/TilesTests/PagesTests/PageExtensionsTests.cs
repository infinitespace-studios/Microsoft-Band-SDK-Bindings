using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;

using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Tiles;
using Microsoft.Band.Portable.Tiles.Pages;
using Microsoft.Band.Portable.Tiles.Pages.Data;

using Xunit;
using Xunit.Sdk;

using PortableBarcodeType = Microsoft.Band.Portable.Tiles.Pages.BarcodeType;
using NativeBarcodeType = Microsoft.Band.Tiles.Pages.BarcodeType;
using PortableHorizontalAlignment = Microsoft.Band.Portable.Tiles.Pages.HorizontalAlignment;
using NativeHorizontalAlignment = Microsoft.Band.Tiles.Pages.HorizontalAlignment;
using PortableVerticalAlignment = Microsoft.Band.Portable.Tiles.Pages.VerticalAlignment;
using NativeVerticalAlignment = Microsoft.Band.Tiles.Pages.VerticalAlignment;
using PortableMargins = Microsoft.Band.Portable.Tiles.Pages.Margins;
using NativeMargins = Microsoft.Band.Tiles.Pages.Margins;
using PortableBandColor = Microsoft.Band.Portable.Personalization.BandColor;
using NativeBandColor = Microsoft.Band.BandColor;
using PortableElementColorSource = Microsoft.Band.Portable.Tiles.Pages.ElementColorSource;
using NativeElementColorSource = Microsoft.Band.Tiles.Pages.ElementColorSource;
using PortableOrientation = Microsoft.Band.Portable.Tiles.Pages.Orientation;
using NativeOrientation = Microsoft.Band.Tiles.Pages.FlowPanelOrientation;
using PortableBaselineAlignment = Microsoft.Band.Portable.Tiles.Pages.TextBlockBaselineAlignment;
using NativeBaselineAlignment = Microsoft.Band.Tiles.Pages.TextBlockBaselineAlignment;
using PortableFont = Microsoft.Band.Portable.Tiles.Pages.TextBlockFont;
using NativeFont = Microsoft.Band.Tiles.Pages.TextBlockFont;
using PortableWrappedFont = Microsoft.Band.Portable.Tiles.Pages.WrappedTextBlockFont;
using NativeWrappedFont = Microsoft.Band.Tiles.Pages.WrappedTextBlockFont;

using PortableElement = Microsoft.Band.Portable.Tiles.Pages.Element;
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using PortableBarcode = Microsoft.Band.Portable.Tiles.Pages.Barcode;
using NativeBarcode = Microsoft.Band.Tiles.Pages.Barcode;
using PortableFilledButton = Microsoft.Band.Portable.Tiles.Pages.FilledButton;
using NativeFilledButton = Microsoft.Band.Tiles.Pages.FilledButton;
using PortableFilledPanel = Microsoft.Band.Portable.Tiles.Pages.FilledPanel;
using NativeFilledPanel = Microsoft.Band.Tiles.Pages.FilledPanel;
using PortableFlowPanel = Microsoft.Band.Portable.Tiles.Pages.FlowPanel;
using NativeFlowPanel = Microsoft.Band.Tiles.Pages.FlowPanel;
using PortableImage = Microsoft.Band.Portable.Tiles.Pages.Image;
using NativeImage = Microsoft.Band.Tiles.Pages.Icon;
using PortableScrollFlowPanel = Microsoft.Band.Portable.Tiles.Pages.ScrollFlowPanel;
using NativeScrollFlowPanel = Microsoft.Band.Tiles.Pages.ScrollFlowPanel;
using PortableTextBlock = Microsoft.Band.Portable.Tiles.Pages.TextBlock;
using NativeTextBlock = Microsoft.Band.Tiles.Pages.TextBlock;
using PortableTextButton = Microsoft.Band.Portable.Tiles.Pages.TextButton;
using NativeTextButton = Microsoft.Band.Tiles.Pages.TextButton;
using PortableWrappedTextBlock = Microsoft.Band.Portable.Tiles.Pages.WrappedTextBlock;
using NativeWrappedTextBlock = Microsoft.Band.Tiles.Pages.WrappedTextBlock;

namespace Microsoft.Band.Portable.Tests.TilesTests.PagesTests
{
    public class PageExtensionsTests
    {
        public static IEnumerable<object[]> GetNativePortableData()
        {
            yield return new object[] 
            { 
                new PortableBarcode 
                { 
                    BarcodeType = PortableBarcodeType.Pdf417,
                    ElementId = 2,
                    HorizontalAlignment = PortableHorizontalAlignment.Center,
                    Margins = new PortableMargins(1,2, 3, 4),
                    Rectangle = new Rectangle(1, 2, 3, 4),
                    VerticalAlignment = PortableVerticalAlignment.Center,
                    Visible = false
                },
                new NativeBarcode(NativeBarcodeType.Pdf417)
                { 
                    ElementId = 2,
                    HorizontalAlignment = NativeHorizontalAlignment.Center,
                    Margins = new NativeMargins(1,2, 3, 4),
                    Rect = new PageRect(1, 2, 3, 4),
                    VerticalAlignment = NativeVerticalAlignment.Center,
                    Visible = false
                },
            };
            yield return new object[] 
            { 
                new PortableBarcode { BarcodeType = PortableBarcodeType.Pdf417 },
                new NativeBarcode(NativeBarcodeType.Pdf417),
            };
            yield return new object[] 
            { 
                new PortableFilledButton { BackgroundColor = new PortableBandColor(255, 0, 0) },
                new NativeFilledButton { BackgroundColor = new NativeBandColor(255, 0, 0) },
            };
            yield return new object[] 
            { 
                new PortableFilledPanel 
                {
                    BackgroundColor = new PortableBandColor(255, 0, 0),
                    BackgroundColorSource = PortableElementColorSource.BandBase 
                },
                new NativeFilledPanel
                { 
                    BackgroundColor = new NativeBandColor(255, 0, 0), 
                    BackgroundColorSource = NativeElementColorSource.BandBase 
                },
            };
            yield return new object[] 
            { 
                new PortableFlowPanel { Orientation = PortableOrientation.Horizontal },
                new NativeFlowPanel { Orientation = NativeOrientation.Horizontal },
            };
            yield return new object[] 
            { 
                new PortableImage 
                {
                    Color = new PortableBandColor(255, 0, 0), 
                    ColorSource = PortableElementColorSource.BandBase 
                },
                new NativeImage 
                {
                    Color = new NativeBandColor(255, 0, 0),
                    ColorSource = NativeElementColorSource.BandBase
                },
            };
            yield return new object[] 
            { 
                new PortableScrollFlowPanel { ScrollbarColor = new PortableBandColor(255, 0, 0), ScrollbarColorSource = PortableElementColorSource.BandBase },
                new NativeScrollFlowPanel { ScrollBarColor = new NativeBandColor(255, 0, 0), ScrollBarColorSource = NativeElementColorSource.BandBase },
            };
            yield return new object[] 
            { 
                new PortableTextBlock
                {
                    AutoWidth = false,
                    Baseline = 1,
                    BaselineAlignment = PortableBaselineAlignment.Absolute,
                    Font = PortableFont.Large,
                    TextColor = new PortableBandColor(255, 0, 0), 
                    TextColorSource = PortableElementColorSource.BandBase 
                },
                new NativeTextBlock
                {
                    AutoWidth = false,
                    Baseline = 1,
                    BaselineAlignment = NativeBaselineAlignment.Absolute,
                    Font = NativeFont.Large,
                    Color = new NativeBandColor(255, 0, 0),
                    ColorSource = NativeElementColorSource.BandBase
                },
            };
            yield return new object[] 
            { 
                new PortableTextButton { PressedColor = new PortableBandColor(255, 0, 0) },
                new NativeTextButton { PressedColor = new NativeBandColor(255, 0, 0) },
            };
            yield return new object[] 
            { 
                new PortableWrappedTextBlock
                {
                    AutoHeight = false,
                    Font = PortableWrappedFont.Medium,
                    TextColor = new PortableBandColor(255, 0, 0), 
                    TextColorSource = PortableElementColorSource.BandBase 
                },
                new NativeWrappedTextBlock
                {
                    AutoHeight = false,
                    Font = NativeWrappedFont.Medium,
                    Color = new NativeBandColor(255, 0, 0),
                    ColorSource = NativeElementColorSource.BandBase
                },
            };
            yield return new object[] 
            { 
                new PortableScrollFlowPanel
                {
                    Orientation = PortableOrientation.Horizontal,
                    ScrollbarColor = new PortableBandColor(0, 255, 0),
                    Elements = { new PortableBarcode() }
                },
                new NativeScrollFlowPanel(new NativeBarcode(NativeBarcodeType.Code39))
                {
                    Orientation = NativeOrientation.Horizontal,
                    ScrollBarColor = new NativeBandColor(0, 255, 0)
                },
            };
        }

        [Theory]
        [MemberData("GetNativePortableData")]
        public void NativeElementsConvertCorrectly(PortableElement expectedElement, NativeElement actualElement)
        {
            var actualPortable = FromNative(actualElement);

            var expected = expectedElement.AsXml().ToString(SaveOptions.DisableFormatting);
            var actual = actualPortable.AsXml().ToString(SaveOptions.DisableFormatting);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData("GetNativePortableData")]
        public void PortableElementsConvertCorrectly(PortableElement actualElement, NativeElement expectedElement)
        {
            var actualNative = ToNative(actualElement);
            var actualPortable = FromNative(actualNative);

            var expectedPortable = FromNative(expectedElement);

            var expected = expectedPortable.AsXml().ToString(SaveOptions.DisableFormatting);
            var actual = actualPortable.AsXml().ToString(SaveOptions.DisableFormatting);

            Assert.Equal(expected, actual);
        }

        private static NativeElement ToNative(PortableElement portableElement)
        {
            var type = typeof(PortableElement).GetTypeInfo();
            var toNatives = type.GetDeclaredMethods("ToNative");
            var toNative = toNatives.Single(m => m.GetParameters().Length == 0);
            return (NativeElement)toNative.Invoke(portableElement, null);
        }

        private static PortableElement FromNative(NativeElement nativeElement)
        {
            // get the type
            var assembly = typeof(PortableElement).GetTypeInfo().Assembly;
            var extensions = assembly.GetType("Microsoft.Band.Portable.PageExtensions");

            // get the method
            var fromNativeMethods = extensions.GetTypeInfo().GetDeclaredMethods("FromNative");
            var fromNative = fromNativeMethods.Single(m =>
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == typeof(NativeElement));

            // get the value
            return (PortableElement)fromNative.Invoke(null, new[] { nativeElement });
        }
    }
}
