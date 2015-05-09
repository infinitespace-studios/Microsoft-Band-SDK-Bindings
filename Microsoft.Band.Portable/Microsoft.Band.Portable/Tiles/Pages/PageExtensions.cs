using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.Band.Portable.Tiles.Pages;
using Microsoft.Band.Portable.Tiles.Pages.Data;

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
using NativeBarcodeType = Microsoft.Band.Tiles.Pages.BarcodeType;
using NativeHorizontalAlignment = Microsoft.Band.Tiles.Pages.HorizontalAlignment;
using NativeVerticalAlignment = Microsoft.Band.Tiles.Pages.VerticalAlignment;
using NativeRectangle = Microsoft.Band.Tiles.Pages.PageRect;
using NativeMargins = Microsoft.Band.Tiles.Pages.Margins;
using NativeElementColorSource = Microsoft.Band.Tiles.Pages.ElementColorSource;
using NativeFlowPanelOrientation = Microsoft.Band.Tiles.Pages.FlowPanelOrientation;
using NativeTextBlockBaselineAlignment = Microsoft.Band.Tiles.Pages.TextBlockBaselineAlignment;
using NativeTextBlockFont = Microsoft.Band.Tiles.Pages.TextBlockFont;
using NativeWrappedTextBlockFont = Microsoft.Band.Tiles.Pages.WrappedTextBlockFont;
using NativeElementData = Microsoft.Band.Tiles.Pages.PageElementData;
using NativeElement = Microsoft.Band.Tiles.Pages.PageElement;
using NativePanel = Microsoft.Band.Tiles.Pages.PagePanel;
#endif

namespace Microsoft.Band.Portable
{
    using BandColor = Microsoft.Band.Portable.Personalization.BandColor;

    internal static class PageExtensions
    {
        internal readonly static Dictionary<string, ConstructorInfo> elementXmlConstructors;
        internal readonly static Dictionary<string, ConstructorInfo> dataXmlConstructors;
        internal readonly static Dictionary<Type, ConstructorInfo> elementNativeConstructors;
        internal readonly static Dictionary<Type, ConstructorInfo> dataNativeConstructors;

        static PageExtensions()
        {
            elementXmlConstructors = new Dictionary<string, ConstructorInfo>();
            dataXmlConstructors = new Dictionary<string, ConstructorInfo>();
            elementNativeConstructors = new Dictionary<Type, ConstructorInfo>();
            dataNativeConstructors = new Dictionary<Type, ConstructorInfo>();

            var elementXmlType = typeof(XElement);
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
            var elementNativeType = typeof(NativeElement).GetTypeInfo();
            var dataNativeType = typeof(NativeElementData).GetTypeInfo();
#endif

            // get types from assembly
            var assembly = typeof(PageExtensions).GetTypeInfo().Assembly;
            var elementType = typeof(Element).GetTypeInfo();
            var elementTypes = assembly.DefinedTypes.Where(t => !t.IsAbstract && elementType.IsAssignableFrom(t)).ToArray();
            var elementDataType = typeof(ElementData).GetTypeInfo();
            var dataTypes = assembly.DefinedTypes.Where(t => !t.IsAbstract && elementDataType.IsAssignableFrom(t)).ToArray();

            foreach (var type in elementTypes)
            {
                // get the type by XML constructor
                var xmlConstructor = type.GetConstructor(elementXmlType);
                if (xmlConstructor != null)
                {
                    elementXmlConstructors[type.Name] = xmlConstructor;
                }

                // get the type by native constructor
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
                Type parameterType;
                var nativeConstructor = type.GetConstructor(elementNativeType, out parameterType);
                if (nativeConstructor != null)
                {
                    elementNativeConstructors[parameterType] = nativeConstructor;
                }
#endif
            }

            foreach (var type in dataTypes)
            {
                // get the type by XML constructor
                var xmlConstructor = type.GetConstructor(elementXmlType);
                if (xmlConstructor != null)
                {
                    dataXmlConstructors[type.Name] = xmlConstructor;
                }

                // get the type by native constructor
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
                Type parameterType;
                var nativeConstructor = type.GetConstructor(dataNativeType, out parameterType);
                if (nativeConstructor != null)
                {
                    dataNativeConstructors[parameterType] = nativeConstructor;
                }
#endif
            }
        }

