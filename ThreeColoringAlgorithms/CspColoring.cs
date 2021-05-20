using CSP;
using CSPGraphConverter;
using CSPSimplifying;
using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreeColoring.Algorithms;

namespace ThreeColoringAlgorithms
{
    public class CspColoring : IThreeColoringAlgorithm
    {
        delegate void Lemma(CspInstance instance, Variable v, out bool applied);
        Lemma[] simpleLemmas = new Lemma[]
           {
                CSPLemmas.Lemma2, CSPLemmas.Lemma3,CSPLemmas.Lemma4,CSPLemmas.Lemma5,CSPLemmas.Lemma6,
           };

        public int[] ThreeColorig(Graph g)
        {
            var instance = Converter.GraphToCSP(g);
           
            return Rec(instance);
        }

        int[] Rec(CspInstance instance)
        {
            bool applied;
            for (int i = 0; i < simpleLemmas.Length; i++)
            {
                foreach (var v in instance.Variables)
                {
                    simpleLemmas[i](instance, v, out applied);
                    if (applied)
                    {
                        return Rec(instance);
                    }
                }
                
            }
            List<CspInstance> instances = new() { instance };

            //lemma 8
            foreach (var variable in instances[0].Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma8(instances[0], variable, color, out applied);
                    if (applied)
                    {
                        foreach (var retInstance in instances)
                        {
                            var result = Rec(retInstance);
                            if (result != null) return result;
                        }
                        return null;
                    }
                }
            }

            //lemma 9
            foreach (var variable in instances[0].Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma9(instances[0], variable, color, out applied);
                    if (applied)
                    {
                        foreach (var retInstance in instances)
                        {
                            var result = Rec(retInstance);
                            if (result != null) return result;
                        }
                        return null;
                    }
                }
            }

            //lemma 10
            foreach (var variable in instances[0].Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    var neighbors = color.Restrictions.Select(r => r.Variable).Distinct();
                    foreach (var neighbor in neighbors)
                    {
                        var restrictionToNeighbor = color.Restrictions.Where(r => r.Variable == neighbor);
                        if (restrictionToNeighbor.Count() >= 2)
                        {
                            instances = CSPLemmas.Lemma10(instances[0], variable, color, restrictionToNeighbor.First().Variable);
                            if (instances.Count > 1)
                            {
                                foreach (var retInstance in instances)
                                {
                                    var result = Rec(retInstance);
                                    if (result != null) return result;
                                }
                                return null;
                            }
                        }
                    }
                }
            }

            //lemma 11
            foreach (var variable in instances[0].Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma11(instances[0], variable, color);
                    if (instances.Count > 1)
                    {
                        foreach (var retInstance in instances)
                        {
                            var result = Rec(retInstance);
                            if (result != null) return result;
                        }
                        return null;
                    }
                }
            }


            //lemma 12
            foreach (var variable in instances[0].Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma12(instances[0], variable, color);
                    if (instances.Count > 1)
                    {
                        foreach (var retInstance in instances)
                        {
                            var result = Rec(retInstance);
                            if (result != null) return result;
                        }
                        return null;
                    }
                }
            }


            //lemma 13
            foreach (var variable in instances[0].Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma13(instances[0], variable, color);
                    if (instances.Count > 1)
                    {
                        foreach (var retInstance in instances)
                        {
                            var result = Rec(retInstance);
                            if (result != null) return result;
                        }
                        return null;
                    }
                }
            }

            //lemma 15
            instances = CSPLemmas.Lemma15(instances[0], out applied);
            if (applied)
            {
                foreach (var retInstance in instances)
                {
                    var result = Rec(retInstance);
                    if (result != null) return result;
                }
                return null;
            }

            //lemma 17
            instances = CSPLemmas.Lemma17(instances[0], out applied);
            if (applied)
            {
                foreach (var retInstance in instances)
                {
                    var result = Rec(retInstance);
                    if (result != null) return result;
                }
                return null;
            }

            //lemma 18
            instances = CSPLemmas.Lemma18(instances[0], out applied);
            if (applied)
            {
                foreach (var retInstance in instances)
                {
                    var result = Rec(retInstance);
                    if (result != null) return result;
                }
                return null;
            }

            //lemma 19
            var isColoring = CSPLemmas.Lemma19(instances[0]);
            if (isColoring)
            {

            }
            else
            {
                return null;
            }

            return null;
        }
    }
}

