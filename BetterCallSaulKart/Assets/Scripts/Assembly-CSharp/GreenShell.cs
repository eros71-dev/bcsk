using System.Collections;
using UnityEngine;

public class GreenShell : Item
{
	public GameObject model;

	public GameObject dammageEffect;

	private Vector3 lastFrameVelocity;

	private Rigidbody rb;

	private float currDieTime;

	private float maxDieTime = 10f;

	private int numBounces;

	public AudioSource bounceSound;

	public AudioSource dieSound;

	public AudioSource moveSound;

	private bool dead;

	private float speed = 30f;

	public GameObject breakSound;

	protected override void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	protected override void Update()
	{
		lastFrameVelocity = rb.velocity;
		model.transform.Rotate(new Vector3(0f, 500f, 0f) * Time.deltaTime);
		currDieTime += Time.deltaTime;
		if (currDieTime >= maxDieTime && !dead)
		{
			StartCoroutine(Die());
		}
	}

	private void FixedUpdate()
	{
		rb.AddForce(new Vector3(0f, -10f, 0f));
		if (rb.velocity.y > 5f)
		{
			rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Dammage"))
		{
			if (!collision.gameObject.GetComponent<Explosion>())
			{
				Object.Destroy(collision.gameObject);
				if (!dead)
				{
					StartCoroutine(Die());
				}
			}
		}
		else if (numBounces < 4)
		{
			Bounce(collision.contacts[0].normal);
		}
		else if (!dead)
		{
			StartCoroutine(Die());
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.gameObject.GetComponent<Vehicle>())
		{
			other.gameObject.GetComponent<Vehicle>().TakeDammage(this);
			if (!dead)
			{
				StartCoroutine(Die());
			}
		}
	}

	private void Bounce(Vector3 collisionNormal)
	{
		if (Mathf.Abs(collisionNormal.x) + Mathf.Abs(collisionNormal.z) > 0.9f)
		{
			Vector3 vector = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
			vector = new Vector3(vector.x, 0f, vector.z);
			rb.velocity = vector * speed;
			bounceSound.Play();
			numBounces++;
			speed -= 3.5f;
			if (speed <= 0f)
			{
				speed = 1f;
			}
		}
	}

	private IEnumerator Die()
	{
		Object.Instantiate(breakSound, base.transform.position, Quaternion.identity);
		Object.Instantiate(dammageEffect, base.transform.position, Quaternion.identity);
		dead = true;
		itemIsActive = false;
		moveSound.Stop();
		model.SetActive(value: false);
		GetComponent<SphereCollider>().enabled = false;
		yield return new WaitForSeconds(2f);
		Object.Destroy(base.gameObject);
	}
}
