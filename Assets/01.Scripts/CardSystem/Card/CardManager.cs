using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoSingleton<CardManager>
{
	public List<Card> CardHandList { get; private set; }
	private List<Card> _removedCardList = new List<Card>();
	[SerializeField] private CardListSO _cardListSO;

	private void Awake()
    {
		//현재 들고 있는 카드 초기화
		CardHandList = new List<Card>(new Card[5]);

		//만약 가지고 있는 카드가 없다면
		if (DeckManager.GetCurrentCards().Count <= 0)
		{
			//기본 카드를 지급한다. (디버그)

			var firstCard = CreateCard(ECardType.ChainOfAtonement);
			firstCard.curses.Add(ECurse.Envy);

			DeckManager.AddCard(firstCard);
			DeckManager.AddCard(CreateCard(ECardType.ChainOfAtonement));
			DeckManager.AddCard(CreateCard(ECardType.ChainOfAtonement));
			DeckManager.AddCard(CreateCard(ECardType.ChainOfAtonement));
			DeckManager.AddCard(CreateCard(ECardType.ChainOfAtonement));
		}

		CardHandList = DeckManager.PeekRandomCards();
	}

	public int CurHandCardCount => CardHandList.Count;

    private Card CreateCard(ECardType cardType)		
    {
		Type t = Type.GetType($"{cardType.ToString()}Card");
		Card newCard = Activator.CreateInstance(t) as Card;
		newCard.cost = _cardListSO[cardType].cost;
		newCard.cardSO = _cardListSO[cardType];
		newCard.cardManager = this;
		return newCard;
    }

	public void UseCard(Card card)
	{
		Card usedCard = CardHandList.First(x=>x==card);
		usedCard.OnUse();
		usedCard.isLock = false;
		CardHandList.Remove(usedCard);
	}

	public void CardLock(Card card, bool isLock)
	{
		if(CardHandList.Contains(card))
		{
			card.isLock = isLock;
		}
	}
}
