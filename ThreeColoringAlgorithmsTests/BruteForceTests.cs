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
        public void TestColoringSuccesfull(Graph g)
        {
            var coloring = new BruteForce().ThreeColorig(g);

            CheckColoringCorrectness(g, coloring);
        }
        [Theory]
        [MemberData(nameof(GetDataFailure))]
        public void TestColoringImposible(Graph g)
        {
            Assert.Null(new BruteForce().ThreeColorig(g));
        }

        private static void CheckColoringCorrectness(Graph g, int[] coloring)
        {
            var maxColor = coloring.Max();
            Assert.True(maxColor >= 0 && maxColor <= 2);

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
            new object[] { GetGraph1() },
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

        private static Graph GetGraph1()
        {
            int veritceCount = 40;
            Graph g = new(veritceCount);
            for (int i = 0; i < veritceCount; i++)
            {
                g.AddEdge(i, (i + 1) % veritceCount);
                g.AddEdge(i, (i + 3) % veritceCount);
                g.AddEdge(i, (i + 11) % veritceCount);
            }
            return g;
        }

        public static IEnumerable<object[]> GetDataFailure()
        {
            var data = new List<object[]>
            {
            new object[] { NoColoring1() },
            };

            return data;
        }

        private static Graph NoColoring1()
        {
            Graph g = new(4);
            g.AddEdge(0, 1);
            g.AddEdge(0, 2);
            g.AddEdge(0, 3);
            g.AddEdge(1, 2);
            g.AddEdge(1, 3);
            g.AddEdge(2, 3);

            return g;
        }

        #endregion


    }
}
