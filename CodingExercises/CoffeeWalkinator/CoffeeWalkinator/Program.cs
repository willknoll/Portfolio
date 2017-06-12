using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeWalkinator
{
	/// <author>Will Knoll</author>
	/// <summary>
	/// Determines how far it is from a desk to the closest accessible coffee machine, in a specified office layout. 
	/// Offices are laid out in a grid where every cell is either a wall, which is impassible, or a desk (employees 
	/// can walk through other employees desks to get to a coffee machine).
	///
	/// Only wall and coffee machine locations are specified for the grid, thus any other space is assumed to be a desk.
	/// An office can have multiple coffee machines, each of which may be forward, back, or even adjacent to the specified desk.
	/// Must return whichever machine is closest to DeskLocation, provided it can be reached without passing through a wall.
	///
	/// Origin (Desk) = *
	/// Desk = -
	/// Wall = X
	/// Coffee = C
	/// Unprocessed location = ?
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			TestOfficeNode();
			TestDistanceToCoffee();

			Console.WriteLine("\r\n\r\nPress <enter> to exit...");
			Console.ReadLine();
		}

		/// <summary>
		/// Retrieves the number of steps required to reach the closest coffee machine, without passing through a wall.
		/// </summary>
		/// <param name="numRows">Represents the number of rows (Y) in the office layout grid.</param>
		/// <param name="numColumns">Represents the number of columns (X) in the office layout grid.</param>
		/// <param name="DeskLocation">Coordinates of the desired user's desk. (Y, X)</param>
		/// <param name="coffeeLocations">List of coordinates which are designated as coffee machines. (Y, X)</param>
		/// <param name="walls">List of coordinates which are designated as walls. (Y, X)</param>
		/// <returns>Integer representing the number of steps required to reach the closest accessible coffee machine, from the specified DeskLocation.</returns>
		public static int DistanceToCoffee(int numRows, int numColumns, Tuple<int, int> DeskLocation, 
			List<Tuple<int, int>> coffeeLocations, List<Tuple<int, int>> walls)
		{
			// A little basic input validation, not intended to be exhaustive for this implementation.
			// Would possibly throw exceptions, but for these purposes, just return PathError.
			if ((numRows <= 0) || (numColumns <= 0) || (coffeeLocations.Count < 1))
			{
				return Constants.PathError;
			}

			int distanceToCoffee = 0;

			Console.WriteLine("\r\nFinding nearest coffee machine...");

			OfficeLayout layout = new OfficeLayout(numRows, numColumns, DeskLocation, coffeeLocations, walls);
			distanceToCoffee = layout.FindCoffeeDistance();

			PrintOfficeLayoutGrid(layout.OfficeMap);

			return distanceToCoffee;
		}

		/// <summary>
		/// No frills visualization of what the grid looks like in memory (more or less).  Depending on search path,
		/// not all desk nodes will be populated, as desk nodes are only created as necessary.
		/// </summary>
		/// <param name="officeLayout">The grid of office layout nodes to print.</param>
		public static void PrintOfficeLayoutGrid(OfficeNode[,] officeLayout)
		{
			if (officeLayout == null)
				return;

			int rows = officeLayout.GetLength(0);
			int columns = officeLayout.GetLength(1);

			Console.WriteLine("Discovered state of this office layout:");
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < rows; i++)
			{
				sb.Clear();
				sb.Append("\t");
				for (int j = 0; j < columns; j++)
				{
					if (officeLayout[i, j] != null)
					{
						switch (officeLayout[i, j].LocationType)
						{
							case OfficeLocationType.Desk:
								sb.Append($"{(officeLayout[i, j].IsOrigin ? Constants.Origin : Constants.Desk)} ");
								break;
							case OfficeLocationType.Coffee:
								sb.Append($"{Constants.Coffee} ");
								break;
							case OfficeLocationType.Wall:
								sb.Append($"{Constants.Wall} ");
								break;
							default:
								break;
						}
					}
					else
					{
						sb.Append($"{Constants.Unknown} ");
					}
				}
				Console.WriteLine(sb);
			}
		}

		#region TestCode
		// This would normally be run as a separate actual test project, but including some simple tests here to ensure things are working.
		// Not exhaustive by any means, but enough to cover a few bases.

		/// <summary>
		/// Runs various tests against the DistanceToCoffee functionality
		/// </summary>
		public static void TestDistanceToCoffee()
		{
			List<Tuple<int, int>> BadCoffeeLocations = new List<Tuple<int, int>>();

			/// Sample grids used for testing
			/// 
			///    A          B           C          D         E          F            G
			/// - - C -    - X C -    - - C -    - - - -    - - - -     X X X     - - - - - - 
			/// - X X -    - X X -    - X X -    - X X -    - - - C     - X C     - - - - C -
			/// X C - -    X C - -    - C - -    X C - -    - - - -     X X X     - - - - - - 
			///                                                                   - C - - - -

			#region TestDataA
			var DeskLocationALeft = new Tuple<int, int>(2, 1);
			var DeskLocationARight = new Tuple<int, int>(3, 4);
			List<Tuple<int, int>> CoffeeLocationsA = new List<Tuple<int, int>>(2)
			{
				new Tuple<int, int>(1, 3),
				new Tuple<int, int>(3, 2),
			};
			List<Tuple<int, int>> WallLocationsA = new List<Tuple<int, int>>(3)
			{
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(2, 3),
				new Tuple<int, int>(3, 1),
			};
			#endregion

			#region TestDataB
			var DeskLocationB = new Tuple<int, int>(2, 1);
			List<Tuple<int, int>> CoffeeLocationsB = new List<Tuple<int, int>>(2)
			{
				new Tuple<int, int>(1, 3),
				new Tuple<int, int>(3, 2),
			};
			List<Tuple<int, int>> WallLocationsB = new List<Tuple<int, int>>(4)
			{
				new Tuple<int, int>(1, 2),
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(2, 3),
				new Tuple<int, int>(3, 1),
			};
			#endregion

			#region TestDataC
			var DeskLocationC = new Tuple<int, int>(2, 1);
			List<Tuple<int, int>> CoffeeLocationsC = new List<Tuple<int, int>>(2)
			{
				new Tuple<int, int>(1, 3),
				new Tuple<int, int>(3, 2),
			};
			List<Tuple<int, int>> WallLocationsC = new List<Tuple<int, int>>(2)
			{
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(2, 3),
			};
			#endregion

			#region TestDataD
			var DeskLocationD = new Tuple<int, int>(2, 1);
			List<Tuple<int, int>> CoffeeLocationsD = new List<Tuple<int, int>>(1)
			{
				new Tuple<int, int>(3, 2),
			};
			List<Tuple<int, int>> WallLocationsD = new List<Tuple<int, int>>(3)
			{
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(2, 3),
				new Tuple<int, int>(3, 1),
			};
			#endregion

			#region TestDataE
			var DeskLocationE = new Tuple<int, int>(2, 1);
			List<Tuple<int, int>> CoffeeLocationsE = new List<Tuple<int, int>>(1)
			{
				new Tuple<int, int>(2, 4),
			};
			List<Tuple<int, int>> WallLocationsE = new List<Tuple<int, int>>(0);
			#endregion

			#region TestDataF
			var DeskLocationF = new Tuple<int, int>(2, 1);
			List<Tuple<int, int>> CoffeeLocationsF = new List<Tuple<int, int>>(1)
			{
				new Tuple<int, int>(3, 2),
			};
			List<Tuple<int, int>> WallLocationsF = new List<Tuple<int, int>>(7)
			{
				new Tuple<int, int>(1, 1),
				new Tuple<int, int>(1, 2),
				new Tuple<int, int>(1, 3),
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(3, 1),
				new Tuple<int, int>(3, 2),
				new Tuple<int, int>(3, 3),
			};
			#endregion

			#region TestDataG
			var DeskLocationGLeft = new Tuple<int, int>(2, 1);
			var DeskLocationGRight = new Tuple<int, int>(3, 6);
			List<Tuple<int, int>> CoffeeLocationsG = new List<Tuple<int, int>>(2)
			{
				new Tuple<int, int>(2, 5),
				new Tuple<int, int>(4, 2),
			};
			List<Tuple<int, int>> WallLocationsG = new List<Tuple<int, int>>(0);
			#endregion

			int result = DistanceToCoffee(0, 4, DeskLocationALeft, CoffeeLocationsA, WallLocationsA);
			PrintTestResult("Invalid minimum rows fails", result == Constants.PathError, Constants.PathError.ToString(), result.ToString());

			result = DistanceToCoffee(3, 0, DeskLocationALeft, CoffeeLocationsA, WallLocationsA);
			PrintTestResult("Invalid minimum columns fails", result == Constants.PathError, Constants.PathError.ToString(), result.ToString());

			result = DistanceToCoffee(3, 4, DeskLocationALeft, BadCoffeeLocations, WallLocationsA);
			PrintTestResult("Invalid minimum coffee locations fails", result == Constants.PathError, Constants.PathError.ToString(), result.ToString());

			// Test the A grid
			result = DistanceToCoffee(3, 4, DeskLocationALeft, CoffeeLocationsA, WallLocationsA);
			PrintTestResult("Grid A - Left Desk - Correct path found", result == 3, "3", result.ToString());

			result = DistanceToCoffee(3, 4, DeskLocationARight, CoffeeLocationsA, WallLocationsA);
			PrintTestResult("Grid A - Right Desk - Correct path found", result == 2, "2", result.ToString());

			// Test the B grid
			result = DistanceToCoffee(3, 4, DeskLocationB, CoffeeLocationsB, WallLocationsB);
			PrintTestResult("Grid B - No path found", result == Constants.PathError, Constants.PathError.ToString(), result.ToString());

			// Test the C grid
			result = DistanceToCoffee(3, 4, DeskLocationC, CoffeeLocationsC, WallLocationsC);
			PrintTestResult("Grid C - Correct path found", result == 2, "2", result.ToString());

			// Test the D grid
			result = DistanceToCoffee(3, 4, DeskLocationD, CoffeeLocationsD, WallLocationsD);
			PrintTestResult("Grid D - Correct path found", result == 8, "8", result.ToString());

			// Test the E grid
			result = DistanceToCoffee(3, 4, DeskLocationE, CoffeeLocationsE, WallLocationsE);
			PrintTestResult("Grid E - Correct path found", result == 3, "3", result.ToString());

			// Test the F grid
			result = DistanceToCoffee(3, 3, DeskLocationF, CoffeeLocationsF, WallLocationsF);
			PrintTestResult("Grid F - No path found", result == Constants.PathError, Constants.PathError.ToString(), result.ToString());

			// Test the G grid
			result = DistanceToCoffee(4, 6, DeskLocationGLeft, CoffeeLocationsG, WallLocationsG);
			PrintTestResult("Grid G - Left Desk - Correct path found", result == 3, "3", result.ToString());

			result = DistanceToCoffee(4, 6, DeskLocationGRight, CoffeeLocationsG, WallLocationsG);
			PrintTestResult("Grid G - Right Desk - Correct path found", result == 2, "2", result.ToString());
		}

		/// <summary>
		/// Tests that a few basic pieces of OfficeNodes work correctly.
		/// </summary>
		public static void TestOfficeNode()
		{
			bool result = false;

			var nodeLocation = new Tuple<int, int>(2, 1);
			OfficeNode officeNode = new OfficeNode(nodeLocation);

			result = officeNode.LocationType == OfficeLocationType.Desk;
			PrintTestResult("Desk node correctly set by default.", result, OfficeLocationType.Desk.ToString(), officeNode.LocationType.ToString());

			result = officeNode.MatrixRow == nodeLocation.Item1 - 1;
			PrintTestResult("Matrix row matches expectation.", result, (nodeLocation.Item1 - 1).ToString(), officeNode.MatrixRow.ToString());

			result = officeNode.MatrixColumn == nodeLocation.Item2 - 1;
			PrintTestResult("Matrix column matches expectation.", result, (nodeLocation.Item2 - 1).ToString(), officeNode.MatrixColumn.ToString());

			result = officeNode.IsOrigin == false;
			PrintTestResult("Node is not set as origin by default.", result, bool.FalseString, officeNode.IsOrigin.ToString());

			result = officeNode.State == OfficeLocationState.NotVisited;
			PrintTestResult("Node state is NotVisited by default.", result, OfficeLocationState.NotVisited.ToString(), officeNode.State.ToString());

			officeNode = new OfficeNode(nodeLocation, OfficeLocationType.Coffee, true);

			result = officeNode.LocationType == OfficeLocationType.Coffee;
			PrintTestResult("Location type is correctly set.", result, OfficeLocationType.Coffee.ToString(), officeNode.LocationType.ToString());

			result = officeNode.IsOrigin == true;
			PrintTestResult("Origin node is correctly set.", result, bool.TrueString, officeNode.IsOrigin.ToString());
		}

		/// <summary>
		/// Single entry point for printing a test result.
		/// </summary>
		/// <param name="testName">Name of the test which was run.</param>
		/// <param name="testPassed">Test pass or failure.</param>
		/// <param name="expected">Expected result of test.</param>
		/// <param name="actual">Actual result of test.</param>
		public static void PrintTestResult(string testName, bool testPassed, string expected, string actual)
		{
			string passFail = (testPassed) ? "PASS" : "FAIL";
			Console.WriteLine($"{passFail} - Test '{testName}': Expected:  ({expected})  Actual:  ({actual})");
		}

		#endregion
	}
}
