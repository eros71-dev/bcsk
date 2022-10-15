using UnityEngine;

public class BounceSelectIcon : MonoBehaviour
{
	public float frequency;

	public float magnitude;

	public bool absoluteValue;

	private Vector3 originalPosition;

	private void Start()
	{
		originalPosition = base.transform.position;
	}

	private void Update()
	{
		float num = 0f;
		num = ((!absoluteValue) ? (originalPosition.x - Mathf.Cos(Time.unscaledTime * frequency) * magnitude * 10f) : (originalPosition.x - Mathf.Abs(Mathf.Cos(Time.unscaledTime * frequency) * magnitude * 10f)));
		base.transform.position = new Vector3(num, base.transform.position.y, base.transform.position.z);
	}

	public void resetOriginalPosition(Vector3 pos)
	{
		originalPosition = pos;
	}
}
