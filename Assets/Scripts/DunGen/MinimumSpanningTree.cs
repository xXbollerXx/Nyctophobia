using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen
{
    public class MinimumSpanningTree : MonoBehaviour
    {
        private static Edge[] edges;
        
        public static Edge[] GenerateTree(Cell[] rooms, bool flag)
        {
             
            edges = Triangulation.DelaunayTri(rooms);//triangulate all rooms

            if (flag)
            {
                Triangulation.DrawTriangulation(edges);
            }

            return edges;
        }
    }
}
