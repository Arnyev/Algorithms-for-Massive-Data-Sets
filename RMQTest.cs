using System;
using System.Linq;

namespace Algs
{
	class RMQTest
	{
		public static void BasicTest()
		{
			int[] arr = new[] { 0, 34, 65, 85, 23, 434, 43, 26, 786, 563, 2, 4, 46, 47, 74, 3, 13, 45, 85 };

			RMQHelper helper = new RMQHelper(arr);
			int expected = arr.Skip(2).Take(5).Min();
			int actual = helper.GetMinimum(2, 6).Value;

			if (expected != actual)
				throw new Exception("Basic test failed");
		}

		public static void LongGetBoundedTest(int maxTestArrayLength = 10000000, int testArraysCount = 2, int testsPerArray = 5)
		{
			Random rand = new Random();

			for (int testArrayNr = 0; testArrayNr < testArraysCount; testArrayNr++)
			{
				int testArrayLength = rand.Next() % (maxTestArrayLength - 10) + 10;
				int[] testArray = new int[testArrayLength];
				for (int j = 1; j < testArrayLength; j++)
					testArray[j] = rand.Next();

				RMQHelper finder = new RMQHelper(testArray);

				for (int test = 0; test < testsPerArray; test++)
				{
					int i = rand.Next() % (testArrayLength - 1);
					int count = rand.Next() % (testArrayLength - i - 1) + 1;
					int j = i + count - 1;
					var x = rand.Next();

					var expected = testArray.Skip(i).Take(count).Where(v => v <= x).OrderBy(v => v).ToArray();
					var actual = finder.GetBounded(i, j, x).OrderBy(v => v).ToArray();
					var result = expected.SequenceEqual(actual);

					if (!result)
						throw new Exception("Long test failed");
				}
			}
		}

		public static void LongTest(int maxTestArrayLength = 10000000, int testArraysCount = 2, int testsPerArray = 5)
		{
			Random rand = new Random();

			for (int testArrayNr = 0; testArrayNr < testArraysCount; testArrayNr++)
			{
				int testArrayLength = rand.Next() % (maxTestArrayLength - 10) + 10;
				int[] testArray = new int[testArrayLength];
				for (int j = 1; j < testArrayLength; j++)
					testArray[j] = rand.Next();

				RMQHelper finder = new RMQHelper(testArray);

				for (int test = 0; test < testsPerArray; test++)
				{
					int i = rand.Next() % (testArrayLength - 1);
					int count = rand.Next() % (testArrayLength - i - 1) + 1;
					int j = i + count - 1;

					int expected = testArray.Skip(i).Take(count).Min();
					int actual = finder.GetMinimum(i, j).Value;

					if (actual != expected)
						throw new Exception("Long test failed");
				}
			}
		}
	}
}
