using DG.Tweening;
using System.Data.SqlTypes;
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

public class Board : MonoBehaviour
{
    private Tilemap _tilemap;
    private Vector2Int _offset;
    private Square[,] _squares;
    [SerializeField]
    private PieceOnBoard[] _pieceOnBoard;

    private void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        Vector2 offset = visualTrm.position = transform.position;
        _tilemap = visualTrm.Find("Tilemap").GetComponent<Tilemap>();

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
                    Debug.DrawLine(square.position - Vector2.one * 0.5f, square.position + Vector2.one * 0.5f, Color.red, 100);
                }
            }
        }

        for(int i = 0; i < _pieceOnBoard.Length; i++)
        {
            Vector2Int pos = _pieceOnBoard[i].position;
            Piece piece = Instantiate(_pieceOnBoard[i].piecePrefab, _squares[pos.y, pos.x].position, Quaternion.identity);
            piece.Initialize(this, pos);
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
