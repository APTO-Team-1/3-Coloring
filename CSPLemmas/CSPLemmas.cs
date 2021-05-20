using CSP;
using System;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void RemoveVariableWith2Colors(CspInstance instance, Variable v)
        {
#if DEBUG
            if (v.AvalibleColors.Count != 2)
            {
                throw new ArgumentException("Variable doesn't have 2 avalible colors");
            }
#endif
            foreach (var pair1 in v.AvalibleColors[0].Restrictions)
            {
                foreach (var pair2 in v.AvalibleColors[1].Restrictions)
                {
                    instance.AddRestriction(pair1, pair2);
                }
            }

            instance.RemoveVariable(v);
        }
    }
}
