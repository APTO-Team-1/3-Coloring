using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeColoring.Algorithms;

namespace GraphLib.Algorithms
{
    public class BruteForce: IThreeColoringAlgorithm
    {
        public int[] ThreeColorig(Graph g)
        {
            int[] coloring = new int[g.VerticesCount];
            for (int i = 0; i < coloring.Length; i++) coloring[i] = -1;

            var success = ThreeColoringInternal(0);
            return success ? coloring : null;

            bool ThreeColoringInternal(int currV)
            {
                if (currV >= g.VerticesCount)
                    return true;

                bool[] isColorFree = new bool[] { true, true, true };

                foreach (var n in g.GetNeighbors(currV))
                {
                    if(coloring[n] != -1)
                        isColorFree[coloring[n]] = false;
                }
                for (int i = 0; i < isColorFree.Length; i++)
                {
                    if (isColorFree[i])
                    {
                        coloring[currV] = i;
                        if (ThreeColoringInternal(currV + 1)) return true;
                        
                        int v = currV+1;
                        do
                        {
                            if (coloring[v] == -1) break;
                            coloring[v++] = -1;
                        } while (v < coloring.Length);
                    }
                }
                return false;
            }
        }
    }
}
