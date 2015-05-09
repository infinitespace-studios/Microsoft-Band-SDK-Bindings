using System;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElementData = Microsoft.Band.Tiles.Pages.PageElementData;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages.Data
{
    public abstract class TextBlockBaseData : ElementData
    {
        private static readonly string DefaultText = string.Empty;

        public TextBlockBaseData()
        {
            Text = DefaultText;
        }

        public abstract string Text { get; set; }

        internal TextBlockBaseData(XElement element)
            : base(element)
        {
            Text = element.ReadAttribute("Text", DefaultText);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                throw new InvalidOperationException("Cannot serialize an abstract type.");
            }

            element.AddAttribute("Text", Text, DefaultText);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal TextBlockBaseData(NativeElementData native)
            : base(native)
        {
        }
#endif
    }
}
