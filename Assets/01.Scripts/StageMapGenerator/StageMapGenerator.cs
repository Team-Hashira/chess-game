using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum StageType
{
	None,
	Battle,
	Store,
	BlackMarket,
	Boss
}

namespace StageMapGenerator
{
	[System.Serializable]
	public class Stage
	{
		public StageType stageType = StageType.None;
		public Vector3 pos;
		public Stack<Stage> targets = new Stack<Stage>();
	}

	public class StageMapGenerator : MonoBehaviour
	{
		[SerializeField] private StageMapDataSO _stageMapDataSO;
		public Dictionary<StageType, int> _stageOfCountMap;

		public int startStageCount = 2;
		public int edgeCount = 30;
		public int maxDepth = 7;
		public int maxRange = 5;

		public int seed = 0;

		[SerializeField] private Line _linePrefab;
		[SerializeField] private GameObject _IconPrefab;

		private int stageTypeMaxCount;

		private Stage[,] _map;

		private int[] RandomPos(int range, int ranAmount = 5)
		{
			int[] positions = new int[range];
			for (int i = 0; i < range; i++)
			{
				positions[i] = i;
			}

			for (int i = 0; i < ranAmount; i++)
			{
				int pos1 = UnityEngine.Random.Range(0, range), pos2 = UnityEngine.Random.Range(0, range);

				int temp = positions[pos1];
				positions[pos1] = positions[pos2];
				positions[pos2] = temp;
			}

			return positions;
		}
	
		private void AddRandomTarget(int curY, int curX)
		{
			Stage stage = _map[curY, curX];

			stage.stageType = StageType.Battle;
			stage.pos = new Vector3(curX, curY);
			Instantiate(_IconPrefab, stage.pos, Quaternion.identity);
			
			int targetY = curY + 1; //한층 올라가기

			if (targetY < maxDepth-1)
			{
				int targetX = 0;
				Stage targetStage = null;

				if (targetY == maxDepth-2) targetX =  maxRange / 2;
				else targetX = Mathf.Clamp(curX + UnityEngine.Random.Range(-1, 2), 0, maxRange - 1);

				targetStage = _map[targetY, targetX];

				stage.targets.Push(targetStage);

				AddRandomTarget(targetY, targetX);
			}
		}

		private StageType GetRandomStageType()
		{
			bool isComplete = false;
			StageType result = StageType.Battle;

			bool isAllZero = true;

			//전부 비어있는지 확인
			foreach (var stageType in _stageOfCountMap)
			{
				if(stageType.Value != 0)
				{
					isAllZero = false;
					break;
				}
			}

			//전부 비어있다면 Battle을 리턴
			if (isAllZero == true)
			{
				return result;
			}

			while (isComplete == false) 
			{
				result = (StageType)UnityEngine.Random.Range((int)StageType.Battle, stageTypeMaxCount);
				if(_stageOfCountMap[result] > 0)
				{
					--_stageOfCountMap[result];
					isComplete = true;
				}
			}
			return result;
		}

		private void Awake()
		{
			_map = new Stage[maxDepth, maxRange];

            for (int i = 0; i < maxDepth; i++)
				for (int j = 0; j < maxRange; j++)
					_map[i, j] = new Stage();

            //StageType의 최대 개수 가지고 오기
            stageTypeMaxCount = Enum.GetValues(typeof(StageType)).Length;

			//Stage들의 정보 가지고 오기
			_stageOfCountMap = new Dictionary<StageType, int>(_stageMapDataSO.stageOfCountMap);

			var firstRanPos = RandomPos(maxRange);
			for (int i = 0; i < startStageCount; i++)
			{
				Stage stage = _map[0, firstRanPos[i]];
				//처음은 무조건 전투 스테이지
				stage.stageType = StageType.Battle;
				AddRandomTarget(0, firstRanPos[i]);
			}

			//선 잇기
			for (int i = 0; i < maxRange; i++)
			{
				if (_map[0, i].stageType != StageType.None)
				{
					DrawLine(0, i);
				}
			}
		}

		private void DrawLine(int y, int x)
		{
			var line = Instantiate(_linePrefab, _map[y, x].pos, Quaternion.identity);

			Vector3 targetPos = Vector3.zero;

			Stack<Stage> targets = _map[y, x].targets;
			if (targets.Count > 0)
				targetPos = targets.Pop().pos;

			line.TargetTo(targetPos);

			if (y + 1 < maxDepth-2)
			{
				DrawLine(y + 1, (int)targetPos.x);
			}	
		}
	}
}