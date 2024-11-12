using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
	private Dictionary<ECurse, Effect> curseDict = new Dictionary<ECurse, Effect>();
	private Dictionary<EBlessing, Effect> blessingDict = new Dictionary<EBlessing, Effect>();

	private void Awake()
	{
		curseDict = new Dictionary<ECurse, Effect>();
		blessingDict = new Dictionary<EBlessing, Effect>();

		CreateEffectInstance(typeof(ECurse));
		CreateEffectInstance(typeof(EBlessing));
	}

	private void CreateEffectInstance(Type enumType)
	{
		foreach (Enum e in Enum.GetValues(enumType))
		{
			Type t = Type.GetType($"{e.ToString()}Effect");

			if(enumType == typeof(ECurse))
			{
				curseDict.Add((ECurse)e, Activator.CreateInstance(t) as Effect);
			}
			else if(enumType == typeof(EBlessing))
			{
				blessingDict.Add((EBlessing)e, Activator.CreateInstance(t) as Effect);
			}
		}

	}
}