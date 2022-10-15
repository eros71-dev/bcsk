using UnityEngine;

public class PanCamera : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y + 5f * Time.deltaTime, base.transform.eulerAngles.z);
	}
}
