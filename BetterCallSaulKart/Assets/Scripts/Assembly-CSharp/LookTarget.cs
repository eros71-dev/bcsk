using UnityEngine;

public class LookTarget : MonoBehaviour
{
	private Vector3 moveToPosition;

	private float moveToSpeed;

	private bool movingToPosition;

	private Transform defaultPosition;

	private bool movingToDefaultPosition;

	private HeadLookAt headLookAt;

	private void Start()
	{
	}

	private void Update()
	{
		if (movingToPosition)
		{
			if (Vector3.Distance(base.transform.position, moveToPosition) > 0.1f)
			{
				float maxDistanceDelta = moveToSpeed * Time.deltaTime;
				base.transform.position = Vector3.MoveTowards(base.transform.position, moveToPosition, maxDistanceDelta);
			}
			else
			{
				movingToPosition = false;
			}
		}
		else if (movingToDefaultPosition)
		{
			if (Vector3.Distance(base.transform.position, moveToPosition) > 0.01f)
			{
				float maxDistanceDelta2 = moveToSpeed * Time.deltaTime;
				base.transform.position = Vector3.MoveTowards(base.transform.position, moveToPosition, maxDistanceDelta2);
			}
			else
			{
				movingToDefaultPosition = false;
				headLookAt.FinishedMovingToDefaultPosition();
			}
		}
		else if ((bool)headLookAt && !headLookAt.ikActive)
		{
			base.transform.position = defaultPosition.position;
		}
	}

	public void setDefaultPosition(Transform defaultPosition)
	{
		this.defaultPosition = defaultPosition;
		base.transform.position = defaultPosition.position;
	}

	public void setHeadLookAt(HeadLookAt headLookAt)
	{
		this.headLookAt = headLookAt;
	}

	public void setPositionToMoveTo(Vector3 moveToPosition, float moveToSpeed)
	{
		if (moveToSpeed > 0.1f)
		{
			movingToPosition = true;
			this.moveToPosition = moveToPosition;
			this.moveToSpeed = moveToSpeed;
		}
		else
		{
			base.transform.position = moveToPosition;
		}
	}

	public void moveToDefaultPosition(float moveToSpeed)
	{
		if (moveToSpeed > 0.1f)
		{
			movingToDefaultPosition = true;
			moveToPosition = defaultPosition.position;
			this.moveToSpeed = moveToSpeed;
		}
		else
		{
			base.transform.position = defaultPosition.position;
			headLookAt.FinishedMovingToDefaultPosition();
		}
	}
}
