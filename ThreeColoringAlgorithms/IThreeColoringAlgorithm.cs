using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeColoring.Algorithms
{
    interface IThreeColoringAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <returns>An array of colors corresponding to vertices of <paramref name="g"/> starting with 0.</returns>
        public int[] ThreeColorig(Graph g);
    }
}
