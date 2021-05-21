using CSP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static List<CspInstance> Lemma15(CspInstance instance, out bool applied)
        {
            List<CspInstance> result = new();
            HashSet<Pair> set = FindBadThreeComponent(instance, true);

            if (set != null)
            {
                applied = true;
#if DEBUG       
                foreach (var pair in set)
                {
                    if (!instance.Variables.Contains(pair.Variable)) throw new ApplicationException("Pair is not in the instance");
                }
#endif
                if (set.Count == 12)
                {
                    List<Pair> list = BruteColor(set, instance.Restrictions);
                    if (list != null)
                    {
                        foreach (Pair p in list)
                        {
                            instance.AddToResult(p);
                        }
                        result.Add(instance);
                    }
                    else
                    {
                        return new();
                    }
                }
                else if (set.Count == 8)
                {
                    bool isTriangle = ContainsTriangle(set);
                    if (isTriangle)
                    {
                        bool[] addedToResult = new bool[8];
                        var i1p = set.ToArray();
                        (CspInstance instance2, Pair[] i2p) = instance.CloneAndReturnCorresponding(i1p);
                        (CspInstance instance3, Pair[] i3p) = instance.CloneAndReturnCorresponding(i1p);

                        var instances = new[] { instance, instance2, instance3 };
                        var ip = new[] { i1p, i2p, i3p };

                        for (int i = 0; i < 3; i++)
                        {
                            bool[] used = addedToResult.Clone() as bool[];
                            for (int j = 0; j < used.Length; j++)
                            {
                                if (used[j]) continue;
                                foreach (var neighbor in ip[i][j].Color.Restrictions)
                                {
                                    int index = Array.IndexOf(ip[i], neighbor);
                                    used[index] = true;
                                }
                                foreach (var color in ip[i][j].Variable.AvalibleColors)
                                {
                                    int index = Array.IndexOf(ip[i], new Pair(ip[i][j].Variable, color));
                                    if(index > 0)
                                    {
                                        used[index] = true;
                                    }
                                    
                                }
                                instances[i].AddToResult(ip[i][j]);
                                addedToResult[j] = true;
                                used[j] = true;
                            }
                        }



                        result.Add(instance);
                        result.Add(instance2);
                        result.Add(instance3);
                        return result;
                    }
                    else
                    {
                        List<Pair> list = BruteColor(set, instance.Restrictions);
                        foreach (Pair p in list)
                        {
                            instance.AddToResult(p);
                        }
                        result.Add(instance);
                    }
                }
                return result;
            }
            else
            {
                applied = false;
                return new() { instance };
            }

        }

        private static bool ContainsTriangle(HashSet<Pair> set)
        {
            foreach (var pair in set)
            {
                foreach (var resPair in pair.Color.Restrictions)
                {
                    foreach (var resPair2 in pair.Color.Restrictions)
                    {
                        if (resPair != resPair2)
                        {
                            if (resPair.IsNeighborOf(resPair2))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static HashSet<Pair> FindBadThreeComponent(CspInstance instance, bool small = true)
        {
            HashSet<Pair> NotVisited = new();
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    if (color.Restrictions.Count == 3)
                    {
                        NotVisited.Add(new Pair(variable, color));
                    }
                }
            }
            HashSet<Pair> CurrentThreeCompnent = new();
            while (NotVisited.Count > 0)
            {
                void Rec(Pair pair)
                {
                    NotVisited.Remove(pair);
                    CurrentThreeCompnent.Add(pair);
                    foreach (var p2 in pair.Color.Restrictions)
                    {
                        if (NotVisited.Contains(p2))
                        {
                            Rec(p2);
                        }
                    }
                }
                var pair = NotVisited.First();
                Rec(pair);
                if (CurrentThreeCompnent.Count > 4) // it's bad
                {
                    if (small)
                    {
                        if (CurrentThreeCompnent.Select(p => p.Variable).Distinct().Count() == 4) // it's small
                        {
                            return CurrentThreeCompnent;
                        }
                    }
                    else
                    {
                        if (CurrentThreeCompnent.Select(p => p.Variable).Distinct().Count() > 4) // it's big
                        {
                            return CurrentThreeCompnent;
                        }
                    }
                }
            }
            return null;
        }




        private static List<Pair> BruteColor(HashSet<Pair> pairs, IReadOnlySet<Restriction> restrictions)
        {
            List<Pair> pairs1 = new();
            pairs1.AddRange(pairs);
            for (int i = 0; i < pairs1.Count; i++)
            {
                for (int j = i + 1; j < pairs1.Count; j++)
                {
                    for (int k = j + 1; k < pairs1.Count; k++)
                    {
                        for (int l = k + 1; l < pairs1.Count; l++)
                        {
                            if (pairs1[i].Variable == pairs1[j].Variable ||
                                pairs1[i].Variable == pairs1[k].Variable ||
                                pairs1[i].Variable == pairs1[l].Variable ||
                                pairs1[j].Variable == pairs1[k].Variable ||
                                pairs1[j].Variable == pairs1[l].Variable ||
                                pairs1[k].Variable == pairs1[l].Variable) continue;

                            if (restrictions.Contains(new Restriction(pairs1[i], pairs1[j])) ||
                                restrictions.Contains(new Restriction(pairs1[i], pairs1[k])) ||
                                restrictions.Contains(new Restriction(pairs1[i], pairs1[l])) ||
                                restrictions.Contains(new Restriction(pairs1[j], pairs1[k])) ||
                                restrictions.Contains(new Restriction(pairs1[j], pairs1[l])) ||
                                restrictions.Contains(new Restriction(pairs1[k], pairs1[l]))) continue;

                            return new() { pairs1[i], pairs1[j], pairs1[k], pairs1[l] };
                        }
                    }
                }
            }
            return null;
        }
    }
}
