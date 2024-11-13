using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
	private static Dictionary<Guid, Card> _cardDict = new Dictionary<Guid, Card>();
	private static List<Card> _cardList = new List<Card>();

	public static void AddCard(Guid guid, Card card)
	{
		if (_cardDict == null) _cardDict = new Dictionary<Guid, Card>();

		if (_cardDict.ContainsKey(guid)) return;
		_cardDict.Add(guid, card);
	}

	public static void RemoveCard(Guid guid)
	{
		if (_cardDict.ContainsKey(guid))
			_cardDict.Remove(guid);
	}

	public static Card GetCurrentCard(Guid guid)
	{
		return _cardDict[guid];
	}

	public static List<Card> GetCurrentCards()
	{
		return _cardList;
	}

	public static List<Card> OrderByRandomCurrentCards()
	{
		if (_cardList == null) _cardList = new List<Card>();
		else _cardList.Clear();

		_cardList = _cardDict.Values.ToList();

		for (int i = 0; i < 10; i++)
		{
			int randX = UnityEngine.Random.Range(0, _cardList.Count);
			int randY = UnityEngine.Random.Range(0, _cardList.Count);

			//Swap
			var temp = _cardList[randX];
			_cardList[randX] = _cardList[randY];
			_cardList[randY] = temp;
		}

		return _cardList;
	}
}
