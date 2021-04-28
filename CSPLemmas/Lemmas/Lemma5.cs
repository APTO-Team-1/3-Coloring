using CSP;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        public static void Lemma5(CspInstance instance)
        {
            foreach (var v in instance.Variables)
            {
                for (int i = 0; i < v.AvalibleColors.Count; i++)
                {
                    var c = v.AvalibleColors[i];
                    if(c.Restrictions.Count == 0)
                    {
                        instance.AddToResult(v, c);
                    }

                }
            }
        }
    }
}
