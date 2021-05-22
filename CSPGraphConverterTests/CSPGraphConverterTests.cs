using System;
using System.Collections.Generic;
using Xunit;

namespace CSPGraphConverter.Tests
{
    public class CSPGraphConverterTests
    {
        #region GettingData

        public static IEnumerable<object[]> GetDataGraphToCSP()
        {
            var data = new List<object[]>();
            Random r = new(123);
            for (int i = 0; i < 50; i++)
            {
                var graph = new GraphLib.Definitions.Graph(r.Next(1, 100));
                
                for(int j = 0; j < graph.VerticesCount * (graph.VerticesCount-1) / 2; j++)
                {
                    var from = r.Next(0, graph.VerticesCount);
                    var to = r.Next(0, graph.VerticesCount);
                    if(from != to && !graph.ContainsEdge(from, to))
                    {
                        graph.AddEdge(from, to);
                    }
                }

                data.Add(new object[] { graph });
            }
            return data;
        }

        public static IEnumerable<object[]> GetDataBiGraphToCSP()
        {
            var data = new List<object[]>();
            Random r = new(123);
            for (int i = 0; i < 50; i++)
            {
                var graph = new GraphLib.Definitions.BipartieGraph(r.Next(1, 100), r.Next(1, 100));

                for (int j = 0; j < graph.PartAVertices.Length * graph.PartBVertices.Length; j++)
                {
                    var from = r.Next(0, graph.PartAVertices.Length);
                    var to = r.Next(0, graph.PartBVertices.Length);
                    if (!graph.ContainsEdgeAtoB(from, to))
                    {
                        graph.AddEdge(from, to);
                    }
                }

                data.Add(new object[] { graph });
            }
            return data;
        }

        #endregion


        [Theory]
        [MemberData(nameof(GetDataGraphToCSP))]
        public void GraphToCSP(GraphLib.Definitions.Graph graph)
        {
            int verticesCount = graph.VerticesCount;
            int edgesCount = 0;
            for(int i = 0; i < verticesCount; i++)
            {
                edgesCount += graph.GetNeighbors(i).Count;
            }
            edgesCount /= 2;

            CSP.CspInstance cspInstance3 = Converter.GraphToCSP(graph, 3);
            CSP.CspInstance cspInstance4 = Converter.GraphToCSP(graph, 4);

            Assert.Equal(verticesCount, cspInstance3.Variables.Count);
            Assert.Equal(3 * edgesCount, cspInstance3.Restrictions.Count);
            Assert.Equal(verticesCount, cspInstance4.Variables.Count);
            Assert.Equal(4 * edgesCount, cspInstance4.Restrictions.Count);
        }

    }
}
