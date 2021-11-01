using System;
using System.Collections.Generic;

namespace fmp
{
    public enum NodeType
    {
        AVAILABLE,
        INVALID
    }

    /// <summary>
    /// This is the interface that must be implemented by user.
    /// </summary>
    public interface INavMesh
    {
        /// <summary>
        /// This will calculate the distance from a node to goal.
        /// Is the Heuristic, that will return the distance from nodeIndex to goalIndex.
        /// </summary>
        /// <param name="goalIndex"> is the index of the node that represent the goal.</param>
        /// <param name="nodeIndex"> is the index of the node that you want to calculate the distance.</param>
        /// <returns> the distance from nodeIndex to goalIndex.</returns>
        public double ComputeDistanceToGoal(ulong goalIndex, ulong nodeIndex);

        /// <summary>
        /// This will calculate the movement cost from a node to a neighbor node.
        /// </summary>
        /// <param name="neighborIndex"></param>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        public double ComputeCostToNeighbor(ulong neighborIndex, ulong nodeIndex);

        /// <summary>
        /// Use this to get all the valid heighbod nodes. A valid node
        /// is considered a node that can be used in the path => do not return the
        /// nodes that are invalid (aka are walla, edges, holes, etc...).
        /// </summary>
        /// <param name="nodeIndex">The index of the node</param>
        /// <returns></returns>
        public List<ulong> GetNeighbors(ulong nodeIndex);

        /// <summary>
        /// Use this to get the type of a node. The value is a custom value that means something
        /// for the app that implement this interface. 
        /// </summary>
        /// <param name="nodeIndex">The index of the node</param>
        /// <returns></returns>
        public NodeType GetNodeType(ulong nodeIndex);

    }
}
