using System;
using System.Xml;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElementData = Microsoft.Band.Tiles.Pages.PageElementData;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages.Data
{
    public abstract class ElementData
    {
        private const short DefaultElementId = -1;

        public ElementData()
        {
            ElementId = DefaultElementId;
        }

        public short ElementId { get; set; }

        public XElement AsXml()
        {
            return AsXml(null);
        }
        public static ElementData FromXml(XElement element)
        {
            return PageExtensions.ElementDataFromXml(element);
        }

        internal ElementData(XElement element)
        {
            ElementId = element.ReadAttribute("ElementId", value => XmlConvert.ToInt16(value), DefaultElementId);
        }

        internal virtual XElement AsXml(XElement element)
        {
            if (element == null)
            {
                throw new InvalidOperationException("Cannot serialize an abstract type.");
            }

            element.AddAttribute("ElementId", ElementId, value => XmlConvert.ToString(value), DefaultElementId);

            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal ElementData(NativeElementData native)
        {
            ElementId = (short)native.ElementId;
        }

        internal abstract NativeElementData ToNative();
#endif
    }
}