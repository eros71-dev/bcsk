using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
	public FadeInScript fadeInScript;

	private void Start()
	{
		StartCoroutine("changeLevel");
	}

	private void Update()
	{
	}

	private IEnumerator changeLevel()
	{
		yield return new WaitForSeconds(5f);
		fadeInScript.BeginFade(1);
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("TitleScreen");
	}
}
