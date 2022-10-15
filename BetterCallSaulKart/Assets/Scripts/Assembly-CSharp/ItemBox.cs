using UnityEngine;

public class ItemBox : MonoBehaviour
{
	public bool isHidden;

	private float currHideTime;

	private float maxHideTime = 3f;

	public GameObject model;

	public GameObject itemCollectEffect;

	public AudioSource breakSound;

	private void Start()
	{
		isHidden = false;
	}

	private void Update()
	{
		if (isHidden)
		{
			currHideTime += Time.deltaTime;
			if (currHideTime >= maxHideTime)
			{
				currHideTime = 0f;
				Show();
			}
		}
	}

	public void Hide()
	{
		if (!isHidden)
		{
			isHidden = true;
			model.SetActive(value: false);
			GetComponent<BoxCollider>().enabled = false;
			Object.Instantiate(itemCollectEffect, base.transform.position, Quaternion.identity);
			if ((bool)breakSound)
			{
				breakSound.Play();
			}
		}
	}

	public void Show()
	{
		isHidden = false;
		model.SetActive(value: true);
		GetComponent<BoxCollider>().enabled = true;
	}
}
