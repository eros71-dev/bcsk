using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
	public AudioSource theme;

	public AudioSource returnSound;

	public AudioSource cheaterAudioSource;

	public AudioSource deleteAudioSource;

	private Texture2D fadeOutTexture;

	public Color fadeColor = Color.black;

	private float fadeSpeed = 1f;

	private int drawDepth = -1000;

	private float alpha = 1f;

	private int fadeDir = -1;

	private float audioFadeTime = 1f;

	private bool returnPressBool;

	public string titleScene;

	private Resolution[] resolutions;

	public Dropdown resolutionDropDown;

	public Toggle fullScreenToggle;

	public Dropdown qualityDropDown;

	public GameObject saveDeletedText;

	public GameObject cheaterText;

	private float cheatTimeCurr;

	private void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		configUI();
		returnPressBool = false;
		if (theme != null)
		{
			theme.Play();
			theme.volume = 0f;
			StartCoroutine("fadeInSound");
		}
	}

	private void configUI()
	{
		resolutions = Screen.resolutions;
		resolutionDropDown.ClearOptions();
		List<string> list = new List<string>();
		int value = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string item = resolutions[i].width + " x " + resolutions[i].height;
			list.Add(item);
			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				value = i;
			}
		}
		resolutionDropDown.AddOptions(list);
		resolutionDropDown.value = value;
		resolutionDropDown.RefreshShownValue();
		qualityDropDown.ClearOptions();
		qualityDropDown.AddOptions(QualitySettings.names.ToList());
		qualityDropDown.value = QualitySettings.GetQualityLevel();
		fullScreenToggle.isOn = Screen.fullScreen;
		getGlobalPlayerPrefs();
	}

	public void getGlobalPlayerPrefs()
	{
		try
		{
			if (GlobalPlayerPrefs.quality.Length > 0)
			{
				int num = int.Parse(GlobalPlayerPrefs.quality);
				QualitySettings.SetQualityLevel(num);
				qualityDropDown.value = num;
			}
			if (GlobalPlayerPrefs.fullscreen.Length > 0)
			{
				bool isOn = (Screen.fullScreen = bool.Parse(GlobalPlayerPrefs.fullscreen));
				fullScreenToggle.isOn = isOn;
			}
			if (GlobalPlayerPrefs.screenWidth.Length <= 0 || GlobalPlayerPrefs.screenHeight.Length <= 0)
			{
				return;
			}
			int width = int.Parse(GlobalPlayerPrefs.screenWidth);
			int height = int.Parse(GlobalPlayerPrefs.screenHeight);
			Resolution resolution = default(Resolution);
			resolution.width = width;
			resolution.height = height;
			int value = 0;
			for (int i = 0; i < resolutions.Length; i++)
			{
				if (resolutions[i].width == resolution.width && resolutions[i].height == resolution.height)
				{
					value = i;
				}
			}
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
			resolutionDropDown.value = value;
			resolutionDropDown.RefreshShownValue();
		}
		catch (Exception message)
		{
			Debug.Log("Error parseing playerPrefs");
			Debug.Log(message);
		}
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.C))
		{
			cheatTimeCurr += Time.deltaTime;
			if (cheatTimeCurr >= 4f)
			{
				cheatTimeCurr = 0f;
				Debug.Log("Cheater!");
				GlobalGameData.bestPlaces[1] = 1;
				GlobalGameData.bestPlaces[2] = 1;
				GlobalGameData.bestPlaces[3] = 1;
				GlobalGameData.bestTimes[1] = 30f;
				GlobalGameData.bestTimes[2] = 30f;
				GlobalGameData.bestTimes[3] = 30f;
				cheaterAudioSource.Play();
				cheaterText.SetActive(value: true);
				saveDeletedText.SetActive(value: false);
				GlobalGameData.SaveToPrefs();
			}
		}
	}

	public void returnPress()
	{
		if (!returnPressBool)
		{
			StartCoroutine("returnToTitle");
			StartCoroutine("fadeOutSound");
		}
	}

	private void setupFadeTexture()
	{
		fadeOutTexture = new Texture2D(100, 100);
		for (int i = 0; i < 100; i++)
		{
			for (int j = 0; j < 100; j++)
			{
				fadeOutTexture.SetPixel(i, j, fadeColor);
			}
		}
		fadeOutTexture.Apply();
	}

	private IEnumerator returnToTitle()
	{
		if ((bool)returnSound)
		{
			returnSound.Play();
		}
		BeginFade(1);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(titleScene);
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
			while (theme.volume < 1f && !returnPressBool)
			{
				theme.volume += Time.deltaTime / audioFadeTime;
				yield return null;
			}
		}
	}

	private void OnGUI()
	{
		alpha += (float)fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fadeOutTexture);
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	private void OnEnable()
	{
		setupFadeTexture();
		BeginFade(-1);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
		PlayerPrefs.SetString("fullscreen", isFullScreen.ToString());
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
		PlayerPrefs.SetString("quality", qualityIndex.ToString());
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		PlayerPrefs.SetString("screenWidth", resolution.width.ToString());
		PlayerPrefs.SetString("screenHeight", resolution.height.ToString());
	}

	public void DeleteSave()
	{
		saveDeletedText.SetActive(value: true);
		cheaterText.SetActive(value: false);
		deleteAudioSource.Play();
		GlobalGameData.Reset();
		GlobalGameData.SaveToPrefs();
	}
}
