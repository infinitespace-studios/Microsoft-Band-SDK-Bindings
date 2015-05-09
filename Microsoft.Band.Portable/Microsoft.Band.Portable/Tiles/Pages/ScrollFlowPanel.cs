using System.Xml;
using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativeScrollFlowPanel = Microsoft.Band.Tiles.Pages.ScrollFlowPanel;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public class ScrollFlowPanel : FlowPanel
    {
        private const ElementColorSource DefaultScrollbarColorSource = ElementColorSource.Custom;
        private static readonly BandColor DefaultScrollbarColor = BandColor.Empty;

        public ScrollFlowPanel()
        {
            ScrollbarColor = DefaultScrollbarColor;
            ScrollbarColorSource = DefaultScrollbarColorSource;
        }

        public BandColor ScrollbarColor { get; set; }
        public ElementColorSource ScrollbarColorSource { get; set; }

        internal ScrollFlowPanel(XElement element)
            : base(element)
        {
            ScrollbarColor = element.ReadAttribute("ScrollbarColor", DefaultScrollbarColor);
            ScrollbarColorSource = element.ReadEnumAttribute("ScrollbarColorSource", DefaultScrollbarColorSource);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("ScrollFlowPanel");
            }

            element.AddAttribute("ScrollbarColor", ScrollbarColor, DefaultScrollbarColor);
            element.AddBasicAttribute("ScrollbarColorSource", ScrollbarColorSource, DefaultScrollbarColorSource);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal ScrollFlowPanel(NativeScrollFlowPanel native)
            : base(native)
        {
            ScrollbarColor = native.ScrollBarColor.FromNative();
            ScrollbarColorSource = native.ScrollBarColorSource.FromNative();
        }

        internal override NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativeScrollFlowPanel>(element);
            if (native == null)
            {
#if __ANDROID__ || __IOS__
                native = new NativeScrollFlowPanel(Rectangle.ToNative());
#elif WINDOWS_PHONE_APP
                native = new NativeScrollFlowPanel();
#endif
            }
            if (ScrollbarColor != BandColor.Empty)
            {
                native.ScrollBarColor = ScrollbarColor.ToNative();
            }
            native.ScrollBarColorSource = ScrollbarColorSource.ToNative();
            return base.ToNative(native);
        }
#endif
    }
}