        internal static string ReadAttribute(this XElement element, string localName, string defaultValue)
        {
            return element.ReadAttribute(localName, v => v, defaultValue);
        }
        internal static BandColor ReadAttribute(this XElement element, string localName, BandColor defaultValue)
        {
            return element.ReadAttribute(localName, v => BandColor.FromHex(v), defaultValue);
        }
        internal static T ReadEnumAttribute<T>(this XElement element, string localName, T defaultValue)
            where T : struct
        {
            return element.ReadAttribute(localName, v => (T)Enum.Parse(typeof(T), v, true), defaultValue);
        }
        internal static T ReadAttribute<T>(this XElement element, string localName, Func<string, T> getXmlValue, T defaultValue)
        {
            var attribute = element.Attribute(localName);
            if (attribute != null)
            {
                return getXmlValue(attribute.Value);
            }
            return defaultValue;
        }

        internal static void AddAttribute(this XElement element, string localName, string value, string defaultValue)
        {
            element.AddAttribute(localName, value, v => v, defaultValue);
        }
        internal static void AddAttribute(this XElement element, string localName, BandColor value, BandColor defaultValue)
        {
            element.AddAttribute(localName, value, v => v.Hex, defaultValue);
        }
        internal static void AddBasicAttribute<T>(this XElement element, string localName, T value, T defaultValue)
        {
            element.AddAttribute(localName, value, v => v.ToString(), defaultValue);
        }
        internal static void AddAttribute<T>(this XElement element, string localName, T value, Func<T, string> getXmlValue, T defaultValue)
        {
            if (!value.Equals(defaultValue))
            {
                element.Add(new XAttribute(localName, getXmlValue(value)));
            }
        }

        internal static Element ElementFromXml(XElement element)
        {
            return FromXml<Element>(element, elementXmlConstructors);
        }
        internal static ElementData ElementDataFromXml(XElement element)
        {
            return FromXml<ElementData>(element, dataXmlConstructors);
        }

        private static TPortable FromXml<TPortable>(XElement element, Dictionary<string, ConstructorInfo> constructors)
            where TPortable : class
        {
            var elementType = element.Name.LocalName;
            if (!constructors.ContainsKey(elementType))
            {
                throw new ArgumentException(string.Format("No matching portable type was found for {0}", elementType), "element");
            }
            return (TPortable)constructors[elementType].Invoke(new[] { element });
        }

        private static TPortable FromNative<TPortable, TNative>(TNative native, Dictionary<Type, ConstructorInfo> constructors)
            where TPortable : class
            where TNative : class
        {
            var nativeType = native.GetType();
            if (!constructors.ContainsKey(nativeType))
            {
                throw new ArgumentException(string.Format("No matching portable type was found for {0}", nativeType.FullName), "native");
            }
            return (TPortable)constructors[nativeType].Invoke(new[] { native });
        }

        private static ConstructorInfo GetConstructor(this TypeInfo type, Type parameterType)
        {
            foreach (var constructor in type.DeclaredConstructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == parameterType)
                {
                    return constructor;
                }
            }
            return null;
        }

        private static ConstructorInfo GetConstructor(this TypeInfo type, TypeInfo parameterBaseType, out Type parameterType)
        {
            foreach (var constructor in type.DeclaredConstructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == 1 && parameterBaseType.IsAssignableFrom(parameters[0].ParameterType.GetTypeInfo()))
                {
                    parameterType = parameters[0].ParameterType;
                    return constructor;
                }
            }
            parameterType = null;
            return null;
        }

