using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void RemoveVariableWith2Colors(CspInstance instance, Variable v)
        {
#if DEBUG
            if (v.AvalibleColors.Count > 2)
            {
                throw new ArgumentException("Variable doesn't have 2 avalible colors");
            }
#endif
            if (v.AvalibleColors.Count == 2)
            {
                var c1Neighbors = new List<Pair>(v.AvalibleColors[0].Restrictions);
                var vc0 = new Pair(v, v.AvalibleColors[0]);
                var vc1 = new Pair(v, v.AvalibleColors[1]);
                List<Pair> restrictionClone = new(vc0.Color.Restrictions);
                instance.ResultRules.Add((2, restrictionClone, vc0, vc1));

                foreach (var pair1 in v.AvalibleColors[0].Restrictions)
                {
                    foreach (var pair2 in v.AvalibleColors[1].Restrictions)
                    {
                        instance.AddRestriction(pair1, pair2);
                    }
                }
                instance.RemoveVariable(v);
            }
            else if (v.AvalibleColors.Count == 1)
            {
                instance.AddToResult(new Pair(v, v.AvalibleColors[0]));
            }

        }

    }
}
