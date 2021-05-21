using GraphLib.Algorithms;
using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreeColoringAlgorithms;
using Xunit;

namespace ThreeColoringAlgorithmsTests
{
    public class ColoringTests
    {
        public static Graph GetRandomGraph(int vericesCount = 20, int restrictionPercentage = 10)
        {
            Graph g = new(vericesCount);
            Random r = new();
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
            for (int i = 0; i < 100; i++)
            {
                Graph g = GetRandomGraph(restrictionPercentage: i);
                data.Add(new[] { g });
            }

            return data;
        }

        [Theory]
        [MemberData(nameof(GetRandomGraphs))]
        public void TestColoring(Graph g)
        {
            var brut = new BruteForce().ThreeColorig(g);
            var super = new CspColoring().ThreeColorig(g);
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
