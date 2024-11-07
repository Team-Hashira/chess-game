using DG.Tweening;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    private Tilemap _tilemap;
    private Vector2Int _offset;
    private Square[,] _squares;

    private void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        _tilemap = visualTrm.Find("Tilemap").GetComponent<Tilemap>();

        BoundsInt bounds = _tilemap.cellBounds;
        _offset.x = bounds.min.x;
        _offset.y = bounds.min.y;
        _squares = new Square[bounds.size.y, bounds.size.x];
        TileBase[] tiles = _tilemap.GetTilesBlock(bounds);
        for (int i = bounds.min.y; i < bounds.max.y; i++)
        {
            for (int j = bounds.min.x; j < bounds.max.x; j++)
            {
                TileBase tile = _tilemap.GetTile(new Vector3Int(j, i));
                if (tile != null)
                {
                    Square square = new Square();
                    square.position = new Vector2(j + 0.5f, i + 0.5f);
                    _squares[i - _offset.y, j - _offset.x] = square;
                    Debug.DrawLine(square.position - Vector3.one * 0.5f, square.position + Vector3.one * 0.5f, Color.red, 100);
                }
            }
        }
    }

    public bool CanMove(Vector2Int position)
        => _squares[position.y, position.x] != null;
}
