using UnityEngine;

public class DieAfterXTime : MonoBehaviour
{
	private float currTime;

	public float dieTime = 2f;

	private void Start()
	{
	}

	private void Update()
	{
		currTime += Time.deltaTime;
		if (currTime >= dieTime)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
