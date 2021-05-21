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

        public Color(int value, IEnumerable<Pair> restrictions)
        {
            this.Value = value;
            foreach (var restriction in restrictions)
            {
                this.restrictions.Add(restriction);
            }
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
    }
}
