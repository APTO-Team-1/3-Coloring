using Xunit;
using CSPLemmas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP;

namespace CSPLemmas.Tests
{
    public class CSPLemmasTests
    {
        [Theory]
        [MemberData(nameof(GetDataVariableWith2Colors))]
        public void RemoveVariableWith2ColorsTest(CspInstance instance, Variable variable)
        {
            int variableCount = instance.Variables.Count;

            List<Pair> c1Neighbors = new();
            c1Neighbors.AddRange(variable.AvalibleColors[0].Restrictions);

            List<Pair> c2Neighbors = new();
            c2Neighbors.AddRange(variable.AvalibleColors[1].Restrictions);

            CSPLemmas.RemoveVariableWith2Colors(instance, variable);

            Assert.False(instance.Variables.Contains(variable));
            Assert.Equal(variableCount - 1, instance.Variables.Count);
            foreach (var p1 in c1Neighbors)
            {
                foreach (var p2 in c2Neighbors)
                {
                    if (p1.Variable != p2.Variable)
                    {
                        Assert.Contains(instance.Restrictions, r => r.Contains(p1.Color) && r.Contains(p2.Color));
                    }
                }
            }
        }

       

        [Theory]
        [MemberData(nameof(GetDataVariableWith2Colors))]
        public void Lemma2Test(CspInstance instance, Variable variable)
        {
            CSPLemmas.Lemma2(instance);
            NoVariableWith2Colors(instance);
        }
        private static void NoVariableWith2Colors(CspInstance instance)
        {
            foreach (var variable in instance.Variables)
            {
                Assert.True(variable.AvalibleColors.Count > 2);
            }
        }
        #region GettingData
        public static IEnumerable<object[]> GetDataVariableWith2Colors()
        {
            var data = new List<object[]>();
            for (int i = 1; i < 1000; i += 50)
            {
                CspInstance instance = new();
                Variable v2colors = new(2);
                instance.Variables.Add(v2colors);
                List<Variable> variables = new();
                for (int j = 0; j < i; j++)
                {
                    var v = new Variable(j % 4 + 2);
                    instance.Variables.Add(v);
                    variables.Add(v);
                }
                for (int j = 0; j < i * 10; j++)
                {
                    var v1 = variables[(j * 2137 + 14) % variables.Count];
                    var v2 = variables[(j * 420 + 13) % variables.Count];
                    var c1 = v1.AvalibleColors[(j * 1337 + 7) % v1.AvalibleColors.Count];
                    var c2 = v2.AvalibleColors[(j * 1234 + 3) % v2.AvalibleColors.Count];
                    instance.AddRestriction(new Pair(v1, c1), new Pair(v2, c2));
                }
                data.Add(new object[] { instance, v2colors });
            }

            return data;
        }
        #endregion

      
    }
}