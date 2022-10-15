using UnityEngine;

public class DrawSphereGizmo : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, 0.1f);
		Gizmos.color = Color.white;
	}
}
