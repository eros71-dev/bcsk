using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<MeshCollider>().convex = true;
		base.gameObject.AddComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
	}

	public void Hit(Vector3 velocity)
	{
		velocity = Quaternion.AngleAxis(Random.Range(-15, 15), Vector3.up) * velocity;
		velocity = Quaternion.AngleAxis(Random.Range(-15, 15), Vector3.right) * velocity;
		GetComponent<Rigidbody>().AddForce(velocity * Random.Range(30, 60));
		GetComponent<Rigidbody>().AddTorque(velocity * Random.Range(30, 60));
		base.gameObject.layer = 2;
	}
}
