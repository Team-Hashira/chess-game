using UnityEngine;
using StageMap;
using System.Collections.Generic;
using System;

public class StageController : MonoSingleton<StageController>
{
    [SerializeField] private StageMapGenerator _stageMapGenerator;
    [SerializeField] private StageSceneListSO _stageSceneListSO;

    public event Action OnSelectEndEvent;

	public static StageType stageType;

    public List<Stage> _curSelectableStages;
    public Stage curSelectedStage;

	private void Start()
	{
        Stage[,] stages = StageMapGenerator.GetStage();

  //      for (int i = 0; i < stages.GetLength(0); i++)
  //      {
  //          _curSelectableStages.Add(stages[0, i]);
		//}
	}

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