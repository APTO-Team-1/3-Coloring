using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static List<CspInstance> Lemma17(CspInstance instance, out bool applied)
        {
            List<CspInstance> result = new();
            HashSet<Pair> set = FindBadThreeComponent(instance, false);
            if (set != null)
            {
                applied = true;
                List<Pair> witness = FindWitness(set, instance.Restrictions);
                if (witness != null)
                {
                    CspInstance instance1 = instance.Clone();
                    int neighbours = 0;
                    if (instance.Restrictions.Contains(new Restriction(witness[4], witness[1]))) neighbours++;
                    if (instance.Restrictions.Contains(new Restriction(witness[4], witness[2]))) neighbours++;
                    if (instance.Restrictions.Contains(new Restriction(witness[4], witness[3]))) neighbours++;

                    if (neighbours == 1)
                    {
                        CspInstance instance2 = instance.Clone();
                        instance1.AddToResult(witness[4]);
                        instance2.RemoveColor(witness[4]);
                        if (instance.Restrictions.Contains(new Restriction(witness[1], witness[2])))
                        {
                            CspInstance instance3 = instance2.Clone();
                            CspInstance instance4 = instance2.Clone();
                            instance2.AddToResult(witness[0]);
                            instance3.AddToResult(witness[1]);
                            instance4.AddToResult(witness[2]);

                            result.Add(instance1);
                            result.Add(instance2);
                            result.Add(instance3);
                            result.Add(instance4);
                        }
                        else if (instance.Restrictions.Contains(new Restriction(witness[1], witness[3])))
                        {
                            CspInstance instance3 = instance2.Clone();
                            CspInstance instance4 = instance2.Clone();
                            instance2.AddToResult(witness[0]);
                            instance3.AddToResult(witness[1]);
                            instance4.AddToResult(witness[3]);

                            result.Add(instance1);
                            result.Add(instance2);
                            result.Add(instance3);
                            result.Add(instance4);
                        }
                        else
                        {
                            result.AddRange(Lemma13(instance1, witness[0].Variable, witness[0].Color));
                            result.AddRange(Lemma13(instance2, witness[0].Variable, witness[0].Color));
                        }
                    }
                    else if (neighbours == 2)
                    {
                        CspInstance instance2 = instance1.Clone();
                        instance1.AddToResult(witness[4]);
                        foreach (Pair p in witness[4].Color.Restrictions)
                        {
                            RemoveVariableWith2Colors(instance1, p.Variable);
                        }
                        instance2.RemoveColor(witness[4]);
                        result.Add(instance1);
                        result.Add(instance2);
                    }
                    else if (neighbours == 3)
                    {
                        CspInstance instance2 = instance1.Clone();
                        instance1.AddToResult(witness[4]);
                        instance1.AddToResult(witness[0]);
                        instance2.RemoveColor(witness[4]);
                        result.Add(instance1);
                        result.Add(instance2);
                    }
                }
                else //whitness not found
                {
                    throw new ApplicationException("whitness not found");
                }
                return result;
            }
            else
            {
                applied = false;
                return new() { instance };
            }

        }

        private static List<Pair> FindWitness(HashSet<Pair> component, IReadOnlySet<Restriction> restrictions)
        {
            foreach (Pair p1 in component)
            {
                foreach (var p2 in p1.Color.Restrictions)
                {
                    var p3p4 = p1.Color.Restrictions.Where(r => r != p2);
                    var p3 = p3p4.ElementAt(0);
                    var p4 = p3p4.ElementAt(1);
                    var p5list = p2.Color.Restrictions.Where(p => p != p1 && p != p2 && p != p3);
                    if (p5list.Any())
                    {
                        return new() { p1, p2, p3, p4, p5list.First() };
                    }
                }
            }
            return null;
        }
    }
}