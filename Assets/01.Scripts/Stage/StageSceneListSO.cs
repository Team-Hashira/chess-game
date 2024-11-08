using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageSceneListSO")]
public class StageSceneListSO : ScriptableObject
{
    public SerializedDictionary<StageType, List<string>> stageScenes;

}
