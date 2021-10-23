using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DunGen
{

    public class Triangle
    {
        public Edge E1, E2, E3;
        public List<Triangle> SubTris; // holds list of triangles that are a result of a subdivide
        public Triangle(Edge _E1, Edge _E2, Edge _E3)
        {
            E1 = _E1;
            E2 = _E2;
            E3 = _E3;
            SubTris = new List<Triangle>();
        }
        public Triangle(Vector2 V1, Vector2 V2, Vector2 V3)
        {
            E1 = new Edge(V1, V2, Vector2.Distance(V1, V2));
            E2 = new Edge(V2, V3, Vector2.Distance(V2, V3));
            E3 = new Edge(V3, V1, Vector2.Distance(V3, V1));
            SubTris = new List<Triangle>();
        }

        // is vertex in bounds
        public bool VertexInTri(Vector2 Vertex)
        {
            Vector2 V1Dir;
            Vector2 V2Dir;

            //Cal interior angle
            V1Dir = (E3.Vertex1 - E3.Vertex2).normalized;
            V2Dir = (E1.Vertex2 - E1.Vertex1).normalized;

            float InteriorAngle = Vector2.SignedAngle(V1Dir, V2Dir);

            //cal angle to new vertex 
            V1Dir = (E3.Vertex1 - E3.Vertex2).normalized;
            V2Dir = (Vertex - E1.Vertex1).normalized;

            float AngleToVertex = Vector2.SignedAngle(V1Dir, V2Dir);

            //Cal interior angle
            V1Dir = (E1.Vertex1 - E1.Vertex2).normalized;
            V2Dir = (E1.Vertex2 - E1.Vertex1).normalized;

            float InteriorAngle2 = Vector2.SignedAngle(V1Dir, V2Dir);

            //cal angle to new vertex 
            V1Dir = (E3.Vertex1 - E3.Vertex2).normalized;
            V2Dir = (Vertex - E1.Vertex2).normalized;

            float AngleToVertex2 = Vector2.SignedAngle(V1Dir, V2Dir);

            if(AngleToVertex >= 0 && AngleToVertex <= InteriorAngle) // if angle is bigger than 0 but smaller than interior angle
            {
                if (AngleToVertex2 >= 0 && AngleToVertex2 <= InteriorAngle2) // if angle is bigger than 0 but smaller than interior angle
                {
                    return true;
                }
            }


          //  if (AngleToVertex > InteriorAngle && AngleToVertex2 > InteriorAngle2)
          //  {
          //      return true;
           // }

            return false;
        }

        public void Subdivide(Vector2 vertex)
        {
            SubTris.Add(new Triangle(vertex, E1.Vertex1, E1.Vertex2));
            SubTris.Add(new Triangle(vertex, E2.Vertex1, E2.Vertex2));
            SubTris.Add(new Triangle(vertex, E3.Vertex1, E3.Vertex2));
        }
        
    }

    public class Edge
    {
        public Vector2 Vertex1, Vertex2;
        public float Weight;

        public Edge(Vector2 vertex1, Vector2 vertex2, float weight)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Weight = weight;
        }

        protected bool Equals(Edge other)
        {
            return Vertex1.Equals(other.Vertex1) && Vertex2.Equals(other.Vertex2) || Vertex1.Equals(other.Vertex2) && Vertex2.Equals(other.Vertex1);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Edge)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return (Vertex1.GetHashCode() * 397) ^ Vertex2.GetHashCode();
            }
        }
        
        
    }
}
