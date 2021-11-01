using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace fmp
{
    /// <summary>
    /// This is the Main class that implemnts the generic A * (A star) search algorithm.
    /// </summary>
    public class FindMyPath: IDisposable
    {
		List<CancellationTokenSource> m_cancelTokensSrc = new List<CancellationTokenSource>();

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="navMesh">Is a pointer to the user's navmesh class.</param>
        public FindMyPath(INavMesh navMesh)
        {
            NavMesh = navMesh;
        }

		/// <summary>
        /// This will create an async task that will start the A* algorithm to find the path.
        /// </summary>
        /// <param name="ticket">is the ticket request.</param>
        /// <param name="cancelTokenSrc">is the cancelation token</param>
        /// <returns>a Task that will be competed when the path will be detected.</returns>
		public async Task<Ticket> FindPathAsync(Ticket ticket, CancellationTokenSource cancelTokenSrc)
        {
			m_cancelTokensSrc.Add(cancelTokenSrc);

			await Task.Run(() =>
            { 
				//Console.WriteLine("Task.Run");

				try
				{
					while (!ProcessTicket(ticket, cancelTokenSrc.Token));					
				}
				catch(OperationCanceledException)
                {

                }
				//Console.WriteLine("Task.Run 2");

            }, cancelTokenSrc.Token);

			return ticket;
        }

		/// <summary>
		/// Cancel all in progress jobs.
		/// </summary>
		public void CalcelAll()
		{
			foreach(CancellationTokenSource cts in m_cancelTokensSrc)
            {
				if(!cts.IsCancellationRequested)
                {
					cts.Cancel();
                }
            }

			Console.WriteLine(this.GetType().FullName + ".CalcelAll");
		}

		/// <summary>
		/// This will release all internal allocated resources when this object will
		/// not be used anymore. Resources = ongoing process, memory, etc...
		/// </summary>
		public void Dispose()
        {
			CalcelAll();

			Console.WriteLine(this.GetType().FullName + ".Dispose");
        }


		/// <summary>
        /// This is the A* function. This will be called multiple times until
        /// the path is calculated. All the states values are storred in Ticket.
        /// To calcualte the path this function must be called multiple times until
        /// will return true!
        /// </summary>
        /// <param name="ticket">the request</param>
        /// <param name="token">the cancelation token</param>
        /// <returns>true if the processing finished.</returns>
		private bool ProcessTicket(Ticket ticket, CancellationToken token)
		{
			/// Check the cancelation token.
			if (token.IsCancellationRequested)
			{
				/// In case the Cancel was requested from external,
				/// the function will return.
				ticket.State = Ticket.STATE.CANCELLED;
				token.ThrowIfCancellationRequested();
			}

			/// Switch the state to Processing.
			ticket.State = Ticket.STATE.PROCESSING;

			ticket.Steps++;

			if(NavMesh.GetNodeType(ticket.GoalIndex) == NodeType.INVALID)
            { 
				ticket.State = Ticket.STATE.INVALID_GOAL;
				return true;
			}

			/// Chekc if the StartIndex is the same with GoalIndex
			if (ticket.StartIndex == ticket.GoalIndex)
			{
				ticket.Path.Add(ticket.StartIndex);

				/// Search is stopped because the goal si the same with start node
				ticket.State = Ticket.STATE.COMPLETED;
				return true;
			}

			
			/// This is the first step -> closed list is empty.
			/// Add the start node to the closed list.
			if (ticket.ClosedList.Count == 0)
			{
				Node startNode = new Node(ticket.StartIndex);

				/// calculate the distance to target
				startNode.DistanceToTarget = NavMesh.ComputeDistanceToGoal(ticket.GoalIndex, ticket.StartIndex);

				/// Calculate the "F" value
				startNode.F = startNode.DistanceToTarget;

				/// Add the start node to the open list
				ticket.OpenList[ticket.StartIndex] = startNode;

				/// Set the current node to be the start node
				ticket.CurrentNode = startNode;
			}

			//Console.WriteLine(this.GetType().FullName + ".ProcessTicket ticket.CurrentNode.Index=" + ticket.CurrentNode.Index);

			/// If the start node does not have valid neighbors, try to GetNeighbors and add them to the open list
			if (ticket.CurrentNode.Neighbors.Count == 0)
			{
				ticket.CurrentNode.Neighbors = NavMesh.GetNeighbors(ticket.CurrentNode.Index);// ticket->m_current->GetNeighbors();
			}

			foreach(ulong neighborIndex in ticket.CurrentNode.Neighbors)
			{
				/// Check if the goal is one of the neighbors
				if (ticket.GoalIndex == neighborIndex)
				{
					ticket.Path.Add(ticket.GoalIndex);

					Node node = ticket.CurrentNode;
					do
					{
						ticket.Path.Add(node.Index);
						//std::cout << "result " << node << " " << node->m_index << " " << node->m_f << std::endl;
						node = node.Parent;
					}
					while (node != null);

					ticket.State = Ticket.STATE.COMPLETED;
					return true;
				}

				bool addedAlready = false;
				Node neighborNode = null;


				/// check if is in open list
				if (ticket.OpenList.ContainsKey(neighborIndex))
				{
					addedAlready = true;
					neighborNode = ticket.OpenList[neighborIndex];
				}
				else if (ticket.ClosedList.ContainsKey(neighborIndex))
				{
					addedAlready = true;
					neighborNode = ticket.ClosedList[neighborIndex];
				}

				if (!addedAlready)
				{
					neighborNode = new Node(neighborIndex);
					neighborNode.Parent = ticket.CurrentNode;
				}



				/// calculate the distance to target
				neighborNode.DistanceToTarget = NavMesh.ComputeDistanceToGoal(ticket.GoalIndex, neighborIndex);

				/// calculate the cost to travel from startIndex node to the neighbor note
				neighborNode.Cost = NavMesh.ComputeCostToNeighbor(neighborIndex, ticket.CurrentNode.Index);

				/// Calculate the "F" value
				double _f = neighborNode.DistanceToTarget + neighborNode.Cost;

				if (!addedAlready)
				{
					neighborNode.F = _f;

					/// Add the neighbor node to the open list
					ticket.OpenList[neighborIndex] = neighborNode;
				}
				else if (neighborNode.F > _f)
				{
					neighborNode.F = _f;
				}
			}


			/// Chekc if there are some nodes in Open list
			if (ticket.OpenList.Count == 0)
			{
				Node node = ticket.CurrentNode;
				do
				{
					ticket.Path.Add(node.Index);
					
					node = node.Parent;
				}
				while (node != null);

				/// If no nodes -> search is stopped due to no path to goal.
				ticket.State = Ticket.STATE.STOPPED;
				return true;
			}
			



			/// Get the object with the minimal "F"
			double f = Double.MaxValue;
			foreach (ulong index in ticket.OpenList.Keys)
			{
				if (ticket.OpenList[index].F < f)
				{
					f = ticket.OpenList[index].F;
					ticket.CurrentNode = ticket.OpenList[index];
				}
			}


			ticket.OpenList.Remove(ticket.CurrentNode.Index);
			
			ticket.ClosedList[ticket.CurrentNode.Index] = ticket.CurrentNode;

			return false;
		}

		private INavMesh NavMesh { get; set; } = null;
  
    }
}
