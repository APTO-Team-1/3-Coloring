using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphLib;
using System.Diagnostics;

namespace GraphCSPConverterTests
{
    [TestClass]
    public class BiGraphToCSP
    {
        [TestMethod]
        public void TestMethod1()
        {
            GraphLib.Definitions.BipartieGraph graph = new(2, 2);
            graph.AddEdge(0, 0);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 0);

            CSP.CspInstance cspInstance = GraphCSPConverter.Converter.BipartieGraphToCSP(graph);

            foreach (var x in cspInstance.Restrictions)
            {
                Debug.WriteLine(x.Pair1.Variable.Id + "-" + x.Pair1.Color.Value + ", " + x.Pair2.Variable.Id + "-" + x.Pair2.Color.Value);
            }

            Assert.AreEqual(4, cspInstance.Variables.Count);
            Assert.AreEqual(9, cspInstance.Restrictions.Count);
        }
    }
}
