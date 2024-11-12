using System;
using System.Collections.Generic;
using UnityEngine;

public class CardHandManager : MonoBehaviour
{
	private List<Card> _cardHandList = new List<Card>();

	private void Awake()
	{
		_cardHandList.Clear();

		if(DeckManager.GetCurrentCards().Count <= 0 )
		{
		}

		DeckManager.OrderByRandomCurrentCards();
		var curCardList = DeckManager.GetCurrentCards();

		for (int i = 0; i < 5; i++)
		{
			_cardHandList[i] = curCardList[i];
		}
	}

	public void Lock(Card card, bool isLock)
	{

	}
}