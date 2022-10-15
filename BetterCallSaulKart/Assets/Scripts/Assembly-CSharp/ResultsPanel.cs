using UnityEngine;
using UnityEngine.UI;

public class ResultsPanel : MonoBehaviour
{
	public Image[] racerImages;

	public Image[] rowImages;

	public AudioSource confirmSound;

	private float currTime;

	private FadeInScript fadeInScript;

	private bool isFadingOut;

	public GameObject resultsRows;

	public GameObject continueRetryRows;

	private float size;

	private float colorFade;

	private void Start()
	{
		Debug.Log("Results panel start");
		resultsRows.transform.localScale = Vector3.zero;
		GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
	}

	private void Update()
	{
		size += Time.deltaTime * 4f;
		if (size >= 1f)
		{
			size = 1f;
			resultsRows.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			resultsRows.transform.localScale = new Vector3(size, 1f, 1f);
		}
		colorFade += Time.deltaTime * 4f;
		if (colorFade >= 0.75f)
		{
			colorFade = 0.75f;
		}
		GetComponent<Image>().color = new Color(0f, 0f, 0f, colorFade);
		currTime += Time.deltaTime;
		if (currTime > 1f && Input.GetKeyDown(KeyCode.Space) && !isFadingOut)
		{
			isFadingOut = true;
			Debug.Log("END GAME");
			confirmSound.Play();
			continueRetryRows.SetActive(value: true);
			resultsRows.SetActive(value: false);
		}
	}
}
