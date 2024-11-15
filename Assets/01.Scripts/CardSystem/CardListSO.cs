using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardListSO", menuName = "SO/CardListSO")]
public class CardListSO : ScriptableObject
{
    [SerializeField] private SerializedDictionary<ECardType, CardSO> _dict; 

    public CardSO this[ECardType cardType]
    {
        get => _dict[cardType];
        set => _dict[cardType] = value;
    }

	private void Reset()
	{
        foreach (ECardType type in Enum.GetValues(typeof(ECardType)))
        {
            _dict.Add(type, null);
        }
	}
}
