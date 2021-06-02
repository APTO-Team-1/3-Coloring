using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        // in main alg:
        //      for every (v, c) in instance:
        //          if c ma 2 ograniczenia (lub więcej) do jednej zmiennej
        //          {
        //              var tempTab = c.restrictions.ToArray();
        //              if(tempTab[0].variable == tempTab[1].variable)
        //                  res = lemma10(instance , ...)
        //                  recursion
        //

        public static List<CspInstance> Lemma10(CspInstance instance, Variable v, Color c, Variable v2)
        {
#if DEBUG
            if (c.Restrictions.Where(r => r.Variable == v2).Count() <= 1) throw new ApplicationException("There should be 2 or more restriction from c to v2");
#endif

            // w instance bierzemy kolor impikowany a w instance2 nie bierzemy
            List<Color> notNeighbors = new(v2.AvalibleColors);
            var restrictionToV2 = c.Restrictions.Where(r => r.Variable == v2);
            foreach (var res in restrictionToV2)
            {
                notNeighbors.Remove(res.Color);
            }

            if (notNeighbors.Count == 1) //
            {
                var c2 = notNeighbors[0]; // c2 jest implikowana przez c1;
                var implicationFrom = GetImplicationFrom(new Pair(v2, c2));
                //List<Pair> cycle = new() { new Pair(v, c) };
                //bool isCycle = CreateImplicationCycle(cycle);
                //if (isCycle) // jest cykl implikacji
                //{
                //    bool allRestrictionInCycle = true;
                //    Pair vR = new();
                //    for (int i = 0; i < cycle.Count - 1; i++)
                //    {
                //        if (cycle[i].Color.Restrictions.Any(r => r.Color != cycle[i + 1].Color))
                //        {
                //            allRestrictionInCycle = false;
                //            vR = cycle[i];
                //            break;
                //        }
                //    }
                //    if (cycle[^1].Color.Restrictions.Any(r => r.Color != cycle[0].Color))
                //    {
                //        allRestrictionInCycle = false;
                //        vR = cycle[^1];
                //    }
                //    if (allRestrictionInCycle) // wybieramy wszystkie kolory z cyklu
                //    {
                //        foreach (var pair in cycle)
                //        {
                //            instance.AddToResult(pair);
                //        }
                //        return new() { instance };
                //    }
                //    else
                //    {
                //        (var instance2, var i2v, var i2c) = instance.CloneAndReturnCorresponding(cycle.Select(p => p.Variable).ToArray(), cycle.Select(p => p.Color).ToArray());
                //        foreach (var pair in cycle) // instance1 wybiera cykl
                //        {
                //            instance.AddToResult(pair);
                //        }
                //        for (int i = 0; i < i2v.Length; i++) // instance2 nie wybiera cyklu
                //        {
                //            instance2.RemoveColor(i2v[i], i2c[i]);
                //        }

                //        return new() { instance, instance2 };

                //    }
                //}
                //else // nie ma cyklu
                //{
                (var instance2,var i2v, var i2c) = instance.CloneAndReturnCorresponding(new[] { v, v2 }, new[] { c, c2 });

                instance.AddToResult(v2, c2); // instance1 wybiera c2
                instance2.RemoveColor(i2v[1], i2c[1]); // instance2 nie wybiera c2
                instance2.RemoveColor(i2v[0], i2c[0]); // , więc nie może też wybrać c

                return new() { instance, instance2 };
                //}
            }
            else if (notNeighbors.Count == 2)
            {
                var c21 = notNeighbors[0];
                var c22 = notNeighbors[1];
                (var instance2, var i2v, var i2c) = instance.CloneAndReturnCorresponding(new[] { v2, v2, v }, new[] { c21, c22, c });
                instance2.RemoveColor(i2v[0], i2c[0]);
                instance2.RemoveColor(i2v[1], i2c[1]);
                instance2.RemoveColor(i2v[2], i2c[2]); // usuwamy v,c bo nie można go ustawić
                // instance 1 zostawia nie sąsiadów
                instance.RemoveColor(v2, c21);
                instance.RemoveColor(v2, c22);
                // instance 2 zostawia sąsiadów
                
                return new() { instance, instance2 };
            }
            else
            {
                throw new ApplicationException("Not expected");
            }
        }
        private static List<Pair> GetImplicationFrom(Pair pair)
        {
            var ret = new List<Pair>();
            (var v, var c) = pair;
            var neighbors = c.Restrictions.Select(r => r.Variable).Distinct();
            foreach (var neighbor in neighbors)
            {
                var restrictionToNeighbor = c.Restrictions.Where(r => r.Variable == neighbor).Select(r => r.Color);
                if (restrictionToNeighbor.Count() == neighbor.AvalibleColors.Count - 1)
                {
                    ret.Add(new Pair(neighbor, neighbor.AvalibleColors.First(c => !restrictionToNeighbor.Contains(c))));
                }
            }
            return ret;
        }
        //private static bool CreateImplicationCycle(List<Pair> cycle)
        //{
        //    var implicationFrom = GetImplicationFrom(cycle.Last());
        //    foreach (var pair in implicationFrom)
        //    {
        //        if (pair == cycle.First())
        //        {
        //            return true;
        //        }
        //        cycle.Add(pair);
        //        if (CreateImplicationCycle(cycle)) return true;
        //    }
        //    return false;
        //}
    }


}
