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
    public int cost;
    public CardManager cardManager;
    public int cardhandIndex = -1;

    public Card()
    {
        curses = new List<ECurse>();
        blessings = new List<EBlessing>();
    }

    /// <summary>
    /// 5���� ī�尡 �п� ��� ���� ��, �п� �ڽ��� �ְų� �п� ���� ������ ����� ȣ���
    /// </summary>
    public virtual void Refresh()
    {
		EffectManager.Instance.SetEffect(this);
	}
    public abstract void OnShow();
    public virtual void OnUse()
    {
		EffectManager.Instance.OnCardUse(this);
	}
}
