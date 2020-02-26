using System;
using System.Linq;

namespace Algs
{
	class SortedSubrangeFinder
	{
		int[] inputArray;
		int[][][] subArrays;
		int[][][] indexArrays;

		public SortedSubrangeFinder(int[] inputArray)
		{
			this.inputArray = inputArray;
			int length = inputArray.Length;
			int log = (int)Math.Ceiling(Math.Log(length, 2));

			subArrays = new int[log + 1][][];
			indexArrays = new int[log + 1][][];
			subArrays[0] = new int[1][];
			indexArrays[0] = new int[1][];
			subArrays[0][0] = inputArray.ToArray();
			indexArrays[0][0] = Enumerable.Range(0, length).ToArray();
			Array.Sort(subArrays[0][0], indexArrays[0][0]);

			for (int i = 1; i < log + 1; i++)
			{
				int maxSubArraySize = 1 << log - i;
				int count = (length + maxSubArraySize - 1) / maxSubArraySize;
				subArrays[i] = new int[count][];
				indexArrays[i] = new int[count][];

				for (int j = 0; j < count; j++)
				{
					int firstIndex = maxSubArraySize * j;
					int lastIndex = Math.Min(length, maxSubArraySize * (j + 1));
					int subArrayLength = lastIndex - firstIndex;
					subArrays[i][j] = new int[subArrayLength];
					indexArrays[i][j] = Enumerable.Range(firstIndex, subArrayLength).ToArray();

					Array.Copy(inputArray, firstIndex, subArrays[i][j], 0, subArrayLength);
					Array.Sort(subArrays[i][j], indexArrays[i][j]);
				}
			}
		}

		public int[] GetSortedSubrange(int i, int j)
		{
			if (i < 0 || i > inputArray.Length - 1 || j > inputArray.Length - 1)
				throw new ArgumentException("Wrong indices.");

			if (j < i)
				return Array.Empty<int>();

			int count = j - i + 1;
			int log = (int)Math.Ceiling(Math.Log(count, 2));

			if (log == subArrays.Length)
				return FilterByIndex(subArrays[0][0], indexArrays[0][0], i, j);

			int arraySize = 1 << log;
			int midIndex = j - j % arraySize;

			int diffLeft = midIndex - i;
			int diffRight = j - midIndex + 1;

			int logDiffLeft = (int)Math.Ceiling(Math.Log(diffLeft, 2));
			int logDiffRight = (int)Math.Ceiling(Math.Log(diffRight, 2));

			int lvlLeft = subArrays.Length - 1 - logDiffLeft;
			int lvlRight = subArrays.Length - 1 - logDiffRight;

			int posLeft = i >> logDiffLeft;
			int posRight = j >> logDiffRight;

			if (diffLeft <= 0)
				return FilterByIndex(subArrays[lvlRight][posRight], indexArrays[lvlRight][posRight], i, j);

			return MergeAndFilterByIndex(subArrays[lvlLeft][posLeft], subArrays[lvlRight][posRight],
				indexArrays[lvlLeft][posLeft], indexArrays[lvlRight][posRight], i, j);
		}

		private static int[] MergeAndFilterByIndex(int[] ar1, int[] ar2, int[] indAr1, int[] indAr2, int i, int j)
		{
			int positionInAr1 = 0;
			int positionInAr2 = 0;

			int outputCount = j - i + 1;
			int[] returnArray = new int[outputCount];
			if (ar1.Length + ar2.Length > 2 * outputCount)
				throw new Exception("Too many items in arrays.");

			int positionInReturn = 0;

			while (positionInAr1 < ar1.Length && positionInAr2 < ar2.Length)
			{
				if (indAr1[positionInAr1] < i || indAr1[positionInAr1] > j)
				{
					positionInAr1++;
					continue;
				}
				if (indAr2[positionInAr2] < i || indAr2[positionInAr2] > j)
				{
					positionInAr2++;
					continue;
				}

				if (ar1[positionInAr1] < ar2[positionInAr2])
					returnArray[positionInReturn++] = ar1[positionInAr1++];
				else
					returnArray[positionInReturn++] = ar2[positionInAr2++];
			}

			while (positionInAr1 < ar1.Length)
				if (indAr1[positionInAr1] >= i && indAr1[positionInAr1] <= j)
					returnArray[positionInReturn++] = ar1[positionInAr1++];
				else
					positionInAr1++;

			while (positionInAr2 < ar2.Length)
				if (indAr2[positionInAr2] >= i && indAr2[positionInAr2] <= j)
					returnArray[positionInReturn++] = ar2[positionInAr2++];
				else
					positionInAr2++;

			return returnArray;
		}

		private static int[] FilterByIndex(int[] itemArray, int[] indexArray, int i, int j)
		{
			int outputCount = j - i + 1;
			int[] returnArray = new int[outputCount];
			int indInReturn = 0;
			if (itemArray.Length > 2 * outputCount)
				throw new Exception("Too many items in array.");

			for (int ind = 0; ind < itemArray.Length; ind++)
				if (indexArray[ind] >= i && indexArray[ind] <= j)
					returnArray[indInReturn++] = itemArray[ind];

			return returnArray;
		}
	}
}
