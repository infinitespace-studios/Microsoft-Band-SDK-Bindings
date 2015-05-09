using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativePanel = Microsoft.Band.Tiles.Pages.PagePanel;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    public abstract class Panel : Element
    {
        public Panel()
        {
            Elements = new List<Element>();
        }

        public List<Element> Elements { get; private set; }

        internal Panel(XElement element)
            : base(element)
        {
            Elements = element.Elements().Select(e => Element.FromXml(e)).ToList();
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                throw new InvalidOperationException("Cannot serialize an abstract type.");
            }

            if (Elements.Count > 0)
            {
                element.Add(Elements.Select(e => e.AsXml()).ToArray());
            }

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal Panel(NativePanel native)
            : base(native)
        {
            Elements = native.Elements.Select(e => e.FromNative()).ToList();
        }

        internal override NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativePanel>(element, false);
#if __ANDROID__ || __IOS__
            native.AddElements(Elements.Select(e => e.ToNative()).ToArray());
#elif WINDOWS_PHONE_APP
            foreach (var childElement in Elements)
            {
                native.Elements.Add(childElement.ToNative());
            }
#endif
            return base.ToNative(native);
        }
#endif
    }
}
