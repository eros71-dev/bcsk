using System;
using UnityEngine;

public class JsonHelper
{
	[Serializable]
	private class Wrapper<T>
	{
		public T[] array;
	}

	public static T[] getJsonArray<T>(string json)
	{
		return JsonUtility.FromJson<Wrapper<T>>(json).array;
	}

	public static string arrayToJson<T>(T[] array)
	{
		return JsonUtility.ToJson(new Wrapper<T>
		{
			array = array
		});
	}
}
