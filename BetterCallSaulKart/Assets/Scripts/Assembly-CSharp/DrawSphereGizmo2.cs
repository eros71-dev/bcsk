using UnityEngine;

public class DrawSphereGizmo2 : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, 0.05f);
		Gizmos.color = Color.white;
	}
}
