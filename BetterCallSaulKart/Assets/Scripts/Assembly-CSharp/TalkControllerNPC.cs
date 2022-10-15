using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkControllerNPC : MonoBehaviour, ITalkController
{
	public GameObject messegePanel;

	public bool isTalking;

	public GameObject[] actorGameObjects;

	public string JSONPath;

	public string JSONPath2;

	public string JSONPath3;

	public string JSONPath4;

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

	private bool inRange;

	public GameObject highlightIcon;

	public bool playMusic;

	public AudioSource musicToPlay;

	public bool isKramer;

	private void Start()
	{
		currentScenePath = JSONPath;
		flags = new bool[10];
	}

	private void Update()
	{
		if (inRange && !isTalking && Input.GetButtonDown("Jump"))
		{
			hideAdventureJerry();
			if (playMusic && !musicToPlay.isPlaying)
			{
				StartCoroutine("startMusic");
			}
			talk();
		}
	}

	public void talk()
	{
		if (isTalking)
		{
			return;
		}
		highlightIcon.SetActive(value: false);
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
		if (isKramer)
		{
			actorGameObjects[1].GetComponent<FaceBlinkTwitch>().stopAll();
			actorGameObjects[1].GetComponent<FaceBlinkTwitch>().dead = true;
			actorGameObjects[1].GetComponent<FaceBlinkTwitch>().enabled = false;
			actorGameObjects[1].GetComponent<Animator>().speed = 0f;
			SkinnedMeshRenderer component = actorGameObjects[1].GetComponent<JawFlapFace>().face.GetComponent<SkinnedMeshRenderer>();
			component.SetBlendShapeWeight(12, 100f);
			component.SetBlendShapeWeight(13, 100f);
		}
		isTalking = false;
		highlightIcon.SetActive(value: true);
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

	private IEnumerator startMusic()
	{
		musicToPlay.Play();
		float volume = 0f;
		while (volume < 1f)
		{
			volume += Time.deltaTime * 0.5f;
			musicToPlay.volume = volume;
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(0f);
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			inRange = true;
			highlightIcon.SetActive(value: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && inRange)
		{
			inRange = false;
			highlightIcon.SetActive(value: false);
		}
	}
}
