using CSP;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static void Lemma2(CspInstance instance)
        {
            foreach (var variable in instance.Variables)
            {
                if (variable.AvalibleColors.Count == 2)
                {
                    RemoveVariableWith2Colors(instance, variable);
                }
            }
        }
    }
}
