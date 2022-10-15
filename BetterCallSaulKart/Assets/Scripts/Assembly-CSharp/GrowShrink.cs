using UnityEngine;

public class GrowShrink : MonoBehaviour
{
	public float frequency;

	public Vector3 magnitude;

	public bool absoluteValue;

	private Vector3 originalScale;

	private float myTime;

	private void Start()
	{
		originalScale = base.transform.localScale;
		myTime = 0f;
	}

	private void Update()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (absoluteValue)
		{
			num = originalScale.x - Mathf.Abs(Mathf.Cos(myTime * frequency) * magnitude.x * 10f);
			num2 = originalScale.y - Mathf.Abs(Mathf.Cos(myTime * frequency) * magnitude.y * 10f);
			num3 = originalScale.z - Mathf.Abs(Mathf.Cos(myTime * frequency) * magnitude.z * 10f);
		}
		else
		{
			num = Mathf.Sin(myTime * frequency) * magnitude.x;
			num2 = Mathf.Sin(myTime * frequency) * magnitude.y;
			num3 = Mathf.Sin(myTime * frequency) * magnitude.z;
		}
		if (absoluteValue)
		{
			base.transform.localScale = new Vector3(num, num2, num3);
		}
		else
		{
			base.transform.localScale += new Vector3(num, num2, num3);
		}
		myTime += Time.deltaTime;
	}
}
