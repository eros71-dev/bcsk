using UnityEngine;

public class DealDammage : MonoBehaviour
{
	public GameObject dammageEffect;

	public bool destroyOnDealDammage;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.gameObject.GetComponent<Vehicle>())
		{
			if (TryGetComponent<Item>(out var component))
			{
				other.gameObject.GetComponent<Vehicle>().TakeDammage(component);
			}
			else
			{
				other.gameObject.GetComponent<Vehicle>().TakeDammage(null);
			}
			if ((bool)dammageEffect)
			{
				Object.Instantiate(dammageEffect, base.transform.position, Quaternion.identity);
			}
			if (destroyOnDealDammage)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
