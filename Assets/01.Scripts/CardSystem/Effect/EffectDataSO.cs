using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[System.Serializable]
public struct EffectData
{
    public string name;
    [TextArea(3, 10)]
    public string description;
}

[CreateAssetMenu(fileName = "EffectDataSO", menuName = "SO/EffectDataSO")]
public class EffectDataSO : ScriptableObject
{
    public SerializedDictionary<ECurse, EffectData> curseDict;
    public SerializedDictionary<EBlessing, EffectData> blessingDict;

	private void Reset()
	{
		foreach (ECurse curse in Enum.GetValues(typeof(ECurse)))
        {
            curseDict.Add(curse, new EffectData());
		}

		foreach (EBlessing blessing in Enum.GetValues(typeof(EBlessing)))
		{
			blessingDict.Add(blessing, new EffectData());
		}
	}

	public EffectData this[ECurse curse]
    {
        get => curseDict[curse];
    }

    public EffectData this[EBlessing blessing]
    {
        get => blessingDict[blessing];
    }
}
