using System;
using System.Text.RegularExpressions;

namespace PalindromeChecker
{
	class Program
	{
		static void Main()
		{
			Check("abcba", true);
			Check("abcde", false);
			Check("Mr owl ate my metal worm", true);
			Check("Never Odd Or Even", true);
			Check("Never Even Or Odd", false);

			Console.ReadKey();
		}

		/// <summary>
		/// Helper method for testing IsPalindrome
		/// </summary>
		/// <param name="s">String to test</param>
		/// <param name="shouldBePalindrome">Expected result</param>
		static void Check(string s, bool shouldBePalindrome)
		{
			Console.WriteLine(IsPalindrome(s) == shouldBePalindrome ? "pass" : "FAIL");
		}

		/// <summary>
		/// Inspects a string to see if it is a valid palindrome, ignoring whitespace and capitalization.
		/// Implementation does not take culture into consideration.
		/// </summary>
		/// <param name="s">Potential palindrome string.</param>
		/// <returns>True if the input string is a valid palindrome, otherwise false.</returns>
		static bool IsPalindrome(string s)
		{
			// Remove whitespace to start. 
			// Trade off:  Allocates new string, but reduces overall algorithm complexity.
			///[^ a - z0 - 9 +] +/ gi, '+'
			//var condensed = Regex.Replace(s, @"\s", "");
			var condensed = Regex.Replace(s, @"(?i)[^a-z0-9+]", "");

			var length = condensed.Length;
			var midpoint = length/2;

			// Inspect string contents from both ends while moving through the loop.
			// Since working from both ends, there is no need to continue once the midpoint of the 
			// string is reached. Note that the midpoint element isn't actually inspected
			// for strings with an odd-numbered length; the indexes are the same, thus
			// the position is automatically known to be equal.
			for (int i = 0, j = length - 1; i < midpoint; i++, j--)
			{
				// Uncomment to see comparisons
				//Console.WriteLine($"{condensed[i]} - {condensed[j]}");

				if (char.ToUpper(condensed[i]) != char.ToUpper(condensed[j]))
				{
					// Exit early, if a non-match is encountered.
					return false;
				}
			}

			return true;
		}
	}
}
