using CSP;
using System;
using System.Collections.Generic;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        // in main alg:
        //      for every (v, c) in instance:
        //          for every restriction in  c.Restrictions:
        //              var v2 = restriction.Variable
        //              var c2 = restriction.Color
        //              if c!=c2:
        //                  for every restriction2 in c.Restrictions:
        //                      if resticiton!=restriction2 and  restriction2.Varaible = v2 and ( restriction2.Color != c and != c2):
        //                          var resInstances = Lemma10(instance, v, c, v2, c2, restriction2.Color)
        //                              if( resInstances.Count > 1)
        //                                  rescursion for all resInstances 
        public static List<CspInstance> Lemma10(CspInstance instance, Variable v, Color c, Variable v2, Color c2_1, Color c2_2)
        {
            // target of an implication is not source of another implication
            throw new NotImplementedException();
        }
    }
}
