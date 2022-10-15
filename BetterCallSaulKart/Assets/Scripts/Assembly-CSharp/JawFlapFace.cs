using UnityEngine;

public class JawFlapFace : MonoBehaviour, ICanTalk, IActor
{
	private float currentTalkTime;

	private float talkTimeLimit = 0.09f;

	private bool isTalking;

	private bool isMouthOpen;

	public GameObject face;

	private SkinnedMeshRenderer faceRenderer;

	private int blendShapeCount = 62;

	public Animator anim;

	private bool isWalking;

	private Vector3 walkTarget;

	private float walkSpeed = 1f;

	private FaceBlinkTwitch faceBlinkTwitch;

	private HeadLookAt headLookAt;

	private float jawFlapTime;

	private void Start()
	{
		isTalking = false;
		currentTalkTime = 0f;
		isMouthOpen = false;
		isWalking = false;
		faceRenderer = face.GetComponent<SkinnedMeshRenderer>();
		faceBlinkTwitch = GetComponent<FaceBlinkTwitch>();
		headLookAt = GetComponent<HeadLookAt>();
	}

	private void Update()
	{
		if (isTalking)
		{
			if (currentTalkTime >= talkTimeLimit)
			{
				toggleMouthOpen();
				currentTalkTime = 0f;
			}
			currentTalkTime += Time.deltaTime;
			jawFlapTime += Time.deltaTime;
			float num = 20f;
			float num2 = 38f;
			float num3 = 10f;
			float value = Mathf.Sin(jawFlapTime * num) * num2 + num3;
			if ((bool)face)
			{
				faceRenderer.SetBlendShapeWeight(0, value);
			}
		}
		ManageWalking();
	}

	private void ManageWalking()
	{
		if (isWalking)
		{
			if (Vector3.Distance(base.transform.position, walkTarget) <= 0.1f)
			{
				isWalking = false;
				anim.SetTrigger("Idle");
			}
			else
			{
				base.transform.LookAt(walkTarget);
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
				base.transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
			}
		}
	}

	public void toggleMouthOpen()
	{
		if (isMouthOpen)
		{
			isMouthOpen = false;
		}
		else
		{
			isMouthOpen = true;
		}
	}

	public void startTalking()
	{
		if (!isTalking)
		{
			isTalking = true;
			currentTalkTime = 0f;
			toggleMouthOpen();
			jawFlapTime = 0f;
		}
	}

	public void stopTalking()
	{
		isTalking = false;
		if ((bool)face)
		{
			faceRenderer.SetBlendShapeWeight(0, 0f);
		}
		currentTalkTime = 0f;
	}

	public void Happy()
	{
		Blank();
		faceRenderer.SetBlendShapeWeight(38, 100f);
		faceRenderer.SetBlendShapeWeight(39, 100f);
	}

	public void Sad()
	{
		Blank();
	}

	public void Angry()
	{
		Blank();
		faceRenderer.SetBlendShapeWeight(1, 100f);
		faceRenderer.SetBlendShapeWeight(2, 100f);
		faceRenderer.SetBlendShapeWeight(4, 100f);
		faceRenderer.SetBlendShapeWeight(5, 100f);
		faceRenderer.SetBlendShapeWeight(40, 100f);
		faceRenderer.SetBlendShapeWeight(41, 100f);
	}

	public void Blank()
	{
		for (int i = 0; i < blendShapeCount; i++)
		{
			face.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, 0f);
		}
	}

	public void Blush()
	{
		Blank();
		faceRenderer.SetBlendShapeWeight(3, 100f);
	}

	public void Wink()
	{
		Blank();
		faceRenderer.SetBlendShapeWeight(12, 100f);
	}

	public void Worried()
	{
		Blank();
		faceRenderer.SetBlendShapeWeight(3, 100f);
		faceRenderer.SetBlendShapeWeight(10, 60f);
	}

	public void TeleportTo(Vector3 position)
	{
		base.transform.position = position;
	}

	public void WalkTo(Vector3 position)
	{
		anim.SetTrigger("Walk");
		isWalking = true;
		walkTarget = position;
	}

	public void RunTo(Vector3 position)
	{
	}

	public void LookAt(Vector3 position, float lookSpeed)
	{
		headLookAt.ikActive = true;
		headLookAt.lookTarget.setPositionToMoveTo(position, lookSpeed);
	}

	public void StopLookAt(float lookSpeed)
	{
		headLookAt.lookTarget.moveToDefaultPosition(lookSpeed);
	}

	public void SetRotation(Vector3 eulerAngles)
	{
		base.transform.eulerAngles = eulerAngles;
	}

	public void Animate(string animation)
	{
		anim.SetTrigger(animation);
	}

	public bool checkIfMoving()
	{
		if (isWalking)
		{
			return true;
		}
		return false;
	}
}
