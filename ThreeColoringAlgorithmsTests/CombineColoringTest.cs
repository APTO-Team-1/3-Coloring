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
                g = GetRandomGraph(restrictionPercentage: i, randomSeed: i, vericesCount: i);
                data.Add(new[] { g });
            }


            g = GetRandomGraph(restrictionPercentage: 50, randomSeed: 500, vericesCount: 500);
            data.Add(new[] { g });

            g = GetRandomGraph(restrictionPercentage: 50, randomSeed: 1000, vericesCount: 1000);
            data.Add(new[] { g });

            return data;
        }

        static int e = 0;
        [Theory]
        [MemberData(nameof(GetRandomGraphs))]
        public void TestColoring(Graph g)
        {
            e++;
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
    }
}
