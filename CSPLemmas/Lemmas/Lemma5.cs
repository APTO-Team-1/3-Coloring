using CSP;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void Lemma5(CspInstance instance, Variable v, out bool applied)
        {
            applied = false;
            for (int i = 0; i < v.AvalibleColors.Count; i++)
            {
                var c = v.AvalibleColors[i];
                if (c.Restrictions.Count == 0)
                {
                    applied = true;
                    instance.AddToResult(v, c);
                    return;
                }

            }

        }
    }
}
