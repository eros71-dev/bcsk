using UnityEngine;

public class VehicleModel : MonoBehaviour
{
	[Header("Body")]
	public Transform vehicleBody;

	[Header("Wheels")]
	public Transform wheelFrontLeft;

	public Transform wheelFrontLeftHolder;

	public Transform wheelFrontRight;

	public Transform wheelFrontRightHolder;

	public Transform wheelRearLeft;

	public Transform wheelRearLeftHolder;

	public Transform wheelRearRight;

	public Transform wheelRearRightHolder;

	public Driver driver;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
