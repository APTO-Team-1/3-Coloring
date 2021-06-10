using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ThreeColoringAlgorithms;
using Xunit;

namespace ThreeColoringAlgorithmsTests
{
    public class ColoringTestUtils
    {
        public static Graph GenerateGraph(int verticesCount = 100, int? minNeighbours = null, int? maxNeighbours = null, int? delta = null,
           double? verticesWithHighDegree = null, bool? isColorable = null, int? randomSeed = null)
        {
            int min, max;
            min = minNeighbours ?? 0;
            max = maxNeighbours.HasValue ? maxNeighbours.Value + 1 : verticesCount - 1;
            double bigDeltaVChance = verticesWithHighDegree ?? 0.001;
            Random r = randomSeed.HasValue ? new(randomSeed.Value) : new();

            bool colorable = isColorable ?? false;
            int[] coloring;
            HashSet<int>[] adjacencyList = new HashSet<int>[verticesCount];
            int edges_count = 0;
            int max_edges = verticesCount * (verticesCount - 1) / 4;

            for (int i = 0; i < verticesCount; i++)
            {
                adjacencyList[i] = new HashSet<int>();
            }
            if (colorable)
            {
                coloring = new int[verticesCount];
                for (int i = 0; i < verticesCount; i++)
                {
                    coloring[i] = r.Next(0, 3);
                }

                for (int i = 0; i < verticesCount - max; i++)
                {
                    int neighbours = delta.HasValue ? (r.Next(min, max) + delta.Value) / 2 : r.Next(min, max);
                    if (bigDeltaVChance > r.NextDouble()) neighbours = max < verticesCount / 10 ? max * 2 : max;
                    if (edges_count > max_edges) break;
                    for (int j = adjacencyList[i].Count; j < neighbours; j++)
                    {
                        int counter = 0;
                        int neighbour = r.Next(i + 1, verticesCount);
                        while (counter < 10 && adjacencyList[neighbour].Count >= maxNeighbours && coloring[neighbour] == coloring[i])
                        {
                            counter++;
                            neighbour = r.Next(i + 1, verticesCount);
                        }
                        if (counter >= 9) continue;
                        adjacencyList[i].Add(neighbour);
                        adjacencyList[neighbour].Add(i);
                        edges_count++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < verticesCount - max; i++)
                {
                    int neighbours = delta.HasValue ? (r.Next(min, max) + delta.Value) / 2 : r.Next(min, max + 1);
                    if (bigDeltaVChance > r.NextDouble()) neighbours = max < verticesCount / 10 ? max * 2 : max;
                    if (edges_count > max_edges) break;
                    for (int j = adjacencyList[i].Count; j < neighbours; j++)
                    {
                        int counter = 0;
                        int neighbour = r.Next(i + 1, verticesCount);
                        while (counter < 10 && adjacencyList[neighbour].Count >= maxNeighbours)
                        {
                            counter++;
                            neighbour = r.Next(i + 1, verticesCount);
                        }
                        if (counter >= 9) continue;
                        adjacencyList[i].Add(neighbour);
                        adjacencyList[neighbour].Add(i);
                        edges_count++;
                    }
                }
            }
            Graph g = new(adjacencyList);
            return g;
        }

        public static void CheckAndWriteOutput(Graph g)
        {
            Stopwatch sw = new();
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            var found = super == null ? "No" : "Yes";
            if (super != null)
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            File.AppendAllLines(@"..\result.txt", new string[]
            {$"Graph with {g.VerticesCount} vertices:"
                , $"CSP   time: {sw.Elapsed}. Found:  {found}"
                ,"-----------------------------------------------"});
        }

        public static void CheckColoringCorrectness(Graph g, int[] coloring)
        {
            Assert.NotNull(coloring);
            var maxColor = coloring.Length > 0 ? coloring.Max() : 0;
            Assert.True(maxColor >= 0 && maxColor <= 2);

            for (int i = 0; i < g.VerticesCount; i++)
                foreach (var j in g.GetNeighbors(i))
                    Assert.NotEqual(coloring[i], coloring[j]);
        }

        #region getting data

        public static IEnumerable<object[]> GetData()
        {
            var data = new List<object[]>
            {
            new object[] { GetSimpleGraph() },
            new object[] { GetSimpleGraph2() },
            new object[] { GetCycleGraph() },
            new object[] { GetGraph1() },
            };

            return data;
        }
        private static Graph GetSimpleGraph()
        {
            Graph g = new(3);
            g.AddEdge(0, 1);
            g.AddEdge(0, 2);
            g.AddEdge(1, 2);

            return g;
        }

        private static Graph GetSimpleGraph2()
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
            new object[] { NoColoring11() },
            new object[] { NoColoring1() },
            new object[] { NoColoring2() },
            new object[] { NoColoring3() },
            };

            return data;
        }
        private static Graph NoColoring11()
        {
            int veritceCount = 7;
            Graph g = new(veritceCount);
            g.AddEdge(3, 4);
            g.AddEdge(3, 5);
            g.AddEdge(3, 6);
            g.AddEdge(4, 5);
            g.AddEdge(4, 6);
            g.AddEdge(5, 6);

            for (int i = 0; i < veritceCount * 15; i++)
            {
                int v1 = new Random().Next(0, veritceCount - 1);
                int v2 = new Random().Next(0, veritceCount - 1);
                if (g.ContainsEdge(v1, v2)) continue;
                g.AddEdge(v1, v2);
            }
            return g;
        }
        private static Graph NoColoring1()
        {
            Graph g = new(6);
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
