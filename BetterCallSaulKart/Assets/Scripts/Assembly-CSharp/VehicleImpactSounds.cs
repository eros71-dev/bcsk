using UnityEngine;

public class VehicleImpactSounds : MonoBehaviour
{
	public AudioSource hitObjectSound;

	public GameObject hitEffect;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.impulse.magnitude > 900f && Mathf.Abs(collision.impulse.x) + Mathf.Abs(collision.impulse.x) > 450f && !hitObjectSound.isPlaying)
		{
			hitObjectSound.Play();
			Object.Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
		}
	}
}
