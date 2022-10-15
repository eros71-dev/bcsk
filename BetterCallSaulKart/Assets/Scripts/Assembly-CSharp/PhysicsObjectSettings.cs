using UnityEngine;
using UnityEngine.Rendering;

public class PhysicsObjectSettings : MonoBehaviour
{
	private void Awake()
	{
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (transform.name.Contains("%physics"))
			{
				transform.gameObject.AddComponent<PhysicsObject>();
			}
			if (transform.name.Contains("%billboard"))
			{
				transform.gameObject.GetComponent<MeshCollider>().enabled = false;
			}
			if (transform.name.Contains("%water"))
			{
				transform.gameObject.GetComponent<MeshCollider>().enabled = false;
				transform.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
			}
		}
	}
}
