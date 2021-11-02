using System;
using System.Collections.Generic;
using fmp;

using UnityEngine;

namespace rcs
{
    public class PositionOnPlayspace
    {
        public ulong x = 0;
        public ulong y = 0;

        public PositionOnPlayspace(ulong posX, ulong posY) { x = posX; y = posY; }
    }
   
    public class Playspace : INavMesh
    {
        const uint ROW = 0;
        const uint COL = 1;

        public const long O = 0; // 
        public const long I = 1; // Invalid
        public const long R = 2; // Resource
        public const long S = 3; // Storage


        //const ulong k_w = 8;
        //const ulong Height = 8;
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
            /*0*/  S, 0, 0, 0, 0, 0, 0, R, 
            /*1*/  1, 0, 1, 0, 0, 0, 0, 0, 
            /*2*/  1, 0, 1, 0, 1, 1, 0, 0, 
            /*3*/  1, 0, 1, 0, 1, R, 1, 0, 
            /*4*/  1, 0, 1, 0, 1, 1, 0, 0, 
            /*5*/  0, 0, 1, 0, 1, 0, 1, 0, 
            /*6*/  0, 0, 0, 1, 1, 1, R, 0, 
            /*7*/  S, 0, 0, 1, R, 0, 0, 0,
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
        public ulong Width
        {
            get { return k_w; }
        }

        //======================================================================
        public ulong Height
        {
            get { return k_h; }
        }


        //======================================================================
        public void PrintMap(List<ulong> solution)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("    |  0  1  2  3  4  5  6  7    ");
            Console.WriteLine("---------------------------------");
            for (ulong y = 0; y < Height; y++)
            {
                String line = "  " + y + " |  ";
                for (ulong x = 0; x < Width; x++)
                {
                    ulong index = y * Width + x;
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
        public Playspace()
        {
            Console.WriteLine(this.GetType().FullName);
        }

        //======================================================================
        public long GetNodeValue(ulong x, ulong y)
        {
            ulong nodeIndex = y * Width + x;

            if (nodeIndex >= (Width * Height))
            {
                return I;
            }

            return m_map[nodeIndex];            
        }

        //======================================================================
        public ulong GetIndex(ulong x, ulong y)
        {
            return y * Width + x;
        }

        //======================================================================
        public ulong[] GetCoords(ulong index)
        {
            ulong x = index % Width;
            ulong y = index / Width;

            return new ulong[2] { x, y };
        }

        //======================================================================
        public PositionOnPlayspace GetMapPos(ulong index)
        {
            PositionOnPlayspace mapPos = new PositionOnPlayspace(0, 0);
            mapPos.x = index % Width;
            mapPos.y = index / Width;

            return mapPos;
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
                    if ((y < 0) || (y >= (long)Height))
                        continue;

                    /// if is outside the mesh do not add it
                    if ((x < 0) || (x >= (long)Width))
                        continue;

                    /// if is the current node do not add it
                    if ((x == (long)nodeX) && (y == (long)nodeY))
                        continue;
                    
                    ulong neighborIndex = GetIndex((ulong)x, (ulong)y);//y * (long)Width + x;

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
            if(nodeIndex >= (Width*Height))
            {
                return NodeType.INVALID;
            }
            else if(m_map[nodeIndex] == I)
            {
                return NodeType.INVALID;
            }

            return NodeType.AVAILABLE;
        }
    }
}
