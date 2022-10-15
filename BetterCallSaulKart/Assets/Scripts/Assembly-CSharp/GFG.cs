using System.Collections.Generic;
using UnityEngine;

internal class GFG : IComparer<Vehicle>
{
	public int Compare(Vehicle x, Vehicle y)
	{
		if (x.lap == y.lap)
		{
			if (x.currentNode == y.currentNode)
			{
				if ((x.GetHitMid() && y.GetHitMid()) || (!x.GetHitMid() && !y.GetHitMid()))
				{
					float num = Vector3.Distance(x.transform.position, x.path.nodes[x.currentNode].position);
					float num2 = Vector3.Distance(y.transform.position, y.path.nodes[y.currentNode].position);
					if (num < num2)
					{
						return -1;
					}
					return 1;
				}
				if (x.GetHitMid())
				{
					return -1;
				}
				return 1;
			}
			if (x.GetHitMid() && x.currentNode == 0)
			{
				return -1;
			}
			if (y.GetHitMid() && y.currentNode == 0)
			{
				return 1;
			}
			if (x.currentNode > y.currentNode)
			{
				return -1;
			}
			return 1;
		}
		if (x.lap > y.lap)
		{
			return -1;
		}
		return 1;
	}
}
