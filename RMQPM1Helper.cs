﻿using System;
using System.Linq;

namespace Algs
{
	class RMQPM1Helper
	{
		private readonly int[] inputArray;
		private readonly int[][] sparseTable;
		private readonly int[][] sparseTableIndices;

		private readonly int[][,] precomputed;
		private readonly int[] indicesInPrecomputed;
		private readonly int[] precomputedMinimums;
		private readonly int[] precomputedMinimumIndices;

		private readonly int halfLog;
		private readonly int globalMin;
		private readonly int globalMinIndex;

		public RMQPM1Helper(int[] inputArray)
		{
			this.inputArray = inputArray;
			int len = inputArray.Length;
			int log = (int)Math.Ceiling(Math.Log(inputArray.Length, 2));
			halfLog = log / 2;
			int precomputedCount = 1 << (halfLog - 1);

			precomputed = Enumerable.Range(0, precomputedCount).Select(x => GetPrecomputed(x, halfLog)).ToArray();

			indicesInPrecomputed = GetCompressed(inputArray, len, halfLog);
			precomputedMinimumIndices = indicesInPrecomputed.Select((x, i) => precomputed[x][0, halfLog - 1] + i * halfLog).ToArray();

			precomputedMinimums = precomputedMinimumIndices.Select(x => inputArray[x]).ToArray();
			(sparseTable, sparseTableIndices) = BuildSparseTable(precomputedMinimums);

			globalMin = inputArray[0];
			globalMinIndex = 0;
			for (int i = 1; i < inputArray.Length; i++)
				if (inputArray[i] < globalMin)
				{
					globalMinIndex = i;
					globalMin = inputArray[i];
				}
		}

		public (int Index, int Value) GetMinimum(int i, int j)
		{
			if (i == 0 && j == inputArray.Length - 1)
				return (globalMinIndex, globalMin);

			int firstInterval = i / halfLog;
			int lastInterval = j / halfLog;

			if (firstInterval == lastInterval)
				return GetFromSinglePrecomputedArray(i, j, firstInterval);

			int firstInSparse = i % halfLog == 0 ? firstInterval : firstInterval + 1;
			int lastInSparse = j % halfLog == halfLog - 1 ? lastInterval : lastInterval - 1;

			int min = int.MaxValue;
			int minIndex = -1;

			if (firstInSparse == lastInSparse)
			{
				min = precomputedMinimums[firstInSparse];
				minIndex = precomputedMinimumIndices[firstInSparse];
			}
			else if (firstInSparse < lastInSparse)
				(minIndex, min) = GetMinFromSparseTable(firstInSparse, lastInSparse);

			if (firstInSparse != firstInterval)
			{
				int[,] precomputedArray = precomputed[indicesInPrecomputed[firstInterval]];
				int minLIndex = precomputedArray[i % halfLog, halfLog - 1];
				int minL = inputArray[firstInterval * halfLog + minLIndex];
				if (minL <= min)
				{
					min = minL;
					minIndex = firstInterval * halfLog + minLIndex;
				}
			}
			if (lastInSparse != lastInterval)
			{
				int[,] precomputedArray = precomputed[indicesInPrecomputed[lastInterval]];
				int minRIndex = precomputedArray[0, j % halfLog];
				int minR = inputArray[lastInterval * halfLog + minRIndex];

				if (minR < min)
				{
					min = minR;
					minIndex = lastInterval * halfLog + minRIndex;
				}
			}

			return (minIndex, min);
		}

		private (int Index, int Value) GetFromSinglePrecomputedArray(int i, int j, int first)
		{
			int indexInPrecomputed = indicesInPrecomputed[first];
			int[,] precomputedArray = precomputed[indexInPrecomputed];
			int minLIndex = precomputedArray[i % halfLog, j % halfLog];
			var valIndex = first * halfLog + minLIndex;
			return (valIndex, inputArray[valIndex]);
		}

		private (int Index, int Value) GetMinFromSparseTable(int firstInSparse, int lastInSparse)
		{
			int logDiff = (int)Math.Log(lastInSparse - firstInSparse + 1, 2);
			int lvlInSparse = sparseTable.Length - logDiff;
			int lastPosInSparse = lastInSparse - (1 << logDiff) + 1;

			int minL = sparseTable[lvlInSparse][firstInSparse];
			int minR = sparseTable[lvlInSparse][lastPosInSparse];

			int min;
			int minIndex;

			if (minL <= minR)
			{
				min = minL;
				minIndex = precomputedMinimumIndices[sparseTableIndices[lvlInSparse][firstInSparse]];
			}
			else
			{
				min = minR;
				minIndex = precomputedMinimumIndices[sparseTableIndices[lvlInSparse][lastPosInSparse]];
			}
			return (minIndex, min);
		}

