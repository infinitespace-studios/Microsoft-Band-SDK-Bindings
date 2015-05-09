using System;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public abstract class TextBlockBase : Element
    {
        private static readonly BandColor DefaultTextColor = BandColor.Empty;
        private const ElementColorSource DefaultTextColorSource = ElementColorSource.Custom;

        public TextBlockBase()
        {
            TextColor = DefaultTextColor;
            TextColorSource = DefaultTextColorSource;
        }

        public abstract BandColor TextColor { get; set; }
        public abstract ElementColorSource TextColorSource { get; set; }

        internal TextBlockBase(XElement element)
            : base(element)
        {
            TextColor = element.ReadAttribute("TextColor", DefaultTextColor);
            TextColorSource = element.ReadEnumAttribute("TextColorSource", DefaultTextColorSource);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                throw new InvalidOperationException("Cannot serialize an abstract type.");
            }

            element.AddAttribute("TextColor", TextColor, value => value.Hex, DefaultTextColor);
            element.AddBasicAttribute("TextColorSource", TextColorSource, DefaultTextColorSource);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal TextBlockBase(NativeElement native)
            : base(native)
        {
        }
#endif
    }
}