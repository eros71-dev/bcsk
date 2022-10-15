using UnityEngine;

public interface IActor
{
	bool checkIfMoving();

	void Happy();

	void Sad();

	void Angry();

	void Blank();

	void Blush();

	void Wink();

	void Worried();

	void TeleportTo(Vector3 position);

	void WalkTo(Vector3 position);

	void RunTo(Vector3 position);

	void LookAt(Vector3 position, float lookSpeed);

	void StopLookAt(float lookSpeed);

	void SetRotation(Vector3 eulerAngles);

	void Animate(string animation);
}
