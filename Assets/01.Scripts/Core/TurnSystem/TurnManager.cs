using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : MonoSingleton<TurnManager>
{
	[SerializeField] private ETeamType _startTeam = ETeamType.Player;
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

    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
		{
			UseTurnCurTeam();
			NextTurn();
		}
    }

    public int GetCurTeamTurnCount() => _turnCountDictionary[CurTeam];

	public void NextTurn()
	{
		if (_turnCountDictionary[CurTeam] > 0)
		{
			Debug.Log("아직 턴이 전부 소모되지 않았습니다.");
			return;
		}
		CurTeam = (ETeamType)(((int)CurTeam + 1) % 2);
		OnTurnEndEvent?.Invoke(CurTeam);
	}
	public void UseTurnCurTeam()
	{
		if(_turnCountDictionary[CurTeam] <= 0)
		{
			Debug.Log("턴을 전부 소모하셨습니다.");
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
