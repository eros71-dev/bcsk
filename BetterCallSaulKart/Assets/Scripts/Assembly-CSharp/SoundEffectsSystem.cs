using System;
using UnityEngine;

public class SoundEffectsSystem : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void playSoundEffect(string soundEffect)
	{
		try
		{
			base.transform.Find(soundEffect).gameObject.GetComponent<AudioSource>().Play();
		}
		catch (Exception)
		{
			Debug.Log("<color=red>Error: </color>Sound effect not found: " + soundEffect);
		}
	}
}
