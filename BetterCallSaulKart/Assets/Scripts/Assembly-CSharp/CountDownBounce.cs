using UnityEngine;

public class CountDownBounce : MonoBehaviour
{
	public float currTime;

	public float peakTime = 0.15f;

	public float maxTime = 0.3f;

	private float scale;

	private void Start()
	{
	}

	private void Update()
	{
		currTime += Time.deltaTime;
		if (currTime < peakTime)
		{
			scale = (currTime - 0f) / (peakTime - 0f);
			base.transform.localScale = new Vector3(scale, scale, scale);
		}
		else if (currTime < maxTime)
		{
			scale = 1f - (currTime - peakTime) / 1.5f;
			base.transform.localScale = new Vector3(scale, scale, scale);
		}
	}
}
