namespace Algs
{
	class CartesianTreeNode
	{
		public CartesianTreeNode Up;
		public readonly int index;
		public readonly int value;

		public CartesianTreeNode Left;
		public CartesianTreeNode Right;
		int depth;

		public CartesianTreeNode(CartesianTreeNode up, int index, int value)
		{
			Up = up;
			this.index = index;
			this.value = value;
		}

		public static CartesianTreeNode BuildCartesian(int[] inputArray)
		{
			var root = new CartesianTreeNode(null, 0, inputArray[0]);
			var current = root;
			for (int i = 1; i < inputArray.Length; i++)
			{
				var val = inputArray[i];

				while (current.value > val && current != null)
					current = current.Up;

				var node = new CartesianTreeNode(current, i, val);
				if (current != null)
				{
					node.Left = current.Right;
					if (node.Left != null)
						node.Left.Up = node;
					current.Right = node;
				}
				else
				{
					node.Left = root;
					root.Up = node;
					root = node;
				}

				current = node;
			}

			root.UpdateDepth(0);
			return root;
		}

		public void UpdateDepth(int d)
		{
			depth = d;
			Left?.UpdateDepth(d + 1);
			Right?.UpdateDepth(d + 1);
		}

		public int WriteToArrays(int[] Earray, int arrayIndex, int[] Aarray, int[] Rarray)
		{
			Rarray[index] = arrayIndex;
			Earray[arrayIndex] = index;
			Aarray[arrayIndex] = depth;

			var leftMax = Left?.WriteToArrays(Earray, arrayIndex + 1, Aarray, Rarray) ?? arrayIndex;
			if (Left != null)
			{
				leftMax++;
				Earray[leftMax] = index;
				Aarray[leftMax] = depth;
			}

			var rightMax = Right?.WriteToArrays(Earray, leftMax + 1, Aarray, Rarray) ?? leftMax;
			if (Right != null)
			{
				rightMax++;
				Earray[rightMax] = index;
				Aarray[rightMax] = depth;
			}

			return rightMax;
		}

		public override string ToString()
		{
			return  $"{index}, {value}";
		}
	}
}
