using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "StageMapDataSO", menuName = "SO/StageMapDataSO")]
public class StageMapDataSO : ScriptableObject
{
	public SerializedDictionary<StageType, int> stageOfCountMap;
}
