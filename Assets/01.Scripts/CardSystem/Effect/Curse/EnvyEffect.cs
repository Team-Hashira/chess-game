using System.Collections.Generic;
using UnityEngine;

public class EnvyEffect : Effect
{
	private List<Card> _lockedCards = new List<Card>();

	public override void ApplyEffect(Card owner)
	{
		base.ApplyEffect(owner);
		int ownerIdx = _cardManager.CardHandList.IndexOf(owner);

		int rightCardIdx = ownerIdx + 1;
		int leftCardIdx = ownerIdx - 1;

		try
		{
			Card righCard = _cardManager.CardHandList[rightCardIdx];
			righCard.isLock = true;
			_lockedCards.Add(righCard);
		}
		catch (System.Exception) { }

		try
		{
			Card leftCard = _cardManager.CardHandList[leftCardIdx];
			leftCard.isLock = true;
			_lockedCards.Add(leftCard);
		}
		catch (System.Exception) { }
	}

	public override void OnCardUse(Card owner)
	{
		foreach (Card card in _lockedCards)
		{
			card.isLock = false;
		}

		_lockedCards.Clear();
	}
}