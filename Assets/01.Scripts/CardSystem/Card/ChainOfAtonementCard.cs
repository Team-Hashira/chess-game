using UnityEngine;

public class ChainOfAtonementCard : Card
{
	public override void OnShow()
	{

	}

	public override void OnUse()
	{
		Debug.Log("ī�� ���");
		Debug.Log($"{Cost.Get()}");
		Debug.Log($"{this.cost}");
	}
}