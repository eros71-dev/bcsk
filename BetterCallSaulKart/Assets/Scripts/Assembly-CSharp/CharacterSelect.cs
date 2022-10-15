using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
	private string nextScene = "TrackSelect";

	public AudioSource selectSound;

	public AudioSource chooseSound;

	public AudioSource waltuhSound;

	public AudioSource dingSound;

	public List<AudioSource> voices;

	public VehiclePrefabs vehiclePrefabs;

	public GameObject rotator;

	private List<GameObject> Models;

	private int currentChoice = 1;

	public Text RacerNameText;

	public List<FlashPlace> flashPlaceIcons;

	public FadeInScript fader;

	private bool hasChosen;

	private float debounceTimeCurr;

	private float debounceTimeMax = 0.4f;

	public GameObject menuMusicPrefab;

	private void Start()
	{
		HandleMenuMusic();
		Models = new List<GameObject>();
		GameObject[] prefabs = vehiclePrefabs.prefabs;
		for (int i = 0; i < prefabs.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(prefabs[i], rotator.transform);
			gameObject.SetActive(value: false);
			gameObject.transform.position = new Vector3(0f, 0f, 0f);
			gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			Models.Add(gameObject);
		}
		Models[currentChoice].SetActive(value: true);
		flashPlaceIcons[currentChoice].gameObject.SetActive(value: true);
		RacerNameText.text = Models[currentChoice].GetComponent<RacerInfo>().racerName;
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
				Models[currentChoice].SetActive(value: false);
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: false);
				currentChoice++;
				if (currentChoice >= Models.Count)
				{
					currentChoice = 0;
				}
				else if (currentChoice < 0)
				{
					currentChoice = Models.Count - 1;
				}
				Models[currentChoice].SetActive(value: true);
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: true);
				RacerNameText.text = Models[currentChoice].GetComponent<RacerInfo>().racerName;
				selectSound.Play();
				rotator.transform.eulerAngles = Vector3.zero;
			}
			else if (Input.GetButtonDown("Left"))
			{
				Models[currentChoice].SetActive(value: false);
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: false);
				currentChoice--;
				if (currentChoice >= Models.Count)
				{
					currentChoice = 0;
				}
				else if (currentChoice < 0)
				{
					currentChoice = Models.Count - 1;
				}
				Models[currentChoice].SetActive(value: true);
				flashPlaceIcons[currentChoice].gameObject.SetActive(value: true);
				RacerNameText.text = Models[currentChoice].GetComponent<RacerInfo>().racerName;
				selectSound.Play();
				rotator.transform.eulerAngles = Vector3.zero;
			}
			if (!Input.GetButtonDown("Jump"))
			{
				return;
			}
			chooseSound.Play();
			hasChosen = true;
			if (currentChoice == 0)
			{
				StartCoroutine(fadeOutSound());
				StartCoroutine(ReturnToTitle());
				return;
			}
			if (voices[currentChoice - 1] != null)
			{
				voices[currentChoice - 1].Play();
			}
			GlobalGameData.currentCharacter = currentChoice - 1;
			Models[currentChoice].GetComponent<VehicleModel>().driver.Cheer();
			StartCoroutine(ChangeScene());
		}
	}

	private IEnumerator ChangeScene()
	{
		yield return new WaitForSeconds(0.5f);
		fader.BeginFade(1);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(nextScene);
	}

	private IEnumerator ReturnToTitle()
	{
		fader.BeginFade(1);
		yield return new WaitForSeconds(1f);
		MenuMusic[] array = Object.FindObjectsOfType<MenuMusic>();
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i].gameObject);
		}
		SceneManager.LoadScene("Title");
	}

	private void HandleMenuMusic()
	{
		if (Object.FindObjectsOfType<MenuMusic>().Length == 0)
		{
			Object.Instantiate(menuMusicPrefab);
		}
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
}
