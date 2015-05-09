using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativePageLayout = Microsoft.Band.Tiles.Pages.PageLayout;
using NativePanel = Microsoft.Band.Tiles.Pages.PagePanel;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    public class PageLayout
    {
        public PageLayout()
        {
        }

        public Panel Root { get; set; }

        public XElement AsXml()
        {
            var element = new XElement("PageLayout");
            if (Root != null)
            {
                element.Add(Root.AsXml());
            }

            return element;
        }

        public static PageLayout FromXml(XElement element)
        {
            if (element.Name == "PageLayout")
            {
                Panel panel = null;
                var child = element.Elements().FirstOrDefault();
                if (child != null)
                {
                    panel = (Panel)Element.FromXml(child);
                }
                return new PageLayout { Root = panel };
            }
            else
            {
                throw new XmlException("Element must be a <PageLayout>.");
            }
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal PageLayout(NativePageLayout native)
        {
            Root = native.Root.FromNative();
        }

        internal NativePageLayout ToNative()
        {
            return new NativePageLayout((NativePanel)Root.ToNative());
        }
#endif
    }
}