using UnityEngine;

public class ChoicePanel : MonoBehaviour
{
	public GameObject selectionOneIcon;

	public GameObject selectionTwoIcon;

	public GameObject selectionThreeIcon;

	public GameObject selectionFourIcon;

	public PanelConfig myPanelConfig;

	public AudioSource selectSound;

	public AudioSource moveSound;

	public int currentChoiceHighlighted;

	public bool canMakeChoice;

	public float currTime;

	private float makeChoiceTime = 0.5f;

	public bool movedUp;

	public bool movedDown;

	public bool useArrowKeys;

	private int numChoices = 4;

	private void Start()
	{
		reset();
	}

	private void Update()
	{
		currTime += Time.deltaTime;
		if (currTime >= makeChoiceTime)
		{
			canMakeChoice = true;
		}
		else
		{
			canMakeChoice = false;
		}
		if (canMakeChoice && useArrowKeys && Input.GetButtonDown("Jump"))
		{
			Debug.Log("Choice Selected!");
			selectSound.Play();
			makeChoice();
		}
		if (useArrowKeys)
		{
			if (Input.GetButtonDown("Up"))
			{
				moveCursorUp();
			}
			if (Input.GetButtonDown("Down"))
			{
				moveCursorDown();
			}
			if (currentChoiceHighlighted == 1)
			{
				selectionOneIcon.SetActive(value: true);
			}
			else
			{
				selectionOneIcon.SetActive(value: false);
			}
			if (currentChoiceHighlighted == 2)
			{
				selectionTwoIcon.SetActive(value: true);
			}
			else
			{
				selectionTwoIcon.SetActive(value: false);
			}
			if (currentChoiceHighlighted == 3)
			{
				selectionThreeIcon.SetActive(value: true);
			}
			else
			{
				selectionThreeIcon.SetActive(value: false);
			}
			if (currentChoiceHighlighted == 4)
			{
				selectionFourIcon.SetActive(value: true);
			}
			else
			{
				selectionFourIcon.SetActive(value: false);
			}
		}
	}

	private void moveCursorUp()
	{
		moveSound.Play();
		currentChoiceHighlighted--;
		if (currentChoiceHighlighted < 1)
		{
			currentChoiceHighlighted = numChoices;
		}
		movedUp = true;
	}

	private void moveCursorDown()
	{
		moveSound.Play();
		currentChoiceHighlighted++;
		if (currentChoiceHighlighted > numChoices)
		{
			currentChoiceHighlighted = 1;
		}
		movedDown = true;
	}

	public void makeChoice()
	{
		canMakeChoice = false;
		myPanelConfig.makeChoice(currentChoiceHighlighted);
	}

	public void reset()
	{
		currTime = 0f;
		canMakeChoice = false;
		currentChoiceHighlighted = 1;
		selectionOneIcon.SetActive(value: false);
		selectionTwoIcon.SetActive(value: false);
		selectionThreeIcon.SetActive(value: false);
		selectionFourIcon.SetActive(value: false);
	}
}
