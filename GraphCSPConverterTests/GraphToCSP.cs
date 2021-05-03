using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphLib;
using System.Diagnostics;

namespace GraphCSPConverterTests
{
    [TestClass]
    public class GraphToCSP
    {
        [TestMethod]
        public void TestMethod1()
        {
            GraphLib.Definitions.Graph graph = new(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 2);

            CSP.CspInstance cspInstance = GraphCSPConverter.Converter.GraphToCSP(graph);

            foreach(var x in cspInstance.Restrictions)
            {
                Debug.WriteLine(x.Pair1.Variable.Id + "-" + x.Pair1.Color.Value + ", " + x.Pair2.Variable.Id + "-" + x.Pair2.Color.Value);
            }

            Assert.AreEqual(3, cspInstance.Variables.Count);
            Assert.AreEqual(9, cspInstance.Restrictions.Count);
        }
    }
}
