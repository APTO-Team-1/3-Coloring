using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Color
    {
        public Color(int value)
        {
            this.Value = value;
        }
        public Color(int value, IEnumerable<Color> restrictions)
        {
            this.Value = value;
            foreach (var restriction in restrictions)
            {
                this.restrictions.Add(restriction);
            }
        }

        public int Value { get; set; }

        private HashSet<Color> restrictions = new HashSet<Color>();

        public IReadOnlySet<Color> Restrictions { get => restrictions; } 

        internal void AddRestriction(Color color)
        {
            restrictions.Add(color);
        }
        internal void RemoveRestriction(Color color)
        {
            restrictions.Remove(color);
        }
    }
}
