using GraphLib.Algorithms;
using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ThreeColoringAlgorithmsTests
{
    public class ColoringTestUtils
    {

        public static void CheckColoringCorrectness(Graph g, int[] coloring)
        {
            Assert.NotNull(coloring);
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
            new object[] { NoColoring2() },
            new object[] { NoColoring3() },
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

        private static Graph NoColoring2()
        {
            int veritceCount = 40;
            Graph g = new(veritceCount);
            for (int i = 0; i < veritceCount; i++)
            {
                g.AddEdge(i, (i + 1) % veritceCount);
                g.AddEdge(i, (i + 3) % veritceCount);
                g.AddEdge(i, (i + 8) % veritceCount);
                g.AddEdge(i, (i + 11) % veritceCount);
            }
            return g;
        }

        private static Graph NoColoring3()
        {
            int veritceCount = 150;
            Graph g = new(veritceCount);
            g.AddEdge(100, 101);
            g.AddEdge(100, 102);
            g.AddEdge(100, 103);
            g.AddEdge(101, 102);
            g.AddEdge(101, 103);
            g.AddEdge(102, 103);
            
            for (int i = 0; i < veritceCount * 15; i++)
            {
                int v1 = new Random().Next(0, veritceCount - 1);
                int v2 = new Random().Next(0, veritceCount - 1);
                if (g.ContainsEdge(v1, v2)) continue;
                g.AddEdge(v1, v2);
            }
            return g;
        }

        #endregion


    }
}
