using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private Dictionary<ECardType, Type> cardTypesDict = new Dictionary<ECardType, Type>();

    private void Awake()
    {
        CreateEffectInstance(typeof(ECurse));
        CreateEffectInstance(typeof(EBlessing));
    }

    private void CreateEffectInstance(Type enumType)
    {
        foreach (Enum e in Enum.GetValues(enumType))
        {
            Type t = Type.GetType($"{e.ToString()}Effect");

            cardTypesDict.Add((ECardType)e, t);
        }

    }
}
