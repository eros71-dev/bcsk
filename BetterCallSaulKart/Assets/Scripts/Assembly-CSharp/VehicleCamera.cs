using UnityEngine;

public class VehicleCamera : MonoBehaviour
{
	public enum View
	{
		Full = 0,
		HalfTop = 1,
		HalfBottom = 2,
		HalfLeft = 3,
		HalfRight = 4,
		QuarterTopLeft = 5,
		QuarterTopRight = 6,
		QuarterBottomLeft = 7,
		QuarterBottomRight = 8
	}

	[Header("Components")]
	public Transform rig;

	[Header("Settings")]
	public View view;

	[Range(1f, 30f)]
	public float followSpeed = 16f;

	[Range(1f, 20f)]
	public float rotationSpeed = 12f;

	public bool followRotation = true;

	private Vector3 cameraPositionOffset;

	private Vector3 cameraRotationOffset;

	[HideInInspector]
	public Camera vehicleCamera;

	public bool spin;

	private void Awake()
	{
		cameraPositionOffset = rig.localPosition;
		cameraRotationOffset = rig.localEulerAngles;
		vehicleCamera = rig.GetChild(0).GetComponent<Camera>();
		UpdateCamera();
		rig.position = base.transform.position + cameraPositionOffset;
	}

	private void UpdateCamera()
	{
		switch (view)
		{
		case View.Full:
			vehicleCamera.rect = new Rect(0f, 0f, 1f, 1f);
			break;
		case View.HalfTop:
			vehicleCamera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
			break;
		case View.HalfBottom:
			vehicleCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
			break;
		case View.HalfLeft:
			vehicleCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
			break;
		case View.HalfRight:
			vehicleCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
			break;
		case View.QuarterTopLeft:
			vehicleCamera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
			break;
		case View.QuarterTopRight:
			vehicleCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
			break;
		case View.QuarterBottomLeft:
			vehicleCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
			break;
		case View.QuarterBottomRight:
			vehicleCamera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
			break;
		}
	}

	private void FixedUpdate()
	{
		if (spin)
		{
			followSpeed = 16f;
			rig.transform.eulerAngles = new Vector3(rig.transform.eulerAngles.x, rig.transform.eulerAngles.y + 1.2f, rig.transform.eulerAngles.z);
			rig.position = Vector3.Lerp(rig.position, base.transform.position + cameraPositionOffset, Time.deltaTime * followSpeed);
			return;
		}
		rig.position = Vector3.Lerp(rig.position, base.transform.position + cameraPositionOffset, Time.deltaTime * followSpeed);
		if (followRotation)
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			rig.rotation = Quaternion.Lerp(rig.rotation, Quaternion.Euler(eulerAngles + cameraRotationOffset), Time.deltaTime * rotationSpeed);
		}
	}
}
