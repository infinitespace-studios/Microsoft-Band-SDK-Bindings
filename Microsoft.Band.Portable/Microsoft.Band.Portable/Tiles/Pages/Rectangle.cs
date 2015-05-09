using System.Linq;
using System.Xml;

namespace Microsoft.Band.Portable.Tiles.Pages
{
    public struct Rectangle
    {
        public readonly static Rectangle Empty = new Rectangle();

        public Rectangle(short x, short y, short width, short height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public short Y { get; set; }
        public short X { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }

        public Point Location
        {
            get { return new Point(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Size Size
        {
            get { return new Size(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public bool IsEmpty
        {
            get { return X == 0 && Y == 0 && Width == 0 && Height == 0; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle))
            {
                return false;
            }
            Rectangle Rectangle = (Rectangle)obj;
            return Rectangle == this;
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Width ^ Height;
        }

        public override string ToString()
        {
            return string.Format("[X={0}, Y={1}, Width={2}, Height={3}]", X, Y, Width, Height);
        }

        internal static Rectangle FromXmlString(string xmlString)
        {
            var parts = xmlString.Split(',').Select(p => short.Parse(p.Trim())).ToArray();

            if (parts.Length == 4)
            {
                return new Rectangle(parts[0], parts[1], parts[2], parts[3]);
            }

            throw new XmlException("Rectangle was incorrectly formed. It should consist of 4 comma separated Int16s.");
        }

        internal string ToXmlString()
        {
            return string.Format(
                "{0}, {1}, {2}, {3}",
                XmlConvert.ToString(X), XmlConvert.ToString(Y), XmlConvert.ToString(Width), XmlConvert.ToString(Height));
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }
    }
}