#if __ANDROID__ || __IOS__ || WINDOWS_PHONE_APP
        internal static NativeBarcodeType ToNative(this BarcodeType barcodeType)
        {
            // can't use switch on Android as this is not an enum
            if (barcodeType == BarcodeType.Code39)
                return NativeBarcodeType.Code39;
            if (barcodeType == BarcodeType.Pdf417)
                return NativeBarcodeType.Pdf417;
            throw new ArgumentOutOfRangeException("barcodeType", "Invalid BarcodeType specified.");
        }
        internal static BarcodeType FromNative(this NativeBarcodeType barcodeType)
        {
            // can't use switch on Android as this is not an enum
            if (barcodeType == NativeBarcodeType.Code39)
                return BarcodeType.Code39;
            if (barcodeType == NativeBarcodeType.Pdf417)
                return BarcodeType.Pdf417;
            throw new ArgumentOutOfRangeException("barcodeType", "Invalid BarcodeType specified.");
        }

        internal static NativeFlowPanelOrientation ToNative(this Orientation orientation)
        {
            // can't use switch on Android as this is not an enum
            if (orientation == Orientation.Horizontal)
                return NativeFlowPanelOrientation.Horizontal;
            if (orientation == Orientation.Vertical)
                return NativeFlowPanelOrientation.Vertical;
            throw new ArgumentOutOfRangeException("orientation", "Invalid Orientation specified.");
        }
        internal static Orientation FromNative(this NativeFlowPanelOrientation orientation)
        {
            // can't use switch on Android as this is not an enum
            if (orientation == NativeFlowPanelOrientation.Horizontal)
                return Orientation.Horizontal;
            if (orientation == NativeFlowPanelOrientation.Vertical)
                return Orientation.Vertical;
            throw new ArgumentOutOfRangeException("orientation", "Invalid FlowPanelOrientation specified.");
        }

        internal static NativeElementColorSource ToNative(this ElementColorSource elementColorSource)
        {
            // can't use switch on Android as this is not an enum
            if (elementColorSource == ElementColorSource.Custom)
                return NativeElementColorSource.Custom;

            if (elementColorSource == ElementColorSource.BandBase)
                return NativeElementColorSource.BandBase;
            if (elementColorSource == ElementColorSource.BandHighContrast)
                return NativeElementColorSource.BandHighContrast;
            if (elementColorSource == ElementColorSource.BandHighlight)
                return NativeElementColorSource.BandHighlight;
            if (elementColorSource == ElementColorSource.BandLowlight)
                return NativeElementColorSource.BandLowlight;
            if (elementColorSource == ElementColorSource.BandMuted)
                return NativeElementColorSource.BandMuted;
            if (elementColorSource == ElementColorSource.BandSecondaryText)
                return NativeElementColorSource.BandSecondaryText;

            if (elementColorSource == ElementColorSource.TileBase)
                return NativeElementColorSource.TileBase;
            if (elementColorSource == ElementColorSource.TileHighContrast)
                return NativeElementColorSource.TileHighContrast;
            if (elementColorSource == ElementColorSource.TileHighlight)
                return NativeElementColorSource.TileHighlight;
            if (elementColorSource == ElementColorSource.TileLowlight)
                return NativeElementColorSource.TileLowlight;
            if (elementColorSource == ElementColorSource.TileMuted)
                return NativeElementColorSource.TileMuted;
            if (elementColorSource == ElementColorSource.TileSecondaryText)
                return NativeElementColorSource.TileSecondaryText;

            throw new ArgumentOutOfRangeException("elementColorSource", "Invalid ElementColorSource specified.");
        }
        internal static ElementColorSource FromNative(this NativeElementColorSource elementColorSource)
        {
            // can't use switch on Android as this is not an enum
            if (elementColorSource == NativeElementColorSource.Custom)
                return ElementColorSource.Custom;

            if (elementColorSource == NativeElementColorSource.BandBase)
                return ElementColorSource.BandBase;
            if (elementColorSource == NativeElementColorSource.BandHighContrast)
                return ElementColorSource.BandHighContrast;
            if (elementColorSource == NativeElementColorSource.BandHighlight)
                return ElementColorSource.BandHighlight;
            if (elementColorSource == NativeElementColorSource.BandLowlight)
                return ElementColorSource.BandLowlight;
            if (elementColorSource == NativeElementColorSource.BandMuted)
                return ElementColorSource.BandMuted;
            if (elementColorSource == NativeElementColorSource.BandSecondaryText)
                return ElementColorSource.BandSecondaryText;

            if (elementColorSource == NativeElementColorSource.TileBase)
                return ElementColorSource.TileBase;
            if (elementColorSource == NativeElementColorSource.TileHighContrast)
                return ElementColorSource.TileHighContrast;
            if (elementColorSource == NativeElementColorSource.TileHighlight)
                return ElementColorSource.TileHighlight;
            if (elementColorSource == NativeElementColorSource.TileLowlight)
                return ElementColorSource.TileLowlight;
            if (elementColorSource == NativeElementColorSource.TileMuted)
                return ElementColorSource.TileMuted;
            if (elementColorSource == NativeElementColorSource.TileSecondaryText)
                return ElementColorSource.TileSecondaryText;

            throw new ArgumentOutOfRangeException("elementColorSource", "Invalid ElementColorSource specified.");
        }

        internal static NativeRectangle ToNative(this Rectangle rectangle)
        {
#if __ANDROID__ || WINDOWS_PHONE_APP
            return new NativeRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
#elif __IOS__
            return NativeRectangle.Create((ushort)rectangle.X, (ushort)rectangle.Y, (ushort)rectangle.Width, (ushort)rectangle.Height);
#endif
        }
        internal static Rectangle FromNative(this NativeRectangle rectangle)
        {
            return new Rectangle((short)rectangle.X, (short)rectangle.Y, (short)rectangle.Width, (short)rectangle.Height);
        }

        internal static NativeMargins ToNative(this Margins margins)
        {
#if __ANDROID__ || WINDOWS_PHONE_APP
            return new NativeMargins(margins.Left, margins.Top, margins.Right, margins.Bottom);
#elif __IOS__
            return NativeMargins.Create(margins.Left, margins.Top, margins.Right, margins.Bottom);
#endif
        }
        internal static Margins FromNative(this NativeMargins margins)
        {
            return new Margins((short)margins.Left, (short)margins.Top, (short)margins.Right, (short)margins.Bottom);
        }

        internal static NativeTextBlockBaselineAlignment ToNative(this TextBlockBaselineAlignment baselineAlignment)
        {
            // can't use switch on Android as this is not an enum
            if (baselineAlignment == TextBlockBaselineAlignment.Absolute)
                return NativeTextBlockBaselineAlignment.Absolute;
            if (baselineAlignment == TextBlockBaselineAlignment.Relative)
                return NativeTextBlockBaselineAlignment.Relative;
            return NativeTextBlockBaselineAlignment.Automatic;
        }
        internal static TextBlockBaselineAlignment FromNative(this NativeTextBlockBaselineAlignment baselineAlignment)
        {
            // can't use switch on Android as this is not an enum
            if (baselineAlignment == NativeTextBlockBaselineAlignment.Absolute)
                return TextBlockBaselineAlignment.Absolute;
            if (baselineAlignment == NativeTextBlockBaselineAlignment.Relative)
                return TextBlockBaselineAlignment.Relative;
            return TextBlockBaselineAlignment.Automatic;
        }

        internal static NativeTextBlockFont ToNative(this TextBlockFont font)
        {
            // can't use switch on Android as this is not an enum
            if (font == TextBlockFont.Small)
                return NativeTextBlockFont.Small;
            if (font == TextBlockFont.Medium)
                return NativeTextBlockFont.Medium;
            if (font == TextBlockFont.Large)
                return NativeTextBlockFont.Large;
            if (font == TextBlockFont.ExtraLargeNumbers)
                return NativeTextBlockFont.ExtraLargeNumbers;
            if (font == TextBlockFont.ExtraLargeNumbersBold)
                return NativeTextBlockFont.ExtraLargeNumbersBold;
            throw new ArgumentOutOfRangeException("font", "Invalid TextBlockFont specified.");
        }
        internal static TextBlockFont FromNative(this NativeTextBlockFont font)
        {
            // can't use switch on Android as this is not an enum
            if (font == NativeTextBlockFont.Small)
                return TextBlockFont.Small;
            if (font == NativeTextBlockFont.Medium)
                return TextBlockFont.Medium;
            if (font == NativeTextBlockFont.Large)
                return TextBlockFont.Large;
            if (font == NativeTextBlockFont.ExtraLargeNumbers)
                return TextBlockFont.ExtraLargeNumbers;
            if (font == NativeTextBlockFont.ExtraLargeNumbersBold)
                return TextBlockFont.ExtraLargeNumbersBold;
            throw new ArgumentOutOfRangeException("font", "Invalid TextBlockFont specified.");
        }

        internal static NativeWrappedTextBlockFont ToNative(this WrappedTextBlockFont font)
        {
            // can't use switch on Android as this is not an enum
            if (font == WrappedTextBlockFont.Small)
                return NativeWrappedTextBlockFont.Small;
            if (font == WrappedTextBlockFont.Medium)
                return NativeWrappedTextBlockFont.Medium;
            throw new ArgumentOutOfRangeException("font", "Invalid WrappedTextBlockFont specified.");
        }
        internal static WrappedTextBlockFont FromNative(this NativeWrappedTextBlockFont font)
        {
            // can't use switch on Android as this is not an enum
            if (font == NativeWrappedTextBlockFont.Small)
                return WrappedTextBlockFont.Small;
            if (font == NativeWrappedTextBlockFont.Medium)
                return WrappedTextBlockFont.Medium;
            throw new ArgumentOutOfRangeException("font", "Invalid WrappedTextBlockFont specified.");
        }

        internal static NativeHorizontalAlignment ToNative(this HorizontalAlignment horizontalAlignment)
        {
            // can't use switch on Android as this is not an enum
            if (horizontalAlignment == HorizontalAlignment.Center)
                return NativeHorizontalAlignment.Center;
            if (horizontalAlignment == HorizontalAlignment.Left)
                return NativeHorizontalAlignment.Left;
            if (horizontalAlignment == HorizontalAlignment.Right)
                return NativeHorizontalAlignment.Right;
            throw new ArgumentOutOfRangeException("horizontalAlignment", "Invalid HorizontalAlignment specified.");
        }
        internal static HorizontalAlignment FromNative(this NativeHorizontalAlignment horizontalAlignment)
        {
            // can't use switch on Android as this is not an enum
            if (horizontalAlignment == NativeHorizontalAlignment.Center)
                return HorizontalAlignment.Center;
            if (horizontalAlignment == NativeHorizontalAlignment.Left)
                return HorizontalAlignment.Left;
            if (horizontalAlignment == NativeHorizontalAlignment.Right)
                return HorizontalAlignment.Right;
            throw new ArgumentOutOfRangeException("horizontalAlignment", "Invalid HorizontalAlignment specified.");
        }

        internal static NativeVerticalAlignment ToNative(this VerticalAlignment verticalAlignment)
        {
            // can't use switch on Android as this is not an enum
            if (verticalAlignment == VerticalAlignment.Center)
                return NativeVerticalAlignment.Center;
            if (verticalAlignment == VerticalAlignment.Top)
                return NativeVerticalAlignment.Top;
            if (verticalAlignment == VerticalAlignment.Bottom)
                return NativeVerticalAlignment.Bottom;
            throw new ArgumentOutOfRangeException("verticalAlignment", "Invalid VerticalAlignment specified.");
        }
        internal static VerticalAlignment FromNative(this NativeVerticalAlignment verticalAlignment)
        {
            // can't use switch on Android as this is not an enum
            if (verticalAlignment == NativeVerticalAlignment.Center)
                return VerticalAlignment.Center;
            if (verticalAlignment == NativeVerticalAlignment.Top)
                return VerticalAlignment.Top;
            if (verticalAlignment == NativeVerticalAlignment.Bottom)
                return VerticalAlignment.Bottom;
            throw new ArgumentOutOfRangeException("verticalAlignment", "Invalid VerticalAlignment specified.");
        }

        internal static ElementData FromNative(this NativeElementData native)
        {
            return FromNative<ElementData, NativeElementData>(native, dataNativeConstructors);
        }
        internal static Element FromNative(this NativeElement native)
        {
            return FromNative<Element, NativeElement>(native, elementNativeConstructors);
        }
        internal static Panel FromNative(this NativePanel native)
        {
            return FromNative<Panel, NativePanel>(native, elementNativeConstructors);
        }
#endif
    }
}
