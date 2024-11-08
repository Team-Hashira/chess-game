using NUnit.Framework;
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
		public Vector2 posAtWorld;
		public List<Stage> targets = new List<Stage>();
		public int visitCount = 0;
	}

	public class StageMapGenerator : MonoBehaviour
	{
		[SerializeField] private StageMapDataSO _stageMapDataSO;
		public Dictionary<StageType, int> _stageOfCountMap;

		public int startStageCount = 2;
		public int maxDepth = 7;
		public int maxRange = 5;
		public float interval = 2;
		public float ranWorldOffset = 0.2f;
		public int edgePerent = 30;

		[SerializeField] private Line _linePrefab;
		[SerializeField] private Node _nodePrefab;

		private int stageTypeMaxCount;

		private Stage[,] _map;
		private Vector2 _mapCenter;
		
		private void Awake()
		{
			GenerateMap();
		}

		[ContextMenu("GenerateMap")]
		public void GenerateMap()
		{
			_map = new Stage[maxDepth, maxRange];

			for (int i = 0; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}


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

				stage.posAtWorld = 
					new Vector2(firstRanPos[i], 0) * interval 
					+ UnityEngine.Random.insideUnitCircle * ranWorldOffset;

				var icon = Instantiate(_nodePrefab, stage.posAtWorld, Quaternion.identity);
				icon.transform.SetParent(transform, true);
				AddRandomTarget(0, firstRanPos[i]);
			}

			//선 잇기
			for (int i = 0; i < maxRange; i++)
			{
				if (_map[0, i].stageType != StageType.None)
				{
					DrawLine(_map[0, i]);
				}
			}

			//카메라 셋
			_mapCenter = new Vector2(maxRange / 2, (maxDepth / 2)-1) * interval;
			Camera.main.transform.position = new Vector3(_mapCenter.x, _mapCenter.y, -10);
		}

		//ranAmount까지의 모든 숫자를 랜덤한 순서로 섞은 배열을 리턴합니다.
		private int[] RandomPos(int range, int ranAmount = 5)
		{
			int[] positions = new int[range];
			for (int i = 0; i < range; i++)
			{
				positions[i] = i;
			}

			for (int i = 0; i < ranAmount; i++)
			{
				int pos1 = UnityEngine.Random.Range(0, range);
				int pos2 = UnityEngine.Random.Range(0, range);

				int temp = positions[pos1];
				positions[pos1] = positions[pos2];
				positions[pos2] = temp;
			}

			return positions;
		}

		private void AddRandomTarget(int curY, int curX)
		{
			Stage stage = _map[curY, curX];

			if (stage.stageType == StageType.None)
			{
				stage.stageType = GetRandomStageType();

				stage.posAtWorld =
					new Vector2(curX, curY) * interval
					+ UnityEngine.Random.insideUnitCircle * ranWorldOffset;

				var icon = Instantiate(_nodePrefab, stage.posAtWorld, Quaternion.identity);
				icon.transform.SetParent(transform, true);

			}

			int targetY = curY + 1; //한층 올라가기

			if (targetY < maxDepth - 1)
			{
				int targetX = 0;
				Stage targetStage = null;

				int loopRange = 100;
				bool[] dirCheck = new bool[3];
				do
				{
					int dirRange = UnityEngine.Random.Range(-1, 2);
					if (dirCheck[dirRange + 1] == true) break;
					dirCheck[dirRange + 1] = true;
					if (targetY == maxDepth - 2) targetX = maxRange / 2;
					else targetX = Mathf.Clamp(curX + dirRange, 0, maxRange - 1);

					targetStage = _map[targetY, targetX];

					stage.targets.Add(targetStage);

					AddRandomTarget(targetY, targetX);
					loopRange = UnityEngine.Random.Range(0, 100);
				}
				while (loopRange < edgePerent);
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
				if (stageType.Value != 0)
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
				if (_stageOfCountMap[result] > 0)
				{
					--_stageOfCountMap[result];
					isComplete = true;
				}
			}
			return result;
		}

		private void DrawLine(Stage stage)
		{
			Queue<Stage> q = new Queue<Stage>();
			q.Enqueue(stage);

			while (q.Count > 0)
			{
				stage = q.Dequeue();

				for (int i = 0; i < stage.targets.Count; i++)
					if(stage.visitCount < stage.targets.Count)
					{
						stage.visitCount++;
						var line = Instantiate(_linePrefab, stage.posAtWorld, Quaternion.identity);
						line.transform.SetParent(transform, true);
						line.TargetTo(stage.targets[i].posAtWorld);
						q.Enqueue(stage.targets[i]);
					}
			}
		}

	}
}