using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLib.Definitions
{
    public class Edge
    {
        public int From { get; }
        public int To { get; }

        public Edge(int from, int to)
        {
            From = from;
            To = to;
        }

    }
}
