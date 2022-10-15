using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfig : MonoBehaviour
{
	private bool isActive;

	public Text characterNameText;

	public Text messegeText;

	public GameObject continueIcon;

	public GameObject choicesPanel;

	public GameObject textPanel;

	public AudioSource voice;

	public AudioSource selectionSound;

	public AudioSource selectionSound2;

	public List<AudioClip> voicesList;

	public SoundEffectsSystem soundEffectsSystem;

	private float currentTime;

	private float messegeSpeed = 0.02f;

	private PanelManager panelManager;

	public ChoicePanel choicePanelScript;

	private int currentIndex;

	private string currentMessegeTextCopy;

	private int currentLetter;

	private GameObject[] actorGameObjects;

	public Button[] optionButtons;

	private bool currentlyHaveOptions;

	private ITalkController myTalkController;

	public GameObject[] cameras;

	private bool atEndOfMessege;

	private List<int> currSpeakingActorsList;

	private string currentSoundEffectAfterMessage;

	private string playerName;

	private Branch currentBranchCopy;

	private int stepIndexCopy;

	private bool waitingForTimer;

	private List<string> currChoicesTextCopy;

	public float debounceTime;

	private bool choiceButtonsShown;

	private float buttonFadeOutSpeed = 4f;

	private float buttonFadeInSpeed = 8f;

	private void Start()
	{
		panelManager = GetComponent<PanelManager>();
		Button[] array = optionButtons;
		foreach (Button obj in array)
		{
			obj.gameObject.SetActive(value: false);
			obj.enabled = false;
		}
		currChoicesTextCopy = new List<string>();
	}

	public void Configure(Branch currentBranch, int stepIndex)
	{
		if (currentBranch.messages.Length == 0)
		{
			return;
		}
		Message message = currentBranch.messages[stepIndex];
		if (message.flagNumber >= 0)
		{
			myTalkController.setFlag(message.flagNumber);
		}
		if (message.hideMessageBox)
		{
			textPanel.SetActive(value: false);
		}
		else
		{
			textPanel.SetActive(value: true);
		}
		if (message.advanceAfterXSeconds > 0f)
		{
			atEndOfMessege = false;
			waitingForTimer = true;
			StartCoroutine("WaitForTimeThenAdvance", message.advanceAfterXSeconds);
		}
		currSpeakingActorsList = new List<int>();
		for (int i = 0; i < message.actorStates.Length; i++)
		{
			if (message.actorStates[i].isSpeaking)
			{
				currSpeakingActorsList.Add(message.actorStates[i].actorNumber);
			}
		}
		currentSoundEffectAfterMessage = message.soundEffectAfterMessage;
		if (myTalkController != null)
		{
			myTalkController.setIsTalking(isTalking: true);
		}
		atEndOfMessege = false;
		currentBranchCopy = currentBranch;
		stepIndexCopy = stepIndex;
		continueIcon.SetActive(value: false);
		checkActorStates(message);
		if (message.soundEffectBeforeMessage != null && message.soundEffectBeforeMessage.Length > 0 && (bool)soundEffectsSystem)
		{
			soundEffectsSystem.playSoundEffect(message.soundEffectBeforeMessage);
		}
		checkCurrCameraNum(message);
		checkCurrCameraEffect(message.cameraNum, message.cameraEffect);
		if (currentBranch.choices != null && currentBranch.choices.Length != 0 && stepIndex == currentBranch.messages.Length - 1)
		{
			currChoicesTextCopy = new List<string>();
			Choice[] choices = currentBranch.choices;
			foreach (Choice choice in choices)
			{
				currChoicesTextCopy.Add(choice.choiceText);
			}
			currentlyHaveOptions = true;
		}
		else
		{
			currentlyHaveOptions = false;
		}
		if (message.name == "<playername>")
		{
			characterNameText.text = playerName;
		}
		else
		{
			characterNameText.text = message.name;
		}
		currentMessegeTextCopy = message.messageText;
		if (currentMessegeTextCopy.Contains("<playername>"))
		{
			currentMessegeTextCopy = currentMessegeTextCopy.Replace("<playername>", playerName);
			Debug.Log(currentMessegeTextCopy);
		}
		messegeText.text = "";
		currentLetter = 0;
		if (isActive)
		{
			currentIndex++;
		}
		else
		{
			messegeText.text = "";
		}
	}

	private void Update()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		float num = 0f;
		currentTime += Time.deltaTime;
		if (debounceTime > 0f)
		{
			debounceTime -= Time.deltaTime;
		}
		if (!isActive)
		{
			return;
		}
		if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) && (!atEndOfMessege || !(debounceTime > 0f)))
		{
			advance();
		}
		if (!(currentTime >= messegeSpeed))
		{
			return;
		}
		currentIndex++;
		if (atEndOfMessege)
		{
			return;
		}
		if (currentLetter < currentMessegeTextCopy.Length)
		{
			if (!char.IsWhiteSpace(currentMessegeTextCopy[currentLetter]))
			{
				flag4 = true;
			}
			if (currentMessegeTextCopy[currentLetter] == '.' || currentMessegeTextCopy[currentLetter] == '?' || currentMessegeTextCopy[currentLetter] == '!')
			{
				flag = true;
			}
			else if (currentMessegeTextCopy[currentLetter] == ',')
			{
				flag2 = true;
			}
			if (currentMessegeTextCopy[currentLetter] == '<')
			{
				flag3 = true;
				int num2 = currentMessegeTextCopy.IndexOf('>', currentLetter);
				if (currentMessegeTextCopy.Substring(currentLetter + 1, 5) == "pause")
				{
					int num3 = currentMessegeTextCopy.IndexOf('=', currentLetter);
					int num4 = num2 - num3;
					string s = string.Empty;
					try
					{
						s = currentMessegeTextCopy.Substring(num3 + 1, num4 - 1);
					}
					catch (Exception message)
					{
						Debug.Log("ERROR PARSING PAUSE TIME IN JSON TAG");
						Debug.Log(message);
					}
					num = float.Parse(s);
				}
				currentLetter = num2 + 1;
				flag4 = false;
			}
			else
			{
				messegeText.text += currentMessegeTextCopy[currentLetter];
				currentLetter++;
			}
			if (flag4)
			{
				if (!voice.isPlaying)
				{
					if (voicesList.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, voicesList.Count);
						voice.clip = voicesList[index];
					}
					voice.Play();
				}
				foreach (int currSpeakingActors in currSpeakingActorsList)
				{
					actorGameObjects[currSpeakingActors].GetComponent<ICanTalk>().startTalking();
				}
			}
		}
		else
		{
			foreach (int currSpeakingActors2 in currSpeakingActorsList)
			{
				actorGameObjects[currSpeakingActors2].GetComponent<ICanTalk>().stopTalking();
			}
			FinishMessage();
		}
		if (flag3)
		{
			currentTime = 0f - num;
			flag3 = false;
			{
				foreach (int currSpeakingActors3 in currSpeakingActorsList)
				{
					actorGameObjects[currSpeakingActors3].GetComponent<ICanTalk>().stopTalking();
				}
				return;
			}
		}
		if (flag)
		{
			if (currentLetter == currentMessegeTextCopy.Length)
			{
				currentTime = messegeSpeed;
			}
			else
			{
				currentTime = -0.5f;
			}
			flag = false;
			{
				foreach (int currSpeakingActors4 in currSpeakingActorsList)
				{
					actorGameObjects[currSpeakingActors4].GetComponent<ICanTalk>().stopTalking();
				}
				return;
			}
		}
		if (flag2)
		{
			if (currentLetter == currentMessegeTextCopy.Length)
			{
				currentTime = messegeSpeed;
			}
			else
			{
				currentTime = -0.25f;
			}
			flag2 = false;
			{
				foreach (int currSpeakingActors5 in currSpeakingActorsList)
				{
					actorGameObjects[currSpeakingActors5].GetComponent<ICanTalk>().stopTalking();
				}
				return;
			}
		}
		currentTime = 0f;
	}

	public void clear()
	{
		isActive = false;
		characterNameText.text = "";
		messegeText.text = "";
		foreach (int currSpeakingActors in currSpeakingActorsList)
		{
			actorGameObjects[currSpeakingActors].GetComponent<ICanTalk>().stopTalking();
		}
		continueIcon.SetActive(value: false);
		if (myTalkController != null)
		{
			myTalkController.setIsTalking(isTalking: false);
		}
		GameObject[] array = cameras;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		voicesList = new List<AudioClip>();
		forceKillButtons();
		if (myTalkController != null)
		{
			myTalkController.finish();
		}
	}

	private void checkActorStates(Message currentMessage)
	{
		if (currentMessage.actorStates == null)
		{
			return;
		}
		ActorState[] actorStates = currentMessage.actorStates;
		foreach (ActorState actorState in actorStates)
		{
			if (actorGameObjects[actorState.actorNumber] != null)
			{
				if (!string.IsNullOrWhiteSpace(actorState.faceEmotion))
				{
					switch (actorState.faceEmotion)
					{
					case "Happy":
						actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Happy();
						break;
					case "Sad":
						actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Sad();
						break;
					case "Angry":
						actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Angry();
						break;
					case "Blank":
						actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Blank();
						break;
					case "Blush":
						actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Blush();
						break;
					case "Wink":
						actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Wink();
						break;
					case "Worried":
						actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Worried();
						break;
					default:
						Debug.Log("faceEmotion: " + actorState.faceEmotion + " not found. Check spelling / capitals");
						break;
					}
				}
				else
				{
					actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Blank();
				}
			}
			if (actorState.teleportTo)
			{
				actorGameObjects[actorState.actorNumber].GetComponent<IActor>().TeleportTo(actorState.teleportToPosition);
			}
			if (actorState.walkTo)
			{
				actorGameObjects[actorState.actorNumber].GetComponent<IActor>().WalkTo(actorState.walkToPosition);
			}
			if (actorState.runTo)
			{
				actorGameObjects[actorState.actorNumber].GetComponent<IActor>().RunTo(actorState.runToPosition);
			}
			if (actorState.setRotation)
			{
				actorGameObjects[actorState.actorNumber].GetComponent<IActor>().SetRotation(actorState.setRotationEulerAngles);
			}
			if (actorState.animation != null && actorState.animation.Length > 0)
			{
				actorGameObjects[actorState.actorNumber].GetComponent<IActor>().Animate(actorState.animation);
			}
			if (actorState.lookAtTarget)
			{
				actorGameObjects[actorState.actorNumber].GetComponent<IActor>().LookAt(actorState.lookTargetPosition, actorState.lookSpeed);
			}
			if (actorState.stopLookAtTarget)
			{
				actorGameObjects[actorState.actorNumber].GetComponent<IActor>().StopLookAt(actorState.lookSpeed);
			}
		}
	}

	private void checkCurrCameraNum(Message currentMessage)
	{
		GameObject[] array;
		if (currentMessage.cameraNum >= 0 && currentMessage.cameraNum < cameras.Length)
		{
			array = cameras;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			cameras[currentMessage.cameraNum].SetActive(value: true);
			cameras[currentMessage.cameraNum].GetComponent<CameraEffects>().setInitialPosition();
			return;
		}
		if (currentMessage.cameraNum >= cameras.Length)
		{
			Debug.Log("<color=red>Error: </color> can't access camera: " + currentMessage.cameraNum + " It does not exsist. (remember cameras are zero index based)");
			throw new Exception("ERROR! can't access camera: " + currentMessage.cameraNum + " It does not exsist. (remember cameras are zero index based)");
		}
		Debug.Log("<color=red>Error: </color> currCameraNum is < 0");
		Debug.Log("<color=red>DEFAULTING currCamera TO ZERO!</color> ");
		currentMessage.cameraNum = 0;
		array = cameras;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		cameras[0].SetActive(value: true);
	}

	public void checkCurrCameraEffect(int currCamera, string cameraEffect)
	{
		if (currCamera >= cameras.Length)
		{
			Debug.Log("ERROR! can't access camera: " + currCamera + " It does not exsist. (remember cameras are zero index based)");
			throw new Exception("ERROR! can't access camera: " + currCamera + " It does not exsist. (remember cameras are zero index based)");
		}
		cameras[currCamera].GetComponent<CameraEffects>().lookAtTarget();
		switch (cameraEffect)
		{
		case "followTarget":
			cameras[currCamera].GetComponent<CameraEffects>().followTarget();
			break;
		case "shake":
			cameras[currCamera].GetComponent<CameraEffects>().shake();
			break;
		case "zoomIn":
			cameras[currCamera].GetComponent<CameraEffects>().zoomIn();
			break;
		case "zoomOut":
			cameras[currCamera].GetComponent<CameraEffects>().zoomOut();
			break;
		case "fadeIn":
			cameras[currCamera].GetComponent<CameraEffects>().fadeIn();
			break;
		case "fadeOut":
			cameras[currCamera].GetComponent<CameraEffects>().fadeOut();
			break;
		case "fillWithColor":
			cameras[currCamera].GetComponent<CameraEffects>().fillWithColor();
			break;
		case "stopFillWithColor":
			cameras[currCamera].GetComponent<CameraEffects>().stopFillWithColor();
			break;
		case "pan":
			cameras[currCamera].GetComponent<CameraEffects>().pan();
			break;
		default:
			cameras[currCamera].GetComponent<CameraEffects>().reset();
			break;
		}
	}

	public void makeChoice(int choice)
	{
		if (choice - 1 < currentBranchCopy.choices.Length)
		{
			int choiceOutcomeBranchId = currentBranchCopy.choices[choice - 1].choiceOutcomeBranchId;
			panelManager.StartEventFromBranchId(choiceOutcomeBranchId);
			hideChoiceButtons(choice - 1);
		}
		else
		{
			Debug.Log("ERROR, Invalid Choice!");
		}
	}

	public bool getCurrentlyHaveOptions()
	{
		return currentlyHaveOptions;
	}

	public void setActorGameObjects(GameObject[] actorGameObjects)
	{
		this.actorGameObjects = actorGameObjects;
	}

	public void setTalkController(ITalkController talkController)
	{
		myTalkController = talkController;
	}

	public void setVoiceAudioClip(AudioClip audioClip)
	{
		voice.clip = audioClip;
	}

	public void setVoicePitch(float pitch)
	{
		voice.pitch = pitch;
	}

	public void addToVoicesList(AudioClip inputClip)
	{
		if (voicesList == null)
		{
			voicesList = new List<AudioClip>();
		}
		voicesList.Add(inputClip);
	}

	public void setCameras(GameObject[] cameras)
	{
		this.cameras = new GameObject[cameras.Length];
		for (int i = 0; i < this.cameras.Length; i++)
		{
			this.cameras[i] = cameras[i];
		}
	}

	public void setPlayerName(string playerName)
	{
		this.playerName = playerName;
	}

	public void advance()
	{
		foreach (int currSpeakingActors in currSpeakingActorsList)
		{
			actorGameObjects[currSpeakingActors].GetComponent<ICanTalk>().stopTalking();
		}
		if (atEndOfMessege)
		{
			if (!checkIfAnyActorsStillMoving() && !getCurrentlyHaveOptions() && !waitingForTimer)
			{
				atEndOfMessege = false;
				panelManager.UpdatePanelState();
				if (selectionSound.isActiveAndEnabled)
				{
					selectionSound.Play();
				}
			}
		}
		else
		{
			debounceTime = 0.2f;
			string replacement = "";
			Regex regex = new Regex("<[^>]*>");
			currentMessegeTextCopy = regex.Replace(currentMessegeTextCopy, replacement);
			currentLetter = currentMessegeTextCopy.Length;
			messegeText.text = currentMessegeTextCopy;
			FinishMessage();
		}
	}

	public void FinishMessage()
	{
		if (!atEndOfMessege && !checkIfAnyActorsStillMoving())
		{
			continueIcon.SetActive(value: true);
			atEndOfMessege = true;
			if (stepIndexCopy == currentBranchCopy.messages.Length - 1 && currentBranchCopy.choices != null && currentBranchCopy.choices.Length != 0)
			{
				showChoiceButtons();
			}
			if (currentSoundEffectAfterMessage != null && currentSoundEffectAfterMessage.Length > 0 && (bool)soundEffectsSystem)
			{
				soundEffectsSystem.playSoundEffect(currentSoundEffectAfterMessage);
			}
		}
	}

	private bool checkIfAnyActorsStillMoving()
	{
		GameObject[] array = actorGameObjects;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<IActor>().checkIfMoving())
			{
				return true;
			}
		}
		return false;
	}

	public void showChoiceButtons()
	{
		if (currentBranchCopy.choices == null || currentBranchCopy.choices.Length == 0)
		{
			return;
		}
		if (!choiceButtonsShown)
		{
			StartCoroutine("fadeInChoiceButtons");
			continueIcon.SetActive(value: false);
			choicesPanel.SetActive(value: true);
			choicePanelScript.reset();
			for (int i = 0; i < currentBranchCopy.choices.Length; i++)
			{
				optionButtons[i].gameObject.SetActive(value: true);
			}
		}
		else
		{
			Debug.Log("Buttons still shown, wait and try again. " + currentMessegeTextCopy);
			continueIcon.SetActive(value: false);
			StartCoroutine("tryShowChoiceButtonsAgain");
		}
	}

	private IEnumerator tryShowChoiceButtonsAgain()
	{
		float waitTime = 0f;
		while (choiceButtonsShown)
		{
			waitTime += Time.deltaTime;
			yield return null;
		}
		showChoiceButtons();
	}

	private IEnumerator fadeInChoiceButtons()
	{
		int num = 0;
		foreach (string item in currChoicesTextCopy)
		{
			optionButtons[num].GetComponentInChildren<Text>().text = item;
			num++;
		}
		float alpha = 0f;
		Button[] array;
		while (alpha < 1f)
		{
			alpha += Time.deltaTime * buttonFadeInSpeed;
			array = optionButtons;
			foreach (Button obj in array)
			{
				Image component = obj.GetComponent<Image>();
				component.color = new Color(component.color.r, component.color.g, component.color.b, alpha);
				foreach (Transform item2 in obj.gameObject.transform)
				{
					Text component2 = item2.GetComponent<Text>();
					if ((bool)component2)
					{
						component2.color = new Color(component2.color.r, component2.color.g, component2.color.b, alpha);
					}
				}
			}
			yield return null;
		}
		array = optionButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		choiceButtonsShown = true;
	}

	public void hideChoiceButtons(int lastChoice)
	{
		if (choiceButtonsShown)
		{
			StartCoroutine("fadeOutChoiceButtons", lastChoice);
		}
	}

	private IEnumerator fadeOutChoiceButtons(int lastChoice)
	{
		Button[] array = optionButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		float alpha2 = 1f;
		while (alpha2 > 0f)
		{
			alpha2 -= Time.deltaTime * buttonFadeOutSpeed;
			for (int j = 0; j < optionButtons.Length; j++)
			{
				if (j != lastChoice)
				{
					setButtonAlpha(optionButtons[j], alpha2);
				}
			}
			yield return null;
		}
		yield return new WaitForSeconds(0.4f);
		alpha2 = 1f;
		while (alpha2 > 0f)
		{
			alpha2 -= Time.deltaTime * buttonFadeOutSpeed;
			setButtonAlpha(optionButtons[lastChoice], alpha2);
			yield return null;
		}
		choicesPanel.SetActive(value: false);
		array = optionButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: false);
		}
		choiceButtonsShown = false;
	}

	private void setButtonAlpha(Button b, float alpha)
	{
		Image component = b.GetComponent<Image>();
		component.color = new Color(component.color.r, component.color.g, component.color.b, alpha);
		foreach (Transform item in b.gameObject.transform)
		{
			Text component2 = item.GetComponent<Text>();
			if ((bool)component2)
			{
				component2.color = new Color(component2.color.r, component2.color.g, component2.color.b, alpha);
			}
		}
	}

	private void forceKillButtons()
	{
		for (int i = 0; i < optionButtons.Length; i++)
		{
			setButtonAlpha(optionButtons[i], 0f);
		}
		Button[] array = optionButtons;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].gameObject.SetActive(value: false);
		}
		choiceButtonsShown = false;
	}

	public bool getIsActive()
	{
		return isActive;
	}

	public void setIsActive(bool active)
	{
		isActive = active;
	}

	private IEnumerator WaitForTimeThenAdvance(float waitTime)
	{
		Debug.Log("START WAITING: " + waitTime);
		yield return new WaitForSeconds(waitTime);
		Debug.Log("STOP WAITING");
		atEndOfMessege = true;
		waitingForTimer = false;
		advance();
	}
}
