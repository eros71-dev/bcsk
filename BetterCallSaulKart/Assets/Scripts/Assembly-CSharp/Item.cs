using UnityEngine;

public class Item : MonoBehaviour
{
	protected bool itemIsActive = true;

	public Vehicle throwBy;

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	public bool GetItemIsActive()
	{
		return itemIsActive;
	}
}
