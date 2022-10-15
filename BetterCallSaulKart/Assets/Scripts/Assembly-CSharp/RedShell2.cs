using System.Collections;
using UnityEngine;

public class RedShell2 : Item
{
	public GameObject model;

	public GameObject dammageEffect;

	public GameObject target;

	private float currTimeSpeedMultiplier = 0.41f;

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

	private float debounceTime;

	private float topSpeed = 0.8f;

	public GameObject breakSound;

	protected override void Start()
	{
		base.Start();
		rb = GetComponent<Rigidbody>();
		if ((bool)target && target.GetComponent<Vehicle>().isPlayer)
		{
			Debug.Log("Chasing Player");
			topSpeed = 0.55f;
		}
	}

	protected override void Update()
	{
		base.Update();
		_ = target != null;
		debounceTime += Time.deltaTime;
	}

	private void FixedUpdate()
	{
		if (target != null && debounceTime > 0.12f)
		{
			Vector3 vector = Vector3.Normalize(target.transform.position - base.transform.position) * currTimeSpeedMultiplier;
			rb.MovePosition(base.transform.position + vector);
			if (currTimeSpeedMultiplier < topSpeed)
			{
				currTimeSpeedMultiplier += 0.01f;
			}
		}
		else
		{
			rb.MovePosition(base.transform.position + base.transform.forward * 0.75f);
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
		else
		{
			Vector3 normal = collision.contacts[0].normal;
			if (Mathf.Abs(normal.x) + Mathf.Abs(normal.z) > 0.9f && !dead)
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
