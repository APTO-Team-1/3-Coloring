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
        //          res = Lemma12(instance, v, c)
        //          if res.Count > 1:
        //              for each inst in res:
        //                  recurrsion for inst
        public static List<CspInstance> Lemma12(CspInstance instance, Variable v, Color c)
        {
            if (c.Restrictions.Count == 3)
            {
                foreach(var restrictionPair in c.Restrictions)
                {
                    (var v2, var c2) = restrictionPair;
                    if (v2.AvalibleColors.Count == 4) // Lemma12 applies
                    {
#if DEBUG
                        if (c2.Restrictions.Count!=2)
                            throw new ArgumentException("We can assume (v2, c2) has only two constraints, else it would be covered by the previous lemma.");
#endif
                        // second neighbor of v2
                        (var v3, var c3) = c2.Restrictions.Where(r => r.Variable != v).First();

                        if(Are_v1c1v2c2v3c3_FormTraingle(v, c, c3))
                        {
                            (var instance2, var i2vArr, var i2cArr) = instance.CloneAndReturnCorresponding(new Variable[] { v, v2 }, new Color[] { c, c2 });
                            var i2v = i2vArr[0]; var i2v2 = i2vArr[1];
                            var i2c = i2cArr[0]; var i2c2 = i2cArr[1];
                            instance.AddToResult(v, c);
                            instance2.RemoveColor(i2v, i2c); // creating a dangling constraint at (v2, c2)

                            var res = new List<CspInstance>() { instance };
                            res.AddRange(Lemma9(instance2, i2v2, i2c2));
                            
                            return res;
                        }
                        else
                        {
                            (var instance2, var i2v2, var i2c2) = instance.CloneAndReturnCorresponding(v2, c2);
                            (var instance3, var i2v3, var i2c3) = instance.CloneAndReturnCorresponding(v3, c3);
                            instance.AddToResult(v, c);
                            instance2.AddToResult(i2v2, i2c2);
                            instance3.AddToResult(i2v3, i2c3);
                            return new() { instance, instance2, instance3 };
                        }
                    }
                }
            }
           
            return new() { instance };


            bool Are_v1c1v2c2v3c3_FormTraingle(Variable v, Color c, Color c3)
            {
                // we know that (v, c)-----(v2, c2) and (v2, c2)-----(v3, c3).
                // let's check whether (v3, c3)-----(v, c):
                return c3.Restrictions.Any(r => r.Variable == v && r.Color == c);
            }
        }

       
    }
}
