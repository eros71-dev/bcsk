using UnityEngine;

public class Explosion : Item
{
	private float currHurtTime;

	private float maxHurtTime = 0.25f;

	private float currDieTime;

	private float maxDieTime = 2f;

	public GameObject destroyObjectEffect;

	public GameObject redSphere;

	public MeshRenderer resSphereMeshRenderer;

	private float alpha = 0.8f;

	protected override void Start()
	{
		resSphereMeshRenderer = redSphere.GetComponent<MeshRenderer>();
	}

	protected override void Update()
	{
		currHurtTime += Time.deltaTime;
		if (currHurtTime >= maxHurtTime)
		{
			GetComponent<SphereCollider>().enabled = false;
			redSphere.SetActive(value: false);
		}
		resSphereMeshRenderer.material.color = new Color(1f, 0f, 0f, alpha);
		if (alpha < 0f)
		{
			alpha = 0f;
		}
		else
		{
			alpha -= Time.deltaTime * 3.5f;
		}
		currDieTime += Time.deltaTime;
		if (currDieTime >= maxDieTime)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Dammage") && !other.gameObject.GetComponent<Explosion>())
		{
			Object.Instantiate(destroyObjectEffect, other.transform.position, Quaternion.identity);
			Object.Destroy(other.gameObject);
		}
	}
}
