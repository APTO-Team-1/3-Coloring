using CSP;
using CSPGraphConverter;
using CSPSimplifying;
using GraphLib.Definitions;
using System;
using ThreeColoring.Algorithms;

namespace ThreeColoringAlgorithms
{
    public class CspColoring : IThreeColoringAlgorithm
    {
        public int[] ThreeColorig(Graph g)
        {
            var csp = Converter.GraphToCSP(g);
            var simpleLemmas = new Action<CspInstance>[]
            {
                CSPLemmas.Lemma2, CSPLemmas.Lemma3,CSPLemmas.Lemma4,CSPLemmas.Lemma5,CSPLemmas.Lemma6,
            };

            for (int i = 0; i < simpleLemmas.Length; i++)
            {
                simpleLemmas[i](csp);
            }


            return null;
        }
    }
}
