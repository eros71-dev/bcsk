using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostGameOptions : MonoBehaviour
{
	private int currSelection;

	public List<Image> optionImages;

	public FadeInScript fadeInScript;

	private bool isFadingOut;

	public AudioSource confirmSound;

	public AudioSource moveCursorSound;

	public PlaceTracker placeTracker;

	public float currTime;

	private float size;

	private void Start()
	{
		base.transform.localScale = Vector3.zero;
		for (int i = 0; i < optionImages.Count; i++)
		{
			optionImages[i].GetComponent<FlashResultsRow>().enabled = false;
			optionImages[i].color = new Color(optionImages[i].color.r, optionImages[i].color.g, optionImages[i].color.b, 0f);
		}
	}

	private void Update()
	{
		size += Time.deltaTime * 4f;
		if (size >= 1f)
		{
			size = 1f;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			base.transform.localScale = new Vector3(size, 1f, 1f);
		}
		if (isFadingOut)
		{
			return;
		}
		currTime += Time.deltaTime;
		if (currTime >= 0.5f)
		{
			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				currSelection++;
				moveCursorSound.Play();
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			{
				currSelection--;
				moveCursorSound.Play();
			}
			if (currSelection < 0)
			{
				currSelection = optionImages.Count - 1;
			}
			else if (currSelection >= optionImages.Count)
			{
				currSelection = 0;
			}
		}
		for (int i = 0; i < optionImages.Count; i++)
		{
			if (i == currSelection)
			{
				optionImages[i].GetComponent<FlashResultsRow>().enabled = true;
				optionImages[i].color = new Color(optionImages[i].color.r, optionImages[i].color.g, optionImages[i].color.b, 0.5f);
			}
			else
			{
				optionImages[i].GetComponent<FlashResultsRow>().enabled = false;
				optionImages[i].color = new Color(optionImages[i].color.r, optionImages[i].color.g, optionImages[i].color.b, 0f);
			}
		}
		if (currTime >= 0.5f && Input.GetKeyDown(KeyCode.Space) && !isFadingOut)
		{
			isFadingOut = true;
			StartCoroutine(ChangeScene());
			confirmSound.Play();
		}
	}

	private IEnumerator ChangeScene()
	{
		placeTracker.resultsFirstPlace.Stop();
		placeTracker.resultsLastPlace.Stop();
		fadeInScript.BeginFade(1);
		yield return new WaitForSeconds(1.5f);
		if (currSelection == 0)
		{
			SceneManager.LoadScene("TrackSelect");
			yield break;
		}
		if (currSelection == 1)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			yield break;
		}
		_ = currSelection;
		_ = 2;
	}
}
