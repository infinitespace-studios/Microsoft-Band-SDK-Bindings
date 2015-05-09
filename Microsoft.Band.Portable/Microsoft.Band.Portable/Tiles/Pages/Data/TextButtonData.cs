using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeTextButtonData = Microsoft.Band.Tiles.Pages.TextButtonData;
using NativeElementData = Microsoft.Band.Tiles.Pages.PageElementData;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages.Data
{
    public class TextButtonData : ButtonBaseData
    {
        private static readonly string DefaultText = string.Empty;

        public TextButtonData()
        {
            Text = DefaultText;
        }

        public string Text { get; set; }

        internal TextButtonData(XElement element)
            : base(element)
        {
            Text = element.ReadAttribute("Text", DefaultText);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("TextButtonData");
            }

            element.AddAttribute("Text", Text, DefaultText);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal TextButtonData(NativeTextButtonData native)
            : base(native)
        {
        }

        internal override NativeElementData ToNative()
        {
            NativeTextButtonData native = null;
#if __ANDROID__
            native = new NativeTextButtonData(ElementId, Text);
#elif __IOS__
            Foundation.NSError error;
            native = NativeTextButtonData.Create((ushort)ElementId, Text, out error);
#elif WINDOWS_PHONE_APP
            native = new NativeTextButtonData(ElementId, Text);
#endif
            return native;
        }
#endif
    }
}