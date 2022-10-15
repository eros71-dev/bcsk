using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
	public AudioSource theme;

	public AudioSource startSound;

	public AudioSource selectSound;

	private float audioFadeTime = 1f;

	private bool buttonPressedBool;

	public string startGameScene;

	public string optionsScene;

	private int currentChoice;

	private float debounceTimeCurr;

	private float debounceTimeMax = 0.4f;

	private bool hasChosen;

	public List<FlashResultsRow> flashPlaceIcons;

	private string nextScene;

	public FadeInScript fadeInScript;

	private void Start()
	{
		Cursor.visible = false;
		getPlayerPrefsGraphicsSettings();
		buttonPressedBool = false;
		if (theme != null)
		{
			theme.Play();
		}
		flashPlaceIcons[currentChoice].gameObject.SetActive(value: true);
	}

	private void Update()
	{
		if (debounceTimeCurr < debounceTimeMax)
		{
			debounceTimeCurr += Time.deltaTime;
		}
		else
		{
			if (hasChosen)
			{
				return;
			}
			if (Input.GetButtonDown("Down"))
			{
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: false);
				currentChoice++;
				if (currentChoice >= flashPlaceIcons.Count)
				{
					currentChoice = 0;
				}
				else if (currentChoice < 0)
				{
					currentChoice = flashPlaceIcons.Count - 1;
				}
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: true);
				selectSound.Play();
			}
			else if (Input.GetButtonDown("Up"))
			{
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: false);
				currentChoice--;
				if (currentChoice >= flashPlaceIcons.Count)
				{
					currentChoice = 0;
				}
				else if (currentChoice < 0)
				{
					currentChoice = flashPlaceIcons.Count - 1;
				}
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: true);
				selectSound.Play();
			}
			if (Input.GetButtonDown("Jump"))
			{
				hasChosen = true;
				if (currentChoice == 0)
				{
					startPress();
				}
				else if (currentChoice == 1)
				{
					optionsPress();
				}
				else if (currentChoice == 2)
				{
					quitPress();
				}
			}
		}
	}

	public void startPress()
	{
		if (!buttonPressedBool)
		{
			nextScene = startGameScene;
			if (startSound != null)
			{
				startSound.Play();
			}
			buttonPressedBool = true;
			StartCoroutine("changeScene");
			StartCoroutine("fadeOutSound");
		}
	}

	public void optionsPress()
	{
		if (!buttonPressedBool)
		{
			nextScene = optionsScene;
			if (startSound != null)
			{
				startSound.Play();
			}
			buttonPressedBool = true;
			StartCoroutine("changeScene");
			StartCoroutine("fadeOutSound");
		}
	}

	private IEnumerator changeScene()
	{
		fadeInScript.BeginFade(1);
		yield return new WaitForSeconds(2.5f);
		SceneManager.LoadScene(nextScene);
	}

	private IEnumerator fadeOutSound()
	{
		if (theme != null)
		{
			while (theme.volume > 0.01f)
			{
				theme.volume -= Time.deltaTime / audioFadeTime;
				yield return null;
			}
			theme.volume = 0f;
			theme.Stop();
		}
	}

	private IEnumerator fadeInSound()
	{
		if (theme != null)
		{
			while (theme.volume < 1f && !buttonPressedBool)
			{
				theme.volume += Time.deltaTime / audioFadeTime;
				yield return null;
			}
		}
	}

	public void getPlayerPrefsGraphicsSettings()
	{
		string @string = PlayerPrefs.GetString("quality");
		string string2 = PlayerPrefs.GetString("fullscreen");
		string string3 = PlayerPrefs.GetString("screenWidth");
		string string4 = PlayerPrefs.GetString("screenHeight");
		Debug.Log("Player Prefs: ");
		Debug.Log("quality: " + @string);
		Debug.Log("fullscreen: " + string2);
		Debug.Log("screenWidth: " + string3);
		Debug.Log("screenHeight: " + string4);
		GlobalGameData.LoadFromPrefs();
		GlobalGameData.PrintData();
		try
		{
			if (@string.Length > 0)
			{
				QualitySettings.SetQualityLevel(int.Parse(@string));
				GlobalPlayerPrefs.quality = @string;
			}
			if (string2.Length > 0)
			{
				Screen.fullScreen = bool.Parse(string2);
				GlobalPlayerPrefs.fullscreen = string2;
			}
			if (string3.Length > 0 && string4.Length > 0)
			{
				int width = int.Parse(string3);
				int height = int.Parse(string4);
				Resolution resolution = default(Resolution);
				resolution.width = width;
				resolution.height = height;
				Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
				GlobalPlayerPrefs.screenWidth = string3;
				GlobalPlayerPrefs.screenHeight = string4;
			}
		}
		catch (Exception message)
		{
			Debug.Log("Error parseing playerPrefs");
			Debug.Log(message);
		}
	}

	public void deleteAllPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	public void quitPress()
	{
		Application.Quit();
		if (startSound != null)
		{
			startSound.Play();
		}
	}
}
