using System.Xml;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeImageData = Microsoft.Band.Tiles.Pages.IconData;
using NativeElementData = Microsoft.Band.Tiles.Pages.PageElementData;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages.Data
{
    public class ImageData : ElementData
    {
        private const ushort DefaultImageIndex = 0;

        public ImageData()
        {
            ImageIndex = DefaultImageIndex;
        }

        public ushort ImageIndex { get; set; }

        internal ImageData(XElement element)
            : base(element)
        {
            ImageIndex = element.ReadAttribute("ImageIndex", value => XmlConvert.ToUInt16(value), DefaultImageIndex);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("ImageData");
            }

            element.AddAttribute("ImageIndex", ImageIndex, value => XmlConvert.ToString(value), DefaultImageIndex);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal ImageData(NativeImageData native)
            : base(native)
        {
            ImageIndex = (ushort)native.IconIndex;
        }

        internal override NativeElementData ToNative()
        {
            NativeImageData native = null;
#if __ANDROID__
            native = new NativeImageData(ElementId, ImageIndex);
#elif __IOS__
            Foundation.NSError error;
            native = NativeImageData.Create((ushort)ElementId, ImageIndex, out error);
#elif WINDOWS_PHONE_APP
            native = new NativeImageData(ElementId, ImageIndex);
#endif
            return native;
        }
#endif
    }
}