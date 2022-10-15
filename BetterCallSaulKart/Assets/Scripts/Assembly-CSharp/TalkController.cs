using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkController : MonoBehaviour, ITalkController
{
	public GameObject messegePanel;

	public bool isTalking;

	public GameObject[] actorGameObjects;

	public string JSONPath;

	private string currentScenePath;

	public GameObject[] cameras;

	public GameObject introCamera;

	public GameObject outroCamera;

	public List<AudioClip> voicesList;

	public FadeInScript fadeInScript;

	public AudioSource outdoorAmbience;

	public AudioSource indoorAmbience;

	public AudioSource creepyMusic;

	public AudioSource sappyMusic;

	public AudioSource happyMusic;

	public bool playIntro = true;

	public bool[] flags;

	public GameObject laloGun;

	public GameObject howardGun;

	public GameObject howardMuzzleFlair;

	public GameObject bloodSpatter;

	public GameObject howardEmmy;

	private void Start()
	{
		currentScenePath = JSONPath;
		flags = new bool[10];
		if (playIntro)
		{
			StartCoroutine(Intro());
		}
		else
		{
			talk();
		}
	}

	private void Update()
	{
	}

	public void talk()
	{
		if (isTalking)
		{
			return;
		}
		if (voicesList.Count > 0)
		{
			foreach (AudioClip voices in voicesList)
			{
				messegePanel.GetComponent<PanelConfig>().addToVoicesList(voices);
			}
		}
		else
		{
			Debug.Log("NO VOICE CLIP!");
		}
		introCamera.SetActive(value: false);
		messegePanel.GetComponent<PanelConfig>().setActorGameObjects(actorGameObjects);
		messegePanel.GetComponent<PanelConfig>().setTalkController(this);
		messegePanel.GetComponent<PanelConfig>().setCameras(cameras);
		messegePanel.GetComponent<PanelManager>().StartEventFromJSONPath(currentScenePath);
	}

	public void finish()
	{
		isTalking = false;
		outroCamera.SetActive(value: true);
		StartCoroutine(changeLevel());
	}

	public void setIsTalking(bool isTalking)
	{
		this.isTalking = isTalking;
	}

	public void setActivator(GameObject activator)
	{
	}

	private IEnumerator changeLevel()
	{
		fadeInScript.BeginFade(1);
		StartCoroutine(FadeOutHappyMusic());
		indoorAmbience.Stop();
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("ThanksForPlaying");
	}

	private IEnumerator Intro()
	{
		Debug.Log("INTRO START");
		outdoorAmbience.Play();
		introCamera.SetActive(value: true);
		yield return new WaitForSeconds(4f);
		outdoorAmbience.Stop();
		indoorAmbience.Play();
		talk();
	}

	public void setFlag(int flagNumber)
	{
		flags[flagNumber] = true;
		if (flagNumber == 0)
		{
			laloGun.SetActive(value: true);
		}
		if (flagNumber == 1)
		{
			howardGun.SetActive(value: true);
			creepyMusic.Stop();
		}
		if (flagNumber == 2)
		{
			howardMuzzleFlair.SetActive(value: true);
		}
		if (flagNumber == 3)
		{
			bloodSpatter.SetActive(value: true);
		}
		if (flagNumber == 4)
		{
			howardGun.SetActive(value: false);
		}
		if (flagNumber == 5)
		{
			howardEmmy.SetActive(value: true);
			happyMusic.Play();
		}
		if (flagNumber == 6)
		{
			StartCoroutine(FadeInCreepyMusic());
		}
		if (flagNumber == 7)
		{
			StartCoroutine(FadeInSappyMusic());
		}
		if (flagNumber == 8)
		{
			StartCoroutine(FadeOutSappyMusic());
		}
		_ = 9;
	}

	private IEnumerator FadeInCreepyMusic()
	{
		creepyMusic.Play();
		float volume = 0f;
		while (volume < 1f)
		{
			volume += Time.deltaTime * 0.5f;
			creepyMusic.volume = volume;
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator FadeInHappyMusic()
	{
		happyMusic.Play();
		float volume = 0f;
		while (volume < 1f)
		{
			volume += Time.deltaTime * 0.7f;
			happyMusic.volume = volume;
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator FadeOutHappyMusic()
	{
		float volume = happyMusic.volume;
		while (volume > 0f)
		{
			volume -= Time.deltaTime * 0.5f;
			happyMusic.volume = volume;
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator FadeInSappyMusic()
	{
		sappyMusic.Play();
		float volume = 0f;
		while (volume < 1f)
		{
			volume += Time.deltaTime * 0.5f;
			sappyMusic.volume = volume;
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator FadeOutSappyMusic()
	{
		float volume = 1f;
		while (volume > 0f)
		{
			volume -= Time.deltaTime * 0.5f;
			sappyMusic.volume = volume;
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(0f);
	}
}
