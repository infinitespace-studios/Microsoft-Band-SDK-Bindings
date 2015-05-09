using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativeImage = Microsoft.Band.Tiles.Pages.Icon;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public class Image : Element
    {
        private static readonly BandColor DefaultColor = BandColor.Empty;
        private const ElementColorSource DefaultColorSource = ElementColorSource.Custom;

        public Image()
        {
            Color = DefaultColor;
            ColorSource = DefaultColorSource;
        }

        public BandColor Color { get; set; }
        public ElementColorSource ColorSource { get; set; }

        internal Image(XElement element)
            : base(element)
        {
            Color = element.ReadAttribute("Color", DefaultColor);
            ColorSource = element.ReadEnumAttribute("ColorSource", DefaultColorSource);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("Image");
            }

            element.AddAttribute("Color", Color, DefaultColor);
            element.AddBasicAttribute("ColorSource", ColorSource, DefaultColorSource);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal Image(NativeImage native)
            : base(native)
        {
            Color = native.Color.FromNative();
            ColorSource = native.ColorSource.FromNative();
        }

        internal override NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativeImage>(element);
            if (native == null)
            {
#if __ANDROID__ || __IOS__
                native = new NativeImage(Rectangle.ToNative());
#elif WINDOWS_PHONE_APP
                native = new NativeImage();
#endif
            }
            if (Color != BandColor.Empty)
            {
                native.Color = Color.ToNative();
            }
            native.ColorSource = ColorSource.ToNative();
            return base.ToNative(native);
        }
#endif
    }
}