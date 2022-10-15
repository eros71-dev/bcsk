using System.Collections.Generic;
using UnityEngine;

public class VehicleSwitcher : MonoBehaviour
{
	public List<Vehicle> vehicles = new List<Vehicle>();

	private int index;

	private void Start()
	{
		foreach (Vehicle vehicle in vehicles)
		{
			vehicle.controllable = false;
		}
		SelectVehicle();
	}

	private void Update()
	{
		SelectVehicle();
	}

	private void SelectVehicle()
	{
		vehicles[index].controllable = false;
		vehicles[index].transform.GetComponent<VehicleCamera>().vehicleCamera.depth = -1f;
		if (Input.GetKeyDown("v"))
		{
			if (index < vehicles.Count - 1)
			{
				index++;
			}
			else
			{
				index = 0;
			}
		}
		vehicles[index].controllable = true;
		vehicles[index].transform.GetComponent<VehicleCamera>().vehicleCamera.depth = 1f;
	}
}
