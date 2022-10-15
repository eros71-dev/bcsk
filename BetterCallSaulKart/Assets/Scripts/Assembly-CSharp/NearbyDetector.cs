using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NearbyDetector : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<GameObject, bool> _003C_003E9__2_0;

		internal bool _003CUpdate_003Eb__2_0(GameObject item)
		{
			return item != null;
		}
	}

	public List<GameObject> nearbyHazards;

	private void Start()
	{
		nearbyHazards = new List<GameObject>();
	}

	private void Update()
	{
		nearbyHazards = nearbyHazards.Where(_003C_003Ec._003C_003E9__2_0 ?? (_003C_003Ec._003C_003E9__2_0 = _003C_003Ec._003C_003E9._003CUpdate_003Eb__2_0)).ToList();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Dammage"))
		{
			nearbyHazards.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Dammage"))
		{
			nearbyHazards.Remove(other.gameObject);
		}
	}
}
