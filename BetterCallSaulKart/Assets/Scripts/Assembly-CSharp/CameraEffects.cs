using UnityEngine;

public class CameraEffects : MonoBehaviour
{
	public GameObject lookTarget;

	private bool followTargetBool;

	private bool shakeBool;

	private bool zoomInBool;

	private bool zoomOutBool;

	private bool panBool;

	private bool isShaking;

	public float initialShakeTime = 0.3f;

	public float initialShakeIntensity = 1f;

	private float currShakeTime;

	public float initialZoomIntensity = 10f;

	public float initialZoomTime = 0.5f;

	private bool isZoomingIn;

	private bool isZoomingOut;

	private float currZoomTime;

	private Vector3 originalPosition;

	private Vector3 originalRotation;

	private bool originalPosistionSet;

	private Texture2D fadeOutTexture;

	public Color fadeColor = Color.black;

	private float fadeSpeed = 0.5f;

	private int drawDepth = -1000;

	private float alpha = 1f;

	private int fadeDir;

	public Vector3 panTarget;

	public float panSpeed;

	private float initalFOV;

	public float fovOverride;

	private void Start()
	{
		currShakeTime = initialShakeTime;
		currZoomTime = initialZoomTime;
		initalFOV = GetComponent<Camera>().fieldOfView;
		if (fovOverride > 0f)
		{
			initalFOV = fovOverride;
		}
		setupFadeTexture();
	}

	private void Update()
	{
		if (followTargetBool)
		{
			if (lookTarget != null)
			{
				base.transform.LookAt(lookTarget.transform.position);
			}
		}
		else if (shakeBool)
		{
			if (isShaking)
			{
				currShakeTime -= Time.deltaTime;
				if (currShakeTime <= 0f)
				{
					isShaking = false;
				}
				base.transform.localPosition = originalPosition + Random.insideUnitSphere * currShakeTime;
			}
			else
			{
				base.transform.localPosition = originalPosition;
				shakeBool = false;
				reset();
			}
		}
		else if (zoomInBool)
		{
			if (isZoomingIn)
			{
				currZoomTime -= Time.deltaTime;
				if (currZoomTime <= 0f)
				{
					isZoomingIn = false;
				}
				GetComponent<Camera>().fieldOfView = initalFOV + currZoomTime * initialZoomIntensity;
			}
			else
			{
				zoomInBool = false;
				GetComponent<Camera>().fieldOfView = initalFOV;
			}
		}
		else if (zoomOutBool)
		{
			if (isZoomingOut)
			{
				currZoomTime -= Time.deltaTime;
				if (currZoomTime <= 0f)
				{
					isZoomingOut = false;
				}
				GetComponent<Camera>().fieldOfView = initalFOV - currZoomTime * initialZoomIntensity;
			}
			else
			{
				zoomOutBool = false;
				GetComponent<Camera>().fieldOfView = initalFOV;
			}
		}
		else if (panBool)
		{
			float maxRadiansDelta = panSpeed * Time.deltaTime;
			Vector3 forward = Vector3.RotateTowards(base.transform.forward, panTarget, maxRadiansDelta, 0f);
			base.transform.rotation = Quaternion.LookRotation(forward);
		}
	}

	private void setupFadeTexture()
	{
		fadeOutTexture = new Texture2D(100, 100);
		for (int i = 0; i < 100; i++)
		{
			for (int j = 0; j < 100; j++)
			{
				fadeOutTexture.SetPixel(i, j, fadeColor);
			}
		}
		fadeOutTexture.Apply();
	}

	public void followTarget()
	{
		reset();
		followTargetBool = true;
	}

	public void shake()
	{
		reset();
		shakeBool = true;
		isShaking = true;
	}

	public void zoomIn()
	{
		reset();
		zoomInBool = true;
		isZoomingIn = true;
		GetComponent<Camera>().fieldOfView = initalFOV - currZoomTime * initialZoomIntensity;
	}

	public void zoomOut()
	{
		reset();
		zoomOutBool = true;
		isZoomingOut = true;
		GetComponent<Camera>().fieldOfView = initalFOV - currZoomTime * initialZoomIntensity;
	}

	public void pan()
	{
		reset();
		panBool = true;
		panTarget = new Vector3(base.transform.rotation.x, base.transform.rotation.y, base.transform.rotation.z + 90f);
		panSpeed = 0.1f;
	}

	public void reset()
	{
		followTargetBool = false;
		shakeBool = false;
		zoomInBool = false;
		zoomOutBool = false;
		panBool = false;
		currShakeTime = initialShakeTime;
		currZoomTime = initialZoomTime;
		isShaking = false;
		isZoomingOut = false;
		isZoomingIn = false;
		base.transform.eulerAngles = originalRotation;
		base.transform.localPosition = originalPosition;
		if (initalFOV > 0f)
		{
			GetComponent<Camera>().fieldOfView = initalFOV;
		}
	}

	public void setInitialPosition()
	{
		if (!originalPosistionSet)
		{
			originalRotation = base.transform.eulerAngles;
			originalPosition = base.transform.localPosition;
			originalPosistionSet = true;
			currShakeTime = initialShakeTime;
			currZoomTime = initialZoomTime;
		}
	}

	public void lookAtTarget()
	{
		if (lookTarget != null)
		{
			base.transform.LookAt(lookTarget.transform.position);
		}
	}

	private void OnGUI()
	{
		if (fadeDir != 0)
		{
			alpha += (float)fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fadeOutTexture);
		}
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	public void fadeIn()
	{
		alpha = 1f;
		fadeSpeed = 1f;
		BeginFade(-1);
	}

	public void fadeOut()
	{
		alpha = 0f;
		fadeSpeed = 1f;
		BeginFade(1);
	}

	public void fillWithColor()
	{
		alpha = 1f;
		fadeSpeed = 1f;
		BeginFade(1);
	}

	public void stopFillWithColor()
	{
		alpha = 0f;
		fadeSpeed = 1f;
		BeginFade(-1);
	}
}
