using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        // in the main alog:
        //      for each (v, c) in instance:
        //          res = Lemma13(instance, v, c)
        //          if res.Count > 1:
        //              for each inst in res:
        //                  recurrsion for inst
        public static List<CspInstance> Lemma13(CspInstance instance, Variable v, Color c)
        {
            if (c.Restrictions.Count == 3)
            {
                foreach(var restrictionPair in c.Restrictions)
                {
                    (var v2, var c2) = restrictionPair;
                    if(c2.Restrictions.Count ==2) // Lemma13 applies
                    {
#if DEBUG
                        if (c.Restrictions.Any(r => r.Variable.AvalibleColors.Count != 3))
                            throw new ArgumentException("Since previous lema is  assumed not to apply, all neighbors of (v, c) must have only three color choices.");
#endif
                        var v3c3 = GetThirdTriangleVertex(v, c, c2);
                        if(v3c3==null)
                        {
                            (var instance2, var i2vArr, var i2cArr) = instance.CloneAndReturnCorresponding(new Variable[] { v, v2 }, new Color[] { c, c2 });
                            var i2v = i2vArr[0]; var i2v2 = i2vArr[1];
                            var i2c = i2cArr[0]; var i2c2 = i2cArr[1];

                            instance.AddToResult(v, c);
                            instance2.RemoveColor(i2v, i2c); // create a dangling constraint (v2, c2)

                            var res = new List<CspInstance>() { instance };
                            res.AddRange(Lemma9(instance2, i2v2, i2c2));

                            return res;
                        }
                        else
                        {
                            (var v3, var c3) = (Pair)v3c3;
                            if(c3.Restrictions.Count == 3)
                            {
                                (var instance2, var i2v2, var i2c2) = instance.CloneAndReturnCorresponding(v2, c2);
                                (var instance3, var i3v3, var i3c3) = instance.CloneAndReturnCorresponding(v3, c3);
                                instance.AddToResult(v, c);
                                instance2.AddToResult(i2v2, i2c2);
                                instance3.AddToResult(i3v3, i3c3);
                                return new() { instance, instance2, instance3 };
                            }
                            else if(c3.Restrictions.Count == 2)
                            {
                                (var instance2, var i2vArr, var i2cArr) = instance.CloneAndReturnCorresponding(new Variable[] { v, v2 }, new Color[] { c, c2 });
                                var i2v = i2vArr[0]; var i2v2 = i2vArr[1];
                                var i2c = i2cArr[0]; var i2c2 = i2cArr[1];

                                /// use (v, c)
                                // find the third neighbor of (v, c) (named v4)
                                (var v4, _) = c.Restrictions.First(r => !((r.Variable == v2 && r.Color == c2) || (r.Variable == v3 && r.Color == c3)));
                                instance.AddToResult(v, c);
                                // since all neigbors of v had 3 available colors: 
                                RemoveVariableWith2Colors(instance, v2);
                                RemoveVariableWith2Colors(instance, v3);
                                RemoveVariableWith2Colors(instance, v4);

                                /// avoid (v, c)
                                instance2.RemoveColor(i2v, i2c); // create an isolated constraint (v2, c2)----(v3, c3)

                                var res = new List<CspInstance>() { instance };
                                res.AddRange(Lemma8(instance2, i2v2, i2c2));

                                return res;

                            }
                        }

                    }
                }
            }
           
            return new() { instance };

            Pair? GetThirdTriangleVertex(Variable v, Color c,Color c2)
            {
                // we know that (v, c)-----(v2, c2) and (v2, c2)-----(v3, c3).
                // let's find (v3, c3) such that (v3, c3)-----(v, c) or return null:
                foreach (var restr in c2.Restrictions)
                    if (restr.Color.Restrictions.Any(r => r.Variable == v && r.Color == c))
                        return restr;
                
                return null;
            }
        }

       
    }
}
