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


        public int[] ThreeColorig(Graph g)
        {
            var instance = Converter.GraphToCSP(g);

            return Rec(instance);
        }

        int[] Rec(CspInstance instance)
        {
            foreach (var restriction in instance.Restrictions)
            {
                if (restriction.Pair1 == restriction.Pair2)
                {
                    instance.RemoveColor(restriction.Pair1);
                }
            }

            foreach (var v in instance.Variables)
            {
                if (v.AvalibleColors.Count == 0)
                {
                    return null;
                }
            }

            bool applied;
            foreach (var v in instance.Variables)
            {
                CSPLemmas.Lemma2(instance, v, out applied);
                if (applied)
                {
                    return Rec(instance);
                }
            }
            foreach (var v in instance.Variables)
            {
                CSPLemmas.Lemma3(instance, v, out applied);
                if (applied)
                {
                    return Rec(instance);
                }
            }
            foreach (var v in instance.Variables)
            {
                CSPLemmas.Lemma4(instance, v, out applied);
                if (applied)
                {
                    return Rec(instance);
                }
            }
            foreach (var v in instance.Variables)
            {
                CSPLemmas.Lemma5(instance, v, out applied);
                if (applied)
                {
                    return Rec(instance);
                }
            }
            foreach (var v in instance.Variables)
            {
                CSPLemmas.Lemma6(instance, v, out applied);
                if (applied)
                {
                    return Rec(instance);
                }
            }

            List<CspInstance> instances;

            //lemma 8
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma8(instance, variable, color, out applied);
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
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma9(instance, variable, color, out applied);
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
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    var neighbors = color.Restrictions.Select(r => r.Variable).Distinct();
                    foreach (var neighbor in neighbors)
                    {
                        var restrictionToNeighbor = color.Restrictions.Where(r => r.Variable == neighbor);
                        if (restrictionToNeighbor.Count() >= 2)
                        {
                            instances = CSPLemmas.Lemma10(instance, variable, color, restrictionToNeighbor.First().Variable);
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
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma11(instance, variable, color);
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
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma12(instance, variable, color);
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
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    instances = CSPLemmas.Lemma13(instance, variable, color);
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
            instances = CSPLemmas.Lemma15(instance, out applied);
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
            instances = CSPLemmas.Lemma17(instance, out applied);
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
            instances = CSPLemmas.Lemma18(instance, out applied);
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
            var isColoring = CSPLemmas.Lemma19(instance);
            if (isColoring)
            {
                return instance.GetResult();
            }
            else
            {
                return null;
            }
        }
    }
}

