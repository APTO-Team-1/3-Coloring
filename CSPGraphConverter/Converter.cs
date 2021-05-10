using System;
using System.Collections.Generic;
using CSP;
using GraphLib;

namespace CSPGraphConverter
{
    public class Converter
    {
        public static CspInstance BipartieGraphToCSP(GraphLib.Definitions.BipartieGraph bipartieGraph, int numberOfColors = 3)
        {
            GraphLib.Definitions.Graph graph = new(bipartieGraph.PartAVertices.Length + bipartieGraph.PartBVertices.Length);
            for (int i = 0; i < bipartieGraph.PartAVertices.Length; i++)
            {
                foreach (int neighbour in bipartieGraph.PartAVertices[i])
                {
                    graph.AddEdge(i, neighbour + bipartieGraph.PartAVertices.Length);
                }
            }
            return GraphToCSP(graph, numberOfColors);
        }

        public static CspInstance GraphToCSP(GraphLib.Definitions.Graph graph, int numberOfColors = 3)
        {
            List<Variable> addedVariables = new();

            CspInstance cspInstance = new();
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                addedVariables.Add(new Variable(numberOfColors));
                cspInstance.AddVariable(addedVariables[i]);
                foreach (var neighbour in graph.GetNeighbors(i))
                {
                    if (neighbour < i)
                    {
                        for(int j = 0; j < numberOfColors; j++)
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