using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
	public Vehicle vehicle;

	private Dictionary<string, bool> controls;

	private void Awake()
	{
		controls = new Dictionary<string, bool>
		{
			{ "left", false },
			{ "right", false },
			{ "accelerate", false },
			{ "brake", false }
		};
	}

	private void Update()
	{
		if (controls["left"])
		{
			vehicle.ControlSteer(-1f);
		}
		if (controls["right"])
		{
			vehicle.ControlSteer(1f);
		}
		if (controls["accelerate"])
		{
			vehicle.ControlAccelerate();
		}
		if (controls["brake"])
		{
			vehicle.ControlBrake();
		}
	}

	public void PressButton(string b)
	{
		controls[b] = true;
	}

	public void ReleaseButton(string b)
	{
		controls[b] = false;
	}
}
