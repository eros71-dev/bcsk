using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTracker : MonoBehaviour
{
	public Path path;

	public List<Vehicle> RacerVehicles;

	public Vehicle player;

	public UIManager uIManager;

	public bool hasRaceStarted;

	private float playerTime;

	public AudioSource courseMusic;

	public AudioSource goalFirstPlace;

	public AudioSource goalSecondPlace;

	public AudioSource goalLastPlace;

	public AudioSource resultsFirstPlace;

	public AudioSource resultsLastPlace;

	public AudioSource nextLapSound;

	public AudioSource FinalLapSound;

	public AudioSource raceOverSound;

	public AudioSource hitSoundHitCam;

	public float timePlayerInFirst;

	public VehiclePrefabs vehiclePrefabs;

	private float updatePlaceTimecurr;

	private float updatePlaceTimeMax = 0.1f;

	public FadeInScript fader;

	private bool isShowingHitCam;

	private void Start()
	{
		uIManager.resultsPanel.gameObject.SetActive(value: false);
		SetRacers();
		UpdatePlayerPlaceText();
		UpdatePlaces();
	}

	private void Update()
	{
		if (!player.raceOver)
		{
			updatePlaceTimecurr += Time.deltaTime;
			if (updatePlaceTimecurr >= updatePlaceTimeMax)
			{
				updatePlaceTimecurr = 0f;
				UpdatePlaces();
			}
		}
		if (hasRaceStarted && !player.raceOver)
		{
			uIManager.pauseController.canPause = true;
		}
		else
		{
			uIManager.pauseController.canPause = false;
		}
		if (!hasRaceStarted || player.raceOver)
		{
			return;
		}
		playerTime += Time.deltaTime;
		uIManager.timeText.text = FormatTime(playerTime);
		if (player.place == 1)
		{
			if (player.lap != player.path.laps)
			{
				timePlayerInFirst += Time.deltaTime;
				if (timePlayerInFirst > 45f)
				{
					StartSuperRubberBandingAll();
					timePlayerInFirst = 0f;
				}
				else if (timePlayerInFirst > 25f)
				{
					StartRubberBandingAll();
					timePlayerInFirst = 0f;
				}
			}
		}
		else
		{
			timePlayerInFirst = 0f;
			StopRubberBandingAll();
		}
	}

	private void UpdatePlaces()
	{
		GFG comparer = new GFG();
		RacerVehicles.Sort(comparer);
		int num = 1;
		foreach (Vehicle racerVehicle in RacerVehicles)
		{
			racerVehicle.place = num;
			num++;
			UpdatePlaceTrackerPanel(racerVehicle);
		}
		setPlayerPlaceText(player.place);
	}

	private void setPlayerPlaceText(int place)
	{
		string text = "";
		switch (place)
		{
		case 1:
			text = "1st";
			break;
		case 2:
			text = "2nd";
			break;
		case 3:
			text = "3rd";
			break;
		case 4:
			text = "4th";
			break;
		case 5:
			text = "5th";
			break;
		case 6:
			text = "6th";
			break;
		case 7:
			text = "7th";
			break;
		case 8:
			text = "8th";
			break;
		}
		uIManager.placeText.text = text;
	}

	public GameObject getVehicleAheadOfMe(int myPlace)
	{
		if (myPlace - 2 < 0)
		{
			return null;
		}
		return RacerVehicles[myPlace - 2].gameObject;
	}

	private static void GenTimeSpanFromSeconds(float seconds)
	{
		string text = TimeSpan.FromSeconds(seconds).ToString();
		int startIndex = text.IndexOf(':');
		startIndex = text.IndexOf('.', startIndex);
		if (startIndex < 0)
		{
			text += "        ";
		}
		Debug.Log($"{seconds,21}{text,26}");
	}

	private string FormatTime(float t)
	{
		int num = Mathf.FloorToInt(t / 60f);
		int num2 = Mathf.FloorToInt(t % 60f);
		return $"{num:00}:{num2:00}";
	}

	public void StartMusic()
	{
		courseMusic.Play();
	}

	public void FinishRace()
	{
		if (GlobalGameData.currentCourse > 0)
		{
			if (playerTime < GlobalGameData.bestTimes[GlobalGameData.currentCourse])
			{
				GlobalGameData.bestTimes[GlobalGameData.currentCourse] = playerTime;
			}
			if (player.place < GlobalGameData.bestPlaces[GlobalGameData.currentCourse])
			{
				GlobalGameData.bestPlaces[GlobalGameData.currentCourse] = player.place;
			}
			if (player.place > 1)
			{
				GlobalGameData.numTimesLost[GlobalGameData.currentCourse]++;
			}
			else
			{
				GlobalGameData.numTimesLost[GlobalGameData.currentCourse] = 0;
			}
			GlobalGameData.SaveToPrefs();
		}
		else
		{
			Debug.LogError("currentCourse is: " + GlobalGameData.currentCourse);
		}
		for (int i = 0; i < RacerVehicles.Count; i++)
		{
			uIManager.resultsPanel.racerImages[i].sprite = RacerVehicles[i].racerInfo.racerIcon;
			if (RacerVehicles[i].isPlayer)
			{
				uIManager.resultsPanel.rowImages[i].GetComponent<FlashResultsRow>().enabled = true;
				uIManager.resultsPanel.rowImages[i].color = new Color(uIManager.resultsPanel.rowImages[i].color.r, uIManager.resultsPanel.rowImages[i].color.g, uIManager.resultsPanel.rowImages[i].color.b, 0.5f);
			}
			else
			{
				uIManager.resultsPanel.rowImages[i].GetComponent<FlashResultsRow>().enabled = false;
				uIManager.resultsPanel.rowImages[i].color = new Color(uIManager.resultsPanel.rowImages[i].color.r, uIManager.resultsPanel.rowImages[i].color.g, uIManager.resultsPanel.rowImages[i].color.b, 0f);
			}
		}
		uIManager.goalImage.gameObject.SetActive(value: true);
		uIManager.PlaceTrackerPanel.SetActive(value: false);
		StartCoroutine(FinishRaceMusic(player.place));
	}

	private IEnumerator FinishRaceMusic(int playerPlace)
	{
		courseMusic.Stop();
		raceOverSound.Play();
		while (raceOverSound.isPlaying)
		{
			yield return null;
		}
		switch (playerPlace)
		{
		case 1:
			goalFirstPlace.Play();
			while (goalFirstPlace.isPlaying)
			{
				yield return null;
			}
			resultsFirstPlace.Play();
			break;
		case 2:
		case 3:
			goalSecondPlace.Play();
			while (goalSecondPlace.isPlaying)
			{
				yield return null;
			}
			resultsFirstPlace.Play();
			break;
		default:
			goalLastPlace.Play();
			while (goalLastPlace.isPlaying)
			{
				yield return null;
			}
			resultsLastPlace.Play();
			break;
		}
		uIManager.resultsPanel.gameObject.SetActive(value: true);
		uIManager.placeText.gameObject.SetActive(value: false);
	}

	public void UpdatePlayerPlaceText()
	{
		uIManager.lapText.text = "Lap " + player.lap + "/" + path.laps;
	}

	public void NextLap()
	{
		nextLapSound.Play();
		UpdatePlayerPlaceText();
		if (player.lap == 2)
		{
			uIManager.lap2Image.gameObject.SetActive(value: true);
		}
	}

	public void FinalLap()
	{
		StartCoroutine(FinalLapMusic());
		UpdatePlayerPlaceText();
		uIManager.finalLapImage.gameObject.SetActive(value: true);
	}

	private IEnumerator FinalLapMusic()
	{
		courseMusic.Stop();
		FinalLapSound.Play();
		while (FinalLapSound.isPlaying)
		{
			yield return null;
		}
		courseMusic.time = 0f;
		courseMusic.Play();
		courseMusic.pitch = 1.08f;
	}

	public void StartRubberBandingAll()
	{
		Debug.Log("StartRubberBandingAll");
		foreach (Vehicle racerVehicle in RacerVehicles)
		{
			if (!racerVehicle.isPlayer)
			{
				racerVehicle.StartRubberBanding();
			}
		}
	}

	public void StartSuperRubberBandingAll()
	{
		Debug.Log("StartSuperRubberBandingAll");
		foreach (Vehicle racerVehicle in RacerVehicles)
		{
			if (!racerVehicle.isPlayer)
			{
				racerVehicle.StartSuperRubberBanding();
			}
		}
	}

	public void StopRubberBandingAll()
	{
		foreach (Vehicle racerVehicle in RacerVehicles)
		{
			if (!racerVehicle.isPlayer)
			{
				racerVehicle.StopRubberBanding();
			}
		}
	}

	public void UpdatePlaceTrackerPanel(Vehicle v)
	{
		if (v.place == 1)
		{
			uIManager.placeTrackerPlace1.sprite = v.racerInfo.racerIcon;
			if (v.isPlayer)
			{
				uIManager.placeTrackerPlace1.color = new Color(1f, 1f, 1f, 1f);
				uIManager.placeTrackerRed1.gameObject.SetActive(value: true);
			}
			else
			{
				uIManager.placeTrackerPlace1.color = new Color(1f, 1f, 1f, 0.6f);
				uIManager.placeTrackerRed1.gameObject.SetActive(value: false);
			}
		}
		else if (v.place == 2)
		{
			uIManager.placeTrackerPlace2.sprite = v.racerInfo.racerIcon;
			if (v.isPlayer)
			{
				uIManager.placeTrackerPlace2.color = new Color(1f, 1f, 1f, 1f);
				uIManager.placeTrackerRed2.gameObject.SetActive(value: true);
			}
			else
			{
				uIManager.placeTrackerPlace2.color = new Color(1f, 1f, 1f, 0.6f);
				uIManager.placeTrackerRed2.gameObject.SetActive(value: false);
			}
		}
		else if (v.place == 3)
		{
			uIManager.placeTrackerPlace3.sprite = v.racerInfo.racerIcon;
			if (v.isPlayer)
			{
				uIManager.placeTrackerPlace3.color = new Color(1f, 1f, 1f, 1f);
				uIManager.placeTrackerRed3.gameObject.SetActive(value: true);
			}
			else
			{
				uIManager.placeTrackerPlace3.color = new Color(1f, 1f, 1f, 0.6f);
				uIManager.placeTrackerRed3.gameObject.SetActive(value: false);
			}
		}
		else if (v.place == 4)
		{
			uIManager.placeTrackerPlace4.sprite = v.racerInfo.racerIcon;
			if (v.isPlayer)
			{
				uIManager.placeTrackerPlace4.color = new Color(1f, 1f, 1f, 1f);
				uIManager.placeTrackerRed4.gameObject.SetActive(value: true);
			}
			else
			{
				uIManager.placeTrackerPlace4.color = new Color(1f, 1f, 1f, 0.6f);
				uIManager.placeTrackerRed4.gameObject.SetActive(value: false);
			}
		}
	}

	private void SetRacers()
	{
		bool[] array = new bool[vehiclePrefabs.prefabs.Length];
		int num = UnityEngine.Random.Range(0, 4);
		if (num == 4)
		{
			num = 3;
		}
		int num2 = GlobalGameData.currentCharacter + num;
		foreach (Vehicle racerVehicle in RacerVehicles)
		{
			if (num2 >= vehiclePrefabs.prefabs.Length)
			{
				num2 = 0;
			}
			if (array[num2])
			{
				num2++;
			}
			array[num2] = true;
			if (racerVehicle.isPlayer)
			{
				racerVehicle.SetRacer(GlobalGameData.currentCharacter);
				num2++;
			}
			else
			{
				racerVehicle.SetRacer(num2);
				num2++;
			}
		}
	}

	public void VehicleHitByPlayer(Vehicle v)
	{
		if (!isShowingHitCam && !player.raceOver && !v.isPlayer)
		{
			if ((bool)hitSoundHitCam)
			{
				hitSoundHitCam.Play();
			}
			isShowingHitCam = true;
			v.hitCam.SetActive(value: true);
			uIManager.niceHitImage.gameObject.SetActive(value: true);
			StartCoroutine(HideHitCam(v.hitCam));
		}
	}

	private IEnumerator HideHitCam(GameObject cam)
	{
		yield return new WaitForSeconds(1.75f);
		cam.SetActive(value: false);
		isShowingHitCam = false;
		uIManager.niceHitImage.gameObject.SetActive(value: false);
	}

	public void PauseAllSound()
	{
		courseMusic.Pause();
		foreach (Vehicle racerVehicle in RacerVehicles)
		{
			racerVehicle.PauseAllSound();
		}
	}

	public void ResumeAllSound()
	{
		if (!hasRaceStarted || player.raceOver)
		{
			return;
		}
		courseMusic.Play();
		foreach (Vehicle racerVehicle in RacerVehicles)
		{
			racerVehicle.ResumeAllSound();
		}
	}
}
