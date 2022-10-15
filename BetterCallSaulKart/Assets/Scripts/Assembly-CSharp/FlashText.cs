using UnityEngine;
using UnityEngine.UI;

public class FlashText : MonoBehaviour
{
	private Text text;

	public float frequency = 8f;

	public float magnitude = 0.15f;

	private float alpha;

	public float alphaOffset;

	private void Start()
	{
		text = GetComponent<Text>();
	}

	private void Update()
	{
		alpha = Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitude + 0.5f + alphaOffset;
		text.color = new Color(text.color.r, text.color.g, alpha, 1f);
	}
}
