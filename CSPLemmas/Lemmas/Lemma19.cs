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
            var Small2Components = FindAll2SmallComponents(instance);
            var Good3Components = FindAllGood3Components(instance);
            Dictionary<int, int> idsToIndex = new();
            int index = 0;
            foreach(Variable var in instance.Variables)
            {
                idsToIndex.Add(var.Id, index);
                index++;
            }

            BipartieGraph g = new(instance.Variables.Count, Small2Components.Count + Good3Components.Count);
            for(int i =0;i<Small2Components.Count;i++)
            {
                g.AddEdge(idsToIndex[Small2Components[i].Item1.Id], i);
                g.AddEdge(idsToIndex[Small2Components[i].Item2.Id], i);
                g.AddEdge(idsToIndex[Small2Components[i].Item3.Id], i);
            }
            for (int i = 0; i < Good3Components.Count; i++)
            {
                g.AddEdge(idsToIndex[Good3Components[i].Item1.Id], i + Small2Components.Count);
                g.AddEdge(idsToIndex[Good3Components[i].Item2.Id], i + Small2Components.Count);
                g.AddEdge(idsToIndex[Good3Components[i].Item3.Id], i + Small2Components.Count);
                g.AddEdge(idsToIndex[Good3Components[i].Item4.Id], i + Small2Components.Count);
            }
            int[] result = new BipartieGraphMaxMatching().FindMaxMatching(g); // pozostaje dodac do rezultatu według tego jak zostało wykonane bipartition
        }

        private static List<(Variable, Variable, Variable)> FindAll2SmallComponents(CspInstance instance)
        {
            List<(Variable, Variable, Variable)> ret = new();
            return ret;
        }
        private static List<(Variable, Variable, Variable, Variable)> FindAllGood3Components(CspInstance instance)
        {
            List<(Variable, Variable, Variable, Variable)> ret = new();
            return ret;
        }
    }
}