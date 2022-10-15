using UnityEngine;

public class Banana : Item
{
	private Rigidbody rb;

	public AudioSource hitGroundSound;

	protected override void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	protected override void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.2f)
		{
			rb.velocity = Vector3.zero;
			hitGroundSound.Play();
			rb.isKinematic = true;
			base.enabled = false;
		}
	}
}
