using System;
using System.Collections.Generic;

namespace fmp
{
    /// <summary>
    /// This is a ticket used to describe a find path request.
    /// </summary>
    public class Ticket
    {

		/// <summary>
		/// Used to describe the possible states of the Ticket.
		/// </summary>
		public enum STATE
		{
			/// <summary>
			/// The ticket is waiting to be processed.
			/// </summary>
			WAITING = 0,

			/// <summary>
			/// The ticket is processed...
			/// </summary>
			PROCESSING,

			/// <summary>
			/// The ticket was processed with success.
			/// </summary>
			COMPLETED,

			/// <summary>
			/// The ticket was processed but the process was stopped for some reason...
			/// </summary>
			STOPPED,

			/// <summary>
			/// The ticket was in processing state but the was cancelled from external...
			/// </summary>
			CANCELLED,

			/// <summary>
			/// The destionation is invalid (aka outside of the map, or is a collision tile, etc...).
			/// </summary>
			INVALID_GOAL,
		};

		/// <summary>
        /// It is the state of this ticket. By default is in WAITING state.
        /// </summary>
		public STATE State { get; internal set; } = STATE.WAITING;


		/// <summary>
		/// Getter for the steps required to determine the path.
		/// </summary>
		public uint Steps { get; internal set; } = 0;

		/// <summary>
		/// Getter for the detected path.
		/// </summary>
		public List<ulong> Path { get; internal set; } = new List<ulong>();

		/// <summary>
		/// Getter for the goal node.
		/// </summary>
		public ulong GoalIndex { get; internal set; }

		/// <summary>
		/// Getter for the start node.
		/// </summary>
		public ulong StartIndex { get; internal set; }


		/// <summary>
		/// Getter for the opened list.
		/// </summary>
		internal SortedDictionary<ulong, Node> OpenList { get; set; } = new SortedDictionary<ulong, Node>();

		/// <summary>
		/// Getter for the closed list.
		/// </summary>
		internal SortedDictionary<ulong, Node> ClosedList { get; set; } = new SortedDictionary<ulong, Node>();


		/// <summary>
        /// It is the current procesed Node.
        /// </summary>
		internal Node CurrentNode { get; set; } = null;

		/// <summary>
        /// The constructor
        /// </summary>
        /// <param name="startIndex">Index of the start Node.</param>
        /// <param name="goalIndex">Index of the goal(aka target) Node.</param>
		public Ticket(ulong startIndex, ulong goalIndex)
        {
			//Console.WriteLine(this.GetType().FullName);

			StartIndex = startIndex;
			GoalIndex = goalIndex;
        }
    }
}
