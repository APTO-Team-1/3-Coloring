using CSP;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        public static void Lemma3(CspInstance instance)
        {
            foreach (var v1 in instance.Variables)
            {
                for (int i = 0; i < v1.AvalibleColors.Count; i++)
                {
                    var c1 = v1.AvalibleColors[i];
                    foreach (var v2 in instance.Variables)
                    {
                        for (int j = 0; j < v2.AvalibleColors.Count; j++)
                        {
                            var c2 = v2.AvalibleColors[j];
                            bool b = true;
                            foreach (var pair in c1.Restrictions)
                            {
                                if(pair.Variable != v2 || pair.Color == c2)
                                {
                                    b = false;
                                    break;
                                }
                            }
                            foreach (var pair in c2.Restrictions)
                            {
                                if (pair.Variable != v1 || pair.Color == c1)
                                {
                                    b = false;
                                    break;
                                }
                            }
                            if(b)
                            {
                                instance.AddToResult(v1, c1);
                                instance.AddToResult(v2, c2);
                            }
                        }
                    }
                }
            }
        }
    }
}
