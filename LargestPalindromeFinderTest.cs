using System;
using System.Collections.Generic;
using System.Text;

namespace Algs
{
	class LargestPalindromeFinderTest
	{
		public static void BasicTest()
		{
			var str1 = "ABCDEFCBA";
			var result = LargestPalindromeFinder.FindLargestPalindrome(str1);
			if (result.Length > 1)
				throw new Exception("Basic test failed");

			var str2 = "acdabacdc";
			if (LargestPalindromeFinder.FindLargestPalindrome(str2) != "aba")
				throw new Exception("Basic test failed");
		}

		public static void LongTest(int maxStringLength = 300, int diffCharCount = 5, int testsPerLength = 200)
		{
			char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			Random rand = new Random();

			for (int testStringLenNr = 0; testStringLenNr < testsPerLength; testStringLenNr++)
			{
				int testStringLength = rand.Next() % (maxStringLength + 1);

				for (int test = 0; test < testsPerLength; test++)
				{
					char[] testStringAr = new char[testStringLength];
					for (int j = 0; j < testStringLength; j++)
						testStringAr[j] = alpha[rand.Next() % diffCharCount];

					var testString = new string(testStringAr);
					var palindrome = LargestPalindromeFinder.FindLargestPalindrome(testString);

					var expectedPalindrome = FindLargestPalindrome(testString);
					var l1 = palindrome.Length;
					var l2 = expectedPalindrome.Length;
					if (palindrome != expectedPalindrome)
						throw new Exception("Long test failed");
				}
			}
		}

		private static string FindLargestPalindrome(string s)
		{
			if (s.Length == 0)
				return string.Empty;

			int maxLen = -1;
			int indexMidPalindrome = -1;
			for (int mid = 0; mid < s.Length; mid++)
			{
				int curPalindrome = 0;
				int maxLeft = mid;
				int maxRight = s.Length - mid - 1;
				int maxSide = Math.Min(maxLeft, maxRight);
				for (; curPalindrome <= maxSide; curPalindrome++)
					if (s[mid - curPalindrome] != s[mid + curPalindrome])
					{
						curPalindrome--;
						break;
					}

				if (curPalindrome > maxSide)
					curPalindrome = maxSide;

				if (curPalindrome > maxLen)
				{
					maxLen = curPalindrome;
					indexMidPalindrome = mid;
				}
			}

			return s.Substring(indexMidPalindrome - maxLen, maxLen * 2 + 1);
		}
	}
}
