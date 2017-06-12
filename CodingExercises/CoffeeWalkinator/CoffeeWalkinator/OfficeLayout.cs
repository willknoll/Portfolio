using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeWalkinator
{
	/// <summary>
	/// Describes the layout of an office, and provides functionality for finding the closest coffee machine.
	/// </summary>
	public class OfficeLayout
	{
		/// <summary>
		/// The origin location (desk) from which the coffee search will begin.
		/// </summary>
		public OfficeNode Origin { get; private set; }

		/// <summary>
		/// Represents the overall map of the office, as known to the program. It contains
		/// all nodes which are specifically added (origin, coffee machines, walls), as well
		/// as any additional nodes which are touched during the search process. (desk nodes are 
		/// only created as needed, since they are not supplied as input)
		/// </summary>
		public OfficeNode[,] OfficeMap { get; private set; }

		/// <summary>
		/// Total columns of the office layout grid (X).
		/// </summary>
		public int Columns { get; set; }

		/// <summary>
		/// Total rows of the office layout grid (Y).
		/// </summary>
		public int Rows { get; set; }

		/// <summary>
		/// Describes the layout of an office, and provides functionality for finding the closest coffee machine.
		/// </summary>
		/// <param name="rows">Total number of rows in the office layout.</param>
		/// <param name="columns">Total number of columns in the office layout.</param>
		/// <param name="origin">The origin location (desk) from which the coffee search will begin.</param>
		/// <param name="coffeeMachines">Coordinate pairs for all coffee machines in the office.</param>
		/// <param name="walls">Coordinate pairs for all walls in the office.</param>
		public OfficeLayout(int rows, int columns, Tuple<int, int> origin, List<Tuple<int, int>> coffeeMachines, List<Tuple<int, int>> walls)
		{
			Rows = rows;
			Columns = columns;
			Origin = new OfficeNode(origin, OfficeLocationType.Desk, true /*isOrigin*/);

			// Initialize matrix to hold nodes
			OfficeMap = new OfficeNode[rows, columns];

			// Set origin desk
			OfficeMap[Origin.MatrixRow, Origin.MatrixColumn] = Origin;

			// Add coffee machines
			foreach (Tuple<int, int> coffeeMachine in coffeeMachines)
			{
				OfficeNode coffeeNode = new OfficeNode(coffeeMachine, OfficeLocationType.Coffee);
				OfficeMap[coffeeNode.MatrixRow, coffeeNode.MatrixColumn] = coffeeNode;
			}

			// Add walls
			foreach (Tuple<int, int> wall in walls)
			{
				OfficeNode wallNode = new OfficeNode(wall, OfficeLocationType.Wall);
				OfficeMap[wallNode.MatrixRow, wallNode.MatrixColumn] = wallNode;
			}
		}

		/// <summary>
		/// Searches an office layout map to find the distance to the nearest accessible coffee machine,
		/// if it exists.
		/// </summary>
		/// <returns>Distance (in matrix squares) from the Origin location to the closest coffee machine.  If no accessible machines
		/// are found, or OfficeMap is not initialized, returns PathError.</returns>
		public int FindCoffeeDistance()
		{
			if (OfficeMap == null)
			{
				return Constants.PathError;
			}

			// Create queue for holding nodes we want to visit
			Queue<OfficeNode> walkingQueue = new Queue<OfficeNode>();

			// Set state of origin to get things started
			Origin.Distance = 0;
			Origin.PreviousNode = null;
			Origin.State = OfficeLocationState.Queued;
			walkingQueue.Enqueue(Origin);

			while (walkingQueue.Count > 0)
			{
				OfficeNode visitNode = walkingQueue.Dequeue();

				if (IsCoffeeMachine(visitNode))
				{
					// TODO: Use this to output the path taken? Could be interesting.
					IEnumerable<OfficeNode> walkedPath = RetracePath(visitNode);
					PrintOptimalPath(walkedPath);

					// We are done, return distance traveled to reach this node
					return visitNode.Distance;
				}

				// Check neighbors to this node, and consider adding to the queue
				foreach (OfficeNode neighborNode in GetNeighbors(visitNode))
				{
					// Don't revisit nodes, and don't bother queuing walls
					if (neighborNode.State == OfficeLocationState.NotVisited && neighborNode.LocationType != OfficeLocationType.Wall)
					{
						neighborNode.State = OfficeLocationState.Queued;
						neighborNode.PreviousNode = visitNode;
						neighborNode.Distance = visitNode.Distance + 1;
						walkingQueue.Enqueue(neighborNode);
					}
				}

				visitNode.State = OfficeLocationState.Visited;
			}
			
			// No accessible coffee machines found
			return Constants.PathError;
		}

		/// <summary>
		/// Compares the current node with the end state
		/// </summary>
		/// <param name="currentNode">Node to inspect</param>
		/// <returns>True if the specified node is a coffee machine (or alternatively, equals the destination)</returns>
		public bool IsCoffeeMachine(OfficeNode currentNode)
		{
			if (currentNode == null)
				return false;

			return currentNode.LocationType == OfficeLocationType.Coffee;
			
			// TODO: remove if no longer using 
			//return currentNode.Equals(Destination);
		}

		/// <summary>
		/// Finds valid neighbors for a node.
		/// Only considers 4 directions (up/down/left/right).  Could be updated to allow for diagonal movement,
		/// however that is currently not supported.
		/// </summary>
		public IEnumerable<OfficeNode> GetNeighbors(OfficeNode currentNode)
		{
			int myRow = currentNode.MatrixRow;
			int myColumn = currentNode.MatrixColumn;

			// Are we within bounds to start?
			if (OutOfBounds(myRow, myColumn))
			{
				return new List<OfficeNode>(0);
			}

			// Maximum of 4 neighbors are possible.  (Diagonal movement would increase to 8)
			List<OfficeNode> neighbors = new List<OfficeNode>(4);

			// Only find immediate neighbors, which fall within the bounds of the matrix
			if (!OutOfBounds(myRow - 1, myColumn)) // up
				neighbors.Add(GetPopulateNode(myRow - 1, myColumn));
			if (!OutOfBounds(myRow, myColumn - 1)) // left
				neighbors.Add(GetPopulateNode(myRow, myColumn - 1));
			if (!OutOfBounds(myRow, myColumn + 1)) // right
				neighbors.Add(GetPopulateNode(myRow, myColumn + 1));
			if (!OutOfBounds(myRow + 1, myColumn)) // down
				neighbors.Add(GetPopulateNode(myRow + 1, myColumn));

			return neighbors;
		}

		/// <summary>
		/// Gets a specific node.
		/// </summary>
		/// <param name="row">Row of the node to retrieve.</param>
		/// <param name="column">Column of the node to retrieve.</param>
		/// <returns>The specified node, if it exists.  Otherwise null.</returns>
		public OfficeNode GetNode(int row, int column)
		{
			if (OutOfBounds(row, column))
			{
				// Throw instead?
				return null;
			}

			return OfficeMap[row, column];
		}

		/// <summary>
		/// Gets the specified node, and creates one if it doesn't exist.
		/// If the specified node does not exist, and is a valid location, a new node is created
		/// for the specified coordinates, and is added to the map as a desk.
		/// Since desk locations are implied, not supplied, this provides a lazy addition of desk locations.
		/// </summary>
		/// <param name="row">Row of the node to retrieve.</param>
		/// <param name="column">Column of the node to retrieve.</param>
		/// <returns>The specified node.  If it does not exist, it is created.</returns>
		public OfficeNode GetPopulateNode(int row, int column)
		{
			if (OutOfBounds(row, column))
			{
				// Throw instead?
				return null;
			}

			if (OfficeMap[row, column] != null)
				return OfficeMap[row, column];

			// Since we are not supplied the location of all the desks to start, create a desk at any empty location
			OfficeNode tempNode = new OfficeNode(new Tuple<int, int>(row + 1, column + 1), OfficeLocationType.Desk);
			OfficeMap[row, column] = tempNode;
			return tempNode;
		}

		/// <summary>
		/// Determines if a specified location is outside the bounds of the matrix
		/// </summary>
		/// <param name="row">Row for a node location</param>
		/// <param name="column">Column for a node location</param>
		/// <returns>True if the location is valid, otherwise false.</returns>
		private bool OutOfBounds(int row, int column)
		{
			return (row < 0 || row > Rows - 1 || column < 0 || column > Columns - 1);
		}

		/// <summary>
		/// Retraces the solution path, to get the visited nodes which lead directly to the destination.
		/// Used for "could be interesting to know" purposes only.
		/// </summary>
		private IEnumerable<OfficeNode> RetracePath(OfficeNode endNode)
		{
			OfficeNode currentNode = endNode;
			ICollection<OfficeNode> path = new List<OfficeNode>();
			while (currentNode != null)
			{
				path.Add(GetNode(currentNode.MatrixRow, currentNode.MatrixColumn));
				currentNode = currentNode.PreviousNode;
			}
			return path;
		}

		/// <summary>
		/// No frills visualization of the chosen path to coffee.
		/// </summary>
		/// <param name="walkedPath">Enumerable of nodes on the path taken.</param>
		private void PrintOptimalPath(IEnumerable<OfficeNode> walkedPath)
		{
			List<OfficeNode> nodes = walkedPath.ToList();
			nodes.Reverse();
			StringBuilder sb = new StringBuilder();
			foreach (OfficeNode node in nodes)
			{
				sb.AppendFormat("({0},{1}) -> ", node.Coordinates.Item1, node.Coordinates.Item2);
			}

			sb.Append("Careful Hot!");

			Console.WriteLine($"Optimal path: {sb}");		}
	}
}
