using UnityEngine;

public class Bomb : Item
{
	private float currExplodeTime;

	private float maxExplodeTime = 1.75f;

	public GameObject model;

	public Material normalMat;

	public Material blinkMat;

	public GameObject explosion;

	private float blinkTime;

	private bool isBlinking;

	protected override void Start()
	{
	}

	protected override void Update()
	{
		currExplodeTime += Time.deltaTime;
		Blink();
		if (currExplodeTime >= maxExplodeTime)
		{
			Explode();
		}
	}

	private void Blink()
	{
		float num = 20f * currExplodeTime / maxExplodeTime;
		blinkTime += Time.deltaTime * num;
		if (blinkTime >= 1f)
		{
			blinkTime = 0f;
			if (isBlinking)
			{
				model.GetComponent<Renderer>().material = blinkMat;
			}
			else
			{
				model.GetComponent<Renderer>().material = normalMat;
			}
			isBlinking = !isBlinking;
		}
	}

	public void Explode()
	{
		Object.Instantiate(explosion, base.transform.position, Quaternion.identity).GetComponent<Explosion>().throwBy = throwBy;
		Object.Destroy(base.gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		Explode();
	}
}
