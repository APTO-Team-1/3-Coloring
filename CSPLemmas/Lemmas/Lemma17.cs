﻿using CSP;
using System;
using System.Collections.Generic;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        public static (List<CspInstance>, bool) Lemma17(CspInstance instance)
        {
            List<CspInstance> result = new();
            HashSet<Pair> set = findBigThreeComponent(instance.Restrictions);
            if(set != null)
            {
                List<Pair> witness = findWitness(set, instance.Restrictions);
                if(witness != null)
                {
                    CspInstance instance1 = instance.Clone();
                    int neighbours = 0;
                    if (instance.Restrictions.Contains(new Restriction(witness[4], witness[1]))) neighbours++;
                    if (instance.Restrictions.Contains(new Restriction(witness[4], witness[2]))) neighbours++;
                    if (instance.Restrictions.Contains(new Restriction(witness[4], witness[3]))) neighbours++;

                    if(neighbours == 1)
                    {
                        CspInstance instance2 = instance.Clone();
                        instance1.AddToResult(witness[4]);
                        instance2.RemoveColor(witness[4]);
                        if(instance.Restrictions.Contains(new Restriction(witness[1], witness[2])))
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
                        else if(instance.Restrictions.Contains(new Restriction(witness[1], witness[3])))
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
                    else if(neighbours == 2)
                    {
                        CspInstance instance2 = instance1.Clone();
                        instance1.AddToResult(witness[4]);
                        foreach(Pair p in witness[4].Color.Restrictions)
                        {
                            RemoveVariableWith2Colors(instance1, p.Variable);
                        }
                        instance2.RemoveColor(witness[4]);
                        result.Add(instance1);
                        result.Add(instance2);
                    }
                    else if(neighbours == 3)
                    {
                        CspInstance instance2 = instance1.Clone();
                        instance1.AddToResult(witness[4]);
                        instance1.AddToResult(witness[0]);
                        instance2.RemoveColor(witness[4]);
                        result.Add(instance1);
                        result.Add(instance2);
                    }
                }
            } 
            else
            {
                return (new() { instance }, false);
            }

            return (result, true);
        }
        public static HashSet<Pair> findBigThreeComponent(IReadOnlySet<Restriction> restrictions)
        {
            foreach (Restriction res in restrictions)
            {
                if (res.Pair1.Color.Restrictions.Count == 3)
                {
                    HashSet<Pair> pairs = new() { res.Pair1 };
                    findComponentForPair(restrictions, res.Pair1, pairs);
                    if (pairs != null)
                    {
                        HashSet<Variable> distinctVariables = new();
                        foreach (Pair p in pairs)
                        {
                            distinctVariables.Add(p.Variable);
                        }
                        if (distinctVariables.Count > 4)
                        {
                            return pairs;
                        }
                    }
                }
                if (res.Pair2.Color.Restrictions.Count == 3)
                {
                    HashSet<Pair> pairs = new() { res.Pair2 };
                    findComponentForPair(restrictions, res.Pair2, pairs);
                    if (pairs != null)
                    {
                        HashSet<Variable> distinctVariables = new();
                        foreach (Pair p in pairs)
                        {
                            distinctVariables.Add(p.Variable);
                        }
                        if (distinctVariables.Count > 4)
                        {
                            return pairs;
                        }
                    }
                }
            }
            return null;
        }
        private static List<Pair> findWitness(HashSet<Pair> component, IReadOnlySet<Restriction> restrictions)
        {
            foreach(Pair p1 in component)
            {
                foreach(Pair p2 in component)
                {
                    if (!restrictions.Contains(new Restriction(p1, p2))) break;
                    foreach(Pair p3 in component)
                    {
                        if (!restrictions.Contains(new Restriction(p1, p3)) || p3.Equals(p2)) break;
                        foreach (Pair p4 in component)
                        {
                            if (!restrictions.Contains(new Restriction(p1, p4)) || p4.Equals(p2) || p4.Equals(p3)) break;
                            foreach (Pair p5 in component)
                            {
                                if (!restrictions.Contains(new Restriction(p2, p5)) || p5.Equals(p1) || p5.Equals(p3) || p5.Equals(p4)) break;
                                    
                                return new List<Pair>() { p1, p2, p3, p4, p5 };
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}