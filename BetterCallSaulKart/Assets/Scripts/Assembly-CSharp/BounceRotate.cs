using UnityEngine;

public class BounceRotate : MonoBehaviour
{
	public float frequency;

	public Vector3 magnitude;

	public Vector3 offset;

	public bool absoluteValue;

	private Vector3 originalRotation;

	private void Start()
	{
		originalRotation = base.transform.eulerAngles;
	}

	private void Update()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (absoluteValue)
		{
			num = originalRotation.x - Mathf.Abs(Mathf.Cos(Time.timeSinceLevelLoad * frequency + offset.x) * magnitude.x * 10f);
			num2 = originalRotation.y - Mathf.Abs(Mathf.Cos(Time.timeSinceLevelLoad * frequency + offset.y) * magnitude.y * 10f);
			num3 = originalRotation.z - Mathf.Abs(Mathf.Cos(Time.timeSinceLevelLoad * frequency + offset.z) * magnitude.z * 10f);
		}
		else
		{
			num = Mathf.Sin(Time.timeSinceLevelLoad * frequency + offset.x) * magnitude.x + offset.x;
			num2 = Mathf.Sin(Time.timeSinceLevelLoad * frequency + offset.y) * magnitude.y + offset.y;
			num3 = Mathf.Sin(Time.timeSinceLevelLoad * frequency + offset.z) * magnitude.z + offset.z;
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

	public void resetOriginalPosition(Vector3 pos)
	{
		originalRotation = pos;
	}
}
