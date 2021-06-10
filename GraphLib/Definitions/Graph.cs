using System;
using System.Collections.Generic;

namespace GraphLib.Definitions
{
    public class Graph
    {
        // vertices numbered with sequential numbers starting form 0
        protected HashSet<int>[] adjacencyList;

        public int VerticesCount { get => adjacencyList.Length; }

        /// <param name="n">number of vertices</param>
        public Graph(int n)
        {
            adjacencyList = new HashSet<int>[n];
            for (int i = 0; i < n; i++)
                adjacencyList[i] = new();
        }

        public Graph(HashSet<int>[] adjacencyList)
        {
            this.adjacencyList = adjacencyList;
        }

        public bool AddEdge(int from, int to)
        {
            if (from == to) return false;
            bool addedFromTo = adjacencyList[from].Add(to);
            bool addedToFrom = adjacencyList[to].Add(from);
            if ((addedFromTo && !addedToFrom) || (!addedFromTo && addedToFrom))
                throw new Exception("Not consistent - graph must not be directed.");

            return addedFromTo && addedToFrom;
        }

        public HashSet<int> GetNeighbors(int v)
        {
            if (v > this.VerticesCount)
                throw new Exception("Vertice number greater than max vertice number in the graph");
            return adjacencyList[v];
        }

        public bool ContainsEdge(int from, int to)
        {
            return adjacencyList[from].Contains(to);
        }

    }
}
