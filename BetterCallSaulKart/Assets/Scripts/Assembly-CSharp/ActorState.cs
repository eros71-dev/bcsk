using System;
using UnityEngine;

[Serializable]
public class ActorState
{
	public int actorNumber;

	public bool isSpeaking;

	public string faceEmotion;

	public string animation;

	public bool walkTo;

	public Vector3 walkToPosition;

	public bool teleportTo;

	public Vector3 teleportToPosition;

	public bool runTo;

	public Vector3 runToPosition;

	public bool setRotation;

	public Vector3 setRotationEulerAngles;

	public bool lookAtTarget;

	public Vector3 lookTargetPosition;

	public float lookSpeed;

	public bool stopLookAtTarget;
}
