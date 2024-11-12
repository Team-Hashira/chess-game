using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CardListSO", menuName = "SO/CardListSO")]
public class CardListSO : ScriptableObject
{
    public List<CardSO> list;

    public CardSO this[int index]
    {
        get => list[index];
        set => list[index] = value;
    }
}
