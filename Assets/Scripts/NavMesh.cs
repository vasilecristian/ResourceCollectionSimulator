using System;
using System.Collections.Generic;
using fmp;

using UnityEngine;

namespace rcs
{
   
    public class NavMesh : INavMesh
    {
        const uint ROW = 0;
        const uint COL = 1;

        const long O = 0; // Road
        const long I = 1; // Building
        const long M = 2; // Mineral
        const long B = 3; // Base


        //const ulong k_w = 8;
        //const ulong k_h = 8;
        //ulong[] map = new ulong[8 * 8]
        //{
        //    /*     0  1  2  3  4  5  6  7  */
        //    /*0*/  1, 1, 1, 1, 1, 1, 1, 1, 
        //    /*1*/  1, 0, 1, 0, 0, 0, 0, 1, 
        //    /*2*/  1, 0, 1, 0, 1, 1, 0, 1, 
        //    /*3*/  1, 0, 1, 0, 1, 0, 1, 1, 
        //    /*4*/  1, 0, 1, 0, 1, 1, 0, 1, 
        //    /*5*/  1, 0, 1, 0, 1, 0, 1, 1, 
        //    /*6*/  1, 0, 0, 1, 1, 1, 0, 1, 
        //    /*7*/  1, 1, 1, 1, 1, 1, 1, 1,
        //};

        const ulong k_w = 8;
        const ulong k_h = 8;
        long[] m_map = new long[8 * 8]
        {
            /*     0  1  2  3  4  5  6  7  */
            /*0*/  0, 0, 0, 0, 0, 0, 0, 0, 
            /*1*/  1, 0, 1, 0, 0, 0, 0, 0, 
            /*2*/  1, 0, 1, 0, 1, 1, 0, 0, 
            /*3*/  1, 0, 1, 0, 1, 0, 1, 0, 
            /*4*/  1, 0, 1, 0, 1, 1, 0, 0, 
            /*5*/  0, 0, 1, 0, 1, 0, 1, 0, 
            /*6*/  0, 0, 0, 1, 1, 1, 0, 0, 
            /*7*/  0, 0, 0, 1, 0, 0, 0, 0,
        };

        //const ulong k_w = 8;
        //const ulong k_h = 4;
        //ulong[] map = new ulong[8 * 4]
        //{
        //    /*     0  1  2  3  4  5  6  7  */
        //    /*0*/  0, 0, 0, 0, 1, 1, 1, 1, 
        //    /*1*/  0, 0, 0, 0, 0, 1, 1, 1, 
        //    /*2*/  1, 1, 0, 1, 1, 0, 0, 0, 
        //    /*3*/  1, 0, 0, 0, 0, 0, 0, 0,
        //};

        //======================================================================
        public void PrintMap(List<ulong> solution)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("    |  0  1  2  3  4  5  6  7    ");
            Console.WriteLine("---------------------------------");
            for (ulong y = 0; y < k_h; y++)
            {
                String line = "  " + y + " |  ";
                for (ulong x = 0; x < k_w; x++)
                {
                    ulong index = y * k_w + x;
                    if (solution != null && solution.Contains(index))
                    {
                        line += "*, ";
                    }
                    else
                    {
                        line += m_map[index] + ", ";
                    }
                }
                Console.WriteLine(line);
            }
            Console.WriteLine("---------------------------------");
        }


