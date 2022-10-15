using UnityEngine;

public class EscToQuit : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("Quitting program");
			Application.Quit();
		}
	}
}
