using CSP;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        // in main alg:
        // for every (v, c) in instance:
        //      resInstances = Lemma9(instance, v, c)
        //      if resInstaces.Count > 0:
        //          recursion for every resnstaces[i]
        public static List<CspInstance> Lemma9(CspInstance instance, Variable v, Color c, out bool applied)
        {
            if (c.Restrictions.Count == 1) // (v,c) has one constraint => dangling constraint with (v2, c2) 
            {
                applied = true;
                (var v2, var c2) = c.Restrictions.First();

                (var instance2, var i2v2, var i2c2) = instance.CloneAndReturnCorresponding(v2, c2);
                instance.RemoveColor(v2, c2);
                instance.AddToResult(v, c);
                instance2.AddToResult(i2v2, i2c2);
                return new() { instance, instance2 };
            }
            applied = false;
            return new() { instance };
        }
    }
}
