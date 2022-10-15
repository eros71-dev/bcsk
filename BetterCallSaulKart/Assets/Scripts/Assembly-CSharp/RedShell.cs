using System.Collections;
using UnityEngine;

public class RedShell : Item
{
	public GameObject model;

	public GameObject dammageEffect;

	public GameObject target;

	private float currTimeSpeedMultiplier = 1.4f;

	private Vector3 lastFrameVelocity;

	private float minVelocity = 10f;

	private float maxVelocity = 100f;

	private Rigidbody rb;

	private float currTargetDelayTime;

	private float maxTargetDelayTime = 1f;

	private float currDieTime;

	private float maxDieTime = 10f;

	public AudioSource bounceSound;

	public AudioSource dieSound;

	public AudioSource moveSound;

	private float speed = 24f;

	private bool dead;

	private int numBounces;

	protected override void Start()
	{
		base.Start();
		rb = GetComponent<Rigidbody>();
	}

	protected override void Update()
	{
		base.Update();
		_ = target != null;
		currDieTime += Time.deltaTime;
		if (currDieTime >= maxDieTime && !dead)
		{
			StartCoroutine(Die());
		}
		lastFrameVelocity = rb.velocity;
		currTargetDelayTime += Time.deltaTime;
		if (currTargetDelayTime >= maxTargetDelayTime)
		{
			currTimeSpeedMultiplier += Time.deltaTime;
			if (!(target == null))
			{
				rb.velocity = Vector3.Normalize(target.transform.position - base.transform.position) * (currTimeSpeedMultiplier * 10f);
				if (rb.velocity.magnitude > maxVelocity)
				{
					rb.velocity = Vector3.Normalize(rb.velocity) * maxVelocity;
				}
			}
		}
		else if (!(target == null))
		{
			rb.AddForce(Vector3.Normalize(target.transform.position - base.transform.position) * 20f);
		}
	}

	private void FixedUpdate()
	{
		rb.AddForce(new Vector3(0f, -50f, 0f));
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (target == null)
		{
			if (numBounces < 3)
			{
				Bounce(collision.contacts[0].normal);
			}
			else if (!dead)
			{
				StartCoroutine(Die());
			}
		}
		if (collision.gameObject.CompareTag("Dammage") && !collision.gameObject.GetComponent<Explosion>())
		{
			Object.Destroy(collision.gameObject);
			if (!dead)
			{
				StartCoroutine(Die());
			}
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
			speed -= 4f;
			if (speed <= 0f)
			{
				speed = 1f;
			}
			numBounces++;
		}
	}

	private IEnumerator Die()
	{
		Object.Instantiate(dammageEffect, base.transform.position, Quaternion.identity);
		dead = true;
		itemIsActive = false;
		moveSound.Stop();
		model.SetActive(value: false);
		GetComponent<SphereCollider>().enabled = false;
		dieSound.Play();
		yield return new WaitForSeconds(2f);
		Object.Destroy(base.gameObject);
	}
}
