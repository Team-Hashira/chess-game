using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EPieceType
{
    Player,
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}


public abstract class Piece : MonoBehaviour
{
    [SerializeField]
    protected TextAsset _pieceMovementText;
    [SerializeField]
    protected EPieceType _pieceType;
    [SerializeField]
    protected int _pieceValue;
    [SerializeField]
    protected bool _canMove = true;

    protected Vector2Int _position;
    protected List<Vector2Int> _pieceMovementList;

    protected Board _board;

    public void Initialize(Board board)
    {
        _board = board;
    }

    public void ReadPieceMovement()
    {
        //어짜피 행마법 텍스트의 행열 길이는 같고 홀수
        string[] movementText = _pieceMovementText.text.Split('\n');
        int offset = movementText.Length / 2;
        for(int i = 0; i < movementText.Length; i++)
        {
            for(int j = 0; j < movementText[i].Length; j++)
            {
                if(movementText[i][j] == '1')
                {
                    _pieceMovementList.Add(new Vector2Int(j - offset, i - offset));
                }
            }
        }
    }

    public List<Vector2Int> GetMovementList()
    {
        return _pieceMovementList.Select(move => move += _position).Where(move => move.x >= 0 && move.y >= 0 && _board.CanMove(move)).ToList();
    }
}
