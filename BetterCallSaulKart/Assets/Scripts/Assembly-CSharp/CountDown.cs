using UnityEngine;

public class CountDown : MonoBehaviour
{
	public UIManager uIManager;

	private float currTime = 5f;

	private bool hitThree;

	private bool hitTwo;

	private bool hitOne;

	private bool hitZero;

	public AudioSource countDownNumberSound;

	public AudioSource countDownGoSound;

	public AudioSource startRace;

	public PlaceTracker placeTracker;

	public bool skipCountDown;

	private void Start()
	{
		uIManager.countdown3.gameObject.SetActive(value: false);
		uIManager.countdown2.gameObject.SetActive(value: false);
		uIManager.countdown1.gameObject.SetActive(value: false);
		uIManager.countdownGo.gameObject.SetActive(value: false);
		foreach (Vehicle racerVehicle in placeTracker.RacerVehicles)
		{
			racerVehicle.isFrozen = true;
		}
		uIManager.countdown3.gameObject.GetComponent<CountDownBounce>().enabled = false;
		uIManager.countdown2.gameObject.GetComponent<CountDownBounce>().enabled = false;
		uIManager.countdown1.gameObject.GetComponent<CountDownBounce>().enabled = false;
		uIManager.countdownGo.gameObject.GetComponent<CountDownBounce>().enabled = false;
		uIManager.countdown3.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
		uIManager.countdown2.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
		uIManager.countdown1.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
		uIManager.countdownGo.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
		if (skipCountDown)
		{
			foreach (Vehicle racerVehicle2 in placeTracker.RacerVehicles)
			{
				racerVehicle2.isFrozen = false;
			}
			placeTracker.hasRaceStarted = true;
			placeTracker.StartMusic();
			base.enabled = false;
		}
		else
		{
			startRace.Play();
		}
	}

	private void Update()
	{
		currTime -= Time.deltaTime;
		if (currTime <= -1f)
		{
			uIManager.countdown3.gameObject.SetActive(value: false);
			uIManager.countdown2.gameObject.SetActive(value: false);
			uIManager.countdown1.gameObject.SetActive(value: false);
			uIManager.countdownGo.gameObject.SetActive(value: false);
			base.enabled = false;
		}
		if (currTime <= 0f)
		{
			StartRace();
		}
		else if (currTime <= 1f)
		{
			if (!hitOne)
			{
				hitOne = true;
				uIManager.countdown1.gameObject.SetActive(value: true);
				uIManager.countdown1.gameObject.GetComponent<CountDownBounce>().currTime = 0f;
				uIManager.countdown1.gameObject.GetComponent<CountDownBounce>().enabled = true;
				uIManager.countdown2.gameObject.SetActive(value: false);
				uIManager.countdown3.gameObject.SetActive(value: false);
				countDownNumberSound.PlayOneShot(countDownNumberSound.clip);
			}
		}
		else if (currTime <= 2f)
		{
			if (!hitTwo)
			{
				hitTwo = true;
				uIManager.countdown2.gameObject.SetActive(value: true);
				uIManager.countdown2.gameObject.GetComponent<CountDownBounce>().currTime = 0f;
				uIManager.countdown2.gameObject.GetComponent<CountDownBounce>().enabled = true;
				uIManager.countdown3.gameObject.SetActive(value: false);
				countDownNumberSound.PlayOneShot(countDownNumberSound.clip);
			}
		}
		else if (currTime <= 3f && !hitThree)
		{
			hitThree = true;
			uIManager.countdown3.gameObject.SetActive(value: true);
			uIManager.countdown3.gameObject.GetComponent<CountDownBounce>().currTime = 0f;
			uIManager.countdown3.gameObject.GetComponent<CountDownBounce>().enabled = true;
			countDownNumberSound.Play();
		}
	}

	public void StartRace()
	{
		if (hitZero)
		{
			return;
		}
		hitZero = true;
		uIManager.countdownGo.gameObject.SetActive(value: true);
		foreach (Vehicle racerVehicle in placeTracker.RacerVehicles)
		{
			racerVehicle.isFrozen = false;
		}
		placeTracker.hasRaceStarted = true;
		uIManager.countdownGo.gameObject.GetComponent<CountDownBounce>().currTime = 0f;
		uIManager.countdownGo.gameObject.GetComponent<CountDownBounce>().enabled = true;
		uIManager.countdown1.gameObject.SetActive(value: false);
		uIManager.countdown2.gameObject.SetActive(value: false);
		uIManager.countdown3.gameObject.SetActive(value: false);
		countDownGoSound.Play();
		placeTracker.StartMusic();
	}
}
