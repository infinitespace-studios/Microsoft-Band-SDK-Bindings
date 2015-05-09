using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

using Microsoft.Band.Portable.Personalization;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
#endif

namespace Microsoft.Band.Portable.Tiles.Pages
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    public abstract class Element
    {
        private const short DefaultElementId = -1;
        private const HorizontalAlignment DefaultHorizontalAlignment = HorizontalAlignment.Left;
        private const VerticalAlignment DefaultVerticalAlignment = VerticalAlignment.Top;
        private const bool DefaultVisible = true;
        private static readonly Margins DefaultMargins = Margins.Empty;
        private static readonly Rectangle DefaultRectangle = Rectangle.Empty;

        private Rectangle rectangle;

        public Element()
        {
            ElementId = DefaultElementId;
            HorizontalAlignment = DefaultHorizontalAlignment;
            VerticalAlignment = DefaultVerticalAlignment;
            Visible = DefaultVisible;
            Margins = DefaultMargins;
            Rectangle = DefaultRectangle;
        }

        public short ElementId { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public bool Visible { get; set; }
        public Margins Margins { get; set; }
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public Point Location
        {
            get { return rectangle.Location; }
            set { rectangle.Location = value; }
        }
        public Size Size
        {
            get { return rectangle.Size; }
            set { rectangle.Size = value; }
        }

        public XElement AsXml()
        {
            return AsXml(null);
        }
        public static Element FromXml(XElement element)
        {
            return PageExtensions.ElementFromXml(element);
        }

        internal Element(XElement element)
        {
            ElementId = element.ReadAttribute("ElementId", value => XmlConvert.ToInt16(value), DefaultElementId);
            HorizontalAlignment = element.ReadEnumAttribute("HorizontalAlignment", DefaultHorizontalAlignment);
            VerticalAlignment = element.ReadEnumAttribute("VerticalAlignment", DefaultVerticalAlignment);
            Visible = element.ReadAttribute("Visible", value => XmlConvert.ToBoolean(value), DefaultVisible);
            Margins = element.ReadAttribute("Margins", value => Margins.FromXmlString(value), DefaultMargins);
            Rectangle = element.ReadAttribute("Rectangle", value => Rectangle.FromXmlString(value), DefaultRectangle);
        }

        internal virtual XElement AsXml(XElement element)
        {
            if (element == null)
            {
                throw new InvalidOperationException("Cannot serialize an abstract type.");
            }

            element.AddAttribute("ElementId", ElementId, value => XmlConvert.ToString(value), DefaultElementId);
            element.AddBasicAttribute("HorizontalAlignment", HorizontalAlignment, DefaultHorizontalAlignment);
            element.AddBasicAttribute("VerticalAlignment", VerticalAlignment, DefaultVerticalAlignment);
            element.AddAttribute("Visible", Visible, value => XmlConvert.ToString(value), DefaultVisible);
            element.AddAttribute("Margins", Margins, value => value.ToXmlString(), DefaultMargins);
            element.AddAttribute("Rectangle", Rectangle, value => value.ToXmlString(), DefaultRectangle);

            return element;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal Element(NativeElement native)
        {
#if WINDOWS_PHONE_APP
            ElementId = native.ElementId ?? DefaultElementId;
#else
            ElementId = (short)native.ElementId;
#endif
            Rectangle = native.Rect.FromNative();
            HorizontalAlignment = native.HorizontalAlignment.FromNative();
            Margins = native.Margins.FromNative();
            VerticalAlignment = native.VerticalAlignment.FromNative();
            Visible = native.Visible;
        }

        internal NativeElement ToNative()
        {
            return ToNative(null);
        }
        internal virtual NativeElement ToNative(NativeElement element)
        {
            var native = EnsureDerived<NativeElement>(element, false);
            if (ElementId > 0)
            {
#if __ANDROID__ || __IOS__
                native.ElementId = (ushort)ElementId;
#elif WINDOWS_PHONE_APP
                native.ElementId = ElementId;
#endif
            }
            if (Rectangle != Rectangle.Empty)
            {
                native.Rect = Rectangle.ToNative();
            }
            native.HorizontalAlignment = HorizontalAlignment.ToNative();
            if (Margins != Margins.Empty)
            {
                native.Margins = Margins.ToNative();
            }
            native.VerticalAlignment = VerticalAlignment.ToNative();
            native.Visible = Visible;
            return native;
        }

        protected T EnsureDerived<T>(NativeElement element)
            where T : NativeElement
        {
            return EnsureDerived<T>(element, true);
        }
        protected T EnsureDerived<T>(NativeElement element, bool allowNull)
            where T : NativeElement
        {
            if (element == null)
            {
                if (allowNull)
                {
                    return null;
                }
                else
                {
                    throw new ArgumentNullException("element");
                }
            }

            var specific = element as T;
            if (specific == null)
            {
                throw new ArgumentException("element", string.Format("element must be of type {0} or a derived type.", typeof(T).FullName));
            }

            return specific;
        }
#endif
    }
}