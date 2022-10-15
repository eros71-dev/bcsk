using UnityEngine;
using UnityEngine.UI;

public class FlashPlace : MonoBehaviour
{
	private Image image;

	public float frequency = 11f;

	public float magnitude = 0.2f;

	private float alpha;

	public float alphaOffset;

	private void Start()
	{
		image = GetComponent<Image>();
	}

	private void Update()
	{
		alpha = Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitude + 0.4f + alphaOffset;
		image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
	}
}
