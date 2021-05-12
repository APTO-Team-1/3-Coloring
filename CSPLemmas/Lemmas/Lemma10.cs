using CSP;
using System;
using System.Collections.Generic;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        // in main alg:
        //      for every (v, c) in instance:
        //          if(c.restrictions.count == 2)
        //          {
        //              var tempTab = c.restrictions.ToArray();
        //              if(tempTab[0].variable == tempTab[1].variable)
        //                  res = lemma10(instance , ...)
        //              if (res.count > 1) recursion
        //

        public static List<CspInstance> Lemma10(CspInstance instance, Variable v, Color c, Variable v2, Color c2_1, Color c2_2)
        {
            // w instance bierzemy kolor impikowany a w instance2 nie bierzemy
            Color R = new(345376545);
            foreach (Color col in v2.AvalibleColors)
                if (col.Value != c2_1.Value && col.Value != c2_2.Value) R = col;
            CspInstance instance2 = instance.Clone();
            if(v2.AvalibleColors.Count == 3)
            {
                instance.AddToResult(v2, R); // bierzemy kolor R
                instance2.RemoveColor(v2, R); // nie bierzemy kolora R
                RemoveVariableWith2Colors(instance2, v2);  // zostały 2 kolory do wyboru
            }
            else
            {
                Color R2 = new(345376545);
                foreach (Color col in v2.AvalibleColors)
                    if (col.Value != c2_1.Value && col.Value != c2_2.Value && col.Value != R.Value) R2 = col;
                instance.RemoveColor(v2, c2_1);  // nie bierzemy c2_1 i c2_2
                instance.RemoveColor(v2, c2_2);

                RemoveVariableWith2Colors(instance, v2); // bierzemy ktorys z 2 pozostałych kolorów

                instance2.RemoveColor(v2, R);  // nie bierzemy R i R2
                instance2.RemoveColor(v2, R2);

                RemoveVariableWith2Colors(instance2, v2); // bierzemy ktorys z 2 pozostałych kolorów
            }
            instance.AddToResult(v, c); // wzieliśmy wczesniej kolor R i bierezmy teraz zafriko c
            if (v.AvalibleColors.Count == 3)
            {             
                instance2.RemoveColor(v, c);  // wzielismy ktoregos z sasiadow c wiec nie bierzemy go teraz
                RemoveVariableWith2Colors(instance2, v);  // zostały 2 kolory do wyboru
            }
            return new(){ instance, instance2 };

        }
    }
}
