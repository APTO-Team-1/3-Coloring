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

        public static Graph GetGraphWithFewNeighbors(int vericesCount = 20, int? randomSeed = null)
        {
            Graph g = new(vericesCount);
            Random r = randomSeed.HasValue ? new(randomSeed.Value) : new();
            for (int i = 0; i < vericesCount; i++)
            {
                var n = r.Next(vericesCount);
                while (n==i) n = r.Next(vericesCount);
                g.AddEdge(i, n);
                n = r.Next(vericesCount);
                while (n == i) n = r.Next(vericesCount);
                g.AddEdge(i, n);
            }
            return g;
        }

        [Fact]
        public void FewNeighbors()
        {
            Graph g = GetGraphWithFewNeighbors(100);
            Stopwatch sw = new();
            sw.Reset();
            sw.Start();
            var brut = new BruteForce().ThreeColorig(g);
            sw.Stop();
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
            {
                Assert.Null(super);
            }
            else
            {
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            }
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
            Stopwatch sw = new();
            sw.Start();
            var brut = new BruteForce().ThreeColorig(g);
            sw.Stop();
            output.WriteLine("-----------------------------------------------");
            output.WriteLine("Brute time: " + sw.Elapsed);
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            output.WriteLine("CSP time  : " + sw.Elapsed);
            output.WriteLine("-----------------------------------------------");
            if (brut == null)
            {
                Assert.Null(super);
            }
            else
            {
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            }

        }

        [Fact]
        public void RandomGraphsTo1000VerticesTest()
        {
            Stopwatch sw = new();
            for (int i = 50; i <= 1000; i += 50)
                for (int j = 10; j <= 90; j += 20)
                {
                    sw.Reset();
                    Graph g = GetRandomGraph(i, j);
                    sw.Start();
                    var brut = new BruteForce().ThreeColorig(g);
                    sw.Stop();
                    output.WriteLine($"Radnom Graph with {i} vertices and {j} edge percentage:");
                    var found =  brut == null ? "No" : "Yes";
                    output.WriteLine($"Brute time: {sw.Elapsed}. Found:  {found}");
                    sw.Reset();
                    sw.Start();
                    var super = new CspColoring().ThreeColorig(g);
                    sw.Stop();
                    found = super == null ? "No" : "Yes";
                    output.WriteLine($"CSP   time: {sw.Elapsed}. Found:  {found}");
                    output.WriteLine("-----------------------------------------------");
                    if (brut == null)
                    {
                        Assert.Null(super);
                        output.WriteLine("Coloring not found");
                        
                    }
                    else
                    {
                        ColoringTestUtils.CheckColoringCorrectness(g, super);
                        ColoringTestUtils.CheckColoringCorrectness(g, brut);
                    }
                    output.WriteLine("-----------------------------------------------");
                }
        }
        [Fact]
        public void  CycleGraphTo500VerticesTest()
        {
            Stopwatch sw = new();
            for (int i = 50; i <= 500; i += 50)
            {
                Graph g = new Graph(i);
                g.AddEdge(i - 1, 0);
                for (int j = 0; j < i - 1; j++)
                {
                    g.AddEdge(j, j + 1);
                }
                sw.Reset();
                sw.Start();
                var brut = new BruteForce().ThreeColorig(g);
                sw.Stop();
                output.WriteLine($"Cycle with {i} vertices:");
                output.WriteLine("Brute time: " + sw.Elapsed);
                sw.Reset();
                sw.Start();
                var super = new CspColoring().ThreeColorig(g);
                sw.Stop();
                output.WriteLine("CSP time  : " + sw.Elapsed);
                output.WriteLine("-----------------------------------------------");
                if (brut == null)
                {
                    Assert.Null(super);
                }
                else
                {
                    ColoringTestUtils.CheckColoringCorrectness(g, super);
                }
                
            }
        }

        [Fact]
        public void CliqueTo1000VerticesTest()
        {
            Stopwatch sw = new();
            for (int i = 2; i <= 1000; i=i<10?i+1:i+50)
            {
                Graph g = new Graph(i);
                
                for (int j = 0; j < i; j++)
                {
                    for (int k = j + 1; k < i; k++)
                        g.AddEdge(j, k);
                }
                sw.Reset();
                sw.Start();
                var brut = new BruteForce().ThreeColorig(g);
                sw.Stop();
                output.WriteLine($"Clique with {i} vertices:");
                output.WriteLine("Brute time: " + sw.Elapsed);
                sw.Reset();
                sw.Start();
                var super = new CspColoring().ThreeColorig(g);
                sw.Stop();
                output.WriteLine("CSP time  : " + sw.Elapsed);
                output.WriteLine("-----------------------------------------------");
                if (brut == null)
                {
                    Assert.Null(super);
                }
                else
                {
                    ColoringTestUtils.CheckColoringCorrectness(g, super);
                }
                if (i == 10) i = 50; 
            }
        }

        [Fact]
        public void BigCliqueTest()
        {
            Stopwatch sw = new();
            for (int i = 1500; i <= 2000; i +=500)
            {
                Graph g = new Graph(i);

                for (int j = 0; j < i; j++)
                {
                    for (int k = j + 1; k < i; k++)
                        g.AddEdge(j, k);
                }
                sw.Reset();
                sw.Start();
                var brut = new BruteForce().ThreeColorig(g);
                sw.Stop();
                output.WriteLine($"Clique with {i} vertices:");
                output.WriteLine("Brute time: " + sw.Elapsed);
                sw.Reset();
                sw.Start();
                var super = new CspColoring().ThreeColorig(g);
                sw.Stop();
                output.WriteLine("CSP time  : " + sw.Elapsed);
                output.WriteLine("-----------------------------------------------");
                if (brut == null)
                {
                    Assert.Null(super);
                }
                else
                {
                    ColoringTestUtils.CheckColoringCorrectness(g, super);
                }
                if (i == 10) i = 50;
            }
        }

        [Fact]
        public void TreeTo500VerticesTest()
        {
            Stopwatch sw = new();
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
                sw.Reset();
                sw.Start();
                var brut = new BruteForce().ThreeColorig(g);
                sw.Stop();
                output.WriteLine($"Tree with {i} vertices:");
                output.WriteLine("Brute time: " + sw.Elapsed);
                sw.Reset();
                sw.Start();
                var super = new CspColoring().ThreeColorig(g);
                sw.Stop();
                output.WriteLine("CSP time  : " + sw.Elapsed);
                output.WriteLine("-----------------------------------------------");
                if (brut == null)
                {
                    Assert.Null(super);
                }
                else
                {
                    ColoringTestUtils.CheckColoringCorrectness(g, super);
                }
            }
        }
        [Fact]
        public void BipartieGraphTest()
        {
            Stopwatch sw = new();
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
                    sw.Reset();
                    sw.Start();
                    var brut = new BruteForce().ThreeColorig(g);
                    sw.Stop();
                    output.WriteLine($"Bipartie Graph with {vertice_count} vertices and {10*res_percent} edge percentage:");
                    output.WriteLine("Brute time: " + sw.Elapsed);
                    sw.Reset();
                    sw.Start();
                    var super = new CspColoring().ThreeColorig(g);
                    sw.Stop();
                    output.WriteLine("CSP time  : " + sw.Elapsed);
                    output.WriteLine("-----------------------------------------------");
                    if (brut == null)
                    {
                        Assert.Null(super);
                    }
                    else
                    {
                        ColoringTestUtils.CheckColoringCorrectness(g, super);
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetBigCycles))]
        public void BigCycleTest(Graph g)
        {
            Stopwatch sw = new();
            sw.Start();
            var brut = new BruteForce().ThreeColorig(g);
            sw.Stop();
            output.WriteLine($"Big cycle with {g.VerticesCount} vertices");
            output.WriteLine("Brute time: " + sw.Elapsed);
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            output.WriteLine("CSP time  : " + sw.Elapsed);
            output.WriteLine("-----------------------------------------------");
            if (brut == null)
            {
                Assert.Null(super);
            }
            else
            {
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            }
        }

       

        [Theory]
        [MemberData(nameof(GetBigTrees))]
        public void BigTreeTest(Graph g)
        {
            Stopwatch sw = new();
            sw.Start();
            var brut = new BruteForce().ThreeColorig(g);
            sw.Stop();
            output.WriteLine($"Big tree with {g.VerticesCount} vertices");
            output.WriteLine("Brute time: " + sw.Elapsed);
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            output.WriteLine("CSP time  : " + sw.Elapsed);
            output.WriteLine("-----------------------------------------------");
            if (brut == null)
            {
                Assert.Null(super);
            }
            else
            {
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            }
        }

        [Theory]
        [MemberData(nameof(GetBigRandomGraphs))]
        public void BigRandomGraphTest(Graph g)
        {
            Stopwatch sw = new();
            sw.Start();
            var brut = new BruteForce().ThreeColorig(g);
            sw.Stop();
            output.WriteLine($"Big random graph with {g.VerticesCount} vertices");
            output.WriteLine("Brute time: " + sw.Elapsed);
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            output.WriteLine("CSP time  : " + sw.Elapsed);
            output.WriteLine("-----------------------------------------------");
            if (brut == null)
            {
                Assert.Null(super);
            }
            else
            {
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            }
        }

        [Theory]
        [MemberData(nameof(GetBigBipartite))]
        public void BigBipartiteTest(Graph g)
        {
            Stopwatch sw = new();
            sw.Start();
            var brut = new BruteForce().ThreeColorig(g);
            sw.Stop();
            output.WriteLine($"Big bipartite graph {g.VerticesCount} vertices");
            output.WriteLine("Brute time: " + sw.Elapsed);
            sw.Reset();
            sw.Start();
            var super = new CspColoring().ThreeColorig(g);
            sw.Stop();
            output.WriteLine("CSP time  : " + sw.Elapsed);
            output.WriteLine("-----------------------------------------------");
            if (brut == null)
            {
                Assert.Null(super);
            }
            else
            {
                ColoringTestUtils.CheckColoringCorrectness(g, super);
            }
        }

        [Fact]
        public void BruteTest()
        {
            Stopwatch sw = new();
            for (int i = 10000; i <= 100000; i += 10000)
                for (int j = 10; j <= 90; j += 20)
                {
                    sw.Reset();
                    Graph g = GetRandomGraph(i, j);
                    sw.Start();
                    var brut = new BruteForce().ThreeColorig(g);
                    sw.Stop();
                    output.WriteLine($"Radnom Graph with {i} vertices and {j} edge percentage:");
                    output.WriteLine("Brute time: " + sw.Elapsed);
                    if (brut == null)
                    {
                        output.WriteLine("Coloring not found");
                    }
                    else
                    { 
                        ColoringTestUtils.CheckColoringCorrectness(g, brut);
                    }
                    output.WriteLine("-----------------------------------------------");

                }
        }
    }
}
