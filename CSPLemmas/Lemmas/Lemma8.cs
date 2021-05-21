using CSP;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        // in main alg:
        // for every (v, c) in instance:
        //      resInstances = Lemma8(instance, v, c)
        //      if resInstaces.Count > 0:
        //          recursion for every resnstaces[i]

        public static List<CspInstance> Lemma8(CspInstance instance, Variable v, Color c, out bool applied)
        {
            if (c.Restrictions.Count == 1) // (v,c) has one constraint
            {
                (var v2, var c2) = c.Restrictions.ElementAt(0);
                if (c2.Restrictions.Count == 1) // (v2, c2) has one constraint too => isolated constraint
                {
                    applied = true;
                    if (v.AvalibleColors.Count == 3 && v2.AvalibleColors.Count == 3)
                    {
                        Variable vCombined = new(0);
                        instance.AddVariable(vCombined);
                        foreach (var col in v.AvalibleColors)
                        {
                            if (col != c)
                            {
                                var newColor = new Color(col.Value);
                                instance.AddColor(vCombined, newColor);
                                foreach (var res in col.Restrictions)
                                {
                                    instance.AddRestriction(new Pair(vCombined, newColor), res);
                                }
                                instance.ResultRules.Add((IList<Pair> result) => {
                                    var replacingList = result.Where(p => p.Color == newColor);
                                    if (replacingList.Any())
                                    {
                                        var replacing = replacingList.Single();
                                        result.Remove(replacing);
                                        result.Add(new Pair(v, col));
                                        result.Add(new Pair(v2, c2));
                                    }
                                });
                            }
                        }
                        foreach (var col in v2.AvalibleColors)
                        {
                            if (col != c2)
                            {
                                var newColor = new Color(vCombined.AvalibleColors.Max(c => c.Value) + 1);
                                instance.AddColor(vCombined, newColor);
                                foreach (var res in col.Restrictions)
                                {
                                    instance.AddRestriction(new Pair(vCombined, newColor), res);
                                }
                                instance.ResultRules.Add((IList<Pair> result) => {
                                    var replacingList = result.Where(p => p.Color == newColor);
                                    if (replacingList.Any())
                                    {
                                        var replacing = replacingList.Single();
                                        result.Remove(replacing);
                                        result.Add(new Pair(v2, col));
                                        result.Add(new Pair(v, c));
                                    }
                                });
                            }
                        }
#if DEBUG
                        if (vCombined.AvalibleColors.Count != 4) throw new System.ApplicationException("Powinien mieć 4 kolory");
#endif
                        instance.RemoveVariable(v);
                        instance.RemoveVariable(v2);

                        return new() { instance };
                    }
                    else
                    {
                        if (v.AvalibleColors.Count == 4 && v2.AvalibleColors.Count == 3)
                        {
                            (var instance2, var i2v2, var i2c2) = instance.CloneAndReturnCorresponding(v2, c2);

                            instance.AddToResult(v, c);
                            RemoveVariableWith2Colors(instance, v2);
                            instance2.AddToResult(i2v2, i2c2);
                            return new() { instance, instance2 };
                        }
                        else if (v.AvalibleColors.Count == 3 && v2.AvalibleColors.Count == 4)
                        {
                            (var instance2, var i2v, var i2c) = instance.CloneAndReturnCorresponding(v, c);

                            instance.AddToResult(v2, c2);
                            RemoveVariableWith2Colors(instance, v);
                            instance2.AddToResult(i2v, i2c);
                            return new() { instance, instance2 };
                        }
                        else
                        {
                            (var instance2, var i2v2, var i2c2) = instance.CloneAndReturnCorresponding(v2, c2);
                            instance.AddToResult(v, c);
                            instance2.AddToResult(i2v2, i2c2);
                            return new() { instance, instance2 };
                        }

                    }
                }
            }
            applied = false;
            return new() { instance };
        }

    }
}
