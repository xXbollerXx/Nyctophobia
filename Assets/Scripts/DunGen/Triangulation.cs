using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace DunGen {
    public class Triangulation : MonoBehaviour
    {
        //NEED TO DO  rewrite trianglelation so it dosent intersect with other edgest and then need to write method which flips common edge of triangle
        public static Edge[] DelaunayTri(Cell[] rooms)
        {
            if (rooms.Length < 3)
            {
                Debug.LogError("rooms were not more than 3");
                throw new DataException();
            }
            
            // start by adding 3 points and making triangle
            List<Vector2> points = new List<Vector2> { rooms[0].Center, rooms[1].Center, rooms[2].Center };

            List<Triangle> triangles = new List<Triangle> { new Triangle(rooms[0].Center, rooms[1].Center, rooms[2].Center) };

            // triangulate all rooms 
            for (int i = 3; i < rooms.Length; i++) // loop for every room on map starting from 4th room 
            {
                points.Add(rooms[i].Center);// increment Vertex point
                CheckNewPoint(triangles, points.ToArray(), points[i]);
            }


            //remove all duplecate edges 
            List<Edge> edges = new List<Edge>(); // holds all common edges 

            foreach (Triangle triangle in triangles)
            {
                CheckArray(triangle, edges);
            }
            return edges.ToArray();
        }
        
        //checks to find if new point is inside of triangle
        private static void CheckNewPoint(List<Triangle> triangles, Vector2[] points, Vector2 newPoint)
        {

            ref List<Triangle> tries = ref triangles; // ref of the list to b looped through. e.g high level or subdivided triangles.

            // check if the point is inside a triangle 
            do
            {
                var flag = true;
                foreach (Triangle tri in tries)
                {
                    if (tri.VertexInTri(newPoint)) //if the point is inside a triangle
                    {
                        //check if this triangle is subdivided and then check those triangles  
                        if (tri.SubTris.Count <= 0)
                        {
                            //subdivide triangle
                            tri.Subdivide(newPoint);
                            return; // new point has been triangulated so leave
                        }
                        tries = ref tri.SubTris;
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    // triangulate new point and add it to list of triangles
                    //triangles.Add(TriangulateNewPoint(points, newPoint));
                    triangles.Add(TriangulateNewPointNew(points, newPoint, triangles));
                    
                    return;
                }
            } while (true);
        }


        //makes a new triangle using a new vertex and the 2 closest vertices 
        private static Triangle TriangulateNewPoint(Vector2[] points, Vector2 point)
        {
            // new point is outside of triangles so triangulate it with closest 2 points 
            Vector2 vertex = point;
            Vector2 point1 = points[0];
            Vector2 point2 = points[1];

            for (int i = 2; i < points.Length - 1; i++)
            {
                // get the variabel with larger distance
                ref Vector2 pointToChange = ref point1;
                if (Vector2.Distance(vertex, point1) < Vector2.Distance(vertex, point2))
                {
                    pointToChange = ref point2;
                }

                // if distance to i is smaller than distance to saved point
                if (Vector2.Distance(vertex, points[i]) < Vector2.Distance(vertex, pointToChange))
                {
                    pointToChange = points[i];
                }
            }

            return new Triangle(vertex, point1, point2);
        }
        private static Triangle TriangulateNewPointNew(Vector2[] points, Vector2 point, List<Triangle> triangles)
        {
            // new point is outside of triangles so triangulate it with closest 2 points 
            Vector2 vertex = point;
            Vector2 point1 = points[0];
            Vector2 point2 = points[1];
            List<Vector2> validPoints = new List<Vector2>();

            foreach (Triangle triangle in triangles)
            {
                //check every triangles points;
                //if signed angle to new point is outside of interiar angle than add it to the valid list

                if (!IsAngleIntersecting(triangle.E1.Vertex1, triangle.E2.Vertex1, triangle.E3.Vertex1, point))
                {
                    validPoints.Add(triangle.E1.Vertex1);
                }
                if (!IsAngleIntersecting(triangle.E2.Vertex1, triangle.E3.Vertex1, triangle.E1.Vertex1, point))
                {
                    validPoints.Add(triangle.E2.Vertex1);
                }
                if (!IsAngleIntersecting(triangle.E3.Vertex1, triangle.E1.Vertex1, triangle.E2.Vertex1, point))
                {
                    validPoints.Add(triangle.E3.Vertex1);
                }


            }
            //check for 2 closest valid points

            foreach (Vector2 vpoint in validPoints)
            {
                // get the variabel with larger distance
                ref Vector2 pointToChange = ref point1;
                if (Vector2.Distance(vertex, point1) < Vector2.Distance(vertex, point2))
                {
                    pointToChange = ref point2;
                }

                // if distance to i is smaller than distance to saved point
                if (Vector2.Distance(vertex, vpoint) < Vector2.Distance(vertex, pointToChange))
                {
                    pointToChange = vpoint;
                }
            }

            return new Triangle(vertex, point1, point2);
            


        }

        private static bool IsAngleIntersecting(Vector2 pointToCheck, Vector2 v2, Vector2 v3, Vector2 newPoint)
        {
            Vector2 V1Dir;
            Vector2 V2Dir;

            //Cal interior angle
            V1Dir = (v3 - pointToCheck).normalized;
            V2Dir = (v2 - pointToCheck).normalized;

            float InteriorAngle = Vector2.SignedAngle(V1Dir, V2Dir);

            //cal angle to new vertex 
            V1Dir = (v3 - pointToCheck).normalized;
            V2Dir = (newPoint - pointToCheck).normalized;

            float AngleToVertex = Vector2.SignedAngle(V1Dir, V2Dir);

            if (AngleToVertex >= 0 && AngleToVertex <= InteriorAngle) // if angle is bigger than 0 but smaller than angle will be intersecting
            {
                return true;
            }
            return false;
        }



        //used to remove remove duplicate edges 
        private static void CheckArray(Triangle T, List<Edge> edges)
        {
            if (T.SubTris.Count > 0)// if this triangle is subdivided
            { 
                foreach (Triangle triangle in T.SubTris) // loop through subdivided triangles 
                {
                    CheckArray(triangle, edges);
                }
            }
            else // if this triangle is not subdivided, check all edges and add if they are not duplecates 
            {
                //need to check if the 3 edges are in the edges array
                int count = 0;
                bool b1 = true, b2 = true, b3 = true;
                foreach (Edge edge in edges)
                {
                    if (edge.Equals(T.E1)) // if value r the same
                    {
                        count++;
                        b1 = false;
                    }
                    else if (edge.Equals(T.E2))
                    {
                        count++;
                        b2 = false;
                    }
                    else if (edge.Equals(T.E3))
                    {
                        count++;
                        b3 = false;
                    }
                    if (count >= 3)
                    {
                        break;
                    }
                }
                if (b1)
                {
                    edges.Add(T.E1);
                }
                if (b2)
                {
                    edges.Add(T.E2);
                }
                if (b3)
                {
                    edges.Add(T.E3);
                }
            }
        }

        public static void DrawTriangulation(Edge[] edges)
        {
            
            foreach (Edge edge in edges)
            {
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector2 position = Vector2.Lerp(edge.Vertex1 * 2, edge.Vertex2 * 2, 0.5f);
                temp.transform.position = new Vector3(position.x, 0, position.y); //place line between points
                Vector3 rotation = new Vector3(edge.Vertex2.x * 2, 0, edge.Vertex2.y * 2) - new Vector3(edge.Vertex1.x * 2, 0, edge.Vertex1.y * 2);
                temp.transform.rotation = Quaternion.LookRotation(rotation);
                temp.transform.localScale = new Vector3(1, 1, edge.Weight * 2);
                
                //temp.transform.position = temp.transform.position + temp.transform.forward * (edge.Weight * 5);
                //Debug.DrawLine(edge.Vertex1,edge.Vertex2, Color.green);
            }
        }


        void Circumcircle(Vector2 v1, Vector2 v2, Vector2 v3, out Vector2 circumcenter, out float circumradius)
        {
            float ax = v1.x;
            float ay = v1.y;
            float bx = v2.x;
            float by = v2.y;
            float cx = v3.x;
            float cy = v3.y;

            float d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));
            float ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            float uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
            circumcenter = new Vector2(ux, uy);
            circumradius = Vector2.Distance(circumcenter, v1);
        }



    }

    
    
}

    
