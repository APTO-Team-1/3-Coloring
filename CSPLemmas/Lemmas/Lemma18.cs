using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        public static List<CspInstance> Lemma18(CspInstance instance)
        {
            List<CspInstance> result = new();

            void Lemma18Internal(CspInstance instance)
            {
                List<Pair> TwoComponent = new();
                foreach (Variable var in instance.Variables)
                {
                    foreach (Color color in var.AvalibleColors)
                    {
                        TwoComponent = new();
                        Color currColor = color;
                        Color lastColor = color;
                        Pair[] tempList = new Pair[2];
                        if (currColor.Restrictions.Count == 2)
                        {
                            TwoComponent.Add(currColor.Restrictions.First());
                            currColor = currColor.Restrictions.First().Color;
                        }
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
                            if (currColor == color)
                            {
                                if (TwoComponent.Count == 4) // cykl długości cztery po czterech variablach
                                {

                                }
                                else if (TwoComponent.Count > 4)
                                {

                                }
                            }
                        }
                    }
                }

            }

            return result;
        }

    }
}