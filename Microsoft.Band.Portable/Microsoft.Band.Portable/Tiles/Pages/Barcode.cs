using System;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativeBarcode = Microsoft.Band.Tiles.Pages.Barcode;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    public class Barcode : Element
    {
        private const BarcodeType DefaultBarcodeType = BarcodeType.Code39;

        public Barcode()
        {
            BarcodeType = DefaultBarcodeType;
        }

        public BarcodeType BarcodeType { get; set; }

        internal Barcode(XElement element)
            : base(element)
        {
            BarcodeType = element.ReadEnumAttribute("BarcodeType", DefaultBarcodeType);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("Barcode");
            }

            element.AddBasicAttribute("BarcodeType", BarcodeType, DefaultBarcodeType);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal Barcode(NativeBarcode native)
            : base(native)
        {
            BarcodeType = native.BarcodeType.FromNative();
        }

        internal override NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativeBarcode>(element);
            if (native == null)
            {
#if __ANDROID__ || __IOS__
                native = new NativeBarcode(Rectangle.ToNative(), BarcodeType.ToNative());
#elif WINDOWS_PHONE_APP
                native = new NativeBarcode(BarcodeType.ToNative());
#endif
            }
            return base.ToNative(native);
        }
#endif
    }
}