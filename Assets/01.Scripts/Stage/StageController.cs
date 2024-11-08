using UnityEngine;
using StageMap;
using System.Collections.Generic;
using System;

public class StageController : MonoBehaviour
{
    [SerializeField] private StageMapGenerator _stageMapGenerator;
    [SerializeField] private StageSceneListSO _stageSceneListSO;

    public event Action OnSelectEndEvent;

	public static StageType stageType;

    private List<Stage> _curSelectableStages;
    public Stage curSelectedStage;

    public void SetCurSelectedStage(Stage stage)
    {
        _curSelectableStages.Clear();
		curSelectedStage = stage;
        OnSelectEndEvent?.Invoke();
	}

    public void AddSelectableStage(Stage stage)
    {
        _curSelectableStages.Add(stage);
    }
}