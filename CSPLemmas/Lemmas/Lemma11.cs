using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {  
        // in the main alog:
        //      for each (v, c) in instance:
        //          res = Lemma11(instance, v, c)
        //          if res.Count > 1:
        //              for each inst in res:
        //                  recurrsion for inst
        public static List<CspInstance> Lemma11(CspInstance instance, Variable v, Color c)
        {
            if((c.Restrictions.Count >=3 && v.AvalibleColors.Count == 4) ||
               (c.Restrictions.Count >= 4 && v.AvalibleColors.Count == 3)) // Lemma11 applies
            {
#if DEBUG
                if (c.Restrictions.Select(r =>r.Variable).Distinct().Count() != c.Restrictions.Select(r => r.Variable).Count())
                {
                    throw new ArgumentException("We can assume form Lemma 10 that each constraint connects (v, R) to a different varaible.");
                }
#endif
                (var instance2, var i2v, var i2c) = instance.CloneAndReturnCorresponding(v, c);
                instance.AddToResult(v, c);
                instance2.RemoveColor(i2v, i2c);
                // TODO?: Analyze above cases more precisely?
                return new() { instance, instance2 };
                
            }
            return new() { instance };

        }
    }
}
