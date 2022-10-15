using UnityEngine;
using UnityEngine.UI;

public class FlashSelectIcon : MonoBehaviour
{
	public float frequency;

	public float magnitude;

	public float offset;

	private Image image;

	public float alpha;

	private void Start()
	{
		image = GetComponent<Image>();
		alpha = Mathf.Cos(Time.unscaledTime * frequency) * magnitude + offset;
		image.color = new Color(255f, 255f, 255f, alpha);
	}

	private void Update()
	{
		alpha = Mathf.Cos(Time.unscaledTime * frequency) * magnitude + offset;
		image.color = new Color(255f, 255f, 255f, alpha);
	}
}
