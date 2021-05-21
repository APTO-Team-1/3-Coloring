using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    public class Color
    {
        public Color(int value)
        {
            this.Value = value;
        }

        public int Value { get; set; }

        private readonly HashSet<Pair> restrictions = new();

        public IReadOnlySet<Pair> Restrictions { get => restrictions; }

        internal bool AddRestriction(Pair pair)
        {
            return restrictions.Add(pair);
        }
        internal bool RemoveRestriction(Pair pair)
        {
            return restrictions.Remove(pair);
        }

        public bool IsNeighborOf(Color color)
        {
            return Restrictions.Any(r => r.Color == color);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
