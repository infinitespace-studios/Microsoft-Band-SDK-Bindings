using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativeTextButton = Microsoft.Band.Tiles.Pages.TextButton;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public class TextButton : ButtonBase
    {
        private static readonly BandColor DefaultPressedColor = BandColor.Empty;

        public TextButton()
        {
            PressedColor = DefaultPressedColor;
        }

        public BandColor PressedColor { get; set; }

        internal TextButton(XElement element)
            : base(element)
        {
            PressedColor = element.ReadAttribute("PressedColor", DefaultPressedColor);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("TextButton");
            }

            element.AddAttribute("PressedColor", PressedColor, DefaultPressedColor);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal TextButton(NativeTextButton native)
            : base(native)
        {
            PressedColor = native.PressedColor.FromNative();
        }

        internal override NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativeTextButton>(element);
            if (native == null)
            {
#if __ANDROID__ || __IOS__
                native = new NativeTextButton(Rectangle.ToNative());
#elif WINDOWS_PHONE_APP
                native = new NativeTextButton();
#endif
            }
            if (PressedColor != BandColor.Empty)
            {
                native.PressedColor = PressedColor.ToNative();
            }
            return base.ToNative(native);
        }
#endif
    }
}