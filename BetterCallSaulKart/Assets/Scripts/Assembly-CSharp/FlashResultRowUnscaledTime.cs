using UnityEngine;
using UnityEngine.UI;

public class FlashResultRowUnscaledTime : MonoBehaviour
{
	private Image image;

	public float frequency = 8f;

	public float magnitude = 0.15f;

	private float alpha;

	public float alphaOffset;

	private void Start()
	{
		image = GetComponent<Image>();
	}

	private void Update()
	{
		alpha = Mathf.Sin(Time.unscaledTime * frequency) * magnitude + 0.5f + alphaOffset;
		image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
	}
}
