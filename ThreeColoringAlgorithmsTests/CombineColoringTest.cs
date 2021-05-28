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
        public void BigRandomGraphsTest()
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
        public void  CycleGraphTest()
        {
            Stopwatch sw = new();
            for (int i = 50; i <= 1000; i += 50)
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
        public void CliqueTest()
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
        public void TreeTest()
        {
            Stopwatch sw = new();
            for (int i = 50; i <= 1000; i+=50)
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
            for (int vertice_count = 50; vertice_count <= 1000; vertice_count += 50)
            {
                for(int res_percent = 2; res_percent < 9; res_percent+=2)
                {
                    Graph g = new Graph(vertice_count);
                    Random r = new Random();
                    int A = new Random().Next(1, 5);
                    for (int k = 0; k < vertice_count * (vertice_count - 1) * res_percent / 10; vertice_count++)
                    {
                        int a = r.Next((int)(vertice_count*A/10));
                        int b = r.Next((int)(vertice_count * A / 10)+1,vertice_count-1);
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
    }
}