		private static int[] GetCompressed(int[] array, int len, int halfLog)
		{
			int compressedSize = (len + halfLog - 1) / halfLog;
			int[] compressed = new int[compressedSize];
			for (int i = 0; i < compressed.Length - ((len % halfLog != 0) ? 1 : 0); i++)
			{
				int compressedVal = 0;
				for (int j = 1; j < halfLog; j++)
				{
					int ind = i * halfLog + j;
					if (array[ind] > array[ind - 1])
						compressedVal |= 1 << (halfLog - 1 - j);
				}
				compressed[i] = compressedVal;
			}

			if (len % halfLog != 0)
			{
				int compressedVal = 0;
				int j = 1;
				for (; j < len % halfLog; j++)
				{
					int ind = (compressed.Length - 1) * halfLog + j;
					if (array[ind] > array[ind - 1])
						compressedVal |= 1 << (halfLog - 1 - j);
				}
				for (; j < halfLog; j++)
					compressedVal |= 1 << (halfLog - 1 - j);

				compressed[compressed.Length - 1] = compressedVal;
			}

			return compressed;
		}

		static (int[][] ValueArray, int[][] IndexArray) BuildSparseTable(int[] array)
		{
			int log = (int)Math.Ceiling(Math.Log(array.Length, 2));
			int levelCount = log - 1;
			int[][] valueArray = new int[levelCount][];
			int[][] indexArray = new int[levelCount][];

			valueArray[levelCount - 1] = new int[array.Length - 1];
			indexArray[levelCount - 1] = new int[array.Length - 1];
			for (int i = 0; i < valueArray[levelCount - 1].Length; i++)
			{
				if (array[i] <= array[i + 1])
				{
					valueArray[levelCount - 1][i] = array[i];
					indexArray[levelCount - 1][i] = i;
				}
				else
				{
					valueArray[levelCount - 1][i] = array[i + 1];
					indexArray[levelCount - 1][i] = i + 1;
				}
			}

			for (int lvl = 1; lvl < levelCount; lvl++)
			{
				int arrayCoverSize = 2 << lvl;
				int prevArrayCoverSize = arrayCoverSize / 2;

				valueArray[levelCount - lvl - 1] = new int[array.Length - arrayCoverSize + 1];
				indexArray[levelCount - lvl - 1] = new int[array.Length - arrayCoverSize + 1];

				for (int j = 0; j < valueArray[levelCount - lvl - 1].Length; j++)
				{
					if (valueArray[levelCount - lvl][j] <= valueArray[levelCount - lvl][j + prevArrayCoverSize])
					{
						valueArray[levelCount - lvl - 1][j] = valueArray[levelCount - lvl][j];
						indexArray[levelCount - lvl - 1][j] = indexArray[levelCount - lvl][j];
					}
					else
					{
						valueArray[levelCount - lvl - 1][j] = valueArray[levelCount - lvl][j + prevArrayCoverSize];
						indexArray[levelCount - lvl - 1][j] = indexArray[levelCount - lvl][j + prevArrayCoverSize];
					}
				}
			}

			return (valueArray, indexArray);
		}

		static int[,] GetPrecomputed(int compressedArray, int arraySize)
		{
			int[] helperArray = new int[arraySize];
			for (int i = 1; i < arraySize; i++)
			{
				int bit = (compressedArray >> (arraySize - i - 1)) & 1;
				int add = (bit == 1) ? 1 : -1;

				helperArray[i] = helperArray[i - 1] + add;
			}

			int[,] result = new int[arraySize, arraySize];
			for (int i = 0; i < arraySize; i++)
				for (int j = i; j < arraySize; j++)
				{
					int minInd = i;
					int minElement = helperArray[i];
					for (int k = i + 1; k <= j; k++)
						if (helperArray[k] < minElement)
						{
							minElement = helperArray[k];
							minInd = k;
						}

					result[i, j] = minInd;
				}

			return result;
		}
	}
}
