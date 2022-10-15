using UnityEngine;

public class TitleSpin : MonoBehaviour
{
	private float x;

	private float y;

	private float z;

	private float speed;

	private void Start()
	{
		x = 0f;
		y = 1f;
		z = 0f;
		speed = 20f;
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(x, y, z) * speed * Time.deltaTime);
	}
}
