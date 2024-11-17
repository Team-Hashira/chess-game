using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
	private static List<Card> _cardList = new List<Card>();

	public static void AddCard(Card card)
	{
		if (_cardList == null) _cardList = new List<Card>();

		if (_cardList.Contains(card)) return;

		_cardList.Add(card);
	}

	public static void RemoveCard(Card card)
	{
		if (_cardList.Contains(card) == false) return;

		_cardList.Remove(card);
	}

	public static List<Card> GetCurrentCards()
	{
		return _cardList;
	}

	public static List<Card> PeekRandomCards(int count = 5)
	{
		List<Card> cards = new List<Card>(_cardList);

		cards.OrderBy(_ => new System.Random().Next(cards.Count));

		cards.RemoveRange(count, cards.Count-count);

		return _cardList;
	}
}
