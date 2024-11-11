using System;
using System.Collections.Generic;

public class CardManager
{
	private static Dictionary<Guid, Card> _cardDict = new Dictionary<Guid, Card>();

	public static void AddCard(Guid guid, Card card)
	{
		if(_cardDict == null) _cardDict = new Dictionary<Guid, Card>();

		if (_cardDict.ContainsKey(guid)) return;
		_cardDict.Add(guid, card);
	}

	public static void RemoveCard(Guid guid)
	{
		if( _cardDict.ContainsKey(guid))
			_cardDict.Remove(guid);
	}

	public static Card GetCurrentCard(Guid guid)
	{
		return _cardDict[guid];
	}
}
