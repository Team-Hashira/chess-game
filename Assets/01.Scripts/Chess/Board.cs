using DG.Tweening;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct PieceOnBoard
{
    public Vector2Int position;
    public Piece piecePrefab;
}

public class PossibleMove
{
    public Piece piece;
    public Vector2Int from;
    public Vector2Int to;
}

public class Board : MonoBehaviour
{
    private Tilemap _tilemap;
    private Vector2Int _offset;
    private Square[,] _squares;
    [SerializeField]
    private PieceOnBoard[] _pieceOnBoard;
    private Dictionary<Piece, Square> _pieceDictionary;
    private List<Piece> _pieces;

    private void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        Vector2 offset = visualTrm.position = transform.position;
        _tilemap = visualTrm.Find("Tilemap").GetComponent<Tilemap>();
        _pieces = new List<Piece>();
        _pieceDictionary = new Dictionary<Piece, Square>();

        BoundsInt bounds = _tilemap.cellBounds;
        _offset.x = bounds.min.x;
        _offset.y = bounds.min.y;
        _squares = new Square[bounds.size.y, bounds.size.x];
        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int i = bounds.min.x; i < bounds.max.x; i++)
            {
                TileBase tile = _tilemap.GetTile(new Vector3Int(i, y));
                if (tile != null)
                {
                    Square square = new Square();
                    square.position = new Vector2(i + 0.5f, y + 0.5f) + offset;
                    _squares[y - _offset.y, i - _offset.x] = square;
                }
            }
        }

        for (int i = 0; i < _pieceOnBoard.Length; i++)
        {
            Vector2Int pos = _pieceOnBoard[i].position;
            Piece piece = Instantiate(_pieceOnBoard[i].piecePrefab, _squares[pos.y, pos.x].position, Quaternion.identity);
            piece.Initialize(this, pos);
            _pieceDictionary.Add(piece, _squares[pos.y, pos.x]);
            _pieces.Add(piece);
        }
    }

    public Square GetSquareByMousePosition(Vector3 mousePos)
    {
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2Int ceiled = ((Vector2)mousePos).ConvertToInt(ConvertType.Ceil);
        ceiled = WorldPointToBoardPoint(ceiled);
        return _squares[ceiled.y, ceiled.x];
    }

    public Vector2Int WorldPointToBoardPoint(Vector2Int point)
    {
        Vector2Int v = point -= _offset;
        v -= Vector2Int.one;
        return v;
    }

    public List<PossibleMove> GetAllEnemiesPossibleMoves()
    {
        List<PossibleMove> moves = new List<PossibleMove>();
        foreach (Piece piece in _pieces)
        {
            if (piece.TeamType == ETeamType.Enemy)
            {
                moves.AddRange(piece.GetPossibleMoves());
            }
        }
        return moves;
    }

    public List<PossibleMove> GetAllPlayerPossibleMoves()
    {
        List<PossibleMove> moves = new List<PossibleMove>();
        foreach (Piece piece in _pieces)
        {
            if (piece.TeamType == ETeamType.Player)
            {
                moves = piece.GetPossibleMoves();
                break;
            }
        }
        return moves;
    }

    public void MovePiece(Piece piece, Vector2Int from, Vector2Int to)
    {
        _squares[from.y, from.x].piece = null;
        _squares[to.y, to.x].piece = piece;
    }

    public void MakeMove(PossibleMove move)
    {
        MovePiece(move.piece, move.from, move.to);
    }

    public void UndoMove(PossibleMove move)
    {
        MovePiece(move.piece, move.to, move.from);
    }

    public Square GetSquare(Vector2Int pos)
        => _squares[pos.y, pos.x];
    public void SetTileColor(Vector2Int pos, Color color)
        => _tilemap.SetColor(new Vector3Int(pos.x + _offset.x, pos.y + _offset.y), color);
    public bool CanMove(Vector2Int position)
    {
        if (position.x < 0 || position.y < 0 || position.x >= _squares.GetLength(1) || position.y >= _squares.GetLength(0))
            return false;
        return _squares[position.y, position.x] != null;
    }
}
