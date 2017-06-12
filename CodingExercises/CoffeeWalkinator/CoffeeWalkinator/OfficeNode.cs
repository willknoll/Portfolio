using System;
using System.Collections.Generic;

namespace CoffeeWalkinator
{
	/// <summary>
	/// Office Node class, used for describing locations in an office layout grid
	/// </summary>
	public class OfficeNode
	{
		/// <summary>
		/// The original coordinates supplied for this location.  Is NOT 0-based.
		/// </summary>
		public Tuple<int, int> Coordinates { get; private set; }

		/// <summary>
		/// The 0-based row location of this node.
		/// </summary>
		public int MatrixRow { get; private set; }

		/// <summary>
		/// The 0-based column location of this node.
		/// </summary>
		public int MatrixColumn { get; private set; }

		/// <summary>
		/// Specifies if this node is the starting point of the search for coffee.
		/// </summary>
		public bool IsOrigin { get; private set; }

		/// <summary>
		/// Specifies the type of this particular location (e.g. Wall).
		/// </summary>
		public OfficeLocationType LocationType { get; private set; }

		/// <summary>
		/// Tracks the search state of this node, when walking the office.
		/// </summary>
		public OfficeLocationState State { get; set; }

		/// <summary>
		/// Link to the previous neighbor node, when walking the office.
		/// Can be used later to trace back the path walked.
		/// </summary>
		public OfficeNode PreviousNode { get; set; }

		/// <summary>
		/// Calculated distance to this node, from the origin.
		/// </summary>
		public int Distance { get; set; }

		/// <summary>
		/// List of neighbor nodes to potentially visit, when walking the office.
		/// </summary>
		public List<OfficeNode> Neighbors { get; set; }

		/// <summary>
		/// Describes a single location in an office layout. Nodes are originally described in a non-0-based format,
		/// therefore the column and row in the search matrix is maintained separately, and calculated based on 
		/// the original coordinates.
		/// </summary>
		/// <param name="coordinates">The originally supplied Y/X coordinates of a specific location in an office layout.</param>
		/// <param name="locationType">The location type of this node.</param>
		/// <param name="isOrigin">When true, designates this node as the starting point of the search.</param>
		public OfficeNode(Tuple<int, int> coordinates, OfficeLocationType locationType = 0, bool isOrigin = false)
		{
			Coordinates = coordinates;
			MatrixRow = coordinates.Item1 - 1; // Nodes will be placed in a 0-based grid
			MatrixColumn = coordinates.Item2 - 1; // Nodes will be placed in a 0-based grid
			IsOrigin = isOrigin;
			Distance = int.MaxValue;
			LocationType = locationType;
			State = OfficeLocationState.NotVisited; // All nodes are unvisited to start

			Neighbors = new List<OfficeNode>();
		}

		/// <summary>
		/// Override function for equals. Compares coordinates and location type to determine equality.
		/// 
		/// Used in an early rendition of the code to find a specific point in an office layout. Useful for 
		/// expanded functionality in that direction, otherwise consider removing.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the OfficeNodes are equal, otherwise false.</returns>
		public override bool Equals(object obj)
		{
			OfficeNode officeNode = obj as OfficeNode;

			if (officeNode == null)
			{
				return false;
			}

			if (ReferenceEquals(officeNode, this))
			{
				return true;
			}

			return (officeNode.Coordinates.Equals(Coordinates) && officeNode.LocationType.Equals(LocationType));
		}

		/// <summary>
		/// Since overriding Equals, we need to override GetHashCode as well
		/// 
		/// Used in an early rendition of the code to find a specific point in an office layout. Useful for 
		/// expanded functionality in that direction, otherwise consider removing.
		/// </summary>
		/// <returns>HashCode using additional information specific to the OfficeNode.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode() + Coordinates.Item1 + Coordinates.Item2 + (int)LocationType;
		}
	}
}
