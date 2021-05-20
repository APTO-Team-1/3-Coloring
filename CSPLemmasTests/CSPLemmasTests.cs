using Xunit;
using CSPSimplifying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP;

namespace CSPSimplifying.Tests
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
            Random r = new(123);
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
            Random r = new(123);
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
            Random r = new(123);
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
           

            CspInstance cspInstance = new();
            Variable v = new(3);
            Variable w = new(3);
            Variable x = new(3);
            Variable y = new(3);
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
            Variable v2 = new(3);
            Variable w2 = new(3);
            Variable x2 = new(3);
            Variable y2 = new(3);
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
            
            CspInstance cspInstance = new();
            Variable v = new(3);
            Variable w = new(3);
            Variable x = new(3);
            Variable y = new(3);
            Variable z = new(3);
            Variable a = new(3);
            Variable b = new(3);
            Variable c = new(3);
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

        public static IEnumerable<object[]> GetDataLemma10()
        {
            var data = new List<object[]>();
            for (int i = 0; i < 10; i++)
            {
                var instance = GetRandomInstance(minColors: 3, maxColors: 4, approximateRestrictionsCount: 50, variableCount: 10);

                data.Add(new object[] { instance });
            }

            return data;
        }
        public static IEnumerable<object[]> GetDataLemma19()
        {
            List<Variable> variables;
            CspInstance instance;
            var data = new List<object[]>();
            Random r = new(12345);
            for (int i = 14; i < 15; i++)
            {
                variables = new();
                instance = new CspInstance();
                for (int j = 0; j < i; j++)
                {
                    var v = new Variable(r.Next(3, 4));
                    instance.AddVariable(v);
                    variables.Add(v);
                }
                for (int j = 0; j < 15; j++)
                {
                    FormGood3Component(i);
                    FormSmall2Component(i);
                }
                foreach (Variable var in variables)
                {
                    bool isInComponent = false;
                    foreach (Color col in var.AvalibleColors)
                    {
                        if (col.Restrictions.Count > 0) isInComponent = true;
                    }
                    if (!isInComponent) instance.RemoveVariable(var);
                }
                data.Add(new object[] { instance });
            }
            return data;

            void FormGood3Component(int varCount)
            {
                Random r = new(123);
                int v1 = r.Next(varCount);
                Color c1 = new(1), c2 = new(1), c3 = new(1), c4 = new(1);
                int v2 = r.Next(varCount);
                int v3 = r.Next(varCount);
                int v4 = r.Next(varCount);
                if (v1 == v2 || v1 == v3 || v1 == v4 || v2 == v3 || v2 == v4 || v3 == v4)
                    return;
                bool flag = false;
                foreach (Color col in variables[v1].AvalibleColors)
                {
                    if (col.Restrictions.Count == 0)
                    {
                        flag = true;
                        c1 = col;
                    }
                }
                if (!flag) return;
                flag = false;
                foreach (Color col in variables[v2].AvalibleColors)
                {
                    if (col.Restrictions.Count == 0)
                    {
                        flag = true;
                        c2 = col;
                    }
                }
                if (!flag) return;
                flag = false;
                foreach (Color col in variables[v3].AvalibleColors)
                {
                    if (col.Restrictions.Count == 0)
                    {
                        flag = true;
                        c3 = col;
                    }
                }
                if (!flag) return;
                flag = false;
                foreach (Color col in variables[v4].AvalibleColors)
                {
                    if (col.Restrictions.Count == 0)
                    {
                        flag = true;
                        c4 = col;
                    }
                }
                if (!flag) return;
                instance.AddRestriction(new Pair(variables[v1], c1), new Pair(variables[v2], c2));
                instance.AddRestriction(new Pair(variables[v1], c1), new Pair(variables[v3], c3));
                instance.AddRestriction(new Pair(variables[v1], c1), new Pair(variables[v4], c4));
                instance.AddRestriction(new Pair(variables[v2], c2), new Pair(variables[v3], c3));
                instance.AddRestriction(new Pair(variables[v2], c2), new Pair(variables[v4], c4));
                instance.AddRestriction(new Pair(variables[v3], c3), new Pair(variables[v4], c4));
            }
            void FormSmall2Component(int varCount)
            {
                Random r = new(123);
                int v1 = r.Next(varCount);
                Color c1 = new(1), c2 = new(1), c3 = new(1);
                int v2 = r.Next(varCount);
                int v3 = r.Next(varCount);
                if (v1 == v2 || v1 == v3 || v2 == v3)
                    return;
                bool flag = false;
                foreach (Color col in variables[v1].AvalibleColors)
                {
                    if (col.Restrictions.Count == 0)
                    {
                        flag = true;
                        c1 = col;
                        break;
                    }
                }
                if (!flag) return;
                flag = false;
                foreach (Color col in variables[v2].AvalibleColors)
                {
                    if (col.Restrictions.Count == 0)
                    {
                        flag = true;
                        c2 = col;
                        break;
                    }
                }
                if (!flag) return;
                flag = false;
                foreach (Color col in variables[v3].AvalibleColors)
                {
                    if (col.Restrictions.Count == 0)
                    {
                        flag = true;
                        c3 = col;
                        break;
                    }
                }
                if (!flag) return;
                instance.AddRestriction(new Pair(variables[v1], c1), new Pair(variables[v2], c2));
                instance.AddRestriction(new Pair(variables[v1], c1), new Pair(variables[v3], c3));
                instance.AddRestriction(new Pair(variables[v2], c2), new Pair(variables[v3], c3));

            }
        }



        #endregion

        delegate void Lemma(CspInstance instance, Variable v, out bool applied);
        private void UseLemma2to6(CspInstance instance, Lemma lemma)
        {
            foreach (var v in instance.Variables)
            {
                lemma(instance, v, out bool applied);
                if (applied)
                {
                    UseLemma2to6(instance, lemma);
                }
            }
        }

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
            UseLemma2to6(instance, CSPLemmas.Lemma2);
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
            UseLemma2to6(instance, CSPLemmas.Lemma3);
            Assert.Contains(pair1, instance.Result);
            Assert.Contains(pair2, instance.Result);
            Assert.DoesNotContain(pair1.Variable, instance.Variables);
            Assert.DoesNotContain(pair2.Variable, instance.Variables);
        }

        [Theory]
        [MemberData(nameof(GetDataLemma4))]
        public void Lemma4Test(CspInstance instance, Pair pair1)
        {
            UseLemma2to6(instance, CSPLemmas.Lemma4);
            Assert.DoesNotContain(pair1.Variable, instance.Variables);
        }

        [Theory]
        [MemberData(nameof(GetDataLemma5))]
        public void Lemma5Test(CspInstance instance, Pair pair)
        {
            UseLemma2to6(instance, CSPLemmas.Lemma5);
            Assert.DoesNotContain(pair.Variable, instance.Variables);
            Assert.Contains(pair, instance.Result);
        }

        [Theory]
        [MemberData(nameof(GetDataLemma6))]
        public void Lemma6Test(CspInstance instance, Pair pair)
        {
            UseLemma2to6(instance, CSPLemmas.Lemma6);
            Assert.DoesNotContain(pair.Color, pair.Variable.AvalibleColors);
        }

        [Theory]
        [MemberData(nameof(GetDataLemma8))]
        public void Lemma8Test(CspInstance instance, Variable v, Color c, Variable v2, Color c2)
        {
            //if two 3 - color vertices changed to one 4 - color
            if (v.AvalibleColors.Count == 3 && v2.AvalibleColors.Count == 3)
            {
                var oldColors = v.AvalibleColors.Select(c => new Color(c.Value, c.Restrictions)).Union(v2.AvalibleColors.Select(c => new Color(c.Value, c.Restrictions))).ToList();
                var res = CSPLemmas.Lemma8(instance, v, c, out _);
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
                var res = CSPLemmas.Lemma8(instance, v, c, out _);
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
            var res = CSPLemmas.Lemma9(instance, v, c, out _);
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
            var res = CSPLemmas.Lemma15(instance, out bool b);
            if (b == true)
            {
                foreach (var inst in res)
                {
                    if (inst != null)
                    {
                        Lemma15Test(inst);
                    }

                }
            }
            else
            {
                Assert.Null(CSPLemmas.FindBadThreeComponent(instance, true));
            }

        }

        [Theory]
        [MemberData(nameof(GetDataLemma17))]
        public void Lemma17Test(CspInstance instance)
        {
            var res = CSPLemmas.Lemma17(instance, out bool applied);
            if (applied == false)
            {
                Assert.Null(CSPLemmas.FindBadThreeComponent(instance, false));
            }
            else
            {
                foreach (var inst in res)
                {
                    Lemma17Test(inst);
                }
            }

        }

        [Theory]
        [MemberData(nameof(GetDataLemma10))]
        public void Lemma10Test(CspInstance instance)
        {
            List<CspInstance> instances = new() { instance };
            Lemma10TestInternal();

            foreach (CspInstance inst in instances)
            {
                foreach (var var in inst.Variables)
                {
                    foreach (var col in var.AvalibleColors)
                    {
                        if (col.Restrictions.Count == 2)
                        {
                            var tempTab = col.Restrictions.ToArray();
                            Assert.NotEqual(tempTab[0].Variable, tempTab[1].Variable);
                        }
                    }
                }
            }


            void Lemma10TestInternal()
            {
                bool foundFlag = false;
                int indexOfInst = 0;
                Variable v = new(1), v2 = new(1);
                Color c = new(1), c2_1 = new(1), c2_2 = new(1);
                foreach (CspInstance inst in instances)
                {
                    foreach (var var in inst.Variables)
                    {
                        foreach (var col in var.AvalibleColors)
                        {
                            foreach (var pair in col.Restrictions)
                            {
                                int resToNeigh = col.Restrictions.Where(r => r.Variable == pair.Variable).Count();
                                if (resToNeigh == pair.Variable.AvalibleColors.Count)
                                {
                                    inst.RemoveRestriction(new Pair(var, col), pair);
                                    resToNeigh--;
                                }
                                if (resToNeigh > 1)
                                {
                                    c = col;
                                    v = var;
                                    indexOfInst = instances.IndexOf(inst);
                                    v2 = pair.Variable;
                                    foundFlag = true;
                                    break;
                                }
                            }
                            if (foundFlag) break;
                        }
                        if (foundFlag) break;
                    }
                    if (foundFlag) break;
                }
                if (foundFlag)
                {
                    CspInstance foundInst = instances[indexOfInst];
                    instances.RemoveAt(indexOfInst);
                    var aaa = CSPLemmas.Lemma10(foundInst, v, c, v2);
                    instances.AddRange(aaa);
                    Lemma10TestInternal();
                }
            }

        }

        [Theory]
        [MemberData(nameof(GetDataLemma10))]
        public void Lemma18Test(CspInstance instance)
        {
            List<CspInstance> instances = new() { instance };
            List<Pair> TwoComponent;

            Lemma18TestInternal();


            foreach (CspInstance inst in instances)  // sprawdzamy czy nie został jakis 2 komponent
            {
                foreach (Variable var in inst.Variables)
                {
                    foreach (Color color in var.AvalibleColors)
                    {
                        if (color.Restrictions.Count == 2)
                        {
                            TwoComponent = new();
                            Color currColor = color;
                            Color lastColor = color;
                            Pair[] tempList = new Pair[2];

                            TwoComponent.Add(currColor.Restrictions.First());
                            currColor = currColor.Restrictions.First().Color;

                            while (currColor.Restrictions.Count == 2)
                            {
                                tempList = currColor.Restrictions.ToArray();
                                if (tempList[0].Color != lastColor)
                                {
                                    TwoComponent.Add(tempList[0]);
                                    lastColor = currColor;
                                    currColor = tempList[0].Color;
                                }
                                else
                                {
                                    TwoComponent.Add(tempList[1]);
                                    lastColor = currColor;
                                    currColor = tempList[1].Color;
                                }
                                Assert.NotEqual(currColor, color);
                            }
                        }
                    }
                }
            }

            void Lemma18TestInternal()
            {
                bool flag = false;
                foreach (CspInstance inst in instances)
                {
                    var afterLemmaInst = CSPLemmas.Lemma18(inst, out bool applied);
                    if (applied)
                    {
                        instances.Remove(inst);
                        instances.AddRange(afterLemmaInst);
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    Lemma18TestInternal();
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetDataLemma19))]
        public void Lemma19Test(CspInstance instance)
        {
            foreach (var R1 in instance.Result)
                foreach (var R2 in instance.Result)
                {
                    bool test = instance.Restrictions.Contains(new Restriction(R1, R2));
                    Assert.False(test);
                }
        }
    }
}