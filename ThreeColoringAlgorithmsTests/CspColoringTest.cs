using GraphLib.Algorithms;
using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreeColoringAlgorithms;
using Xunit;

namespace ThreeColoringAlgorithmsTests
{
    public class CspColoringTests
    {
        public static IEnumerable<object[]> GetData() => ColoringTestUtils.GetData();
        public static IEnumerable<object[]> GetDataFailure() => ColoringTestUtils.GetDataFailure();

        [Theory]
        [MemberData(nameof(GetData))]
        public void TestColoringSuccesfull(Graph g)
        {
            var coloring = new CspColoring().ThreeColorig(g);

            ColoringTestUtils.CheckColoringCorrectness(g, coloring);
        }

        [Theory]
        [MemberData(nameof(GetDataFailure))]
        public void TestColoringImposible(Graph g)
        {
            Assert.Null(new CspColoring().ThreeColorig(g));
        }


    }
}
