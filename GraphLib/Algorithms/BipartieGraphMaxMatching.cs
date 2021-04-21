using GraphLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLib.Algorithms
{
    public  class BipartieGraphMaxMatching
    {
        // Returns maximum number of 
        // matching from M to N
        public int[] FindMaxMatching(BipartieGraph bg)
        {
            // An array to keep track of the 
            // applicants assigned to jobs. 
            // The value of matchR[i] is the 
            // applicant number assigned to job i, 
            // the value -1 indicates nobody is assigned.
            // matchR[B] = A
            int[] matchR = new int[bg.PartBVertices.Length];

            // Initially all jobs are available
            for (int i = 0; i < bg.PartBVertices.Length; ++i)
                matchR[i] = -1;

            // Count of jobs assigned to applicants
            int result = 0;
            for (int u = 0; u < bg.PartAVertices.Length; u++)
            {
                // Mark all jobs as not
                // seen for next applicant.
                bool[] seen = new bool[bg.PartAVertices.Length];
                //for(int i = 0; i < bg.PartAVertices.Length; ++i)
                //    seen[i] = false;

                // Find if the applicant 
                // 'u' can get a job
                if (bpm(bg, u, seen, matchR))
                    result++;
            }
            return matchR;


        }


    // A DFS based recursive function 
    // that returns true if a matching 
    // for vertex u is possible
    bool bpm(BipartieGraph bg, int u, 
             bool []seen, int []matchR)
    {
        // Try every job one by one
        for (int v = 0; v < bg.PartBVertices.Length; v++)
        {
            // If applicant u is interested 
            // in job v and v is not visited
            if ( bg.ContainsEdgeAtoB(u, v) && !seen[v])
            {
                // Mark v as visited
                seen[v] = true; 
  
                // If job 'v' is not assigned to
                // an applicant OR previously assigned 
                // applicant for job v (which is matchR[v])
                // has an alternate job available.
                // Since v is marked as visited in the above 
                // line, matchR[v] in the following recursive 
                // call will not get job 'v' again
                if (matchR[v] < 0 || bpm(bg, matchR[v],
                                         seen, matchR))
                {
                    matchR[v] = u;
                    return true;
                }
            }
        }
        return false;
    }
  
     
    }
}
