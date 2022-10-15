using UnityEngine;

public static class GlobalGameData
{
	public static int currentCharacter = 0;

	public static int currentCourse = 1;

	public static int numTracks = 5;

	public static int[] bestPlaces = new int[5] { 9999, 9999, 9999, 9999, 9999 };

	public static float[] bestTimes = new float[5] { 9999f, 9999f, 9999f, 9999f, 9999f };

	public static bool isPaused = false;

	public static int[] numTimesLost = new int[5];

	public static void Reset()
	{
		currentCharacter = 0;
		currentCourse = -1;
		for (int i = 0; i < numTracks; i++)
		{
			bestPlaces[i] = 9999;
			bestTimes[i] = 9999f;
			numTimesLost[i] = 0;
		}
	}

	public static void LoadFromPrefs()
	{
		string @string = PlayerPrefs.GetString("bestPlaces");
		if (@string.Length > 0)
		{
			bestPlaces = JsonHelper.getJsonArray<int>(@string);
		}
		string string2 = PlayerPrefs.GetString("bestTimes");
		if (string2.Length > 0)
		{
			bestTimes = JsonHelper.getJsonArray<float>(string2);
		}
	}

	public static void SaveToPrefs()
	{
		string value = JsonHelper.arrayToJson(bestPlaces);
		PlayerPrefs.SetString("bestPlaces", value);
		string value2 = JsonHelper.arrayToJson(bestTimes);
		PlayerPrefs.SetString("bestTimes", value2);
	}

	public static void PrintData()
	{
		Debug.Log("---------");
		Debug.Log("bestPlaces: ");
		string text = "";
		int[] array = bestPlaces;
		foreach (int num in array)
		{
			text = text + num + " ";
		}
		Debug.Log(text);
		Debug.Log("---------");
		Debug.Log("bestPlaces: ");
		string text2 = "";
		float[] array2 = bestTimes;
		for (int i = 0; i < array2.Length; i++)
		{
			int num2 = (int)array2[i];
			Debug.Log(num2);
			text2 = text2 + num2 + " ";
		}
		Debug.Log(text2);
		Debug.Log("---------");
	}
}
