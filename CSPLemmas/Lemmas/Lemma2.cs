using CSP;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void Lemma2(CspInstance instance, Variable variable, out bool applied)
        {
            applied = false;
            if (variable.AvalibleColors.Count == 2)
            {
                applied = true;
                RemoveVariableWith2Colors(instance, variable); // ZAMIANA
                return;
            }
        }
    }
}
