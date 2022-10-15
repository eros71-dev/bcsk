using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
	private int currSelection;

	public List<Image> optionImages;

	public FadeInScript fadeInScript;

	private bool isFadingOut;

	public AudioSource confirmSound;

	public AudioSource moveCursorSound;

	public AudioSource pauseSound;

	public PlaceTracker placeTracker;

	private float currTime;

	public GameObject greyBG;

	public bool isPaused;

	public bool canPause = true;

	private void Start()
	{
		GlobalGameData.isPaused = false;
		for (int i = 0; i < optionImages.Count; i++)
		{
			optionImages[i].GetComponent<FlashResultRowUnscaledTime>().enabled = false;
			optionImages[i].color = new Color(optionImages[i].color.r, optionImages[i].color.g, optionImages[i].color.b, 0f);
		}
	}

	private void Update()
	{
		if (isFadingOut)
		{
			return;
		}
		if (!isPaused)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				PauseGame();
			}
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UnPauseGame();
		}
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
		for (int i = 0; i < optionImages.Count; i++)
		{
			if (i == currSelection)
			{
				optionImages[i].GetComponent<FlashResultRowUnscaledTime>().enabled = true;
				optionImages[i].color = new Color(optionImages[i].color.r, optionImages[i].color.g, optionImages[i].color.b, 0.5f);
			}
			else
			{
				optionImages[i].GetComponent<FlashResultRowUnscaledTime>().enabled = false;
				optionImages[i].color = new Color(optionImages[i].color.r, optionImages[i].color.g, optionImages[i].color.b, 0f);
			}
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (currSelection == 0)
			{
				UnPauseGame();
			}
			else if (currSelection == 1)
			{
				Debug.Log("QUIT");
				isFadingOut = true;
				StartCoroutine(ChangeScene());
				confirmSound.Play();
			}
		}
	}

	private void PauseGame()
	{
		if (canPause)
		{
			GlobalGameData.isPaused = true;
			isPaused = true;
			greyBG.SetActive(value: true);
			pauseSound.Play();
			Time.timeScale = 0f;
			placeTracker.PauseAllSound();
		}
	}

	private void UnPauseGame()
	{
		if (canPause)
		{
			GlobalGameData.isPaused = false;
			isPaused = false;
			greyBG.SetActive(value: false);
			pauseSound.Play();
			Time.timeScale = 1f;
			placeTracker.ResumeAllSound();
		}
	}

	private IEnumerator ChangeScene()
	{
		Debug.Log("Change Scene");
		fadeInScript.BeginFade(1);
		yield return new WaitForSecondsRealtime(1.5f);
		Time.timeScale = 1f;
		SceneManager.LoadScene("TrackSelect");
	}
}
