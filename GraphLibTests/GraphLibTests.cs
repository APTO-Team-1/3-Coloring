using GraphLib.Algorithms;
using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphLibTests
{
    public class BipartieGraphMaxMatchngAlgorithmTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void Test(BipartieGraph bg)
        {
            var matching = new BipartieGraphMaxMatching().FindMaxMatching(bg);

            IsMatching(matching);
        }

        private static void IsMatching( int[] matching)
        {
            // apart from -1 values should be unique
            Array.FindAll(matching, v => v != -1);
            Assert.Equal(matching.Length, matching.Distinct().Count()); 
        }

        #region getting data

        public static IEnumerable<object[]> GetData()
        {
            var data = new List<object[]>
            {
            new object[] { GetSimpleGraph() },
            new object[] { GetObviousGraph() },
            };

            return data;
        }

        private static BipartieGraph GetSimpleGraph()
        {
            BipartieGraph bg = new(3, 2);
            bg.AddEdge(0, 0);
            bg.AddEdge(0, 1);
            bg.AddEdge(1, 0);
            bg.AddEdge(2, 1);

            return bg;
        }

        private static BipartieGraph GetObviousGraph()
        {
            BipartieGraph bg = new(100, 100);
            for(int i =0; i<100; i++)
            {
                bg.AddEdge(i, i);
            }

            return bg;
        }



        #endregion


    }
}
