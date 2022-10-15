using JSONFactory;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
	private string eventJSONPath;

	private PanelConfig messegePanel;

	private int branchId;

	private NarrativeEvent currentEvent;

	private int stepIndex;

	public GameObject[] otherUI;

	private bool isDone;

	private void Start()
	{
		messegePanel = GetComponent<PanelConfig>();
	}

	private void Awake()
	{
	}

	public void StartEventFromJSONPath(string eventJSONPath)
	{
		this.eventJSONPath = eventJSONPath;
		stepIndex = 0;
		branchId = 0;
		currentEvent = JSONAssembly.RunJSONFactoryForEvent(eventJSONPath);
		InitializePanels();
		isDone = false;
	}

	public void StartEventFromBranchId(int branchId)
	{
		stepIndex = 0;
		this.branchId = branchId;
		InitializePanels();
		isDone = false;
	}

	private void Update()
	{
	}

	private void InitializePanels()
	{
		showPanel();
		messegePanel = GetComponent<PanelConfig>();
		messegePanel.setIsActive(active: true);
		Branch branchByBranchID = getBranchByBranchID(currentEvent.branches, branchId);
		messegePanel.Configure(branchByBranchID, stepIndex);
		stepIndex++;
	}

	private void ConfigurePanels()
	{
		messegePanel.setIsActive(active: true);
		Branch branchByBranchID = getBranchByBranchID(currentEvent.branches, branchId);
		messegePanel.Configure(branchByBranchID, stepIndex);
	}

	public void UpdatePanelState()
	{
		if (currentEvent != null && !messegePanel.getCurrentlyHaveOptions())
		{
			if (stepIndex < getBranchByBranchID(currentEvent.branches, branchId).messages.Length)
			{
				ConfigurePanels();
				stepIndex++;
			}
			else if (getBranchByBranchID(currentEvent.branches, branchId).nextBranchId > 0)
			{
				StartEventFromBranchId(getBranchByBranchID(currentEvent.branches, branchId).nextBranchId);
			}
			else
			{
				hidePanel();
				messegePanel.clear();
				isDone = true;
			}
		}
	}

	public void showPanel()
	{
		base.gameObject.SetActive(value: true);
		GameObject[] array = otherUI;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	public void hidePanel()
	{
		base.gameObject.SetActive(value: false);
		GameObject[] array = otherUI;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
	}

	public Branch getBranchByBranchID(Branch[] branchList, int branchID)
	{
		foreach (Branch branch in branchList)
		{
			if (branch.branchId == branchId)
			{
				return branch;
			}
		}
		return null;
	}
}
