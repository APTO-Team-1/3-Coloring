using CSP;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSP.Tests
{
    public class CspInstanceTests
    {
        [Fact()]
        public void AddRestrictionsTest()
        {
            var csp = new CspInstance();
            var variable1 = new Variable(2);
            var variable2 = new Variable(2);
            csp.Variables.Add(variable1);
            csp.Variables.Add(variable2);
            var color1 = variable1.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            var restriction = new Restriction(variable1, color1, variable2, color2);
            csp.AddRestriction(restriction);
            Assert.Equal(1, csp.Restrictions.Count);
            csp.AddRestriction(restriction);
            Assert.Equal(1, csp.Restrictions.Count);

            CheckInstanceCorrectness(csp);

        }

        [Fact()]
        public void AddRestrictionsTest2()
        {
            var csp = new CspInstance();
            var variable1 = new Variable(2);
            var variable2 = new Variable(2);
            csp.Variables.Add(variable1);
            csp.Variables.Add(variable2);
            var color1 = variable1.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            var restriction = new Restriction(variable1, color1, variable2, color2);
            var restriction2 = new Restriction(variable1, color1, variable2, color2);
            csp.AddRestriction(restriction);
            csp.AddRestriction(restriction2);
            Assert.Equal(1, csp.Restrictions.Count);

            CheckInstanceCorrectness(csp);

        }

        [Fact()]
        public void AddRestrictionsTest3()
        {
            var csp = new CspInstance();
            var variable1 = new Variable(1);
            var variable2 = new Variable(2);
            csp.Variables.Add(variable1);
            csp.Variables.Add(variable2);
            var color1 = variable1.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            var color3 = variable2.AvalibleColors[1];
            var restriction = new Restriction(variable1, color1, variable2, color2);
            var restriction2 = new Restriction(variable1, color1, variable2, color3);
            csp.AddRestriction(restriction);
            csp.AddRestriction(restriction2);
            Assert.Equal(2, csp.Restrictions.Count);

            CheckInstanceCorrectness(csp);


        }

        [Fact()]
        public void AddRestrictionTest4()
        {
            var csp = new CspInstance();
            var variable1 = new Variable(2);
            var variable2 = new Variable(2);
            csp.Variables.Add(variable1);
            csp.Variables.Add(variable2);
            var color1 = variable1.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            csp.AddRestriction(new Pair(variable1, color1), new Pair(variable2, color2));
            Assert.Equal(1, csp.Restrictions.Count);
            csp.AddRestriction(new Pair(variable1, color1), new Pair(variable2, color2));
            Assert.Equal(1, csp.Restrictions.Count);

            CheckInstanceCorrectness(csp);
        }


        [Fact()]
        public void RemoveRestrictionsTest()
        {
            var csp = new CspInstance();
            var variable1 = new Variable(1);
            var variable2 = new Variable(2);
            csp.Variables.Add(variable1);
            csp.Variables.Add(variable2);
            var color1 = variable1.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            var color3 = variable2.AvalibleColors[1];
            var restriction = new Restriction(variable1, color1, variable2, color2);
            var restriction2 = new Restriction(variable1, color1, variable2, color3);
            csp.AddRestriction(restriction);
            Assert.Equal(1, csp.Restrictions.Count);
            csp.RemoveRestriction(restriction2);
            Assert.Equal(1, csp.Restrictions.Count);
            csp.RemoveRestriction(restriction);
            Assert.Equal(0, csp.Restrictions.Count);

            CheckInstanceCorrectness(csp);

        }

        [Fact()]
        public void RemoveRestrictionsTest2()
        {
            var csp = new CspInstance();
            var variable1 = new Variable(1);
            var variable2 = new Variable(2);
            csp.Variables.Add(variable1);
            csp.Variables.Add(variable2);
            var color1 = variable1.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            var color3 = variable2.AvalibleColors[1];
            csp.AddRestriction(new Pair(variable1, color1), new Pair(variable2, color2));
            Assert.Equal(1, csp.Restrictions.Count);
            csp.RemoveRestriction(new Pair(variable1, color1), new Pair(variable2, color3));
            Assert.Equal(1, csp.Restrictions.Count);
            csp.RemoveRestriction(new Pair(variable1, color1), new Pair(variable2, color2));
            Assert.Equal(0, csp.Restrictions.Count);

            CheckInstanceCorrectness(csp);

        }

        [Fact()]
        public void AddVariableTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            csp.Variables.Add(variable);
            Assert.Equal(1, csp.Variables.Count);
            csp.Variables.Add(variable);
            Assert.Equal(1, csp.Variables.Count);
            var variable2 = new Variable(4);
            csp.Variables.Add(variable2);
            Assert.Equal(2, csp.Variables.Count);

            CheckInstanceCorrectness(csp);
        }

        [Fact()]
        public void RemoveVariableTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            var variable2 = new Variable(4);
            csp.Variables.Add(variable);
            Assert.Equal(1, csp.Variables.Count);
            csp.Variables.Remove(variable2);
            Assert.Equal(1, csp.Variables.Count);
            csp.Variables.Remove(variable);
            Assert.Equal(0, csp.Variables.Count);

            CheckInstanceCorrectness(csp);
        }

        [Fact()]
        public void AddRestrictionToColorTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            var variable2 = new Variable(4);

            csp.Variables.Add(variable);
            csp.Variables.Add(variable2);

            var color1 = variable.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];

            var pair1 = new Pair(variable, color1);
            var pair2 = new Pair(variable2, color2);

            csp.AddRestriction(new Restriction(pair1, pair2));
            Assert.Equal(1, color1.Restrictions.Count);
            Assert.Equal(1, color2.Restrictions.Count);
            Assert.Contains(pair2, color1.Restrictions);
            Assert.Contains(pair1, color2.Restrictions);
            Assert.DoesNotContain(pair2, color2.Restrictions);
            Assert.DoesNotContain(pair1, color1.Restrictions);

            CheckInstanceCorrectness(csp);

        }

        [Fact()]
        public void RemoveRestrictionFromColorTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            var variable2 = new Variable(4);
            csp.Variables.Add(variable);
            csp.Variables.Add(variable2);

            var color1 = variable.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            var color3 = variable2.AvalibleColors[1];

            var pair1 = new Pair(variable, color1);
            var pair2 = new Pair(variable2, color2);
            var pair3 = new Pair(variable2, color3);

            csp.AddRestriction(new Restriction(pair1, pair2));
            csp.AddRestriction(new Restriction(pair1, pair3));
            Assert.Equal(2, color1.Restrictions.Count);

            csp.RemoveRestriction(new Restriction(pair1, pair2));
            Assert.Equal(1, color1.Restrictions.Count);
            Assert.DoesNotContain(pair2, color1.Restrictions);
            Assert.DoesNotContain(pair1, color2.Restrictions);
            Assert.Contains(pair3, color1.Restrictions);

            csp.RemoveRestriction(new Restriction(pair1, pair3));
            Assert.Equal(0, color1.Restrictions.Count);
            Assert.DoesNotContain(pair3, color1.Restrictions);

            CheckInstanceCorrectness(csp);
        }

        [Fact()]
        public void AddToResultTest()
        {
            var instance = new CspInstance();
            var variable = new Variable(2);
            instance.Variables.Add(variable);
            Assert.Equal(1, instance.Variables.Count);
            instance.AddToResult(new Pair(variable, variable.AvalibleColors[0]));
            Assert.Equal(0, instance.Variables.Count);

            CheckInstanceCorrectness(instance);
        }

        [Fact()]
        public void AddToResultTest2()
        {
            var instance = new CspInstance();
            var variable = new Variable(2);
            instance.Variables.Add(variable);
            Assert.Equal(1, instance.Variables.Count);
            instance.AddToResult(variable, variable.AvalibleColors[0]);
            Assert.Equal(0, instance.Variables.Count);

            CheckInstanceCorrectness(instance);
        }

        [Fact()]
        public void AddToResultTest3()
        {
            var instance = new CspInstance();
            var variable1 = new Variable(2);
            var variable2 = new Variable(4);
            var variable3 = new Variable(4);
            instance.Variables.Add(variable1);
            instance.Variables.Add(variable2);
            instance.Variables.Add(variable3);
            Assert.Equal(3, instance.Variables.Count);
            var color11 = variable1.AvalibleColors[0];
            var color12 = variable1.AvalibleColors[1];
            var color21 = variable2.AvalibleColors[0];
            var color22 = variable2.AvalibleColors[1];
            var color23 = variable2.AvalibleColors[3];
            var color31 = variable3.AvalibleColors[0];
            var color32 = variable3.AvalibleColors[1];
            instance.AddRestriction(new Pair(variable1, color11), new Pair(variable2, color21));
            instance.AddRestriction(new Pair(variable1, color11), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable1, color12), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable3, color31), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable3, color32), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable3, color32), new Pair(variable2, color23));
            Assert.Equal(6, instance.Restrictions.Count);
            instance.AddToResult(new Pair(variable1, color11));
            Assert.Equal(2, instance.Variables.Count);
            Assert.Equal(2, variable2.AvalibleColors.Count);
            Assert.Equal(1, instance.Restrictions.Count);

            CheckInstanceCorrectness(instance);
        }

        [Fact()]
        public void SimpleCloneTest()
        {
            var csp = new CspInstance();
            var cloned = csp.Clone();
            Assert.Equal(cloned.Variables.Count, csp.Variables.Count);
            Assert.Equal(cloned.Restrictions.Count, csp.Restrictions.Count);
            Assert.Equal(cloned.Result.Count, csp.Result.Count);

            CheckInstanceCorrectness(cloned);
        }

        [Fact()]
        public void CloneWithVariablesTest()
        {
            var csp = new CspInstance();
            int variablesCount = 1000;
            for (int i = 0; i < variablesCount; i++)
            {
                var variable = new Variable(i % 4 + 1);
                csp.Variables.Add(variable);
            }
            var cloned = csp.Clone();
            Assert.Equal(cloned.Variables.Count, csp.Variables.Count);
            Assert.Equal(cloned.Restrictions.Count, csp.Restrictions.Count);
            Assert.Equal(cloned.Result.Count, csp.Result.Count);

            CheckInstanceCorrectness(cloned);
        }

        [Fact()]
        public void CloneWithRestrictionsTest()
        {
            var csp = new CspInstance();
            var variablesList = new List<Variable>();
            int variablesCount = 1000;
            for (int i = 0; i < variablesCount; i++)
            {
                var variable = new Variable(i % 4 + 1);
                variablesList.Add(variable);
                csp.Variables.Add(variable);
            }

            for (int i = 0; i < variablesCount * 10; i++)
            {
                var variable1 = variablesList[(i * 21 + 5) % variablesCount];
                var variable2 = variablesList[(i * 12 + 11) % variablesCount];
                var color1 = variable1.AvalibleColors[i % variable1.AvalibleColors.Count];
                var color2 = variable2.AvalibleColors[i % variable2.AvalibleColors.Count];
                csp.AddRestriction(new Pair(variable1, color1), new Pair(variable2, color2));
            }

            var cloned = csp.Clone();

            Assert.Equal(cloned.Variables.Count, csp.Variables.Count);
            Assert.Equal(cloned.Restrictions.Count, csp.Restrictions.Count);
            Assert.Equal(cloned.Result.Count, csp.Result.Count);

            foreach (var res in csp.Restrictions)
            {
                Assert.Single(cloned.Restrictions, r =>
                {
                    Pair p1, p2;
                    if (r.Pair1.Variable.Id == res.Pair1.Variable.Id)
                    {
                        p1 = r.Pair1;
                        if (r.Pair2.Variable.Id == res.Pair2.Variable.Id)
                        {
                            p2 = r.Pair2;
                        }
                        else return false;
                    }
                    else if (r.Pair2.Variable.Id == res.Pair1.Variable.Id)
                    {
                        p1 = r.Pair2;
                        if (r.Pair1.Variable.Id == res.Pair2.Variable.Id)
                        {
                            p2 = r.Pair1;
                        }
                        else return false;
                    }
                    else return false;

                    if (p1.Color.Value != res.Pair1.Color.Value || p2.Color.Value != res.Pair2.Color.Value)
                    {
                        return false;
                    }

                    return true;
                });
            }

            CheckInstanceCorrectness(cloned);
        }

        [Fact()]
        public void CloneWithResultsTest()
        {
            var csp = new CspInstance();
            var variablesList = new List<Variable>();
            int variablesCount = 1000;
            for (int i = 0; i < variablesCount; i++)
            {
                var variable = new Variable(i % 4 + 1);
                variablesList.Add(variable);
                csp.Variables.Add(variable);
            }
            for (int i = 0; i < variablesList.Count; i += 5)
            {
                var variable = variablesList[i];
                csp.AddToResult(new Pair(variable, variable.AvalibleColors[i % variable.AvalibleColors.Count]));
            }

            var cloned = csp.Clone();
            Assert.Equal(cloned.Variables.Count, csp.Variables.Count);
            Assert.Equal(cloned.Restrictions.Count, csp.Restrictions.Count);
            Assert.Equal(cloned.Result.Count, csp.Result.Count);

            foreach (var res in csp.Result)
            {
                Assert.Single(cloned.Result, p =>
                {
                    return p.Variable.Id == res.Variable.Id && p.Color.Value == res.Color.Value;
                });
            }

            CheckInstanceCorrectness(cloned);
        }

        [Fact()]
        public void CloneCheckInvariabilityTest()
        {
            var csp = new CspInstance();
            var variablesList = new List<Variable>();
            int variablesCount = 1000;
            for (int i = 0; i < variablesCount; i++)
            {
                var variable = new Variable(i % 4 + 1);
                variablesList.Add(variable);
                csp.Variables.Add(variable);
            }
            for (int i = 0; i < variablesCount * 10; i++)
            {
                var variable1 = variablesList[(i * 21 + 5) % variablesCount];
                var variable2 = variablesList[(i * 12 + 11) % variablesCount];
                var color1 = variable1.AvalibleColors[i % variable1.AvalibleColors.Count];
                var color2 = variable2.AvalibleColors[i % variable2.AvalibleColors.Count];
                csp.AddRestriction(new Pair(variable1, color1), new Pair(variable2, color2));
            }
            for (int i = 0; i < variablesList.Count; i += 5)
            {
                var variable = variablesList[i];
                csp.AddToResult(new Pair(variable, variable.AvalibleColors[i % variable.AvalibleColors.Count]));
            }

            var variableMem = new List<Variable>(csp.Variables);
            var resultMem = new List<Pair>(csp.Result);
            var restrictionsMem = new List<Restriction>(csp.Restrictions);

            _ = csp.Clone();

            Assert.Equal(variableMem.Count, csp.Variables.Count);
            Assert.Equal(resultMem.Count, csp.Result.Count);
            Assert.Equal(restrictionsMem.Count, csp.Restrictions.Count);

            foreach (var variable in variableMem)
            {
                Assert.Contains(variable, csp.Variables);
            }

            foreach (var result in resultMem)
            {
                Assert.Contains(result, csp.Result);
            }

            foreach (var restriction in restrictionsMem)
            {
                Assert.Contains(restriction, csp.Restrictions);
            }

            CheckInstanceCorrectness(csp);
        }

        private static void CheckInstanceCorrectness(CspInstance instance)
        {
            foreach (var result in instance.Result)
            {
                Assert.DoesNotContain(result.Variable, instance.Variables);
                Assert.Contains(result.Color, result.Variable.AvalibleColors);
            }

            foreach (var restriction in instance.Restrictions)
            {
                Assert.Single(instance.Variables, v =>
                {
                    return restriction.Pair1.Variable == v && v.AvalibleColors.Contains(restriction.Pair1.Color);
                });
                Assert.Single(instance.Variables, v =>
                {
                    return restriction.Pair2.Variable == v && v.AvalibleColors.Contains(restriction.Pair2.Color);
                });
            }

            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    foreach (var pair in color.Restrictions)
                    {
                        Assert.Single(instance.Restrictions, r =>
                        {
                            return r.Contains(color) && r.Contains(pair.Color) && r.Contains(variable) && r.Contains(pair.Variable);
                        });
                    }
                }
            }
        }

        [Fact()]
        public void RemoveColorTest()
        {
            var instance = new CspInstance();
            var variable1 = new Variable(2);
            var variable2 = new Variable(4);
            var variable3 = new Variable(4);
            instance.Variables.Add(variable1);
            instance.Variables.Add(variable2);
            instance.Variables.Add(variable3);
            Assert.Equal(3, instance.Variables.Count);
            var color11 = variable1.AvalibleColors[0];
            var color12 = variable1.AvalibleColors[1];
            var color21 = variable2.AvalibleColors[0];
            var color22 = variable2.AvalibleColors[1];
            var color23 = variable2.AvalibleColors[3];
            var color31 = variable3.AvalibleColors[0];
            var color32 = variable3.AvalibleColors[1];
            instance.AddRestriction(new Pair(variable1, color11), new Pair(variable2, color21));
            instance.AddRestriction(new Pair(variable1, color11), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable1, color12), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable3, color31), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable3, color32), new Pair(variable2, color22));
            instance.AddRestriction(new Pair(variable3, color32), new Pair(variable2, color23));
            
            instance.RemoveColor(variable1,color11);

            Assert.Equal(3, instance.Variables.Count);
            Assert.Equal(1, variable1.AvalibleColors.Count);
            Assert.Equal(4, instance.Restrictions.Count);

            instance.RemoveColor(variable2, color23);
            Assert.Equal(3, variable2.AvalibleColors.Count);

            CheckInstanceCorrectness(instance);
        }
    }
}