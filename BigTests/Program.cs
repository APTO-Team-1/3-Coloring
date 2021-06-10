using GraphLib.Definitions;
using System;
using ThreeColoringAlgorithmsTests;

namespace BigTests
{
    class Program
    {
        static void Main(string[] args)
        {
            int neighborsCount = 700;

            Graph g = ColoringTestUtils.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
                isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            ColoringTestUtils.CheckAndWriteOutput(g);


            neighborsCount = 800;

             g = ColoringTestUtils.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
                isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            ColoringTestUtils.CheckAndWriteOutput(g);


            neighborsCount = 900;

             g = ColoringTestUtils.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
                isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            ColoringTestUtils.CheckAndWriteOutput(g);

            neighborsCount = 1000;

            g = ColoringTestUtils.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
               isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            ColoringTestUtils.CheckAndWriteOutput(g);

        }
    }
}
