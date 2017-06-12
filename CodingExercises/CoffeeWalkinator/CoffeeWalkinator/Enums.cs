/// <summary>
/// Enums used across the project.  Adding separately to reduce nesting elsewhere.
/// </summary>
namespace CoffeeWalkinator
{
	/// <summary>
	/// Location types for an office node.
	/// </summary>
	public enum OfficeLocationType : int
	{
		Desk = 0,
		Coffee = 1,
		Wall = 2
	}

	/// <summary>
	/// Search states for an office node.
	/// </summary>
	public enum OfficeLocationState : int
	{
		NotVisited = 0,
		Visited = 1,
		Queued = 2
	}
}
