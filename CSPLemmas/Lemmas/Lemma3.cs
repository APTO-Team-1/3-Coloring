using CSP;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void Lemma3(CspInstance instance, Variable v1, out bool applied)
        {
            applied = false;
            for (int i = 0; i < v1.AvalibleColors.Count; i++)
            {
                var c1 = v1.AvalibleColors[i];
                var v2 = GetDistinctSingleVariableFromColor(c1);
                if (v2 != null)
                {
                    for (int j = 0; j < v2.AvalibleColors.Count; j++)
                    {
                        var c2 = v2.AvalibleColors[j];
                        var v21 = GetDistinctSingleVariableFromColor(c2);
                        if (v21 != null)
                        {
                            if (!c1.Restrictions.Any(p => p.Color == c2) && !c2.Restrictions.Any(p => p.Color == c1))
                            {

                                if (v1 == v21)
                                {
                                    applied = true;
                                    instance.AddToResult(v1, c1);
                                    instance.AddToResult(v2, c2);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Variable GetDistinctSingleVariableFromColor(Color color)
        {
            if (color.Restrictions.Count == 0) return null;
            Pair first = color.Restrictions.First();
            foreach (var pair in color.Restrictions)
            {
                if (pair.Variable.Id != first.Variable.Id) return null;
            }
            return first.Variable;
        }
    }
}
