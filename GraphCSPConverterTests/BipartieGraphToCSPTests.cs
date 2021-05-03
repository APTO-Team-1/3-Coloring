using System;
using Xunit;

namespace GraphCSPConverterTests
{
    public class BipartieGraphToCSPTests
    {
        [Fact]
        public void Test1()
        {
            GraphLib.Definitions.BipartieGraph graph = new(2, 2);
            graph.AddEdge(0, 0);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 0);

            CSP.CspInstance cspInstance = GraphCSPConverter.Converter.BipartieGraphToCSP(graph);

            Assert.Equal(4, cspInstance.Variables.Count);
            Assert.Equal(9, cspInstance.Restrictions.Count);
        }
    }
}
