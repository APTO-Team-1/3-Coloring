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
            File.AppendAllLines(@"C:\Users\micha\source\repos\3-Coloring\Output.txt", new string[]
            {$"Graph with {g.VerticesCount} vertices:"
                , $"CSP   time: {sw.Elapsed}. Found:  {found}"
                ,"-----------------------------------------------"});
        }
        public static IEnumerable<object[]> GetRandomColorableGraphsTo200Vertices()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i <= 10; i++)
            {
                g = GenerateGraph(verticesCount: 100 + i * 10, maxNeighbours: 5, verticesWithHighDegree:0.01, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: 100 + i * 10, maxNeighbours: 8, delta: 4, verticesWithHighDegree: 0.01, isColorable: true);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetRandomGraphsWithAround4VerticesTo600V()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 1; i <= 6; i++)
            {
                g = GenerateGraph(verticesCount: 100 * i, maxNeighbours: 6, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: 100 * i, maxNeighbours: 6, delta: 4, isColorable: false);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetRandomGraphsWithAround4VerticesTo1000V()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 9; i <= 10; i++)
            {
                g = GenerateGraph(verticesCount: 100 * i, maxNeighbours: 6, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = GenerateGraph(verticesCount: 100 * i, maxNeighbours: 6, delta: 4, isColorable: false);
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
        [MemberData(nameof(GetRandomColorableGraphsTo200Vertices))]
        public void RandomColorableGraphsTo200VerticesTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }

        [Theory]
        [MemberData(nameof(GetRandomGraphsWithAround4VerticesTo600V))]
        public void RandomGraphsAround4VerticesTo600V(Graph g)
        {
            CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphsWithAround4VerticesTo1000V))]
        public void RandomGraphsAround4VerticesTo1000V(Graph g)
        {
            CheckAndWriteOutput(g);
        }
        [Fact]
        public void RandomGraphsPart1()
        {
            for(int i = 700; i <= 700; i+=100)
                for(int j=0;j<1;j++)
                {
                    Graph g = GenerateGraph(verticesCount: i, maxNeighbours: 5,minNeighbours:3,
                        isColorable: new Random().NextDouble() > 0.5 ? true : false);
                    CheckAndWriteOutput(g);
                }
        }
        [Fact]
        public void RandomGraphsPart2()
        {
            for (int i = 700; i <= 700; i += 100)
                for (int j = 0; j < 1; j++)
                {
                    Graph g = GenerateGraph(verticesCount: i, maxNeighbours: 5, minNeighbours: 3,
                        isColorable: new Random().NextDouble() > 0.5 ? true : false);
                    CheckAndWriteOutput(g);
                }
        }
    }
}
