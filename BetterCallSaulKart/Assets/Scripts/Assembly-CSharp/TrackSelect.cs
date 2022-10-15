using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackSelect : MonoBehaviour
{
	public AudioSource selectSound;

	public AudioSource chooseSound;

	public AudioSource fartSound;

	private int currentChoice = 1;

	public List<FlashResultsRow> flashPlaceIcons;

	public List<GameObject> goldMedals;

	public FadeInScript fader;

	private bool hasChosen;

	private string[] trackSceneNames = new string[5] { "CharacterSelect", "DesertTrack", "CityTrack", "SaulHeadTrack", "Cutscene" };

	private float debounceTimeCurr;

	private float debounceTimeMax = 0.4f;

	public Text bestTimeText;

	public Text bestPlaceText;

	public Text lockedText;

	public Text playCutsceneText;

	public GameObject cutSceneIcon;

	public GameObject cutSceneIconGrey;

	public GameObject menuMusicPrefab;

	private void Start()
	{
		HandleMenuMusic();
		flashPlaceIcons[currentChoice].gameObject.SetActive(value: true);
		HandleText();
		for (int i = 0; i < GlobalGameData.numTracks; i++)
		{
			if (GlobalGameData.bestPlaces[i] == 1)
			{
				goldMedals[i].SetActive(value: true);
			}
			else
			{
				goldMedals[i].SetActive(value: false);
			}
		}
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
			if (Input.GetButtonDown("Right"))
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
			else if (Input.GetButtonDown("Left"))
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
			HandleText();
			if (!Input.GetButtonDown("Jump"))
			{
				return;
			}
			if (currentChoice == 4 && !GetIsCutSceneUnlocked())
			{
				fartSound.Play();
				return;
			}
			chooseSound.Play();
			fader.BeginFade(1);
			hasChosen = true;
			GlobalGameData.currentCourse = currentChoice;
			StartCoroutine(ChangeScene());
			if (currentChoice != 0)
			{
				StartCoroutine(fadeOutSound());
			}
		}
	}

	private void HandleText()
	{
		bool isCutSceneUnlocked = GetIsCutSceneUnlocked();
		if (isCutSceneUnlocked)
		{
			cutSceneIcon.SetActive(value: true);
			cutSceneIconGrey.SetActive(value: false);
		}
		else
		{
			cutSceneIcon.SetActive(value: false);
			cutSceneIconGrey.SetActive(value: true);
		}
		if (currentChoice == 4)
		{
			if (isCutSceneUnlocked)
			{
				lockedText.gameObject.SetActive(value: false);
				playCutsceneText.gameObject.SetActive(value: true);
			}
			else
			{
				lockedText.gameObject.SetActive(value: true);
				playCutsceneText.gameObject.SetActive(value: false);
			}
		}
		else
		{
			lockedText.gameObject.SetActive(value: false);
			playCutsceneText.gameObject.SetActive(value: false);
		}
		if (currentChoice == 0 || currentChoice == 4)
		{
			bestTimeText.gameObject.SetActive(value: false);
			bestPlaceText.gameObject.SetActive(value: false);
			return;
		}
		bestTimeText.gameObject.SetActive(value: true);
		bestPlaceText.gameObject.SetActive(value: true);
		if (GlobalGameData.bestTimes[currentChoice] >= 9999f)
		{
			bestTimeText.text = "BEST TIME: N/A";
		}
		else
		{
			bestTimeText.text = "BEST TIME: " + GlobalGameData.bestTimes[currentChoice];
		}
		if (GlobalGameData.bestPlaces[currentChoice] >= 9999)
		{
			bestPlaceText.text = "BEST PLACE: N/A";
		}
		else
		{
			bestPlaceText.text = "BEST PLACE: " + GlobalGameData.bestPlaces[currentChoice];
		}
	}

	private IEnumerator ChangeScene()
	{
		yield return new WaitForSeconds(1f);
		if (currentChoice != 0)
		{
			MenuMusic[] array = Object.FindObjectsOfType<MenuMusic>();
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i].gameObject);
			}
		}
		GlobalGameData.currentCourse = currentChoice;
		SceneManager.LoadScene(trackSceneNames[currentChoice]);
	}

	private IEnumerator fadeOutSound()
	{
		float audioFadeTime = 0.5f;
		MenuMusic menuMusic = Object.FindObjectOfType<MenuMusic>();
		AudioSource menuMusicAudioSource = menuMusic.GetComponent<AudioSource>();
		if ((bool)menuMusicAudioSource)
		{
			while (menuMusicAudioSource.volume > 0.01f)
			{
				menuMusicAudioSource.volume -= Time.deltaTime / audioFadeTime;
				yield return null;
			}
			menuMusicAudioSource.volume = 0f;
			menuMusicAudioSource.Stop();
		}
	}

	private void HandleMenuMusic()
	{
		if (Object.FindObjectsOfType<MenuMusic>().Length == 0)
		{
			Object.Instantiate(menuMusicPrefab);
		}
	}

	private bool GetIsCutSceneUnlocked()
	{
		int num = 0;
		int[] bestPlaces = GlobalGameData.bestPlaces;
		for (int i = 0; i < bestPlaces.Length; i++)
		{
			if (bestPlaces[i] == 1)
			{
				num++;
			}
		}
		if (num >= 3)
		{
			return true;
		}
		return false;
	}
}
