namespace CoffeeWalkinator
{
	/// <summary>
	/// Constants used across the project.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Generic error code relating to issues finding a path to a coffee machine
		/// </summary>
		public const int PathError = -1;

		/// <summary>
		/// Visual representations of the various types of locations.  Used for screen printout.
		/// </summary>
		public const string Wall = "X";
		public const string Coffee = "C";
		public const string Desk = "-";
		public const string Origin = "*";
		public const string Unknown = "?";
	}
}
