using CSP;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        public static void Lemma4(CspInstance instance)
        {
            foreach (var v in instance.Variables)
            {
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
                                instance.RemoveColor(v, c2);
                                RemoveVariableWith2Colors(instance, v);

                                i = v.AvalibleColors.Count; // to break 2 loops at once
                                break;

                            }
                        }
                    }

                }
            }
        }
    }
}
