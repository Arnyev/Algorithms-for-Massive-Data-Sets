using System.Collections.Generic;

namespace Algs
{
	public class RMQHelper
	{
		int[] inputArray;
		int[] Earray;
		int[] Aarray;
		int[] Rarray;

		RMQPM1Helper innerHelper;

		public RMQHelper(int[] inputArray)
		{
			this.inputArray = inputArray;
			var cartesian = CartesianTreeNode.BuildCartesian(inputArray);
			Earray = new int[inputArray.Length * 2 - 1];
			Aarray = new int[inputArray.Length * 2 - 1];
			Rarray = new int[inputArray.Length];

			cartesian.WriteToArrays(Earray, 0, Aarray, Rarray);
			innerHelper = new RMQPM1Helper(Aarray);
		}

		public List<int> GetBounded(int i, int j, int x)
		{
			var result = new List<int>();
			GetBoundedRec(i, j, x, result);
			return result;
		}

		public void GetBoundedRec(int i, int j, int x, List<int> result)
		{
			if (i > j)
				return;

			(int index, int value) = GetMinimum(i, j);
			if (value > x)
				return;

			result.Add(value);
			if (i == j)
				return;

			GetBoundedRec(i, index - 1, x, result);
			GetBoundedRec(index + 1, j, x, result);
		}

		public (int Index, int Value) GetMinimum(int i, int j)
		{
			var innerI = Rarray[i];
			var innerJ = Rarray[j];

			if (innerJ < innerI)
			{
				var tmp = innerI;
				innerI = innerJ;
				innerJ = tmp;
			}

			int indexInE = innerHelper.GetMinimum(innerI, innerJ).Index;
			var indexInInput = Earray[indexInE];
			return (indexInInput, inputArray[indexInInput]);
		}
	}
}
