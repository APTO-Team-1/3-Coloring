using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ThreeColoringAlgorithms;
using Xunit;

namespace ThreeColoringAlgorithmsTests
{
    public class CSPColoringExtraTests
    {
       
        public static IEnumerable<object[]> GetRandomGraphs100Vertices()
        {
            int vertices = 100;
            var data = new List<object[]>();
            Graph g;
            for (int i = 0; i < 5; i++)
            {
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 6, verticesWithHighDegree: 0.01, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 3, verticesWithHighDegree: 0.05, isColorable: false);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.01, isColorable: false);
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
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 5, verticesWithHighDegree: 0.01, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 3, verticesWithHighDegree: 0.04, isColorable: false);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.01, isColorable: false);
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
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true, randomSeed: 32);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 5, verticesWithHighDegree: 0.01, isColorable: true, randomSeed: 32);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 3, verticesWithHighDegree: 0.03, isColorable: false, randomSeed: 32);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.01, isColorable: false, randomSeed: 32);
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
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true, randomSeed: 1);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.005, isColorable: true, randomSeed: 1);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 3, verticesWithHighDegree: 0.03, isColorable: false, randomSeed: 1);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 7, delta: 5, verticesWithHighDegree: 0.02, isColorable: false, randomSeed: 1);
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
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.005, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 2, verticesWithHighDegree: 0.02, isColorable: false);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 5, verticesWithHighDegree: 0.02, isColorable: false);
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
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true, randomSeed: 1);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.004, isColorable: true, randomSeed: 1);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 2, verticesWithHighDegree: 0.02, isColorable: false, randomSeed: 1);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 6, delta: 5, verticesWithHighDegree: 0.02, isColorable: false, randomSeed: 1);
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
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 4, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, verticesWithHighDegree: 0.004, isColorable: true);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 4, delta: 2, verticesWithHighDegree: 0.01);
                data.Add(new[] { g });
                g = ColoringTestUtils.GenerateGraph(verticesCount: vertices, maxNeighbours: 5, delta: 5, verticesWithHighDegree: 0.01);
                data.Add(new[] { g });
            }
            return data;
        }

        [Fact]
        public void GeneratorTest()
        {
            Graph g = ColoringTestUtils.GenerateGraph(verticesCount: 200, isColorable: true, maxNeighbours: 5);
            Assert.NotNull(g);
        }

        [Theory]
        [MemberData(nameof(GetRandomGraphs100Vertices))]
        public void RandomGraphs100VerticesTest(Graph g)
        {
            ColoringTestUtils.CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs200Vertices))]
        public void RandomGraphs200VerticesTest(Graph g)
        {
            ColoringTestUtils.CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs300Vertices))]
        public void RandomGraphs300VerticesTest(Graph g)
        {
            ColoringTestUtils.CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs400Vertices))]
        public void RandomGraphs400VerticesTest(Graph g)
        {
            ColoringTestUtils.CheckAndWriteOutput(g);
        }

        [Theory]
        [MemberData(nameof(GetRandomGraphs500Vertices))]
        public void RandomGraphs500VerticesTest(Graph g)
        {
            ColoringTestUtils.CheckAndWriteOutput(g);
        }
        [Theory]
        [MemberData(nameof(GetRandomGraphs600Vertices))]
        public void RandomGraphs600VerticesTest(Graph g)
        {
            ColoringTestUtils.CheckAndWriteOutput(g);
        }
        //[Theory]
        //[MemberData(nameof(GetRandomGraphs700Vertices))]
        //public void RandomGraphs700VerticesTest(Graph g)
        //{
        //    ColoringTestUtils.CheckAndWriteOutput(g);
        //}
    }
}
