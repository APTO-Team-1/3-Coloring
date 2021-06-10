using GraphLib.Definitions;
using System;
using ThreeColoringAlgorithms;
using ThreeColoringAlgorithmsTests;

namespace BigTests
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1000;

            Graph g = CSPColoringExtraTests.GenerateGraph(verticesCount: i, maxNeighbours: 5, minNeighbours: 3,
                isColorable: new Random().NextDouble() > 0.5 ? true : false, randomSeed: 1);

            var result = new CspColoring().ThreeColorig(g);
            Console.WriteLine(result);

            for (int k = 0; k < result.Length; k++)
            {
                Console.WriteLine($"{k}: {result[k]}");
            }

        }
    }
}
