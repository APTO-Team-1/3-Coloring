using CSP;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void Lemma6(CspInstance instance, Variable v, out bool applied)
        {
            applied = false;
            for (int i = 0; i < v.AvalibleColors.Count; i++)
            {
                var c = v.AvalibleColors[i];
                var neighbors = c.Restrictions.Select(r => r.Variable).Distinct();
                foreach (var v2 in neighbors)
                {
                    if (c.Restrictions.Where(r => r.Variable == v2).Count() == 3)
                    {
                        applied = true;
                        instance.RemoveColor(v, c);
                        RemoveVariableWith2Colors(instance, v);
                    }
                }
            }

        }
    }
}
