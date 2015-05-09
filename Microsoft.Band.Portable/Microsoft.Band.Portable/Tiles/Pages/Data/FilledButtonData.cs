using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeFilledButtonData = Microsoft.Band.Tiles.Pages.FilledButtonData;
using NativeElementData = Microsoft.Band.Tiles.Pages.PageElementData;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages.Data
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public class FilledButtonData : ButtonBaseData
    {
        private static readonly BandColor DefaultPressedColor = BandColor.Empty;

        public FilledButtonData()
        {
            PressedColor = DefaultPressedColor;
        }

        public BandColor PressedColor { get; set; }

        internal FilledButtonData(XElement element)
            : base(element)
        {
            PressedColor = element.ReadAttribute("PressedColor", DefaultPressedColor);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("FilledButtonData");
            }

            element.AddAttribute("PressedColor", PressedColor, value => value.Hex, DefaultPressedColor);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal FilledButtonData(NativeFilledButtonData native)
            : base(native)
        {
            PressedColor = native.PressedColor.FromNative();
        }

        internal override NativeElementData ToNative()
        {
            NativeFilledButtonData native = null;
#if __ANDROID__
            native = new NativeFilledButtonData(ElementId, PressedColor.ToNative());
#elif __IOS__
            native = NativeFilledButtonData.Create((ushort)ElementId);
            native.PressedColor = PressedColor.ToNative();
#elif WINDOWS_PHONE_APP
            native = new NativeFilledButtonData(ElementId, PressedColor.ToNative());
#endif
            return native;
        }
#endif
    }
}