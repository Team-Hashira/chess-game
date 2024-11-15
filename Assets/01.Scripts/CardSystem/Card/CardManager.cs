using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	private List<Card> _cardHandList;
	private List<Card> _removedCardList = new List<Card>();
	[SerializeField] private CardListSO _cardListSO;

	private void Awake()
    {
		//현재 들고 있는 카드 초기화
		_cardHandList = new List<Card>(new Card[5]);

		//만약 가지고 있는 카드가 없다면
		if (Deck.GetCurrentCards().Count <= 0)
		{
			//기본 카드를 지급한다. (디버그)
			Deck.AddCard(Guid.NewGuid(), CreateCard(ECardType.ChainOfAtonement));
			Deck.AddCard(Guid.NewGuid(), CreateCard(ECardType.ChainOfAtonement));
			Deck.AddCard(Guid.NewGuid(), CreateCard(ECardType.ChainOfAtonement));
			Deck.AddCard(Guid.NewGuid(), CreateCard(ECardType.ChainOfAtonement));
			Deck.AddCard(Guid.NewGuid(), CreateCard(ECardType.ChainOfAtonement));
		}

		Deck.OrderByRandomCurrentCards();
		var curCardList = Deck.GetCurrentCards();

		for (int i = 0; i < 5; i++)
		{
			_cardHandList[i] = curCardList[i];
		}
	}

    private Card CreateCard(ECardType cardType)		
    {
		Type t = Type.GetType($"{cardType.ToString()}Card");
		Card newCard = Activator.CreateInstance(t) as Card;
		newCard.cost = _cardListSO[cardType].cost;
		newCard.cardSO = _cardListSO[cardType];
		newCard.cardManager = this;
		return newCard;
    }

	public void CardLock(Card card, bool isLock)
	{
		if(_cardHandList.Contains(card))
		{
			card.isLock = isLock;
		}
	}
}
