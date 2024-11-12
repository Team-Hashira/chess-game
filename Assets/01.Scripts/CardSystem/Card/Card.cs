using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public CardSO cardSO;
    public Guid guid;
    public List<ECurse> curse;
    public List<EBlessing> blessing;
    public event Action<int> OnCostModifyEvent;
    protected int _cost;

	public void CostModify(int cost, Color color = default)
	{
		_cost = Mathf.Clamp(_cost + cost, 0, 10);
        OnCostModifyEvent?.Invoke(_cost);
	}

	public bool Use()
	{
        return false;
	}
}
