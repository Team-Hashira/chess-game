using DG.Tweening;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Board _currentBoard;
    private ChessAI.MinimaxResult _result;
    private bool _isCompleted = false;
    private void Awake()
    {
        TurnManager.Instance.OnTurnEndEvent += HandleOnTurnEndEvent;
    }

    private void Update()
    {
        if(_isCompleted)
        {
            ApplyMove(_result);
            _isCompleted = false;
        }
    }

    private void SetCurrentBoard(Board board)
    {
        _currentBoard = board;
    }

    private void HandleOnTurnEndEvent(ETeamType team)
    {
        if (team == ETeamType.Enemy)
        {
            Task.Run(() => CalculateMovement());
        }
    }

    private async Task CalculateMovement()
    {
        _result = await ChessAI.CalculateMovement(_currentBoard, 5, false);
        _isCompleted = true;
    }

    private void ApplyMove(ChessAI.MinimaxResult result)
    {
        Piece piece = result.possibleMove.piece;
        piece.transform.DOJump(_currentBoard.GetSquare(result.possibleMove.to).position, 2f, 1, 0.5f);
        _currentBoard.MakeMove(result.possibleMove);
        TurnManager.Instance.UseTurnCurTeam();
        TurnManager.Instance.NextTurn();
    }
}
