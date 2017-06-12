using System;

namespace TextJustifier
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Text Justifier");
			Console.WriteLine("Will Knoll");
			Console.WriteLine("---------------------------------------\r\n\r\n");

			RunTests();

			Console.WriteLine("\r\n\r\nPress any key to exit...");
			Console.ReadKey();
		}

		/// <summary>
		/// Takes an input string, and justifies it (margin to margin) to a specified line length.
		/// 
		/// Justification is based on monospace, and does not take into account varied font widths.
		/// Note that "extra spaces" are added from left to right.
		/// 
		/// Example:
		/// "my justified text", 20
		/// 
		/// Output:
		/// "my   justified  text"
		/// </summary>
		/// <param name="textToJustify">Text to be justified</param>
		/// <param name="lineLength">Number of characters to consider for a full length line</param>
		/// <returns>The original string, with new spacing to satisfy justification to the line length.</returns>
		private static string Justify(string textToJustify, int lineLength)
		{
			// If the line length is less than the provided text length, this should probably
			// throw an exception, or implement some other business rule for wrapping.
			// However, for these purposes, we'll just return the original string.
			if (lineLength < textToJustify.Length)
			{
				return textToJustify;
			}

			// Get list of words. Any punctuation will be kept together with its nearest word, 
			// and counted as part of it.
			string[] wordList = textToJustify.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

			int totalWords = wordList.Length;

			// Nothing to justify, if there is only one word
			if (totalWords < 2)
			{
				return textToJustify;
			}

			// Find total number of non-space characters which need to be accommodated on the line.
			// FUTURE:  Could use regex to find number of non-space characters using original string.
			int numWordCharacters = 0;
			foreach (string word in wordList)
			{
				numWordCharacters += word.Length;
			}

			// Determine number of characters on the line which will need to be filled with spaces.
			int numSpaceCharacters = lineLength - numWordCharacters;

			// Determine minimum number of spaces between words.
			// Note: there will not be any spaces added after the last word, so it is excluded.
			int spacesAfterEachWord = numSpaceCharacters / (totalWords - 1);

			// Depending on the string, it is likely that spacing will not be spread exactly evenly,
			// so determine if there are extra spaces which will need to be added along the way.
			int extraSpaces = numSpaceCharacters % (totalWords - 1);

			// Add spaces back to all the words.  				
			// Since we are using monospace on all this, just add extra spaces from left to right.
			// This section could be modified to accomodate different "extra space" rules.
			for (int i = 0; i < totalWords - 1; i++)
			{
				if (extraSpaces > 0)
				{
					wordList[i] = wordList[i] + " ";
					extraSpaces--;
				}

				wordList[i] = wordList[i].PadRight(wordList[i].Length + spacesAfterEachWord, ' ');
			}

			return string.Join("", wordList);
		}

		/// <summary>
		/// Runs several tests against the Justify function.
		/// </summary>
		private static void RunTests()
		{
			string testString;
			string expectedString;
			string resultString;
			// Note: Changing this value will invalidate most tests, unless the expectedString values are also updated!
			int lineLength = 52;

			// Test with 1 word
			testString = "ASingleWord";
			expectedString = "ASingleWord";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 01", testString, resultString, expectedString, 11);

			// Test with 2 words
			testString = "my string";
			expectedString = "my                                            string";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 02", testString, resultString, expectedString, lineLength);

			// Test with 3 words
			testString = "my second string";
			expectedString = "my                   second                   string";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 03", testString, resultString, expectedString, lineLength);

			// Test with 4 words
			testString = "my second string again";
			expectedString = "my           second           string           again";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 04", testString, resultString, expectedString, lineLength);

			// Test with a string which is already as long as the line length
			testString = "This string matches the input line length exactly!!!";
			expectedString = "This string matches the input line length exactly!!!";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 05", testString, resultString, expectedString, 52);

			// Test with a string which exceeds the maximum line length
			testString = "This string exceeds the input line length by 10 characters!!!!";
			expectedString = "This string exceeds the input line length by 10 characters!!!!";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 06", testString, resultString, expectedString, 62);

			// Test with a string which begins oddly spaced
			testString = "This   string has       weird   spacing  to start.";
			expectedString = "This    string   has   weird   spacing   to   start.";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 07", testString, resultString, expectedString, lineLength);

			// Wiseguy test
			testString = "          ";
			expectedString = "          ";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 08", testString, resultString, expectedString, 10);

			// Test the originally supplied string
			testString = "The quick brown fox jumps over the lazy dog.";
			expectedString = "The  quick  brown  fox  jumps  over  the  lazy  dog.";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 09", testString, resultString, expectedString, lineLength);

			// Test the originally supplied string, with 1 extra character
			testString = "The quick brown fox jumps over the lazy dogs.";
			expectedString = "The  quick  brown  fox  jumps  over  the  lazy dogs.";
			resultString = Justify(testString, lineLength);
			WriteResult("Test 10", testString, resultString, expectedString, lineLength);
		}

		/// <summary>
		/// Formats a test result, and outputs it to the console.
		/// </summary>
		/// <param name="testName">Name of the test being run</param>
		/// <param name="testString">Original string which was tested</param>
		/// <param name="resultString">Actual string returned from the function</param>
		/// <param name="expectedString">String which was expected to be returned</param>
		/// <param name="lineLength">Number of characters expected in the result string</param>
		private static void WriteResult(string testName, string testString, string resultString, string expectedString, int lineLength)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"{testName}\r\n----------------------------------------");
			Console.ResetColor();
			Console.WriteLine(testString);
			Console.WriteLine(resultString);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("1234567890123456789012345678901234567890123456789012345678901234567890\r\n");
			Console.ResetColor();

			Console.WriteLine($"Expected Length: {lineLength}  Actual Length: {resultString.Length}");

			string spacingMatch = "matches";
			bool pass = true;

			if (!resultString.Equals(expectedString))
			{
				spacingMatch = "DOES NOT match";
				pass = false;
			}

			Console.WriteLine($"Result string {spacingMatch} expected spacing.");

			if (resultString.Length != lineLength)
			{
				pass = false;
			}

			Console.ForegroundColor = (pass) ? ConsoleColor.Green : ConsoleColor.Red;

			Console.WriteLine(pass ? "PASS" : "FAIL");
			Console.WriteLine("\r\n");
			Console.ResetColor();
		}
	}
}
