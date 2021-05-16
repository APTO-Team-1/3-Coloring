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
            (CspInstance instance2,Variable[] clonedV,Color[] ClonedC )= instance.CloneAndReturnCorresponding(new Variable[] {v,v2,v,v2},new Color[] {c,c2_1,c2_2,R});
            Variable vCloned = clonedV[0], v2Cloned = clonedV[1];
            Color cCloned = ClonedC[0],RCloned = ClonedC[3];

            if(v2.AvalibleColors.Count == 3)
            {
                instance.AddToResult(v2, R); // bierzemy kolor R
                instance2.RemoveColor(v2Cloned, RCloned); // nie bierzemy kolora R
                RemoveVariableWith2Colors(instance2, v2Cloned);  // zostały 2 kolory do wyboru
            }
            else if (v2.AvalibleColors.Count == 4)
            {
                Color R2 = new(345376545), R2Cloned = new(345376545);
                foreach (Color col in v2.AvalibleColors)
                    if (col.Value != c2_1.Value && col.Value != c2_2.Value && col.Value != R.Value) R2 = col;
                foreach (Color col in clonedV[1].AvalibleColors)
                    if (col.Value != c2_1.Value && col.Value != c2_2.Value && col.Value != R.Value) R2Cloned = col;

                instance.RemoveColor(v2, c2_1);  // nie bierzemy c2_1 i c2_2
                instance.RemoveColor(v2, c2_2);

                RemoveVariableWith2Colors(instance, v2); // bierzemy ktorys z 2 pozostałych kolorów

                instance2.RemoveColor(v2Cloned, RCloned);  // nie bierzemy R i R2
                instance2.RemoveColor(v2Cloned, R2Cloned);

                RemoveVariableWith2Colors(instance2, v2Cloned); // bierzemy ktorys z 2 pozostałych kolorów
            }
            instance.AddToResult(v, c); // wzieliśmy wczesniej kolor R i bierezmy teraz zafriko c
            if (v.AvalibleColors.Count == 3)
            {             
                instance2.RemoveColor(vCloned, cCloned);  // wzielismy ktoregos z sasiadow c wiec nie bierzemy go teraz
                RemoveVariableWith2Colors(instance2, vCloned);  // zostały 2 kolory do wyboru
            }
            return new(){ instance, instance2 };

        }
    }
}
