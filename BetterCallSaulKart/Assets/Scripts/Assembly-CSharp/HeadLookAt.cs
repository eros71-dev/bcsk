using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HeadLookAt : MonoBehaviour
{
	protected Animator animator;

	public bool ikActive;

	public LookTarget lookTarget;

	public Transform defaultLookTargetPosition;

	private void Start()
	{
		animator = GetComponent<Animator>();
		GameObject face = GetComponent<JawFlapFace>().face;
		defaultLookTargetPosition.position = face.transform.position + face.transform.forward + new Vector3(0f, -0.22f, 0f);
		lookTarget.setDefaultPosition(defaultLookTargetPosition);
		lookTarget.setHeadLookAt(this);
	}

	public LookTarget getLookTarget()
	{
		return lookTarget;
	}

	public void FinishedMovingToDefaultPosition()
	{
		ikActive = false;
	}

	private void OnAnimatorIK()
	{
		if (!animator)
		{
			return;
		}
		if (ikActive)
		{
			if (lookTarget != null)
			{
				animator.SetLookAtWeight(1f, 0.3f, 0.99f);
				animator.SetLookAtPosition(lookTarget.transform.position);
			}
		}
		else
		{
			animator.SetLookAtWeight(0f);
		}
	}
}
