using UnityEngine;

public class Rotate : MonoBehaviour
{
	public Vector3 direction;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(direction * Time.deltaTime);
	}
}
