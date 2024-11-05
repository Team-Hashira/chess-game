using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player
{
    public TeamType team;
    public Piece CurrentSelectedPiece { get; set; }
    [SerializeField] private List<Piece> _pieces;

    public void Init(List<Piece> pieceList)
    {
        _pieces = pieceList;
        foreach (var piece in _pieces)
        {
            piece.Init();
        }
    }

    public void ShowMoveRange(int[,] map, Tilemap signMap, TileBase tile)
    {
        signMap.ClearAllTiles();

        int[,] grid = CurrentSelectedPiece.MovableTiles;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[j, i] == 1)
                {
                    signMap.SetTile(new Vector3Int(j, i), tile);
                }
            }   
        }
    }
}
