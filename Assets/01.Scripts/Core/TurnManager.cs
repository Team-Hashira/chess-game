using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	[SerializeField] private ETeamType _startTeam = ETeamType.White;
	public ETeamType CurTeam { get; private set; }
	private Dictionary<ETeamType, int> _turnCountDictionary;

	public event Action<ETeamType> OnTurnEndEvent;

	private void Awake()
	{
		Debug.Log($"Start : {_startTeam}");
		CurTeam = _startTeam;
		_turnCountDictionary = new Dictionary<ETeamType, int>();
		foreach (ETeamType teamType in Enum.GetValues(typeof(ETeamType)))
		{
			_turnCountDictionary.Add(teamType, 0);
		}
		_turnCountDictionary[CurTeam]++;
	}

	public int GetCurTeamTurnCount() => _turnCountDictionary[CurTeam];

	public void NextTurn()
	{
		if (_turnCountDictionary[CurTeam] > 0)
		{
			Debug.Log("���� ���� ���� �Ҹ���� �ʾҽ��ϴ�.");
			return;
		}
		CurTeam = (ETeamType)(((int)CurTeam + 1) % 2);
		OnTurnEndEvent?.Invoke(CurTeam);
	}
	public void UseTurnCurTeam()
	{
		if(_turnCountDictionary[CurTeam] <= 0)
		{
			Debug.Log("���� ���� �Ҹ��ϼ̽��ϴ�.");
			return;
		}
		_turnCountDictionary[CurTeam]--;
	}
	public void AddTurnCurTeam(int turnCount = 1)
	{
		AddTurn(CurTeam, turnCount);
	}
	public void AddTurn(ETeamType teamType, int turnCount = 1)
	{
		_turnCountDictionary[teamType] += turnCount;
	}
}
