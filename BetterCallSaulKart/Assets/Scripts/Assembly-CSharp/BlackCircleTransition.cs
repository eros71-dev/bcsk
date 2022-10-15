using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackCircleTransition : MonoBehaviour
{
	public Image blackCirlce;

	public AudioSource wooshInSound;

	public AudioSource wooshOutSound;

	private float transistionSpeed = 80f;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void startFadeOut()
	{
		Debug.Log("Start Fade Out");
		blackCirlce.transform.localScale = new Vector3(0f, 0f, 0f);
		StartCoroutine("fadeOut");
	}

	public void startFadeIn()
	{
		blackCirlce.transform.localScale = new Vector3(30f, 30f, 30f);
		StartCoroutine("fadeIn");
	}

	private IEnumerator fadeOut()
	{
		yield return new WaitForSeconds(0.05f);
		wooshInSound.Play();
		while (blackCirlce.transform.localScale.x < 30f)
		{
			float num = 80f;
			blackCirlce.transform.localScale = new Vector3(blackCirlce.transform.localScale.x + Time.deltaTime * num, blackCirlce.transform.localScale.y + Time.deltaTime * num, 1f);
			yield return null;
		}
	}

	private IEnumerator fadeIn()
	{
		yield return new WaitForSeconds(0.05f);
		wooshOutSound.Play();
		while (blackCirlce.transform.localScale.x > 0f)
		{
			blackCirlce.transform.localScale = new Vector3(blackCirlce.transform.localScale.x - Time.deltaTime * transistionSpeed, blackCirlce.transform.localScale.y - Time.deltaTime * transistionSpeed, 1f);
			yield return null;
		}
		if (blackCirlce.transform.localScale.x < 0f)
		{
			blackCirlce.transform.localScale = new Vector3(0f, 0f, 0f);
		}
	}
}
