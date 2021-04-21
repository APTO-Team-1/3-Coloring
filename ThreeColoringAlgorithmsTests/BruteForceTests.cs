using GraphLib.Algorithms;
using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ThreeColoringAlgorithmsTests
{
    public class BruteForceTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void Test(Graph g)
        {
            var coloring = new BruteForce().ThreeColorig(g);

            CheckColoringCorrectness(g, coloring);
        }

        private static void CheckColoringCorrectness(Graph g, int[] coloring)
        {
            var maxColor = coloring.Max();
            Assert.True(maxColor >= 1 && maxColor <= 3);

            for(int i =0; i< g.VerticesCount; i++)
                foreach(var j in g.GetNeighbors(i)) 
                    Assert.NotEqual(coloring[i], coloring[j]);   
        }

        #region getting data

        public static IEnumerable<object[]> GetData()
        {
            var data = new List<object[]>
            {
            new object[] { GetSimpleGraph() },
            new object[] { GetCycleGraph() },
            };

            return data;
        }

        private static Graph GetSimpleGraph()
        {
            Graph g = new(5);
            g.AddEdge(0, 1);
            g.AddEdge(0, 2);
            g.AddEdge(1, 4);
            g.AddEdge(4, 3);
            g.AddEdge(2, 3);
            g.AddEdge(1, 3);

            return g;
        }

        private static Graph GetCycleGraph()
        {
            Graph g = new(6);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 4);
            g.AddEdge(4, 5);
            g.AddEdge(5, 0);

            return g;
        }

        #endregion


    }
}
