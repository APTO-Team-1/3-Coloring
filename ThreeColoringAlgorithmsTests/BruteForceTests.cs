using GraphLib.Algorithms;
using GraphLib.Definitions;
using System.Collections.Generic;
using Xunit;

namespace ThreeColoringAlgorithmsTests
{
    public class BruteForceTests
    {
        public static IEnumerable<object[]> GetData() => ColoringTestUtils.GetData();
        public static IEnumerable<object[]> GetDataFailure() => ColoringTestUtils.GetDataFailure();

        [Theory]
        [MemberData(nameof(GetData))]
        public void TestColoringSuccesfull(Graph g)
        {
            var coloring = new BruteForce().ThreeColorig(g);

            ColoringTestUtils.CheckColoringCorrectness(g, coloring);
        }

        [Theory]
        [MemberData(nameof(GetDataFailure))]
        public void TestColoringImposible(Graph g)
        {
            Assert.Null(new BruteForce().ThreeColorig(g));
        }


    }
}
