using UnityEngine;

public class GrowStop : MonoBehaviour
{
	public Vector3 minSize;

	public Vector3 maxSize;

	public Vector3 speed;

	private Vector3 originalScale;

	private float myTime;

	public bool isMaxSize;

	private void Start()
	{
		originalScale = base.transform.localScale;
		myTime = 0f;
		base.transform.localScale = minSize;
	}

	private void Update()
	{
		if (!isMaxSize)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			num = base.transform.localScale.x + speed.x * Time.deltaTime;
			num2 = base.transform.localScale.y + speed.y * Time.deltaTime;
			num3 = base.transform.localScale.z + speed.z * Time.deltaTime;
			base.transform.localScale = new Vector3(num, num2, num3);
			if (base.transform.localScale.x >= maxSize.x && base.transform.localScale.y >= maxSize.y && base.transform.localScale.z > maxSize.z)
			{
				base.transform.localScale = maxSize;
				isMaxSize = true;
			}
		}
		myTime += Time.deltaTime;
	}

	public void resetSize()
	{
		base.transform.localScale = minSize;
		myTime = 0f;
	}
}
