using UnityEngine;

public class Bounce : MonoBehaviour
{
	public float frequency;

	public Vector3 magnitude;

	public bool absoluteValue;

	private Vector3 originalPosition;

	private void Start()
	{
		originalPosition = base.transform.position;
	}

	private void Update()
	{
		float num = 1f;
		if ((bool)GetComponent<RectTransform>())
		{
			num = (float)Screen.width * 0.004f;
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		if (absoluteValue)
		{
			num2 = originalPosition.x - Mathf.Abs(Mathf.Cos(Time.timeSinceLevelLoad * frequency) * magnitude.x * 10f);
			num3 = originalPosition.y - Mathf.Abs(Mathf.Cos(Time.timeSinceLevelLoad * frequency) * magnitude.y * 10f);
			num4 = originalPosition.z - Mathf.Abs(Mathf.Cos(Time.timeSinceLevelLoad * frequency) * magnitude.z * 10f);
		}
		else
		{
			num2 = Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitude.x * num;
			num3 = Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitude.y * num;
			num4 = Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitude.z * num;
		}
		if (absoluteValue)
		{
			base.transform.position = new Vector3(num2, num3, num4);
		}
		else
		{
			base.transform.position += new Vector3(num2, num3, num4);
		}
	}

	public void resetOriginalPosition(Vector3 pos)
	{
		originalPosition = pos;
	}
}
