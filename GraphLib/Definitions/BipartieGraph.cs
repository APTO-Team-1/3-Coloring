using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLib.Definitions
{
    //public class BipartieGraph : Graph
    //{
    //    /// <summary>
    //    /// Vertices from A part. If not in part A, then in B.
    //    /// </summary>
    //    public int[] PartAVertices { get; }
    //    public int[] PartBVertices { get; }

    //    /// <param name="n">number of vertices</param>
    //    public BipartieGraph(int n, int[]  partAVertices, int[] partBVertices) : base(n)
    //    {
    //        this.PartAVertices = partAVertices;
    //        this.PartBVertices = partBVertices;
    //    }

    //    public BipartieGraph(HashSet<int>[] adjacencyList, int[] partAVertices, int[] partBVertices) : base( adjacencyList)
    //    {
    //        this.PartAVertices = partAVertices;
    //        this.PartBVertices = partBVertices;
    //    }

    //    //public bool IsVertexInPartA(int v)
    //    //{
    //    //    return PartAVertices.Contains(v);
    //    //}
    //}


    public class BipartieGraph
    {
        public HashSet<int>[] PartAVertices { get; }
        public HashSet<int>[] PartBVertices { get; }


        /// <param name="nA">number of vertices in A</param>
        /// <param name="nB">number of vertices in B</param>
        public BipartieGraph(int nA, int nB) 
        {
            this.PartAVertices = new HashSet<int>[nA];
            for (int i = 0; i < nA; i++)
                PartAVertices[i] = new();
            this.PartBVertices = new HashSet<int>[nB];
            for (int i = 0; i < nB; i++)
                PartBVertices[i] = new();
        }

    public BipartieGraph(HashSet<int>[] partAVertices, HashSet<int>[] partBVertices) 
        {
            this.PartAVertices = partAVertices;
            this.PartBVertices = partBVertices;
        }

        public bool AddEdge(int partAVertex, int partBVertex)
        {
            bool addedFromTo = PartAVertices[partAVertex].Add(partBVertex);
            bool addedToFrom = PartBVertices[partBVertex].Add(partAVertex);
            if ((addedFromTo && !addedToFrom) && (!addedFromTo && addedToFrom))
                throw new Exception("Not consistent - graph must not be directed.");

            return addedFromTo && addedToFrom;
        }

        public bool ContainsEdgeAtoB(int partAVertex, int partBVertex)
        {
            return PartAVertices[partAVertex].Contains(partBVertex);
        }
    }
}
