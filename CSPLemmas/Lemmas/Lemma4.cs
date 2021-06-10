using CSP;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void Lemma4(CspInstance instance, Variable v, out bool applied)
        {
            applied = false;
            for (int i = 0; i < v.AvalibleColors.Count; i++)
            {
                var c1 = v.AvalibleColors[i];
                for (int j = 0; j < v.AvalibleColors.Count; j++)
                {
                    var c2 = v.AvalibleColors[j];
                    if (c1 != c2)
                    {
                        if (c1.Restrictions.IsSubsetOf(c2.Restrictions))
                        {
                            applied = true;
                            instance.RemoveColor(v, c2);
                            if (v.AvalibleColors.Count <= 2)
                            {
                                RemoveVariableWith2Colors(instance, v);
                            }

                            i = v.AvalibleColors.Count; // to break 2 loops at once
                            break;
                        }
                    }
                }
            }

        }
    }
}
