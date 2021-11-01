using System;
using System.Collections.Generic;

namespace fmp
{
    /// <summary>
    /// This is the helper class used internally to store node's props.
    /// </summary>
    internal class Node
    {
        /// <summary>
        /// Is the index of the node.
        /// </summary>
        internal ulong Index { get; set; } = 0;

        /// <summary>
        /// Is the parent of this node.
        /// </summary>
        internal Node Parent { get; set; } = null;

        /// <summary>
        /// This is the cost to move to this tile. Is called also "G" value. This must be
		/// a sum of parent cost and the cost to move to this node starting from parent.
		/// By default this is 0.
        /// </summary>
        internal double Cost { get; set; } = 0;

        /// <summary>
        /// This the distance to target. It is calculated using a heuristic. It is called 
		/// also the "H" value. The function ComputeGoalDistanceEstimate will compute it.
        /// </summary>
        internal double DistanceToTarget { get; set; } = 0;

        /// <summary>
        /// This is "F" a sum of "G" and "H"
        /// </summary>
        internal double F { set; get; } = 0;

        /// <summary>
        /// The list with neighbors.
        /// </summary>
        internal List<ulong> Neighbors { get; set; } = new List<ulong>();


        internal Node(ulong index)
        {
            //Console.WriteLine(this.GetType().FullName);

            Index = index;
        }
    }
}
