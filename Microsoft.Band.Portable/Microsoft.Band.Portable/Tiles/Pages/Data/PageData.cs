using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativePageData = Microsoft.Band.Tiles.Pages.PageData;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages.Data
{
    public class PageData
    {
        private static readonly Guid DefaultPageId = Guid.Empty;
        private const int DefaultPageLayoutIndex = -1;

        public PageData()
        {
            PageId = DefaultPageId;
            PageLayoutIndex = DefaultPageLayoutIndex;
            Data = new List<ElementData>();
        }

        public Guid PageId { get; set; }
        public int PageLayoutIndex { get; set; }
        public List<ElementData> Data { get; private set; }

        public XElement AsXml()
        {
            var element = new XElement("PageData");

            element.AddAttribute("PageId", PageId, value => value.ToString("D"), DefaultPageId);
            element.AddAttribute("PageLayoutIndex", PageLayoutIndex, value => XmlConvert.ToString(value), DefaultPageLayoutIndex);
            element.Add(Data.Select(d => d.AsXml()).ToArray());

            return element;
        }

        public static PageData FromXml(XElement element)
        {
            if (element.Name == "PageData")
            {
                var data = new PageData();

                data.PageId = element.ReadAttribute("PageId", value => Guid.Parse(value), DefaultPageId);
                data.PageLayoutIndex = element.ReadAttribute("PageLayoutIndex", value => XmlConvert.ToInt32(value), DefaultPageLayoutIndex);
                data.Data = element.Elements().Select(e => ElementData.FromXml(e)).ToList();

                return data;
            }
            else
            {
                throw new XmlException("Element must be a <PageData>.");
            }
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal PageData(NativePageData native)
        {
            PageId = native.PageId.FromNative();
            PageLayoutIndex = PageLayoutIndex;
            Data = native.Values.Select(e => e.FromNative()).ToList();
        }

        internal NativePageData ToNative()
        {
            NativePageData native = null;
#if __ANDROID__
            native = new NativePageData(PageId.ToNative(), PageLayoutIndex);
            foreach (var data in Data)
            {
                native.Values.Add(data.ToNative());
            }
#elif __IOS__
            native = NativePageData.Create(PageId.ToNative(), (nuint)PageLayoutIndex, Data.Select(d => d.ToNative()).ToArray());
#elif WINDOWS_PHONE_APP
            native = new NativePageData(PageId.ToNative(), PageLayoutIndex, Data.Select(d => d.ToNative()).ToArray());
#endif
            return native;
        }
#endif
    }
}
