using CSP;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void Lemma6(CspInstance instance)
        {
            foreach (var v in instance.Variables)
            {
                for (int i = 0; i < v.AvalibleColors.Count; i++)
                {
                    var c = v.AvalibleColors[i];
                    foreach (var v2 in instance.Variables)
                    {
                        bool b = true;
                        for (int j = 0; j < v2.AvalibleColors.Count; j++)
                        {
                            var c2 = v2.AvalibleColors[j];
                            if (!c.Restrictions.Contains(new Pair(v2, c2)))
                            {
                                b = false;
                                break;
                            }

                        }
                        if (b)
                        {
                            instance.RemoveColor(v, c);
                            RemoveVariableWith2Colors(instance, v);
                        }
                    }

                }
            }
        }
    }
}
