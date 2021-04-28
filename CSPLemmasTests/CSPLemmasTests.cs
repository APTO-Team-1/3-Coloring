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
        #region GettingData
        public static IEnumerable<object[]> GetDataLemma2()
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
                    var v = new Variable(j % 3 + 2);
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
        public static IEnumerable<object[]> GetDataLemma3()
        {
            var data = new List<object[]>();
            for (int i = 0; i < 20; i++)
            {
                var instance = GetRandomInstance(2,3,i * 50 + 1, i * 500 + 1);
                var v1 = new Variable(3);
                var v2 = new Variable(3);
                instance.Variables.Add(v1);
                instance.Variables.Add(v2);
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[0]), new Pair(v2, v2.AvalibleColors[0]));
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[0]), new Pair(v2, v2.AvalibleColors[1]));
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[1]), new Pair(v2, v2.AvalibleColors[0]));
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[1]), new Pair(v2, v2.AvalibleColors[1]));
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[1]), new Pair(v2, v2.AvalibleColors[2]));
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[2]), new Pair(v2, v2.AvalibleColors[0]));
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[2]), new Pair(v2, v2.AvalibleColors[1]));
                instance.AddRestriction(new Pair(v1, v1.AvalibleColors[2]), new Pair(v2, v2.AvalibleColors[2]));
                data.Add(new object[] { instance, new Pair(v1, v1.AvalibleColors[0]), new Pair(v2, v2.AvalibleColors[2]) });
            }
            return data;
        }

        public static IEnumerable<object[]> GetDataLemma4()
        {
            var data = new List<object[]>();
            Random r = new();
            for (int i = 0; i < 20; i++)
            {
                var instance = GetRandomInstance(3,3,i * 50 + 1, i * 500 + 1);
                List<Variable> variables = new();
                variables.AddRange(instance.Variables);
                var v = new Variable(3);
                instance.Variables.Add(v);
                int i1 = Math.Abs(r.Next(0, v.AvalibleColors.Count));
                int i2 = Math.Abs(r.Next(0, v.AvalibleColors.Count));
                i2 = i2 == i1 ? (i2 + 1) % v.AvalibleColors.Count : i2;
                var c1 = v.AvalibleColors[i1];
                var c2 = v.AvalibleColors[i2];
                for (int j = 0; j < r.Next(5,20); j++)
                {
                    var v3 = variables[r.Next(0, variables.Count)];
                    var c3 = v3.AvalibleColors[r.Next(0, v3.AvalibleColors.Count)];
                    instance.AddRestriction(new Pair(v, c1), new Pair(v3, c3));
                    instance.AddRestriction(new Pair(v, c2), new Pair(v3, c3));
                }
                for (int j = 0; j < r.Next(5, 20); j++)
                {
                    var v3 = variables[r.Next(0, variables.Count)];
                    var c3 = v3.AvalibleColors[r.Next(0, v3.AvalibleColors.Count)];
                    instance.AddRestriction(new Pair(v, c2), new Pair(v3, c3));
                }
                foreach (var pair in c1.Restrictions)
                {
                    instance.AddRestriction(new Pair(v, c2), pair);
                }
                data.Add(new object[] { instance, new Pair(v, c1) });
            }
            return data;
        }

        public static IEnumerable<object[]> GetDataLemma5()
        {
            var data = new List<object[]>();
            Random r = new();
            for (int i = 0; i < 20; i++)
            {
                var instance = GetRandomInstance(3, 3, i * 50 + 1, i * 500 + 1);
                List<Variable> variables = new();
                variables.AddRange(instance.Variables);
                var v = variables[r.Next(0, variables.Count)];
                var c = v.AvalibleColors[r.Next(0, v.AvalibleColors.Count)];
                foreach (var pair in c.Restrictions)
                {
                    instance.RemoveRestriction(new Pair(v, c), pair);
                }
                data.Add(new object[] { instance, new Pair(v, c) });
            }
            return data;
        }

        public static IEnumerable<object[]> GetDataLemma6()
        {
            var data = new List<object[]>();
            Random r = new();
            for (int i = 0; i < 20; i++)
            {
                var instance = GetRandomInstance(3, 3, i * 50 + 2, i * 500 + 1);
                List<Variable> variables = new();
                variables.AddRange(instance.Variables);
                int i1 = r.Next(0, variables.Count);
                int i2 = r.Next(0, variables.Count);
                i2 = i2 == i1 ? (i2 + 1) % variables.Count : i2;
                var v = variables[i1];
                var c = v.AvalibleColors[r.Next(0, v.AvalibleColors.Count)];
                var v2 = variables[i2];
                foreach (var color in v2.AvalibleColors)
                {
                    instance.AddRestriction(new Pair(v, c), new Pair(v2,color));
                }
                data.Add(new object[] { instance, new Pair(v, c) });
            }
            return data;
        }

        private static CspInstance GetRandomInstance(int minColors = 2,int maxColors = 4,int variableCount = 100, int approximateRestrictionsCount = 1000)
        {
            var instance = new CspInstance();
            List<Variable> variables = new();
            Random r = new();
            for (int i = 0; i < variableCount; i++)
            {
                var v = new Variable(r.Next(minColors, maxColors+1));
                instance.Variables.Add(v);
                variables.Add(v);
            }
            for (int i = 0; i < approximateRestrictionsCount; i++)
            {

                var v1 = variables[r.Next(0, variables.Count)];
                var v2 = variables[r.Next(0, variables.Count)];
                var c1 = v1.AvalibleColors[r.Next(0, v1.AvalibleColors.Count)];
                var c2 = v2.AvalibleColors[r.Next(0, v2.AvalibleColors.Count)];
                instance.AddRestriction(new Pair(v1, c1), new Pair(v2, c2));
            }
            return instance;
        }
        #endregion


        [Theory]
        [MemberData(nameof(GetDataLemma2))]
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
        [MemberData(nameof(GetDataLemma2))]
        public void Lemma2Test(CspInstance instance, Variable variable)
        {
            CSPLemmas.Lemma2(instance);
            Assert.DoesNotContain(variable, instance.Variables);
            NoVariableWith2Colors(instance);
        }
        private static void NoVariableWith2Colors(CspInstance instance)
        {
            foreach (var variable in instance.Variables)
            {
                Assert.True(variable.AvalibleColors.Count > 2);
            }
        }

        [Theory]
        [MemberData(nameof(GetDataLemma3))]
        public void Lemma3Test(CspInstance instance, Pair pair1, Pair pair2)
        {
            CSPLemmas.Lemma3(instance);
            Assert.Contains(pair1, instance.Result);
            Assert.Contains(pair2, instance.Result);
            Assert.DoesNotContain(pair1.Variable, instance.Variables);
            Assert.DoesNotContain(pair2.Variable, instance.Variables);
        }

        [Theory]
        [MemberData(nameof(GetDataLemma4))]
        public void Lemma4Test(CspInstance instance, Pair pair1)
        {
            CSPLemmas.Lemma4(instance);
            Assert.DoesNotContain(pair1.Variable, instance.Variables);
        }

        [Theory]
        [MemberData(nameof(GetDataLemma5))]
        public void Lemma5Test(CspInstance instance, Pair pair)
        {
            CSPLemmas.Lemma5(instance);
            Assert.DoesNotContain(pair.Variable, instance.Variables);
            Assert.Contains(pair, instance.Result);
        }

        [Theory]
        [MemberData(nameof(GetDataLemma6))]
        public void Lemma6Test(CspInstance instance, Pair pair)
        {
            CSPLemmas.Lemma6(instance);
            Assert.DoesNotContain(pair.Color, pair.Variable.AvalibleColors);
        }
    }
}