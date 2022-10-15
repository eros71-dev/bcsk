using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
	private Renderer rend;

	private float texOffsetX;

	private float texOffsetY;

	public float xSpeed = -0.3f;

	public float ySpeed = 0.3f;

	private void Start()
	{
		rend = GetComponent<Renderer>();
	}

	private void Update()
	{
		texOffsetX += Time.deltaTime * xSpeed;
		texOffsetY += Time.deltaTime * ySpeed;
		rend.material.SetTextureOffset("_MainTex", new Vector2(texOffsetX, texOffsetY));
	}
}
