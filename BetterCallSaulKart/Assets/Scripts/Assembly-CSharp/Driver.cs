using UnityEngine;

public class Driver : MonoBehaviour
{
	public Animator animController;

	public Transform itemHoldLocation;

	private bool isHoldingItem;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void StartHoldItem()
	{
		animController.SetTrigger("HoldItemTrigger");
		animController.SetBool("HoldingItem", value: true);
		isHoldingItem = true;
	}

	public void StopHoldItem()
	{
		animController.SetBool("HoldingItem", value: false);
		isHoldingItem = false;
	}

	public void ThrowItem()
	{
		animController.SetTrigger("ThrowItem");
	}

	public void Cheer()
	{
		animController.SetTrigger("Cheer");
	}

	public void Hurt()
	{
		animController.SetTrigger("Hurt");
	}

	public void Sad()
	{
		animController.SetTrigger("Sad");
	}

	public void Idle()
	{
		if (!isHoldingItem)
		{
			animController.SetTrigger("Idle");
		}
	}
}
