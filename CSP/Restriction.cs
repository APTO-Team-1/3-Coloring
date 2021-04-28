using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Restriction : IEquatable<Restriction>
    {
        public Restriction(Variable variable1, Color color1, Variable variable2, Color color2): this(new Pair(variable1, color1), new Pair(variable2, color2))
        {
        }

        public Restriction(Pair pair1, Pair pair2)
        {
            this.Pair1 = pair1;
            this.Pair2 = pair2;
        }

        public Pair Pair1 { get; }
        public Pair Pair2 { get; }

        public bool Contains(Color color)
        {
            return Pair1.Color == color || Pair2.Color == color;
        }
        public bool Contains(Variable variable)
        {
            return Pair1.Variable == variable || Pair2.Variable == variable;
        }

        public static bool operator ==(Restriction first, Restriction second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Restriction first, Restriction second) => !(first == second);

        public bool Equals(Restriction other)
        {
            return other.Contains(Pair1.Color) && other.Contains(Pair2.Color);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Restriction);
        }

        public override int GetHashCode()
        {
            return Pair1.Color.GetHashCode() + Pair2.Color.GetHashCode();
        }
    }
}
