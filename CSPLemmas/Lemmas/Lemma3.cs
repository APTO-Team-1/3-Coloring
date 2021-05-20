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
                var distinctVariables = c1.Restrictions.Select(r => r.Variable).Distinct();
                if (distinctVariables.Count() == 1)
                {
                    var v2 = distinctVariables.First();
                    for (int j = 0; j < v2.AvalibleColors.Count; j++)
                    {
                        var c2 = v2.AvalibleColors[j];
                        if (!c1.Restrictions.Select(r => r.Color).Contains(c2))
                        {
                            var distinctVariables2 = c2.Restrictions.Select(r => r.Variable).Distinct();
                            if (distinctVariables2.Count() == 1)
                            {
                                var v21 = distinctVariables2.First();
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
    }
}
