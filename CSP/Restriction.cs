using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Restriction : IEquatable<Restriction>
    {
        public Restriction(Color color1, Color color2)
        {
            this.Color1 = color1;
            this.Color2 = color2;
        }

        public Color Color1 { get; }
        public Color Color2 { get; }

        public bool Contains(Color color)
        {
            return Color1 == color || Color2 == color;
        }


        public static bool operator ==(Restriction first, Restriction second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Restriction first, Restriction second) => !(first == second);

        public bool Equals(Restriction other)
        {
            return other.Contains(Color1) && other.Contains(Color2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Restriction);
        }

        public override int GetHashCode()
        {
            return Color1.GetHashCode() + Color2.GetHashCode();
        }
    }
}
