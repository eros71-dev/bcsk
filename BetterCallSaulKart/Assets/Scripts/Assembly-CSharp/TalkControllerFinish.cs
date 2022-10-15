using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkControllerFinish : MonoBehaviour, ITalkController
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

	private bool inRange;

	public GameObject highlightIcon;

	public AudioSource[] soundsToFadeOut;

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
		isTalking = false;
		cameras[1].SetActive(value: true);
		StartCoroutine("changeLevel");
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
		yield return new WaitForSeconds(0.5f);
		float volume = 1f;
		while (volume > 0f)
		{
			volume -= Time.deltaTime * 0.5f;
			for (int i = 0; i < soundsToFadeOut.Length; i++)
			{
				soundsToFadeOut[i].volume = volume;
			}
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene("Epilogue");
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
