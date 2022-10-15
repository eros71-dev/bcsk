using UnityEngine;

public class ItemBoxRotate : MonoBehaviour
{
	private Vector3 rotation;

	private Space space = Space.Self;

	private void Start()
	{
		rotation = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
		if (rotation.magnitude < 50f)
		{
			rotation = new Vector3(60f, 60f, 60f);
		}
	}

	private void Update()
	{
		base.transform.Rotate(rotation * Time.deltaTime, space);
	}
}
