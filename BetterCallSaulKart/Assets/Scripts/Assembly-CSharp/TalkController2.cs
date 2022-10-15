using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkController2 : MonoBehaviour, ITalkController
{
	public GameObject messegePanel;

	public bool isTalking;

	public GameObject[] actorGameObjects;

	public string JSONPath;

	private string currentScenePath;

	public GameObject[] cameras;

	public GameObject mainCamera;

	public List<AudioClip> voicesList;

	public FadeInScript fadeInScript;

	public bool[] flags;

	public int seinScore;

	public GameObject playerObj;

	public GameObject instructionText;

	public GameObject actorJerry;

	public AudioSource[] soundsToFadeIn;

	private void Start()
	{
		currentScenePath = JSONPath;
		flags = new bool[10];
		StartCoroutine("intro");
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
		mainCamera.SetActive(value: false);
		messegePanel.GetComponent<PanelConfig>().setActorGameObjects(actorGameObjects);
		messegePanel.GetComponent<PanelConfig>().setTalkController(this);
		messegePanel.GetComponent<PanelConfig>().setCameras(cameras);
		messegePanel.GetComponent<PanelManager>().StartEventFromJSONPath(currentScenePath);
	}

	public void finish()
	{
		isTalking = false;
		mainCamera.SetActive(value: true);
		showAdventureJerry();
	}

	public void setIsTalking(bool isTalking)
	{
		this.isTalking = isTalking;
	}

	public void setActivator(GameObject activator)
	{
	}

	private IEnumerator intro()
	{
		actorGameObjects[0].GetComponent<IActor>().Animate("Laying");
		mainCamera.SetActive(value: false);
		cameras[0].SetActive(value: true);
		float volume = 0f;
		while (volume < 1f)
		{
			volume += Time.deltaTime * 0.5f;
			for (int i = 0; i < soundsToFadeIn.Length; i++)
			{
				soundsToFadeIn[i].volume = volume;
			}
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(3f);
		talk();
	}

	private IEnumerator changeLevel()
	{
		fadeInScript.BeginFade(1);
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene("Credits");
	}

	public void setFlag(int flagNumber)
	{
		flags[flagNumber] = true;
	}

	public void showAdventureJerry()
	{
		playerObj.SetActive(value: true);
		instructionText.SetActive(value: true);
		actorJerry.SetActive(value: false);
	}

	public void hideAdventureJerry()
	{
		actorJerry.transform.position = playerObj.transform.position;
		playerObj.SetActive(value: false);
		instructionText.SetActive(value: false);
		actorJerry.SetActive(value: true);
	}
}
