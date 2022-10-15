using UnityEngine;

public class Rotate2 : MonoBehaviour
{
	public float frequency;

	public Vector3 magnitude;

	public bool absoluteValue;

	private Vector3 originalPosition;

	public float currTime;

	private void Start()
	{
		originalPosition = base.transform.position;
		base.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		currTime = 0f;
	}

	private void Update()
	{
		currTime += Time.deltaTime;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (absoluteValue)
		{
			num = originalPosition.x - Mathf.Abs(Mathf.Cos(currTime * frequency) * magnitude.x * 10f);
			num2 = originalPosition.y - Mathf.Abs(Mathf.Cos(currTime * frequency) * magnitude.y * 10f);
			num3 = originalPosition.z - Mathf.Abs(Mathf.Cos(currTime * frequency) * magnitude.z * 10f);
		}
		else
		{
			num = Mathf.Sin(currTime * frequency + 1.5f) * magnitude.x;
			num2 = Mathf.Sin(currTime * frequency + 1.5f) * magnitude.y;
			num3 = Mathf.Sin(currTime * frequency + 1.5f) * magnitude.z;
		}
		if (absoluteValue)
		{
			base.transform.eulerAngles = new Vector3(num, num2, num3);
		}
		else
		{
			base.transform.eulerAngles += new Vector3(num, num2, num3);
		}
	}
}
