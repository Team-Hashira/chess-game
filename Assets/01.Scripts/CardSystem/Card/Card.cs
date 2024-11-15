using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public CardSO cardSO;
    public Guid guid;
    public List<ECurse> curses;
    public List<EBlessing> blessings;
    public bool isLock = false;
    public event Action<int> OnCostModifyEvent;
    protected int _cost;

    public Card()
    {
        curses = new List<ECurse>();
        blessings = new List<EBlessing>();
    }

	public void CostModify(int cost, Color color = default)
	{
		_cost = Mathf.Clamp(_cost + cost, 0, 10);
        OnCostModifyEvent?.Invoke(_cost);
	}

    public abstract void OnUse();
}
