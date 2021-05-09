using CSP;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.Definitions;
using GraphLib.Algorithms;

namespace CSPLemmas
{
    public static partial class CSPLemmas
    {
        public static void Lemma19(CspInstance instance)
        {
            Dictionary<Pair, bool> alreadyInACompontent = new();

            List<(Pair, Pair, Pair)> Small2Components = new();
            List<(Pair, Pair, Pair, Pair)> Good3Components = new();

            foreach (Variable var in instance.Variables)
            {
                foreach (Color col in var.AvalibleColors)
                {
                    if (!alreadyInACompontent.ContainsKey(new Pair(var, col)))
                    {
                        alreadyInACompontent[new Pair(var, col)] = true;
                        var tempTab = col.Restrictions.ToArray();
                        if (col.Restrictions.Count == 2)
                        {
                            Small2Components.Add(new(new Pair(var, col), tempTab[0], tempTab[1]));
                            alreadyInACompontent[tempTab[0]] = true;
                            alreadyInACompontent[tempTab[1]] = true;
                        }
                        else if (col.Restrictions.Count == 3)
                        {
                            Good3Components.Add(new(new Pair(var, col), tempTab[0], tempTab[1], tempTab[2]));
                            alreadyInACompontent[tempTab[0]] = true;
                            alreadyInACompontent[tempTab[1]] = true;
                            alreadyInACompontent[tempTab[2]] = true;
                        }
                        else throw new Exception("uhhhh cos chyba jest nie tak");
                    }
                }
            }
            List<Variable> varibles = instance.Variables.ToList();
            Dictionary<int, int> idsToIndex = new();
            Dictionary<int, int> indexToId = new();
            int index = 0;
            foreach (Variable var in instance.Variables)
            {
                idsToIndex.Add(var.Id, index);
                indexToId.Add(index, var.Id);
                index++;
            }

            BipartieGraph g = new(instance.Variables.Count, Small2Components.Count + Good3Components.Count);
            for (int i = 0; i < Small2Components.Count; i++)
            {
                g.AddEdge(idsToIndex[Small2Components[i].Item1.Variable.Id], i);
                g.AddEdge(idsToIndex[Small2Components[i].Item2.Variable.Id], i);
                g.AddEdge(idsToIndex[Small2Components[i].Item3.Variable.Id], i);
            }
            for (int i = 0; i < Good3Components.Count; i++)
            {
                g.AddEdge(idsToIndex[Good3Components[i].Item1.Variable.Id], i + Small2Components.Count);
                g.AddEdge(idsToIndex[Good3Components[i].Item2.Variable.Id], i + Small2Components.Count);
                g.AddEdge(idsToIndex[Good3Components[i].Item3.Variable.Id], i + Small2Components.Count);
                g.AddEdge(idsToIndex[Good3Components[i].Item4.Variable.Id], i + Small2Components.Count);
            }
            int[] result = new BipartieGraphMaxMatching().FindMaxMatching(g); //partA  to są variables a partB to są komponenty
            for (int i = 0; i < Small2Components.Count; i++)
            {
                if (result[i] == -1) throw new Exception("nie da sie pokolorwoać co teraz");
                int currId = indexToId[result[i]]; // id zmiennej ktora została przyznana do small2componentu i
                if (Small2Components[i].Item1.Variable.Id == currId)
                    instance.AddToResult(Small2Components[i].Item1);
                else if (Small2Components[i].Item2.Variable.Id == currId)
                    instance.AddToResult(Small2Components[i].Item2);
                else
                    instance.AddToResult(Small2Components[i].Item3);
            }
            for (int i = 0; i < Good3Components.Count; i++)
            {
                if (result[i] == -1) throw new Exception("nie da sie pokolorwoać co teraz");
                int currId = indexToId[result[i + Small2Components.Count]]; // id zmiennej ktora została przyznana do good3component i
                if (Good3Components[i].Item1.Variable.Id == currId)
                    instance.AddToResult(Good3Components[i].Item1);
                else if (Good3Components[i].Item2.Variable.Id == currId)
                    instance.AddToResult(Good3Components[i].Item2);
                else if (Good3Components[i].Item3.Variable.Id == currId)
                    instance.AddToResult(Good3Components[i].Item3);
                else
                    instance.AddToResult(Good3Components[i].Item4);
            }
        }
    }
}