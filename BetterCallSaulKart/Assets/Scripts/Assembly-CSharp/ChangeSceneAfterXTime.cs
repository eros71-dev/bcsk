using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterXTime : MonoBehaviour
{
	public FadeInScript fadeInScript;

	public string sceneName;

	public float time = 5f;

	public bool allowSkip;

	public AudioSource skipSound;

	private float debounceTime;

	private bool hasSkipped;

	private void Start()
	{
		StartCoroutine(delayChangeLevel());
	}

	private void Update()
	{
		debounceTime += Time.deltaTime;
		if (debounceTime >= 1.25f && debounceTime < time && Input.GetButtonDown("Jump") && !hasSkipped && allowSkip)
		{
			Debug.Log("Skip");
			if ((bool)skipSound)
			{
				skipSound.Play();
			}
			hasSkipped = true;
			StartCoroutine(changeScene());
		}
	}

	private IEnumerator delayChangeLevel()
	{
		yield return new WaitForSeconds(time);
		if (!hasSkipped)
		{
			StartCoroutine(changeScene());
		}
	}

	private IEnumerator changeScene()
	{
		if ((bool)fadeInScript)
		{
			fadeInScript.BeginFade(1);
		}
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(sceneName);
	}
}
