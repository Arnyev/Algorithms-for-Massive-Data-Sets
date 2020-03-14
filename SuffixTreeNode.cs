using System;
using System.Collections.Generic;

namespace Algs
{
	class SuffixTreeNode
	{
		public Dictionary<char, SuffixTreeNode> children = new Dictionary<char, SuffixTreeNode>();

		public readonly int subwordStartIndex;
		public readonly int startIndex;
		public int endIndex;
		public int depth;

		public SuffixTreeNode(int startIndex, int endIndex, int subwordStartIndex)
		{
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.subwordStartIndex = subwordStartIndex;
		}
		public void UpdateDepth(int d)
		{
			depth = d;
			foreach (var child in children)
				child.Value.UpdateDepth(depth + 1);
		}

		public int WriteToArrays(int[] Earray, int arrayIndex, int[] Aarray, int[] Rarray)
		{
			if (children.Count == 0)
				Rarray[subwordStartIndex] = arrayIndex;

			Earray[arrayIndex] = endIndex - subwordStartIndex + 1;
			Aarray[arrayIndex] = depth;

			foreach (var child in children)
			{
				arrayIndex = child.Value.WriteToArrays(Earray, arrayIndex + 1, Aarray, Rarray);

				arrayIndex++;
				Earray[arrayIndex] = endIndex - subwordStartIndex + 1;
				Aarray[arrayIndex] = depth;
			}

			return arrayIndex;
		}

		public static SuffixTreeNode Build(string s)
		{
			var s2 = '\t' + s + '\t';
			var root = new SuffixTreeNode(0, 0, 0);

			for (int i = 1; i < s2.Length - 1; i++)
			{
				var currentNode = root;
				var indexInWord = i;
				bool alreadyAdded = false;
				while (currentNode.children.TryGetValue(s2[indexInWord], out SuffixTreeNode searchedChild))
				{
					int endMatchIndex = indexInWord;
					int indInFound = searchedChild.startIndex;
					while (s2[endMatchIndex] == s2[indInFound] && indInFound <= searchedChild.endIndex)
					{
						endMatchIndex++;
						indInFound++;
					}
					bool wholeWord = indInFound == searchedChild.endIndex + 1;
					if (!wholeWord)
					{
						var nodeOldChildren = new SuffixTreeNode(indInFound, searchedChild.endIndex, searchedChild.subwordStartIndex);
						nodeOldChildren.children = searchedChild.children;
						var newNode = new SuffixTreeNode(endMatchIndex, s2.Length - 1, i);
						searchedChild.children = new Dictionary<char, SuffixTreeNode>
						{
							{ s2[indInFound], nodeOldChildren },
							{ s2[endMatchIndex], newNode }
						};

						searchedChild.endIndex = indInFound - 1;
						alreadyAdded = true;
						break;
					}
					else
					{
						currentNode = searchedChild;
						indexInWord = endMatchIndex;
					}
				}
				if (!alreadyAdded)
				{
					var newNode = new SuffixTreeNode(indexInWord, s2.Length - 1, i);
					currentNode.children.Add(s2[indexInWord], newNode);
				}
			}

			root.UpdateDepth(0);
			return root;
		}
	}
}
