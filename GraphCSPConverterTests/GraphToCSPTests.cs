using System;
using Xunit;

namespace GraphCSPConverterTests
{
    public class GraphToCSPTests
    {
        [Fact]
        public void Test1()
        {
            GraphLib.Definitions.Graph graph = new(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 2);

            CSP.CspInstance cspInstance = GraphCSPConverter.Converter.GraphToCSP(graph);

            Assert.Equal(3, cspInstance.Variables.Count);
            Assert.Equal(9, cspInstance.Restrictions.Count);
        }
    }
}
