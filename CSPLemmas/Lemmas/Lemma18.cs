using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        //w main algorytmie while Lemma18(instance).count > 0 ...
        public static List<CspInstance> Lemma18(CspInstance instance)
        {
            List<Pair> TwoComponent;

            foreach (Variable var in instance.Variables)
            {
                foreach (Color color in var.AvalibleColors)
                {
                    if (color.Restrictions.Count == 2)
                    {
                        TwoComponent = new();
                        Color currColor = color;
                        Color lastColor = color;
                        Pair[] tempList = new Pair[2];

                        TwoComponent.Add(currColor.Restrictions.First());
                        currColor = currColor.Restrictions.First().Color;

                        while (currColor.Restrictions.Count == 2)
                        {
                            tempList = currColor.Restrictions.ToArray();
                            if (tempList[0].Color != lastColor)
                            {
                                TwoComponent.Add(tempList[0]);
                                lastColor = currColor;
                                currColor = tempList[0].Color;
                            }
                            else
                            {
                                TwoComponent.Add(tempList[1]);
                                lastColor = currColor;
                                currColor = tempList[1].Color;
                            }
                            if (currColor == color) // mamy 2 komponent
                            {
                                if (TwoComponent.Count == 4)  // cykl długości 4 kolorujemy po przekątnej
                                {
                                    (CspInstance instance2,Variable[] var13,Color[] col13) = instance.CloneAndReturnCorresponding(new Variable[2] 
                                        { TwoComponent[1].Variable,TwoComponent[3].Variable}, new Color[2] { TwoComponent[1].Color,TwoComponent[3].Color});

                                    instance.AddToResult(TwoComponent[0]);
                                    instance.AddToResult(TwoComponent[2]);

                                    instance2.AddToResult(var13[0],col13[0]);
                                    instance2.AddToResult(var13[1], col13[1]);

                                    return new() { instance, instance2 };
                                }
                                List<Pair> last5Pairs = new List<Pair> { TwoComponent[0], TwoComponent[1], TwoComponent[2], TwoComponent[3], TwoComponent[4] };
                                int lastIndex = 4;
                                Pair currPair = TwoComponent[4];
                                do
                                {
                                    if (last5Pairs[0].Variable == last5Pairs[3].Variable) // cykl postci (v,R), (w,R), (x,R), (v,G)
                                    {
                                        (CspInstance instance2, Variable v, Color c) = instance.CloneAndReturnCorresponding(last5Pairs[2].Variable, last5Pairs[2].Color);
                                        instance.AddToResult(last5Pairs[1]);
                                        instance2.AddToResult(v,c);

                                        return new() { instance, instance2 };
                                    }
                                    else if (last5Pairs[1].Variable == last5Pairs[4].Variable) // cykl postci (v,R), (w,R), (x,R), (v,G)
                                    {
                                        (CspInstance instance2, Variable v, Color c) = instance.CloneAndReturnCorresponding(last5Pairs[3].Variable,last5Pairs[3].Color);
                                        instance.AddToResult(last5Pairs[2]);
                                        instance2.AddToResult(v,c);

                                        return new() { instance, instance2 };
                                    }
                                    else if (HasDifferentVariables(last5Pairs))// 5 różnych variabli pod rząd
                                    {
                                        (CspInstance instance2, Variable v, Color c) = instance.CloneAndReturnCorresponding(last5Pairs[2].Variable, last5Pairs[2].Color);
                                        (CspInstance instance3, Variable[] var03, Color[] col03) = instance.CloneAndReturnCorresponding(new Variable[2]
                                        { TwoComponent[0].Variable,TwoComponent[3].Variable}, new Color[2] { TwoComponent[0].Color, TwoComponent[3].Color });
                                        instance.AddToResult(last5Pairs[1]);
                                        instance2.AddToResult(v, c);
                                        instance3.AddToResult(var03[0],col03[0]);
                                        instance3.AddToResult(var03[1], col03[1]);

                                        return new() { instance, instance2, instance3 };
                                    }
                                    last5Pairs.RemoveAt(0); // usuniecie pierwszej pary
                                    lastIndex++;
                                    lastIndex = lastIndex % TwoComponent.Count;
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
            }
            return new List<CspInstance>();  // nie znaleźliśmu żadnych 2komponentów
        }
        private static bool HasDifferentVariables(List<Pair> pairs)
        {
            for (int i = 0; i < pairs.Count; i++)
                for (int j = i + 1; j < pairs.Count; j++)
                    if (pairs[i].Variable.Id == pairs[j].Variable.Id) return false;
            return true;
        }

    }
}