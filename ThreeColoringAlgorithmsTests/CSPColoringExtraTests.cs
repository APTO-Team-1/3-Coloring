using GraphLib.Algorithms;
using GraphLib.Definitions;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ThreeColoringAlgorithms;
using Xunit;
using Xunit.Abstractions;
using System.Diagnostics;

namespace ThreeColoringAlgorithmsTests
{
    public class CSPColoringExtraTests
    {     
        public static Graph GenerateGraph(int verticesCount = 100, int? minNeighbours = null, int? maxNeighbours = null, int? delta = null, 
            double? verticesWithHighDegree = null, bool? isColorable = null, int? randomSeed = null)
        {
            int min, max;
            min = minNeighbours.HasValue ? minNeighbours.Value : 0;
            max = maxNeighbours.HasValue ? maxNeighbours.Value + 1 : verticesCount - 1;
            double bigDeltaVChance = verticesWithHighDegree.HasValue ? verticesWithHighDegree.Value : 0.001;
            Random r = randomSeed.HasValue ? new(randomSeed.Value) : new();

            bool colorable = isColorable.HasValue ? isColorable.Value : false;
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
                    int neighbours = delta.HasValue ? (r.Next(min, max ) + delta.Value) / 2 : r.Next(min, max);
                    if (bigDeltaVChance > r.NextDouble()) neighbours = max < verticesCount / 10 ? max * 2 : max;
                    if (edges_count > max_edges) break;
                    for (int j = adjacencyList[i].Count; j < neighbours; j++)
                    {
                        int counter = 0;
                        int neighbour = r.Next(i+1, verticesCount);
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
                    int neighbours = delta.HasValue? (r.Next(min, max ) + delta.Value )/2 : r.Next(min, max + 1);
                    if (bigDeltaVChance > r.NextDouble()) neighbours = max < verticesCount / 10 ? max * 2 : max;
                    if (edges_count > max_edges) break;                
                    for (int j = adjacencyList[i].Count; j < neighbours; j++)
                    {
                        int counter = 0;
                        int neighbour = r.Next(i+1, verticesCount);
                        while (counter < 10 && adjacencyList[neighbour].Count >= maxNeighbours )
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
        private static void CheckAndWriteOutput(Graph g)
        {
            Stopwatch sw = new();
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            var found = super == null ? "No" : "Yes";
            if (super != null)
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            File.AppendAllLines(@"..", new string[]
            {$"Graph with {g.VerticesCount} vertices:"
                , $"CSP   time: {sw.Elapsed}. Found:  {found}"
                ,"-----------------------------------------------"});
        }
        public static IEnumerable<object[]> GetRandomGraphs100Vertices()
        {
            int vertices = 100;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta:4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 6, verticesWithHighDegree: 0.01, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 3, verticesWithHighDegree: 0.05, isColorable: false);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.01, isColorable: false);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetRandomGraphs200Vertices()
        {
            int vertices = 200;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 5, verticesWithHighDegree: 0.01, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 3, verticesWithHighDegree: 0.04, isColorable: false);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.01, isColorable: false);
                data.Add(new[] { g });
            }
            return data;
        }
        public static IEnumerable<object[]> GetRandomGraphs300Vertices()
        {
            int vertices = 300;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 5, verticesWithHighDegree: 0.01, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 3, verticesWithHighDegree: 0.03, isColorable: false);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.01, isColorable: false);
                data.Add(new[] { g });
            }
            return data;
        }
        public static IEnumerable<object[]> GetRandomGraphs400Vertices()
        {
            int vertices = 400;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.005, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 3, verticesWithHighDegree: 0.03, isColorable: false);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.02, isColorable: false);
                data.Add(new[] { g });
            }
            return data;
        }
        public static IEnumerable<object[]> GetRandomGraphs500Vertices()
        {
            int vertices = 500;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.005, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 2, verticesWithHighDegree: 0.02, isColorable: false);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 5, verticesWithHighDegree: 0.02, isColorable: false);
                data.Add(new[] { g });
            }
            return data;
        }
        public static IEnumerable<object[]> GetRandomGraphs600Vertices()
        {
            int vertices = 600;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.004, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 2, verticesWithHighDegree: 0.02, isColorable: false);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 5, verticesWithHighDegree: 0.02, isColorable: false);
                data.Add(new[] { g });
            }
            return data;
        }
        public static IEnumerable<object[]> GetRandomGraphs700Vertices()
        {
            int vertices = 700;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.004, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 2, verticesWithHighDegree: 0.01);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 5, verticesWithHighDegree: 0.01);
                data.Add(new[] { g });
            }
            return data;
        }

        [Fact]
        public void GeneratorTest()
        {
            Graph g = GenerateGraph(verticesCount: 200,isColorable: true, maxNeighbours: 5);
            Assert.NotNull(g);
        }

        [Theory]
        [MemberData(nameof(GetRandomGraphs100Vertices))]
        public void RandomGraphs100VerticesTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs200Vertices))]
        public void RandomGraphs200VerticesTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs300Vertices))]
        public void RandomGraphs300VerticesTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs400Vertices))]
        public void RandomGraphs400VerticesTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs500Vertices))]
        public void RandomGraphs500VerticesTest(Graph g)
        {
            for (int i = 700; i <= 700; i += 100)
                for (int j = 0; j < 1; j++)
                {
                    Graph g = GenerateGraph(verticesCount: i, maxNeighbours: 5, minNeighbours: 3,
                        isColorable: new Random().NextDouble() > 0.5 ? true : false, randomSeed: 1);
                    CheckAndWriteOutput(g);
                }
            CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs600Vertices))]
        public void RandomGraphs600VerticesTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }
        //[Theory]
        //[MemberData(nameof(GetRandomGraphs700Vertices))]
        //public void RandomGraphs700VerticesTest(Graph g)
        //{
        //    CheckAndWriteOutput(g);
        //}
    }
}
