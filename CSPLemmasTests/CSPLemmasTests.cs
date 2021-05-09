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
                instance.AddVariable(v2colors);
                List<Variable> variables = new();
                for (int j = 0; j < i; j++)
                {
                    var v = new Variable(j % 3 + 2);
                    instance.AddVariable(v);
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
                var instance = GetRandomInstance(2, 3, i * 50 + 1, i * 500 + 1);
                var v1 = new Variable(3);
                var v2 = new Variable(3);
                instance.AddVariable(v1);
                instance.AddVariable(v2);
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
                var instance = GetRandomInstance(3, 3, i * 50 + 1, i * 500 + 1);
                List<Variable> variables = new();
                variables.AddRange(instance.Variables);
                var v = new Variable(3);
                instance.AddVariable(v);
                int i1 = Math.Abs(r.Next(0, v.AvalibleColors.Count));
                int i2 = Math.Abs(r.Next(0, v.AvalibleColors.Count));
                i2 = i2 == i1 ? (i2 + 1) % v.AvalibleColors.Count : i2;
                var c1 = v.AvalibleColors[i1];
                var c2 = v.AvalibleColors[i2];
                for (int j = 0; j < r.Next(5, 20); j++)
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
                    instance.AddRestriction(new Pair(v, c), new Pair(v2, color));
                }
                data.Add(new object[] { instance, new Pair(v, c) });
            }
            return data;
        }

        public static IEnumerable<object[]> GetDataLemma8()
        {
            var data = new List<object[]>();
            Random r = new(12345);
            for (int i = 0; i < 30; i++)
            {
                var instance = GetRandomInstance(maxColors: i >= 15 ? 3 : 4);
                int idx1;
                int idx2;

                do
                {
                    idx1 = r.Next(0, instance.Variables.Count);
                    idx2 = r.Next(0, instance.Variables.Count);
                }
                while (
                    idx1 == idx2 ||
                    instance.Variables.ElementAt(idx1).AvalibleColors == null ||
                    instance.Variables.ElementAt(idx1).AvalibleColors.Count == 0 ||
                    instance.Variables.ElementAt(idx2).AvalibleColors == null ||
                    instance.Variables.ElementAt(idx2).AvalibleColors.Count == 0
                    );

                var v1 = instance.Variables.ElementAt(idx1);
                var c1 = v1.AvalibleColors[0];
                var v2 = instance.Variables.ElementAt(idx2);
                var c2 = v2.AvalibleColors[0];

                foreach (var restPair in c1.Restrictions)
                    instance.RemoveRestriction(new Pair(v1, c1), restPair);

                foreach (var restPair in c2.Restrictions)
                    instance.RemoveRestriction(new Pair(v2, c2), restPair);

                instance.AddRestriction(new Pair(v1, c1), new Pair(v2, c2)); // isolated costraint

                data.Add(new object[] { instance, v1, c1, v2, c2 });
            }

            return data;
        }

        public static IEnumerable<object[]> GetDataLemma9()
        {
            var data = new List<object[]>();
            Random r = new(12345);
            for (int i = 0; i < 30; i++)
            {
                var instance = GetRandomInstance(maxColors: i >= 15 ? 3 : 4);
                int idx1;
                int idx2;

                do
                {
                    idx1 = r.Next(0, instance.Variables.Count);
                    idx2 = r.Next(0, instance.Variables.Count);
                }
                while (
                    idx1 == idx2 ||
                    instance.Variables.ElementAt(idx1).AvalibleColors == null ||
                    instance.Variables.ElementAt(idx1).AvalibleColors.Count == 0 ||
                    instance.Variables.ElementAt(idx2).AvalibleColors == null ||
                    instance.Variables.ElementAt(idx2).AvalibleColors.Count == 0
                    );

                var v1 = instance.Variables.ElementAt(idx1);
                var c1 = v1.AvalibleColors[0];
                var v2 = instance.Variables.ElementAt(idx2);
                var c2 = v2.AvalibleColors[0];

                foreach (var restPair in c1.Restrictions)
                    instance.RemoveRestriction(new Pair(v1, c1), restPair);

                instance.AddRestriction(new Pair(v1, c1), new Pair(v2, c2)); // dangling costraint

                data.Add(new object[] { instance, v1, c1, v2, c2 });
            }

            return data;
        }

        public static IEnumerable<object[]> GetDataLemma11()
        {
            var data = new List<object[]>();
            Random r = new(12345);
            for (int i = 0; i < 30; i++)
            {
                var instance = GetRandomInstance(maxColors: i >= 15 ? 3 : 4);

                data.Add(new object[] { instance });
            }

            return data;
        }

        public static IEnumerable<object[]> GetDataLemma12()
        {
            var data = new List<object[]>();
            Random r = new(12345);
            for (int i = 0; i < 50; i++)
            {
                var instance = GetRandomInstance(maxColors: i >= 25 ? 3 : 4, approximateRestrictionsCount: i >= 25 ? 1000 : 2000);

                data.Add(new object[] { instance });
            }

            return data;
        }

        public static IEnumerable<object[]> GetDataLemma13()
        {
            var data = new List<object[]>();
            Random r = new(12345);
            for (int i = 0; i < 50; i++)
            {
                var instance = GetRandomInstance(maxColors: i >= 25 ? 3 : 4, approximateRestrictionsCount: i >= 25 ? 1000 : 2000);

                data.Add(new object[] { instance });
            }

            return data;
        }

        public static IEnumerable<object[]> GetDataLemma15()
        {
            var data = new List<object[]>();
            Random r = new(12345);
            for(int i = 0; i < 48; i++)
            {
                var instance = GetRandomInstance(minColors: 3, maxColors: i >= 25 ? 3 : 4, approximateRestrictionsCount: i >= 25 ? 1000 : 2000);
                data.Add(new object[] { instance });
            }

            CspInstance cspInstance = new();
            Variable v = new Variable(3);
            Variable w = new Variable(3);
            Variable x = new Variable(3);
            Variable y = new Variable(3);
            cspInstance.AddVariable(v);
            cspInstance.AddVariable(w);
            cspInstance.AddVariable(x);
            cspInstance.AddVariable(y);
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[1]), new Pair(w, w.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[1]), new Pair(x, x.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[1]), new Pair(y, y.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[2]), new Pair(w, w.AvalibleColors[1]));
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[2]), new Pair(x, x.AvalibleColors[1]));
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[2]), new Pair(y, y.AvalibleColors[1]));
            cspInstance.AddRestriction(new Pair(w, w.AvalibleColors[1]), new Pair(x, x.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(w, w.AvalibleColors[1]), new Pair(y, y.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(w, w.AvalibleColors[2]), new Pair(x, x.AvalibleColors[1]));
            cspInstance.AddRestriction(new Pair(w, w.AvalibleColors[2]), new Pair(y, y.AvalibleColors[1]));
            cspInstance.AddRestriction(new Pair(x, x.AvalibleColors[1]), new Pair(y, y.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(x, x.AvalibleColors[2]), new Pair(y, y.AvalibleColors[1]));
            data.Add(new object[] { cspInstance });


            CspInstance cspInstance2 = new();
            Variable v2 = new Variable(3);
            Variable w2 = new Variable(3);
            Variable x2 = new Variable(3);
            Variable y2 = new Variable(3);
            cspInstance2.AddVariable(v2);
            cspInstance2.AddVariable(w2);
            cspInstance2.AddVariable(x2);
            cspInstance2.AddVariable(y2);
            cspInstance2.AddRestriction(new Pair(v2, v2.AvalibleColors[1]), new Pair(w2, w2.AvalibleColors[2]));
            cspInstance2.AddRestriction(new Pair(v2, v2.AvalibleColors[1]), new Pair(x2, x2.AvalibleColors[2]));
            cspInstance2.AddRestriction(new Pair(v2, v2.AvalibleColors[1]), new Pair(y2, y2.AvalibleColors[2]));
            cspInstance2.AddRestriction(new Pair(v2, v2.AvalibleColors[2]), new Pair(w2, w2.AvalibleColors[1]));
            cspInstance2.AddRestriction(new Pair(v2, v2.AvalibleColors[2]), new Pair(x2, x2.AvalibleColors[1]));
            cspInstance2.AddRestriction(new Pair(v2, v2.AvalibleColors[2]), new Pair(y2, y2.AvalibleColors[1]));
            cspInstance2.AddRestriction(new Pair(w2, w2.AvalibleColors[1]), new Pair(x2, x2.AvalibleColors[2]));
            cspInstance2.AddRestriction(new Pair(w2, w2.AvalibleColors[1]), new Pair(y2, y2.AvalibleColors[2]));
            cspInstance2.AddRestriction(new Pair(w2, w2.AvalibleColors[2]), new Pair(x2, x2.AvalibleColors[1]));
            cspInstance2.AddRestriction(new Pair(w2, w2.AvalibleColors[2]), new Pair(y2, y2.AvalibleColors[1]));
            cspInstance2.AddRestriction(new Pair(x2, x2.AvalibleColors[1]), new Pair(y2, y2.AvalibleColors[1]));
            cspInstance2.AddRestriction(new Pair(x2, x2.AvalibleColors[2]), new Pair(y2, y2.AvalibleColors[2]));
            data.Add(new object[] { cspInstance2 });

            return data;
        }

        public static IEnumerable<object[]> GetDataLemma17()
        {
            var data = new List<object[]>();
            Random r = new(12345);
            for (int i = 0; i < 49; i++)
            {
                var instance = GetRandomInstance(minColors: 3, maxColors: i >= 25 ? 3 : 4, approximateRestrictionsCount: i >= 25 ? 1000 : 2000);
                data.Add(new object[] { instance });
            }

            CspInstance cspInstance = new();
            Variable v = new Variable(3);
            Variable w = new Variable(3);
            Variable x = new Variable(3);
            Variable y = new Variable(3);
            Variable z = new Variable(3);
            Variable a = new Variable(3);
            Variable b = new Variable(3);
            Variable c = new Variable(3);
            cspInstance.AddVariable(v);
            cspInstance.AddVariable(w);
            cspInstance.AddVariable(x);
            cspInstance.AddVariable(y);
            cspInstance.AddVariable(z);
            cspInstance.AddVariable(a);
            cspInstance.AddVariable(b);
            cspInstance.AddVariable(c);
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[2]), new Pair(w, w.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[2]), new Pair(x, x.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(v, v.AvalibleColors[2]), new Pair(y, y.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(w, w.AvalibleColors[2]), new Pair(x, x.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(w, w.AvalibleColors[2]), new Pair(z, z.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(x, x.AvalibleColors[2]), new Pair(a, a.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(y, y.AvalibleColors[2]), new Pair(a, a.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(y, y.AvalibleColors[2]), new Pair(b, b.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(z, z.AvalibleColors[2]), new Pair(b, b.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(z, z.AvalibleColors[2]), new Pair(c, c.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(b, b.AvalibleColors[2]), new Pair(c, c.AvalibleColors[2]));
            cspInstance.AddRestriction(new Pair(a, a.AvalibleColors[2]), new Pair(c, c.AvalibleColors[2]));
            data.Add(new object[] { cspInstance });

            return data;
        }

        private static CspInstance GetRandomInstance(int minColors = 2, int maxColors = 4, int variableCount = 100, int approximateRestrictionsCount = 1000)
        {
            var instance = new CspInstance();
            List<Variable> variables = new();
            Random r = new(123);
            for (int i = 0; i < variableCount; i++)
            {
                var v = new Variable(r.Next(minColors, maxColors + 1));
                instance.AddVariable(v);
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

        [Theory]
        [MemberData(nameof(GetDataLemma8))]
        public void Lemma8Test(CspInstance instance, Variable v, Color c, Variable v2, Color c2)
        {
            //if two 3 - color vertices changed to one 4 - color
            if (v.AvalibleColors.Count == 3 && v2.AvalibleColors.Count == 3)
            {
                var oldColors = v.AvalibleColors.Select(c => new Color(c.Value, c.Restrictions)).Union(v2.AvalibleColors.Select(c => new Color(c.Value, c.Restrictions)));
                var res = CSPLemmas.Lemma8(instance, v, c);
                Assert.Single(res);
                Assert.Null(res[0].Variables.FirstOrDefault(vbl => vbl == v));
                Assert.Null(res[0].Variables.FirstOrDefault(vbl => vbl == v2));
                var vCombined = res[0].Variables.FirstOrDefault(vbl =>
                    vbl.AvalibleColors.Count == 4 &&
                    vbl.AvalibleColors.All(avCol => oldColors.Any(oc => oc.Value == avCol.Value)));
                Assert.NotNull(vCombined);
            }
            //if returend two correct instances
            else
            {
                var res = CSPLemmas.Lemma8(instance, v, c);
                Assert.Equal(2, res.Count);
                Assert.True(
                    res.Any(inst => inst.Result.Any(p => p.Variable == v && p.Color == c)) ||
                    res.Any(inst => inst.Result.Any(p => p.Variable == v2 && p.Color == c2)));
            }
        }

        [Theory]
        [MemberData(nameof(GetDataLemma9))]
        public void Lemma9Test(CspInstance instance, Variable v, Color c, Variable v2, Color c2)
        {
            var res = CSPLemmas.Lemma9(instance, v, c);
            Assert.Equal(2, res.Count);
            Assert.True(
                res.Any(inst => inst.Result.Any(p => p.Variable == v && p.Color == c)) ||
                res.Any(inst => inst.Result.Any(p => p.Variable == v2 && p.Color == c2)));
        }


        [Theory]
        [MemberData(nameof(GetDataLemma11))]
        public void Lemma11Test(CspInstance instance)
        {
            foreach (var v in instance.Variables)
            {
                foreach (var c in v.AvalibleColors)
                {

                    if ((c.Restrictions.Count >= 3 && v.AvalibleColors.Count == 4) ||
                        (c.Restrictions.Count >= 4 && v.AvalibleColors.Count == 3)) // Lemma11 applies
                    {
                        if (c.Restrictions.Select(r => r.Variable).Distinct().Count() != c.Restrictions.Select(r => r.Variable).Count())
                        {
                            Assert.Throws<ArgumentException>(() => CSPLemmas.Lemma11(instance, v, c));
                        }
                        else
                        {
                            var res = CSPLemmas.Lemma11(instance, v, c);
                            Assert.Equal(2, res.Count);
                            Assert.Contains(res, inst => inst.Result.Any(p => p.Variable == v && p.Color == c));
                        }
                    }
                    else
                    {
                        var res = CSPLemmas.Lemma11(instance, v, c);
                        Assert.Single(res);
                    }
                }
            }
        }


        [Theory]
        [MemberData(nameof(GetDataLemma12))]
        public void Lemma12Test(CspInstance instance)
        {
            foreach (var v in instance.Variables)
            {
                foreach (var c in v.AvalibleColors)
                {
                    if (c.Restrictions.Count == 3)
                    {
                        foreach (var restrictionPair in c.Restrictions)
                        {
                            (var v2, var c2) = restrictionPair;
                            if (v2.AvalibleColors.Count == 4) // Lemma12 applies
                            {
                                if (c2.Restrictions.Count != 2)
                                    Assert.Throws<ArgumentException>(() => CSPLemmas.Lemma12(instance, v, c));

                                var res = CSPLemmas.Lemma12(instance, v, c);
                                Assert.True(res.Count >= 2 && res.Count <= 3);
                                Assert.Contains(res, inst => inst.Result.Any(p => p.Variable == v && p.Color == c));
                            }
                            else
                            {
                                var res = CSPLemmas.Lemma12(instance, v, c);
                                Assert.Single(res);
                            }
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetDataLemma13))]
        public void Lemma13Test(CspInstance instance)
        {
            foreach (var v in instance.Variables)
            {
                foreach (var c in v.AvalibleColors)
                {
                    if (c.Restrictions.Count == 3)
                    {
                        foreach (var restrictionPair in c.Restrictions)
                        {
                            (var v2, var c2) = restrictionPair;
                            if (c2.Restrictions.Count == 2) // Lemma13 applies
                            {

                                if (c.Restrictions.Any(r => r.Variable.AvalibleColors.Count != 3))
                                    Assert.Throws<ArgumentException>(() => CSPLemmas.Lemma13(instance, v, c));

                                var res = CSPLemmas.Lemma13(instance, v, c);
                                Assert.Equal(3, res.Count);
                                Assert.Contains(res, inst => inst.Result.Any(p => p.Variable == v && p.Color == c));
                            }
                            else
                            {
                                var res = CSPLemmas.Lemma13(instance, v, c);
                                Assert.Single(res);
                            }
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetDataLemma15))]
        public void Lemma15Test(CspInstance instance)
        {
            var res = CSPLemmas.Lemma15(instance);
            foreach(var inst in res)
            {
                Assert.Null(CSPLemmas.findSmallBadThreeComponent(inst.Restrictions));
            }
        }

        [Theory]
        [MemberData(nameof(GetDataLemma17))]
        public void Lemma17Test(CspInstance instance)
        {
            var res = CSPLemmas.Lemma17(instance);
            foreach (var inst in res)
            {
                Assert.Null(CSPLemmas.findBigThreeComponent(inst.Restrictions));
            }
        }
    }
}