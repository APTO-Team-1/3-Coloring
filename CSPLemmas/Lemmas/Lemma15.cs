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
            
            if(set != null)
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
                    if(list != null)
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
                    List<Pair> setWithTriangles = GetWithTriangle(set, instance.Restrictions);
                    if(setWithTriangles != null) //v1, v2, w1, w2, x1, x2, y1, y2
                    {
                        (var instance2, var i2vArr, var i2cArr) = instance.CloneAndReturnCorresponding(
                            new Variable[] { setWithTriangles[0].Variable, setWithTriangles[2].Variable, setWithTriangles[7].Variable },
                            new Color[] { setWithTriangles[0].Color, setWithTriangles[2].Color, setWithTriangles[7].Color });

                        (var instance3, var i3vArr, var i3cArr) = instance.CloneAndReturnCorresponding(
                            new Variable[] { setWithTriangles[5].Variable, setWithTriangles[6].Variable },
                            new Color[] { setWithTriangles[5].Color, setWithTriangles[6].Color });

                        instance.AddToResult(setWithTriangles[0]);
                        instance.AddToResult(setWithTriangles[2]);
                        instance.AddToResult(setWithTriangles[5]);

                        instance2.AddToResult(i2vArr[0], i2cArr[0]);
                        instance2.AddToResult(i2vArr[1], i2cArr[1]);
                        instance2.AddToResult(i2vArr[2], i2cArr[2]);
                                             
                        instance3.AddToResult(i3vArr[0], i3cArr[0]);
                        instance3.AddToResult(i3vArr[1], i3cArr[1]);


                        result.Add(instance);
                        result.Add(instance2);
                        result.Add(instance3);
                    }
                    else
                    {
                        List<Pair> list = BruteColor(set, instance.Restrictions);
                        foreach(Pair p in list)
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
        public static HashSet<Pair> FindBadThreeComponent(CspInstance instance, bool small = true) 
        {
            HashSet<Pair> NotVisited = new();
            foreach (var variable in instance.Variables)
            {
                foreach (var color in variable.AvalibleColors)
                {
                    if(color.Restrictions.Count == 3)
                    {
                        NotVisited.Add(new Pair(variable, color));
                    }
                }
            }
            HashSet<Pair> CurrentThreeCompnent = new();
            while(NotVisited.Count > 0)
            {
                void Rec(Pair pair)
                {
                    NotVisited.Remove(pair);
                    CurrentThreeCompnent.Add(pair);
                    foreach (var p2 in pair.Color.Restrictions)
                    {
                        if(NotVisited.Contains(p2))
                        {
                            Rec(p2);
                        }
                    }
                }
                var pair = NotVisited.First();
                Rec(pair);
                if(CurrentThreeCompnent.Count > 4) // it's bad
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
      
        private static List<Pair> GetWithTriangle(HashSet<Pair> set, IReadOnlySet<Restriction> restrictions)
        {
            List<Pair> ret = new();
            Pair? t1_1 = null;
            Pair? t1_2 = null;
            Pair? t1_3 = null;
            Pair? t2_1 = null;
            Pair? t2_2 = null;
            Pair? t2_3 = null;

            foreach(Pair p1 in set)
            {
                foreach(Pair p2 in set)
                {
                    foreach(Pair p3 in set)
                    {
                        if( restrictions.Contains(new Restriction(p1, p2)) &&
                            restrictions.Contains(new Restriction(p2, p3)) &&
                            restrictions.Contains(new Restriction(p1, p3)))
                        {
                            if(!t1_1.HasValue)
                            {
                                t1_1 = p1;
                                t1_2 = p2;
                                t1_3 = p3;
                            } else if(!t1_1.Value.Equals(p1) && !t1_2.Value.Equals(p1) && !t1_3.Equals(p1))
                            {
                                t2_1 = p1;
                                t2_2 = p2;
                                t2_3 = p3;
                                break;
                            }
                        }
                    }
                    if (t2_1.HasValue) break;
                }
                if (t2_1.HasValue) break;
            }

            if (!t1_1.HasValue || !t2_1.HasValue) return null;

            if (t1_1.Value.Variable == t2_2.Value.Variable)
            {
                Pair? tmp = t2_1;
                t2_1 = t2_2;
                t2_2 = tmp;
            }
            if (t1_1.Value.Variable == t2_3.Value.Variable)
            {
                Pair? tmp = t2_1;
                t2_1 = t2_3;
                t2_3 = tmp;
            }
            if (t1_2.Value.Variable == t2_1.Value.Variable)
            {
                Pair? tmp = t2_2;
                t2_2 = t2_1;
                t2_1 = tmp;
            }
            if (t1_2.Value.Variable == t2_3.Value.Variable)
            {
                Pair? tmp = t2_2;
                t2_2 = t2_3;
                t2_3 = tmp;
            }
            if (t1_3.Value.Variable == t2_1.Value.Variable)
            {
                Pair? tmp = t2_1;
                t2_1 = t2_3;
                t2_3 = tmp;
            }
            if (t1_3.Value.Variable == t2_2.Value.Variable)
            {
                Pair? tmp = t2_2;
                t2_2 = t2_3;
                t2_3 = tmp;
            }

            if(t1_1.Value.Variable != t2_1.Value.Variable)
            {
                Pair? tmp = t1_1;
                t1_1 = t1_3;
                t1_3 = tmp;
                tmp = t2_1;
                t2_1 = t2_3;
                t2_3 = tmp;
            }
            if (t1_2.Value.Variable != t2_2.Value.Variable)
            {
                Pair? tmp = t1_2;
                t1_2 = t1_3;
                t1_3 = tmp;
                tmp = t2_2;
                t2_2 = t2_3;
                t2_3 = tmp;
            }
            // na tym etapie _1 i _2 to x oraz y, a _3 to v i w

            foreach(Pair p in set)
            {
                if(!p.Equals(t1_3) && p.Variable == t1_3.Value.Variable)
                {
                    ret.Add(p);
                    break;
                }
            }
            ret.Add(t1_3.Value);
            ret.Add(t2_3.Value);
            foreach (Pair p in set)
            {
                if (!p.Equals(t2_3) && p.Variable == t2_3.Value.Variable)
                {
                    ret.Add(p);
                    break;
                }
            }
            if(restrictions.Contains(new Restriction(t1_1.Value, ret[2])))
            {
                ret.Add(t1_1.Value);
                ret.Add(t2_1.Value);
            } else
            {
                ret.Add(t2_1.Value);
                ret.Add(t1_1.Value);
            }
            if(restrictions.Contains(new Restriction(t1_2.Value, ret[2])))
            {
                ret.Add(t1_2.Value);
                ret.Add(t2_2.Value);
            } else
            {
                ret.Add(t2_2.Value);
                ret.Add(t1_2.Value);
            }
            return ret;
        }
        private static List<Pair> BruteColor(HashSet<Pair> pairs, IReadOnlySet<Restriction> restrictions)
        {
            List<Pair> pairs1 = new();
            pairs1.AddRange(pairs);
            for(int i = 0; i < pairs1.Count; i++)
            {
                for(int j = i + 1; j < pairs1.Count; j++)
                {
                    for(int k = j + 1; k < pairs1.Count; k++)
                    {
                        for(int l = k + 1; l < pairs1.Count; l++)
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
