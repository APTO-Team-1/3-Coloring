using GraphLib.Algorithms;
using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreeColoringAlgorithms;
using Xunit;
using Xunit.Abstractions;
using System.Diagnostics;

namespace ThreeColoringAlgorithmsTests
{
    public class ColoringTests
    {
        private readonly ITestOutputHelper output;

        public ColoringTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private void CheckAndWriteOutput(Graph g, string additionalInfo = "")
        {
            if(additionalInfo != "") output.WriteLine(additionalInfo);
            Stopwatch sw = new();
            sw.Reset();
            sw.Start();
            var brut = new BruteForce().ThreeColorig(g);
            sw.Stop();
            output.WriteLine($"Graph with {g.VerticesCount} vertices:");
            var found = brut == null ? "No" : "Yes";
            output.WriteLine($"Brute time: {sw.Elapsed}. Found:  {found}");
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            found = super == null ? "No" : "Yes";
            output.WriteLine($"CSP   time: {sw.Elapsed}. Found:  {found}");
            output.WriteLine("-----------------------------------------------");
            if (brut == null)
                Assert.Null(super);
            else
                ColoringTestUtils.CheckColoringCorrectness(g, super);
        }

        private void CheckAndWriteOutputOnlyCSP(Graph g, string additionalInfo = "")
        {
            if (additionalInfo != "") output.WriteLine(additionalInfo);
            Stopwatch sw = new();
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            output.WriteLine($"Graph with {g.VerticesCount} vertices:");
            var found = super == null ? "No" : "Yes";
            output.WriteLine($"CSP   time: {sw.Elapsed}. Found:  {found}");
            output.WriteLine("-----------------------------------------------");
            if (super != null)
                ColoringTestUtils.CheckColoringCorrectness(g, super);
        }

        #region DENSE GRAPHS
        public static Graph GetRandomGraph(int vericesCount = 20, int restrictionPercentage = 10, int? randomSeed = null)
        {
            Graph g = new(vericesCount);
            Random r = randomSeed.HasValue ? new(randomSeed.Value) : new();
            for (int i = 0; i < vericesCount * (vericesCount - 1) * restrictionPercentage / 100; i++)
            {
                int a = r.Next(g.VerticesCount);
                int b = r.Next(g.VerticesCount);
                g.AddEdge(a, b);
            }
            return g;
        }

        public static IEnumerable<object[]> GetRandomGraphs()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 100; i++)
            {
                g = GetRandomGraph(restrictionPercentage: i, vericesCount: i, randomSeed:i);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetBigRandomGraphs()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 20; i < 100; i+=30)
            {
                g = GetRandomGraph(restrictionPercentage: i, vericesCount: 2000, randomSeed: i);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetBigCycles()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 5000; i <= 10000; i += 1000)
            {
                g = new Graph(i);
                g.AddEdge(i - 1, 0);
                for (int j = 0; j < i - 1; j++)
                {
                    g.AddEdge(j, j + 1);
                }
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetBigTrees()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 5000; i <= 10000; i += 1000)
            {
                g = new Graph(i);
                int son = 1;
                Queue<int> fathers = new Queue<int>();
                fathers.Enqueue(0);
                while (son < i - 1)
                {
                    int currentFather;
                    if (!fathers.TryDequeue(out currentFather)) break;
                    int count = new Random().Next(1, 30);
                    for (int j = 0; j < count; j++)
                    {
                        g.AddEdge(currentFather, son);
                        fathers.Enqueue(son);
                        son++;
                        if (son >= i - 1) break;
                    }
                }
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetBigBipartite()
        {
            var data = new List<object[]>();
            Graph g;
            int vertice_count = 2000;
            for (int i = 20; i <= 80; i += 30)
            {
                g = new Graph(vertice_count);
                Random r = new Random(i);
                int A = new Random().Next(1, 5);
                for (int k = 0; k < vertice_count * (vertice_count - 1) * i / 10; k++)
                {
                    int a = r.Next((int)(vertice_count * A / 10));
                    int b = r.Next((int)(vertice_count * A / 10), vertice_count - 1);
                    g.AddEdge(a, b);
                }
                data.Add(new[] { g });
            }
            return data;
        }

        [Theory]
        [MemberData(nameof(GetRandomGraphs))]
        public void TestColoring(Graph g)
        {
            CheckAndWriteOutput(g);
        }

        [Fact]
        public void RandomGraphsTo1000VerticesTest()
        {
            for (int i = 50; i <= 1000; i += 50)
                for (int j = 10; j <= 90; j += 20)
                {
                    Graph g = GetRandomGraph(i, j);
                    CheckAndWriteOutput(g);
                }
        }
        [Fact]
        public void  CycleGraphTo500VerticesTest()
        {
            for (int i = 50; i <= 500; i += 50)
            {
                Graph g = new Graph(i);
                g.AddEdge(i - 1, 0);
                for (int j = 0; j < i - 1; j++)
                {
                    g.AddEdge(j, j + 1);
                }
                CheckAndWriteOutput(g);

            }
        }

        [Fact]
        public void CliqueTo1000VerticesTest()
        { 
            for (int i = 2; i <= 1000; i=i<10?i+1:i+50)
            {
                Graph g = new Graph(i);
                
                for (int j = 0; j < i; j++)
                {
                    for (int k = j + 1; k < i; k++)
                        g.AddEdge(j, k);
                }
                CheckAndWriteOutput(g);
            }
        }

        [Fact]
        public void BigCliqueTest()
        {
            for (int i = 1500; i <= 2000; i +=500)
            {
                Graph g = new Graph(i);

                for (int j = 0; j < i; j++)
                {
                    for (int k = j + 1; k < i; k++)
                        g.AddEdge(j, k);
                }
                CheckAndWriteOutput(g);
                if (i == 10) i = 50;
            }
        }

        [Fact]
        public void TreeTo500VerticesTest()
        {
            for (int i = 50; i <= 500; i+=50)
            {
                Graph g = new Graph(i);
                int son = 1;
                Queue<int> fathers = new Queue<int>();
                fathers.Enqueue(0);
                while(son < i - 1 )
                {
                    int currentFather;
                    if (!fathers.TryDequeue(out currentFather)) break;
                    int count = new Random().Next(1,15);
                    for (int j = 0; j < count; j++)
                    {
                        g.AddEdge(currentFather, son);
                        fathers.Enqueue(son);
                        son++;
                        if (son >= i - 1) break;
                    }
                }
                CheckAndWriteOutput(g);
            }
        }
        [Fact]
        public void BipartieGraphTest()
        {
            for (int vertice_count = 50; vertice_count <= 500; vertice_count += 50)
            {
                for(int res_percent = 2; res_percent < 9; res_percent+=2)
                {
                    Graph g = new Graph(vertice_count);
                    Random r = new Random();
                    int A = new Random().Next(1, 5);
                    for (int k = 0; k < vertice_count * (vertice_count - 1) * res_percent / 10; k++)
                    {
                        int a = r.Next((int)(vertice_count*A/10));
                        int b = r.Next((int)(vertice_count * A / 10),vertice_count-1);
                        g.AddEdge(a, b);
                    }
                    CheckAndWriteOutput(g);
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetBigCycles))]
        public void BigCycleTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }

       

