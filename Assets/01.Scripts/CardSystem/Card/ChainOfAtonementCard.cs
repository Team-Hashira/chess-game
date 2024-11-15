using UnityEngine;

public class ChainOfAtonementCard : Card
{
	public override void OnShow()
	{

	}

	public override void OnUse()
	{
		Debug.Log("카드 사용");
		Debug.Log($"{Cost.Get()}");
		Debug.Log($"{this.cost}");
	}
}