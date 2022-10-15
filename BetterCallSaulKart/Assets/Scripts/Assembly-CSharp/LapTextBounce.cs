using UnityEngine;

public class LapTextBounce : MonoBehaviour
{
	private float currTime;

	public float peakTime = 0.3f;

	public float shrinkTime = 0.75f;

	public float maxTime = 3f;

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
		else if (currTime < shrinkTime)
		{
			scale = 1f - (currTime - peakTime) / 1.5f;
			base.transform.localScale = new Vector3(scale, scale, scale);
		}
		if (currTime >= maxTime)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