        [Theory]
        [MemberData(nameof(GetBigTrees))]
        public void BigTreeTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }

        [Theory]
        [MemberData(nameof(GetBigRandomGraphs))]
        public void BigRandomGraphTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }

        [Theory]
        [MemberData(nameof(GetBigBipartite))]
        public void BigBipartiteTest(Graph g)
        {
            CheckAndWriteOutput(g);
        }       
        #endregion

        public static Graph GetGraphWithFewNeighbors(int vericesCount = 20, int? randomSeed = null)
        {
            Graph g = new(vericesCount);
            Random r = randomSeed.HasValue ? new(randomSeed.Value) : new();
            for (int i = 0; i < vericesCount; i++)
            {
                var n = r.Next(vericesCount);
                while (n == i) n = r.Next(vericesCount);
                g.AddEdge(i, n);
                n = r.Next(vericesCount);
                while (n == i) n = r.Next(vericesCount);
                g.AddEdge(i, n);
            }
            return g;
        }

        public static Graph GenerateGraphWithApproximateNeighbours(int verticesCount = 20,int approximateNeighbours = 3, int? randomSeed = null)
        {           
            Random r = randomSeed.HasValue ? new(randomSeed.Value) : new();
            int[] coloring = new int[verticesCount];
            HashSet<int>[] adjacencyList = new HashSet<int>[verticesCount];
            for (int i = 0; i < verticesCount; i++) 
            {
                coloring[i] = -1;
                adjacencyList[i] = new HashSet<int>();
            }

            coloring[0] = 0;

            for(int i = 0; i < verticesCount; i++)
            {
                if (coloring[i] == -1) coloring[i] = r.Next(0, 3);
                int neighbours = approximateNeighbours - adjacencyList[i].Count;
                for (int j=0;j<neighbours;j++)
                {
                    int neighbour = r.Next(0, verticesCount);
                    int counter = 0;
                    while(counter < 10 && coloring[neighbour] == coloring[i])
                    {
                        neighbour = r.Next(0, verticesCount);
                        counter++;
                    }
                    if (counter == 10) continue;
                    coloring[neighbour] = r.NextDouble() > 0.5 ? (coloring[i] + 2) % 3 : (coloring[i] + 1) % 3;
                    adjacencyList[i].Add(neighbour);
                    adjacencyList[neighbour].Add(i);
                }
            }

            Graph g = new(adjacencyList);
            return g;
        }

        public static Graph GenerateGraphWithRandomNeighbours(int verticesCount = 20, int maxNeighbours = 10, int? randomSeed = null)
        {
            Random r = randomSeed.HasValue ? new(randomSeed.Value) : new();
            int[] coloring = new int[verticesCount];
            HashSet<int>[] adjacencyList = new HashSet<int>[verticesCount];
            for (int i = 0; i < verticesCount; i++)
            {
                coloring[i] = -1;
                adjacencyList[i] = new HashSet<int>();
            }

            coloring[0] = 0;

            for (int i = 0; i < verticesCount; i++)
            {
                if (coloring[i] == -1) coloring[i] = r.Next(0, 3);
                int neighbours = r.Next(1, maxNeighbours);
                for (int j = 0; j < neighbours; j++)
                {
                    int neighbour = r.Next(0, verticesCount);
                    int counter = 0;
                    while (counter < 10 && coloring[neighbour] == coloring[i])
                    {
                        neighbour = r.Next(0, verticesCount);
                        counter++;
                    }
                    if (counter == 10) continue;
                    coloring[neighbour] = r.NextDouble() > 0.5 ? (coloring[i] + 2) % 3 : (coloring[i] + 1) % 3;
                    adjacencyList[i].Add(neighbour);
                    adjacencyList[neighbour].Add(i);
                    if (adjacencyList[i].Count >= maxNeighbours) break;
                }
            }

            Graph g = new(adjacencyList);
            return g;
        }

        [Fact]
        public void FewNeighborsTest()
        {
            for(int i = 50; i<=100; i+=10)
            {
                Graph g = GetGraphWithFewNeighbors(i);
                CheckAndWriteOutput(g);
            }
        }

        [Fact]
        public void ApproximateNeighboursSmallTest()
        {
            for(int neigh = 2; neigh <= 6; neigh++)
            {
                for (int i = 80; i <= 80; i += 10 )
                {                
                    Graph g = GenerateGraphWithApproximateNeighbours(i, neigh);    
                    CheckAndWriteOutput(g, $"Grpah with approximate {neigh} neighbours");
                }
            }          
        }
        [Fact]
        public void RandomNeighboursSmallTest()
        {
            for (int i = 15; i <= 75; i += 5)
            {
                int maxNeigh = i > 40 ? 7 : 4;
                Graph g = GenerateGraphWithRandomNeighbours(i, maxNeigh);
                CheckAndWriteOutput(g, $"Random graph with max {maxNeigh} neighbours");
            }
        }

        public static IEnumerable<object[]> GetRandomGraphs100V()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 3; i < 7; i++)
            {
                g = GenerateGraphWithRandomNeighbours(100,i);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetRandomGraphs200V()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 3; i < 10; i++)
            {
                g = GenerateGraphWithRandomNeighbours(200, i);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetRandomGraphs500V()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 6; i < 9; i++)
            {
                g = GenerateGraphWithRandomNeighbours(300, i);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetRandomGraphs1000V()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 10; i <= 10; i++)
            {
                g = GenerateGraphWithRandomNeighbours(500, i);
                data.Add(new[] { g });
            }
            return data;
        }

        public static IEnumerable<object[]> GetRandomGraphs3or4neighs()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 650; i <= 1000; i+=50)
            {
                g = GenerateGraphWithApproximateNeighbours(i, new Random().Next(3,5),i+ 97);
                data.Add(new[] { g });
            }
            return data;
        }
        public static IEnumerable<object[]> GetRandomGraphs3or4neighs22()
        {
            var data = new List<object[]>();
            Graph g;
            for (int i = 650; i <= 1000; i += 50)
            {
                g = GenerateGraphWithApproximateNeighbours(i, new Random().Next(3, 5), i + 97);
                data.Add(new[] { g });
            }
            return data;
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs3or4neighs))]
        public void ThreeOrFourNeighsTEst(Graph g)
        {
            CheckAndWriteOutputOnlyCSP(g, $"Random graph with 3 or 4 neighs approximately");
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs3or4neighs))]
        public void Rand3or4neighs(Graph g)
        {
            CheckAndWriteOutputOnlyCSP(g, "3 or 4  neighs");
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs3or4neighs22))]
        public void Rand3or4neigh(Graph  g)
        {
            CheckAndWriteOutputOnlyCSP(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs100V))]
        public void HoundredVerticesRandTest(Graph g)
        {
            CheckAndWriteOutputOnlyCSP(g, $"Random graph");
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs200V))]
        public void TwoHoundredVerticesRandTest(Graph g)
        {
            CheckAndWriteOutputOnlyCSP(g, $"Random graph");
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs500V))]
        public void FiveHoundredVerticesRandTest(Graph g)
        {
            CheckAndWriteOutputOnlyCSP(g, $"Random graph");
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs1000V))]
        public void ThousandVerticesRandTest(Graph g)
        {
            CheckAndWriteOutputOnlyCSP(g, $"Random graph");
        }
    }

}
