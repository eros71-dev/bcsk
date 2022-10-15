using UnityEngine;

public class VehicleSurface : MonoBehaviour
{
	private Material surfaceMaterial;

	private string surfaceMaterialName;

	private void FixedUpdate()
	{
		SurfaceSample();
	}

	private void SurfaceSample()
	{
		if (!Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo, 1.25f))
		{
			return;
		}
		Renderer component = hitInfo.collider.GetComponent<Renderer>();
		MeshCollider meshCollider = hitInfo.collider as MeshCollider;
		if (component == null || component.sharedMaterial == null || meshCollider == null)
		{
			return;
		}
		int num = -1;
		Mesh sharedMesh = meshCollider.sharedMesh;
		int triangleIndex = hitInfo.triangleIndex;
		int num2 = sharedMesh.triangles[triangleIndex * 3];
		int num3 = sharedMesh.triangles[triangleIndex * 3 + 1];
		int num4 = sharedMesh.triangles[triangleIndex * 3 + 2];
		int subMeshCount = sharedMesh.subMeshCount;
		for (int i = 0; i < subMeshCount; i++)
		{
			int[] triangles = sharedMesh.GetTriangles(i);
			for (int j = 0; j < triangles.Length; j += 3)
			{
				if (triangles[j] == num2 && triangles[j + 1] == num3 && triangles[j + 2] == num4)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				break;
			}
		}
		if (num != -1)
		{
			surfaceMaterial = component.materials[num];
			surfaceMaterialName = surfaceMaterial.name.Replace(" (Instance)", "");
		}
		else
		{
			surfaceMaterial = null;
			surfaceMaterialName = null;
		}
	}
}
