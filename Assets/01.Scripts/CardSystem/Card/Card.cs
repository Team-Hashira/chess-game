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
    /// 5개의 카드가 패에 모두 셋팅 후, 패에 자신이 있거나 패에 변경 사항이 생기면 호출됨
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
