using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.LookAt(Camera.main.transform);
	}
}
