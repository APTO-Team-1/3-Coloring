using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPSimplifying
{
    public static partial class CSPLemmas
    {
        //w main algorytmie while applied  ...
        public static List<CspInstance> Lemma18(CspInstance instance, out bool applied)
        {
            foreach (Variable var in instance.Variables)
            {
                foreach (Color color in var.AvalibleColors)
                {
                    if (color.Restrictions.Count == 2)
                    {
                        List<Pair> TwoComponent = GetBig2ComponentFromPair(new Pair(var, color));
                        if (TwoComponent != null) // mamy 2 komponent
                        {
                            applied = true;
                            if (TwoComponent.Count == 4)  // cykl długości 4 kolorujemy po przekątnej
                            {
                                (CspInstance instance2, Pair[] pair13) = instance.CloneAndReturnCorresponding(TwoComponent[1], TwoComponent[3]);

                                instance.AddToResult(TwoComponent[0]);
                                instance.AddToResult(TwoComponent[2]);

                                instance2.AddToResult(pair13[0]);
                                instance2.AddToResult(pair13[1]);

                                return new() { instance, instance2 };
                            }
                            List<Pair> last5Pairs = new() { TwoComponent[0], TwoComponent[1], TwoComponent[2], TwoComponent[3], TwoComponent[4] };
                            int lastIndex = 4;
                            do
                            {
                                if (last5Pairs[0].Variable == last5Pairs[3].Variable) // cykl postci (v,R), (w,R), (x,R), (v,G)
                                {
                                    (CspInstance instance2, Variable v, Color c) = instance.CloneAndReturnCorresponding(last5Pairs[2].Variable, last5Pairs[2].Color);
                                    instance.AddToResult(last5Pairs[1]);
                                    instance2.AddToResult(v, c);

                                    return new() { instance, instance2 };
                                }
                                else if (last5Pairs[1].Variable == last5Pairs[4].Variable) // cykl postci (v,R), (w,R), (x,R), (v,G)
                                {
                                    (CspInstance instance2, Variable v, Color c) = instance.CloneAndReturnCorresponding(last5Pairs[3].Variable, last5Pairs[3].Color);
                                    instance.AddToResult(last5Pairs[2]);
                                    instance2.AddToResult(v, c);

                                    return new() { instance, instance2 };
                                }
                                else if (HasDifferentVariables(last5Pairs))// 5 różnych variabli pod rząd
                                {
                                    (CspInstance instance2, Variable v, Color c) = instance.CloneAndReturnCorresponding(last5Pairs[2].Variable, last5Pairs[2].Color);
                                    (CspInstance instance3, Variable[] var03, Color[] col03) = instance.CloneAndReturnCorresponding(new Variable[2]
                                    { TwoComponent[0].Variable,TwoComponent[3].Variable}, new Color[2] { TwoComponent[0].Color, TwoComponent[3].Color });
                                    instance.AddToResult(last5Pairs[1]);
                                    instance2.AddToResult(v, c);
                                    instance3.AddToResult(var03[0], col03[0]);
                                    instance3.AddToResult(var03[1], col03[1]);

                                    return new() { instance, instance2, instance3 };
                                }
                                last5Pairs.RemoveAt(0); // usuniecie pierwszej pary
                                lastIndex++;
                                lastIndex %= TwoComponent.Count;
                                last5Pairs.Add(TwoComponent[lastIndex]); //dodanie na koniec nastepnej
                            } while (lastIndex != 4);
                            // przeszlismy cały 2komponent i nie znalezlismy zadnych z szczegolnych przypadkow czyli mamy cykl po czterech variablach o długosci 8 lub 12
                            instance.AddToResult(TwoComponent[0]);
                            instance.AddToResult(TwoComponent[2]);
                            instance.AddToResult(TwoComponent[5]);
                            instance.AddToResult(TwoComponent[7]);
                            return new List<CspInstance> { instance };
                        }
                    }
                }
            }
            applied = false;
            return new List<CspInstance> { instance };  // nie znaleźliśmu żadnych 2komponentów
        }

        private static List<Pair> GetBig2ComponentFromPair(Pair pair)
        {
            List<Pair> component = new();
            component.Add(pair);
            var last = pair;
            var p = pair.Color.Restrictions.First();
            while (p != pair)
            {
#if DEBUG
                if (p.Color.Restrictions.Count != 2) throw new ApplicationException("All neighbors should have the same number of neighbors");
#endif
                component.Add(p);
                var tab = p.Color.Restrictions.ToArray();
                if (tab[0] != last)
                {
                    last = p;
                    p = tab[0];
                }
                else if (tab[1] != last)
                {
                    last = p;
                    p = tab[1];
                }
                else
                {
                    return null;
                }
            }

            if (component.Count >= 4) return component;
            return null;
        }

        private static bool HasDifferentVariables(List<Pair> pairs)
        {
            for (int j = 1; j < pairs.Count; j++)
                if (pairs[0].Variable.Id == pairs[j].Variable.Id) return false;
            return true;
        }

    }
}