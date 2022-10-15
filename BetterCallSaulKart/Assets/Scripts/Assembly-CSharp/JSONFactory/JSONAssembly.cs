using System;
using UnityEngine;

namespace JSONFactory
{
	internal class JSONAssembly
	{
		public static NarrativeEvent RunJSONFactoryForEvent(string eventJSONPath)
		{
			Debug.Log("resourcePath: " + Application.streamingAssetsPath + "/" + eventJSONPath);
			try
			{
				NarrativeEvent result = JsonUtility.FromJson<NarrativeEvent>((Resources.Load(eventJSONPath) as TextAsset).text);
				Debug.Log("JSON Seems to be OK");
				return result;
			}
			catch (Exception message)
			{
				Debug.Log("I think the JSON is bad, please check it out.");
				Debug.Log(message);
			}
			NarrativeEvent obj = new NarrativeEvent
			{
				branches = new Branch[1]
			};
			obj.branches[0] = new Branch();
			Message message2 = new Message
			{
				name = "ERROR 001",
				messageText = "ERROR! JSON is probably invalid!"
			};
			Debug.Log(message2.name);
			obj.branches[0].messages = new Message[1];
			obj.branches[0].messages[0] = message2;
			return obj;
		}
	}
}
