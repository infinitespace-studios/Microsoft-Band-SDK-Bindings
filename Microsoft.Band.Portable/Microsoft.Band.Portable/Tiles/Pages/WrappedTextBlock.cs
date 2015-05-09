using System.Xml;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativeWrappedTextBlock = Microsoft.Band.Tiles.Pages.WrappedTextBlock;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public class WrappedTextBlock : TextBlockBase
    {
        private const bool DefaultAutoHeight = true;
        private const WrappedTextBlockFont DefaultFont = WrappedTextBlockFont.Small;

        public WrappedTextBlock()
        {
            AutoHeight = DefaultAutoHeight;
            Font = DefaultFont;
        }

        public bool AutoHeight { get; set; }
        public override BandColor TextColor { get; set; }
        public override ElementColorSource TextColorSource { get; set; }
        public WrappedTextBlockFont Font { get; set; }

        internal WrappedTextBlock(XElement element)
            : base(element)
        {
            AutoHeight = element.ReadAttribute("AutoHeight", value => XmlConvert.ToBoolean(value), DefaultAutoHeight);
            Font = element.ReadEnumAttribute("Font", DefaultFont);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("WrappedTextBlock");
            }

            element.AddAttribute("AutoHeight", AutoHeight, value => XmlConvert.ToString(value), DefaultAutoHeight);
            element.AddBasicAttribute("Font", Font, DefaultFont);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal WrappedTextBlock(NativeWrappedTextBlock native)
            : base(native)
        {
            AutoHeight = native.AutoHeight;
            TextColor = native.Color.FromNative();
            TextColorSource = native.ColorSource.FromNative();
            Font = native.Font.FromNative();
        }

        internal override NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativeWrappedTextBlock>(element);
            if (native == null)
            {
#if __ANDROID__ || __IOS__
                native = new NativeWrappedTextBlock(Rectangle.ToNative(), Font.ToNative());
#elif WINDOWS_PHONE_APP
                native = new NativeWrappedTextBlock();
                native.Font = Font.ToNative();
#endif
            }
            native.AutoHeight = AutoHeight;
            if (TextColor != BandColor.Empty)
            {
                native.Color = TextColor.ToNative();
            }
            native.ColorSource = TextColorSource.ToNative();
            return base.ToNative(native);
        }
#endif
    }
}