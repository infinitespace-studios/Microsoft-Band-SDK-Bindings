using System.Linq;
using System.Xml;

namespace Microsoft.Band.Portable.Tiles.Pages
{
    public struct Margins
    {
        public static Margins Empty = new Margins();

        public Margins(short left, short top, short right, short bottom)
            : this()
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Margins(short leftRight, short topBottom)
            : this(leftRight, topBottom, leftRight, topBottom)
        {
        }

        public Margins(short all)
            : this(all, all, all, all)
        {
        }

        public short Left { get; set; }
        public short Top { get; set; }
        public short Right { get; set; }
        public short Bottom { get; set; }

        public int Horizontal
        {
            get { return Left + Right; }
        }

        public int Vertical
        {
            get { return Top + Bottom; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Margins))
            {
                return false;
            }
            Margins margins = (Margins)obj;
            return margins == this;
        }

        public override int GetHashCode()
        {
            return Left ^ Top ^ Right ^ Bottom;
        }

        public override string ToString()
        {
            return string.Format("[Left={0}, Top={1}, Right={2}, Bottom={3}]", Left, Top, Right, Bottom);
        }

        internal static Margins FromXmlString(string xmlString)
        {
            var parts = xmlString.Split(',').Select(p => XmlConvert.ToInt16(p.Trim())).ToArray();

            if (parts.Length == 4)
            {
                return new Margins(parts[0], parts[1], parts[2], parts[3]);
            }
            else if (parts.Length == 2)
            {
                return new Margins(parts[0], parts[1]);
            }
            else if (parts.Length == 1)
            {
                return new Margins(parts[0]);
            }

            throw new XmlException("Margin was incorrectly formed. It should consist of 4, 2, or 1 comma separated Int16s.");
        }

        internal string ToXmlString()
        {
            if (Left == Right && Left == Top && Left == Bottom)
            {
                return string.Format(
                    "{0}",
                    XmlConvert.ToString(Left));
            }
            else if (Left == Right && Top == Bottom)
            {
                return string.Format(
                    "{0}, {1}",
                    XmlConvert.ToString(Left), XmlConvert.ToString(Top));
            }
            else
            {
                return string.Format(
                    "{0}, {1}, {2}, {3}",
                    XmlConvert.ToString(Left), XmlConvert.ToString(Top), XmlConvert.ToString(Right), XmlConvert.ToString(Bottom));
            }
        }

        public static bool operator ==(Margins left, Margins right)
        {
            return left.Left == right.Left && left.Top == right.Top && left.Right == right.Right && left.Bottom == right.Bottom;
        }

        public static bool operator !=(Margins left, Margins right)
        {
            return !(left == right);
        }
    }
}