        //======================================================================
        public void Generate3DMap()
        { 
            for (ulong y = 0; y < k_h; y++)
            {
                for (ulong x = 0; x < k_w; x++)
                {
                    ulong index = y * k_w + x;
                    
                    if (m_map[index] == 0)
                    {
                        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        plane.transform.position = new Vector3(x * 10, 0, y * 10);
                        plane.GetComponent<Renderer>().material.color = new Color32(60, 84, 86, 1);
                    }
                    else
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x * 10, 0, y * 10);
                        cube.transform.localScale = new Vector3(10, 10, 10);
                        cube.GetComponent<Renderer>().material.color = new Color32(177, 199, 201, 1);
                    }
                }
            }
        }


        //======================================================================
        public void PrintSolution(List<ulong> solution)
        {
            String str = "Solution=";
            foreach (ulong index in solution)
            {
                str += " " + index + ",";
            }
            Console.WriteLine(str);
        }

        //======================================================================
        public NavMesh()
        {
            Console.WriteLine(this.GetType().FullName);
        }

        //======================================================================
        public ulong GetIndex(ulong x, ulong y)
        {
            return y * k_w + x;
        }

        //======================================================================
        public ulong[] GetCoords(ulong index)
        {
            ulong x = index % k_w;
            ulong y = index / k_w;

            return new ulong[2] { x, y };
        }

        //======================================================================
        public double ComputeCostToNeighbor(ulong neighborIndex, ulong nodeIndex)
        {
            ulong[] neighborCoords = GetCoords(neighborIndex);
            ulong neighborX = neighborCoords[ROW];
            ulong neighborY = neighborCoords[COL];

            ulong[] nodeCoords = GetCoords(nodeIndex);
            ulong nodeX = nodeCoords[ROW];
            ulong nodeY = nodeCoords[COL];

            long dx = Math.Abs((long)(neighborX - nodeX));
            long dy = Math.Abs((long)(neighborY - nodeY));

            return Math.Sqrt(dx * dx + dy * dy);

            //if ((dx + dy) >= 2)
            //{
            //    return 14;
            //}

            //if ((dx + dy) == 1)
            //{
            //    return 10;
            //}

            ///// in case the x = 0, y = 0 -> neighbor = this
            ///// so, the cost is 0
            //return 0;
        }

        //======================================================================
        public double ComputeDistanceToGoal(ulong goalIndex, ulong nodeIndex)
        {
            ulong[] goalCoords = GetCoords(goalIndex);
            ulong goalX = goalCoords[ROW];
            ulong goalY = goalCoords[COL];

            ulong[] nodeCoords = GetCoords(nodeIndex);
            ulong nodeX = nodeCoords[ROW];
            ulong nodeY = nodeCoords[COL];

            long dx = Math.Abs((long)(nodeX - goalX));
            long dy = Math.Abs((long)(nodeY - goalY));
            double dist = Math.Sqrt(dx * dx + dy * dy);

            return dist;
        }

        //======================================================================
        public List<ulong> GetNeighbors(ulong nodeIndex)
        {
            List<ulong> neighbors = new List<ulong>();

            ulong[] nodeCoords = GetCoords(nodeIndex);
            ulong nodeX = nodeCoords[ROW];
            ulong nodeY = nodeCoords[COL];

            for (long y = (long)nodeY - 1; y <= (long)nodeY + 1; y++)
            {
                for (long x = (long)nodeX - 1; x <= (long)nodeX + 1; x++)
                {
                    
                    /// if is outside the mesh do not add it
                    if ((y < 0) || (y >= (long)k_h))
                        continue;

                    /// if is outside the mesh do not add it
                    if ((x < 0) || (x >= (long)k_w))
                        continue;

                    /// if is the current node do not add it
                    if ((x == (long)nodeX) && (y == (long)nodeY))
                        continue;
                    
                    ulong neighborIndex = GetIndex((ulong)x, (ulong)y);//y * (long)k_w + x;

                    /// If the tile have collision on it, do not add it
                    if (GetNodeType(neighborIndex) != NodeType.AVAILABLE)
                        continue;


                    neighbors.Add((ulong)neighborIndex);
                }
            }

            return neighbors;
        }

        //======================================================================
        public NodeType GetNodeType(ulong nodeIndex)
        {
            if(nodeIndex >= (k_w*k_h))
            {
                return NodeType.INVALID;
            }
            else if(m_map[nodeIndex] == 1)
            {
                return NodeType.INVALID;
            }

            return NodeType.AVAILABLE;
        }
    }
}
