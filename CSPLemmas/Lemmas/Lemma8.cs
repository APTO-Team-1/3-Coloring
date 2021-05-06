using CSP;
using System.Collections.Generic;
using System.Linq;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        // in main alg:
        // for every (v, c) in instance:
        //      resInstances = Lemma8(instance, v, c)
        //      if resInstaces.Count > 0:
        //          recursion for every resnstaces[i]

        public static List<CspInstance> Lemma8(CspInstance instance, Variable v, Color c)
        {
            if (c.Restrictions.Count == 1) // (v,c) has one constraint
            {
                (var v2, var c2) = c.Restrictions.ElementAt(0);
                if (c2.Restrictions.Count == 1) // (v2, c2) has one constraint too => isolated constraint
                {
                    if (v.AvalibleColors.Count == 3 && v2.AvalibleColors.Count == 3)
                    {
                        List<Color> vCombinedColors = new();
                        foreach (var col in v.AvalibleColors)
                            if (col != c)
                                vCombinedColors.Add(new Color(col.Value, col.Restrictions));
                        foreach (var col in v2.AvalibleColors)
                            if (col != c2)
                            {
                                var existingCombinedColor = vCombinedColors.FirstOrDefault(cc => cc.Value == col.Value);
                                if (existingCombinedColor != null)
                                    existingCombinedColor.Restrictions.Union(col.Restrictions);
                                else
                                    vCombinedColors.Add(new Color(col.Value, col.Restrictions));
                            }

                        instance.RemoveVariable(v);
                        instance.RemoveVariable(v2);
                        instance.AddVariableAndColorsRestrictions(vCombinedColors);
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
            return new() { instance};
        }

    }
}
