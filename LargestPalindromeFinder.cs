using System;
using System.Linq;

namespace Algs
{
	class LargestPalindromeFinder
	{
		public static string FindLargestPalindrome(string s)
		{
			if (s.Length == 0)
				return string.Empty;
			if (s.Length == 1)
				return s[0].ToString();

			var rs = new string(s.Reverse().ToArray());
			var ns = s + '\0' + rs;
			var suffixTree = SuffixTreeNode.Build(ns);
			var Earray = new int[(ns.Length + 2) * 4 - 1];
			var Aarray = new int[(ns.Length + 2) * 4 - 1];
			var Rarray = new int[ns.Length + 1];
			suffixTree.WriteToArrays(Earray, 0, Aarray, Rarray);
			var rmqHelper = new RMQPM1Helper(Aarray);

			var lenMaxPalindrome = 0;
			var midPalindrome = -1;

			for (int i = 0; i < s.Length; i++)
			{
				var indL = Rarray[i + 1];
				var indR = Rarray[ns.Length - i];
				if (indL > indR)
				{
					int tmp = indL;
					indL = indR;
					indR = tmp;
				}

				int indexInE = rmqHelper.GetMinimum(indL, indR).Index;
				var palindromeLength = Earray[indexInE];

				if (palindromeLength > lenMaxPalindrome)
				{
					lenMaxPalindrome = palindromeLength;
					midPalindrome = i + 1;
				}
			}

			var startPalindrome = midPalindrome - lenMaxPalindrome;
			var endPalindrome = startPalindrome + 2 * lenMaxPalindrome - 2;
			var lenPalindrome = endPalindrome - startPalindrome + 1;
			var r = s.Substring(startPalindrome, lenPalindrome);
			return r;
		}
	}
}
