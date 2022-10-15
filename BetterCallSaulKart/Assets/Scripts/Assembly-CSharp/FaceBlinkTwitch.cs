using System.Collections;
using UnityEngine;

public class FaceBlinkTwitch : MonoBehaviour
{
	public SkinnedMeshRenderer skinnedMeshRenderer;

	public bool dead;

	private float blinkPower;

	private float blinkSpeed;

	private float blinkMinPower;

	private float blinkMaxPower = 100f;

	private float mouthPower;

	private float mouthSpeed;

	private float mouthMinPower;

	private float mouthMaxPower = 20f;

	private float eyebrowPower;

	private float eyebrowSpeed;

	private float eyebrowMinPower;

	private float eyebrowMaxPower = 30f;

	private float eyePower;

	private float eyeSpeed = 400f;

	private float eyeMinPower;

	private float eyeMaxPower = 20f;

	private void OnEnable()
	{
		StartCoroutine("blink");
		StartCoroutine("mouth");
		StartCoroutine("eyebrow");
		StartCoroutine("eyes");
	}

	private void Update()
	{
	}

	public void openMouth()
	{
		skinnedMeshRenderer.SetBlendShapeWeight(0, 100f);
	}

	public void closeMouth()
	{
		skinnedMeshRenderer.SetBlendShapeWeight(0, 0f);
	}

	private IEnumerator blink()
	{
		while (!dead)
		{
			blinkSpeed = Random.Range(800, 3500);
			yield return new WaitForSeconds(Random.Range(1f, 5f));
			while (blinkPower < blinkMaxPower)
			{
				blinkPower += Time.deltaTime * blinkSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(12, blinkPower);
				skinnedMeshRenderer.SetBlendShapeWeight(13, blinkPower);
				yield return null;
			}
			blinkPower = blinkMaxPower;
			yield return new WaitForSeconds(0.1f);
			while (blinkPower > blinkMinPower)
			{
				blinkPower -= Time.deltaTime * blinkSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(12, blinkPower);
				skinnedMeshRenderer.SetBlendShapeWeight(13, blinkPower);
				yield return null;
			}
			blinkPower = blinkMinPower;
		}
	}

	private IEnumerator mouth()
	{
		while (!dead)
		{
			int choice = Random.Range(42, 44);
			mouthSpeed = Random.Range(100, 200);
			yield return new WaitForSeconds(Random.Range(5f, 15f));
			while (mouthPower < mouthMaxPower)
			{
				mouthPower += Time.deltaTime * mouthSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(choice, mouthPower);
				yield return null;
			}
			mouthPower = mouthMaxPower;
			yield return new WaitForSeconds(Random.Range(0.4f, 1.5f));
			while (mouthPower > mouthMinPower)
			{
				mouthPower -= Time.deltaTime * mouthSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(choice, mouthPower);
				yield return null;
			}
			mouthPower = mouthMinPower;
		}
	}

	private IEnumerator eyebrow()
	{
		while (!dead)
		{
			int choice = Random.Range(1, 5);
			eyebrowSpeed = Random.Range(100, 200);
			yield return new WaitForSeconds(Random.Range(5f, 15f));
			while (eyebrowPower < eyebrowMaxPower)
			{
				eyebrowPower += Time.deltaTime * eyebrowSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(choice, eyebrowPower);
				yield return null;
			}
			eyebrowPower = eyebrowMaxPower;
			yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
			while (eyebrowPower > eyebrowMinPower)
			{
				eyebrowPower -= Time.deltaTime * eyebrowSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(choice, eyebrowPower);
				yield return null;
			}
			eyebrowPower = eyebrowMinPower;
		}
	}

	private IEnumerator eyes()
	{
		while (!dead)
		{
			int choice = Random.Range(18, 22);
			int num = Random.Range(0, 2);
			bool twoDirections = false;
			int choice2 = Random.Range(18, 22);
			if (num == 1)
			{
				twoDirections = true;
			}
			yield return new WaitForSeconds(Random.Range(1f, 6f));
			while (eyePower < eyeMaxPower)
			{
				eyePower += Time.deltaTime * eyeSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(choice, eyePower);
				if (twoDirections)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(choice2, eyePower);
				}
				yield return null;
			}
			eyePower = eyeMaxPower;
			yield return new WaitForSeconds(Random.Range(0.5f, 2f));
			while (eyePower > eyeMinPower)
			{
				eyePower -= Time.deltaTime * eyeSpeed;
				skinnedMeshRenderer.SetBlendShapeWeight(choice, eyePower);
				if (twoDirections)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(choice2, eyePower);
				}
				yield return null;
			}
			eyePower = eyeMinPower;
		}
	}

	public void stopAll()
	{
		StopAllCoroutines();
	}
}
