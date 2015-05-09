using System.Xml.Linq;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativeFilledPanel = Microsoft.Band.Tiles.Pages.FilledPanel;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public class FilledPanel : Panel
    {
        private static readonly BandColor DefaultBackgroundColor = BandColor.Empty;
        private const ElementColorSource DefaultBackgroundColorSource = ElementColorSource.Custom;

        public FilledPanel()
        {
            BackgroundColor = DefaultBackgroundColor;
            BackgroundColorSource = DefaultBackgroundColorSource;
        }

        public BandColor BackgroundColor { get; set; }
        public ElementColorSource BackgroundColorSource { get; set; }
        
        internal FilledPanel(XElement element)
            : base(element)
        {
            BackgroundColor = element.ReadAttribute("BackgroundColor", DefaultBackgroundColor);
            BackgroundColorSource = element.ReadEnumAttribute("BackgroundColorSource", DefaultBackgroundColorSource);
        }

        internal override XElement AsXml(XElement element)
        {
            if (element == null)
            {
                element = new XElement("FilledPanel");
            }

            element.AddAttribute("BackgroundColor", BackgroundColor, DefaultBackgroundColor);
            element.AddBasicAttribute("BackgroundColorSource", BackgroundColorSource, DefaultBackgroundColorSource);

            base.AsXml(element);
            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal FilledPanel(NativeFilledPanel native)
            : base(native)
        {
            BackgroundColor = native.BackgroundColor.FromNative();
            BackgroundColorSource = native.BackgroundColorSource.FromNative();
        }

        internal override NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativeFilledPanel>(element);
            if (native == null)
            {
#if __ANDROID__ || __IOS__
                native = new NativeFilledPanel(Rectangle.ToNative());
#elif WINDOWS_PHONE_APP
                native = new NativeFilledPanel();
#endif
            }
            if (BackgroundColor != BandColor.Empty)
            {
                native.BackgroundColor = BackgroundColor.ToNative();
            }
            native.BackgroundColorSource = BackgroundColorSource.ToNative();
            return base.ToNative(native);
        }
#endif
    }
}