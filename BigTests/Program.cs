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

            Graph g = CSPColoringExtraTests.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
                isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            CSPColoringExtraTests.CheckAndWriteOutput(g);


            neighborsCount = 800;

             g = CSPColoringExtraTests.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
                isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            CSPColoringExtraTests.CheckAndWriteOutput(g);


            neighborsCount = 900;

             g = CSPColoringExtraTests.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
                isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            CSPColoringExtraTests.CheckAndWriteOutput(g);

            neighborsCount = 1000;

            g = CSPColoringExtraTests.GenerateGraph(verticesCount: neighborsCount, maxNeighbours: 5, minNeighbours: 3,
               isColorable: new Random().NextDouble() > 0.5, randomSeed: 1);

            CSPColoringExtraTests.CheckAndWriteOutput(g);

        }
    }
}
