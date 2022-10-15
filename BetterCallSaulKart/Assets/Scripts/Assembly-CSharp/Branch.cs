using System;

[Serializable]
public class Branch
{
	public int branchId;

	public Message[] messages;

	public Choice[] choices;

	public int nextBranchId;
}
