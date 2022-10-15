using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInScript : MonoBehaviour
{
	private Texture2D fadeOutTexture;

	public Color fadeColor = Color.black;

	public float fadeSpeed = 0.5f;

	private int drawDepth = -1000;

	private float alpha = 1f;

	private int fadeDir = -1;

	public bool fadeInOnStart = true;

	public float fadeInOnStartDelay;

	private float currTime;

	private void OnGUI()
	{
		if (currTime >= fadeInOnStartDelay)
		{
			alpha += (float)fadeDir * fadeSpeed * Time.unscaledDeltaTime;
			alpha = Mathf.Clamp01(alpha);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fadeOutTexture);
		}
		else
		{
			currTime += Time.deltaTime;
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fadeOutTexture);
		}
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		setupFadeTexture();
		if (fadeInOnStart)
		{
			BeginFade(-1);
		}
		else
		{
			alpha = 0f;
		}
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
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
}
