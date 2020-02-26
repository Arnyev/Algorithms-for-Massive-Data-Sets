using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Algs
{
	class RMQPM1Test
	{
		public static void BasicTest()
		{
			int[] arr = new[] { 0, 34, 65, 85, 23, 434, 43, 26, 786, 563, 2, 4, 46, 47, 74, 3, 13, 45, 85 };
			arr = arr.Zip(arr.Skip(1)).Select(t => t.First > t.Second ? -1 : 1).ToArray();
			for (int i = 1; i < arr.Length; i++)
				arr[i] = arr[i] + arr[i - 1];

			RMQPM1Helper helper = new RMQPM1Helper(arr);
			int expected = arr.Skip(2).Take(5).Min();
			int actual = helper.GetMinimum(2, 6).Value;

			if (expected != actual)
				throw new Exception("Basic test failed");
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

				testArray = testArray.Zip(testArray.Skip(1)).Select(t => t.First > t.Second ? -1 : 1).ToArray();
				for (int i = 1; i < testArray.Length; i++)
					testArray[i] = testArray[i] + testArray[i - 1];

				RMQPM1Helper finder = new RMQPM1Helper(testArray);

				for (int test = 0; test < testsPerArray; test++)
				{
					int i = rand.Next() % (testArrayLength - 1);
					int count = rand.Next() % (testArrayLength - i - 1) + 1;
					int j = i + count - 1;

					int expected = testArray.Skip(i).Take(count).Min();
					int actual = finder.GetMinimum(i, j).Value;
					var actualByIndex = testArray[finder.GetMinimum(i, j).Index];

					if (actual != expected || actualByIndex != expected)
						throw new Exception("Long test failed");
				}
			}
		}
	}
}
