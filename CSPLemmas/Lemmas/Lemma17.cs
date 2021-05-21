using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        public static List<CspInstance> Lemma17(CspInstance instance, out bool applied)
        {
            var result = new List<CspInstance>();
            HashSet<Pair> set = FindBadThreeComponent(instance, false);
            if (set != null)
            {
                applied = true;
                List<Pair> witness = FindWitness(set, instance.Restrictions);
                if (witness != null)
                {
                    int neighbours = 0;
                    Pair vR = witness[0], wR = witness[1], xR = witness[2], yR = witness[3], zR = witness[4];
                    if (zR.Color.Restrictions.Any(r => r == wR)) neighbours++;
                    else throw new ApplicationException("Z should be neighbor of W");
                    if (zR.Color.Restrictions.Any(r => r == xR)) neighbours++;
                    if (zR.Color.Restrictions.Any(r => r == yR)) neighbours++;

                    if (neighbours == 1)
                    {
                        (CspInstance instance2,Pair[] i2p )= instance.CloneAndReturnCorresponding(zR);
                        var zRNeighbors = i2p[0].Color.Restrictions.Select(r => r.Variable);
                        instance2.AddToResult(i2p[0]); // instance2 wybiera zR
                        result.Add(instance2);
                        foreach (var v in zRNeighbors)
                        {
                            RemoveVariableWith2Colors(instance2, v);
                        }

                        if (wR.Color.Restrictions.Any(r => r == xR))
                        {
                            (CspInstance instance3, Pair[] i3p) = instance.CloneAndReturnCorresponding(wR);
                            (CspInstance instance4, Pair[] i4p) = instance.CloneAndReturnCorresponding(xR);
                            instance.AddToResult(vR);
                            instance3.AddToResult(i3p[0]);
                            instance4.AddToResult(i4p[0]);

                            result.Add(instance);
                            result.Add(instance3);
                            result.Add(instance4);
                            return result;
                        }
                        else if (wR.Color.Restrictions.Any(r => r == yR))
                        {
                            (CspInstance instance3, Pair[] i3p) = instance.CloneAndReturnCorresponding(wR);
                            (CspInstance instance4, Pair[] i4p) = instance.CloneAndReturnCorresponding(yR);
                            instance.AddToResult(vR);
                            instance3.AddToResult(i3p[0]);
                            instance4.AddToResult(i4p[0]);

                            result.Add(instance);
                            result.Add(instance3);
                            result.Add(instance4);
                            return result;
                        }
                        else
                        {
                            var instances = Lemma13(instance,vR.Variable, vR.Color);
                            result.AddRange(instances);
                            return result;
                        }
                    }
                    else if (neighbours == 2)
                    {
                        (CspInstance instance2, Pair[] i2p) = instance.CloneAndReturnCorresponding(zR); // pomija zR

                        instance.AddToResult(zR);
                        var instances = Lemma9(instance, vR.Variable, vR.Color, out _);
                        instance2.RemoveColor(i2p[0]);
                        result.AddRange(instances);
                        result.Add(instance2);
                        return result;
                    }
                    else if (neighbours == 3)
                    {
                        (CspInstance instance2, Pair[] i2p) = instance.CloneAndReturnCorresponding(zR); // pomija zR
                        instance2.RemoveColor(i2p[0]);
                        instance.AddToResult(zR);
                        instance.AddToResult(vR);
                        result.Add(instance);
                        result.Add(instance2);
                        return result;
                    }
                }
                else //whitness not found
                {
                    throw new ApplicationException("whitness not found");
                }
                return result;
            }
            else
            {
                applied = false;
                return new() { instance };
            }

        }

        private static List<Pair> FindWitness(HashSet<Pair> component, IReadOnlySet<Restriction> restrictions)
        {
            foreach (Pair p1 in component)
            {
                foreach (var p2 in p1.Color.Restrictions)
                {
                    var p3p4 = p1.Color.Restrictions.Where(r => r != p2);
                    var p3 = p3p4.ElementAt(0);
                    var p4 = p3p4.ElementAt(1);
                    var p5list = p2.Color.Restrictions.Where(p => p != p1 && p != p2 && p != p3);
                    if (p5list.Any())
                    {
                        var p5 = p5list.First();
                        Pair vR = p1, wR = p2, xR = p3, yR = p4, zR = p5; 
                        if(p2.IsNeighborOf(p3) && p3.IsNeighborOf(p5)) // ustawiamy tak żeby jeśli występuje trójkąt to nie miał 2 połączeć z zR
                        {
                            vR = p2;
                            wR = p1;
                            xR = p3;
                            yR = p5;
                            zR = p4;
                        }
                        if (p2.IsNeighborOf(p4) && p4.IsNeighborOf(p5))
                        {
                            vR = p2;
                            wR = p1;
                            xR = p4;
                            yR = p5;
                            zR = p3;
                        }
                        return new() { vR,wR,xR,yR,zR };
                    }
                }
            }
            return null;
        }
    }
}