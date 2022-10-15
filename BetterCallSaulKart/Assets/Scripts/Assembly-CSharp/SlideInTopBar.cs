using UnityEngine;

public class SlideInTopBar : MonoBehaviour
{
	public float startPos;

	public float endPos;

	public float speed;

	private float currPos;

	public float delay = 0.3f;

	private float currTime;

	private void Start()
	{
		currPos = startPos;
	}

	private void Update()
	{
		if (currTime >= delay)
		{
			if (base.transform.GetComponent<RectTransform>().anchoredPosition.y <= endPos)
			{
				base.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, endPos, 0f);
				return;
			}
			base.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, currPos, 0f);
			currPos -= Time.deltaTime * speed;
		}
		else
		{
			currTime += Time.deltaTime;
		}
	}
}
