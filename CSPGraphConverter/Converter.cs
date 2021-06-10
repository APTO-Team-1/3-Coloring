using CSP;
using System.Collections.Generic;

namespace CSPGraphConverter
{
    public class Converter
    {
        public static CspInstance GraphToCSP(GraphLib.Definitions.Graph graph, int numberOfColors = 3)
        {
            List<Variable> addedVariables = new();

            CspInstance cspInstance = new();
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                addedVariables.Add(new Variable(numberOfColors));
                addedVariables[i].Id = i;
                cspInstance.AddVariable(addedVariables[i]);
                foreach (var neighbour in graph.GetNeighbors(i))
                {
                    if (neighbour < i)
                    {
                        for (int j = 0; j < numberOfColors; j++)
                        {
                            cspInstance.AddRestriction(new Pair(addedVariables[i], addedVariables[i].AvalibleColors[j]), new Pair(addedVariables[neighbour], addedVariables[neighbour].AvalibleColors[j]));
                        }
                    }
                }
            }
            return cspInstance;
        }
    }
}