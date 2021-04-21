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

            var success = ThreeColoringInternal(0);
            return success ? coloring : null;

            bool ThreeColoringInternal(int currV)
            {
                if (currV >= g.VerticesCount)
                    return true;
                
                for(int c = 1; c<=3; c++)
                {
                    bool isColorFree = true;
                    foreach(var n in g.GetNeighbors(currV))
                    {
                        if(c == coloring[n])
                        {
                            isColorFree = false;
                            break;
                        }    
                    }
                    if (isColorFree)
                    {
                        coloring[currV] = c;
                        break;
                    }
                }

                if (coloring[currV] == 0) // color not found
                    return false;

                currV++;

                return ThreeColoringInternal(currV);
            }
        }
    }
}
