using System;
using System.Diagnostics;
using System.Linq;

namespace Algs
{
	class SortedSubrangeFinderTests
	{
		public static void BasicTest()
		{
			int[] ar = new[] { 1, 45, 74, 23, 5, 36, 857, 85, 34, 8, 06, 25, 32 };
			SortedSubrangeFinder finder = new SortedSubrangeFinder(ar);

			int[] expected = ar.Skip(3).Take(6).OrderBy(x => x).ToArray();
			int[] actual = finder.GetSortedSubrange(3, 8);
			bool result = expected.SequenceEqual(actual);
			if (!result)
				throw new Exception("Basic test failed");
		}

		public static void LongTest(int maxTestArrayLength = 10000000, int testArraysCount = 2, int testsPerArray = 5, bool writeTime = false)
		{
			Random rand = new Random();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			for (int testArrayNr = 0; testArrayNr < testArraysCount; testArrayNr++)
			{
				int testArrayLength = rand.Next() % (maxTestArrayLength - 1) + 1;
				int[] testArray = new int[testArrayLength];
				for (int j = 0; j < testArrayLength; j++)
					testArray[j] = rand.Next();

				SortedSubrangeFinder finder = new SortedSubrangeFinder(testArray);

				for (int test = 0; test < testsPerArray; test++)
				{
					int i = rand.Next() % testArrayLength;
					int count = rand.Next() % (testArrayLength - i);
					int j = i + count - 1;

					long mslinqStart = sw.ElapsedMilliseconds;
					int[] expected = testArray.Skip(i).Take(count).OrderBy(x => x).ToArray();
					long msStart = sw.ElapsedMilliseconds;
					long mslinq = msStart - mslinqStart;
					int[] actual = finder.GetSortedSubrange(i, j);
					long msEnd = sw.ElapsedMilliseconds;
					long taken = msEnd - msStart;

					if (writeTime)
						Console.WriteLine($"Items reported {count}, Array size {testArrayLength}, ms taken {taken}, ms taken linq {mslinq}");

					bool result = expected.SequenceEqual(actual);
					if (!result)
						throw new Exception("Long test failed");
				}
			}
		}
	}
}
