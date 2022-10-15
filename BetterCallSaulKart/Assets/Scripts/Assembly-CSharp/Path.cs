using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
	private Color lineColor = Color.white;

	public int laps = 3;

	public UIManager uIManager;

	public List<Transform> nodes = new List<Transform>();

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = lineColor;
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			if (transform != base.transform)
			{
				nodes.Add(transform);
			}
		}
		for (int j = 0; j < nodes.Count; j++)
		{
			Vector3 position = nodes[j].position;
			Vector3 from = ((j <= 0) ? nodes[nodes.Count - 1].position : nodes[j - 1].position);
			Gizmos.DrawLine(from, position);
			Gizmos.DrawWireSphere(position, 25f);
		}
	}
}
