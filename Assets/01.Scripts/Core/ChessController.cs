using System;
using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChessController : MonoBehaviour
{
    public Tilemap curMap;
    
    public int[,] Map {get; private set;}
    
    //Controllers
    private PlayersController _playersController;
    
    private void Awake()
    {
        _playersController = GetComponent<PlayersController>();
        
        BoundsInt bounds = curMap.cellBounds;
        TileBase[] bases = curMap.GetTilesBlock(bounds);

        Map = new int[bounds.size.x, bounds.size.y];
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {
				Vector3Int cellPosition = new Vector3Int(i + bounds.xMin, j + bounds.yMin, 0);
				TileBase tile = curMap.GetTile(cellPosition);

				// 타일이 있으면 1, 없으면 0으로 저장 (또는 다른 방식으로 설정 가능)
				Map[i, j] = tile != null ? 1 : 0;
			}
        }
    }
    
    private void Start()
    {
        StartCoroutine(TurnProcess());
    }

	private void Update()
	{
	}

	private IEnumerator TurnProcess()
    {
        _playersController.OnClientPosPickEvent += FinalMoveOperation;
		yield return new WaitUntil(()=>_playersController.positionPeekComplete);
		_playersController.OnClientPosPickEvent -= FinalMoveOperation;
		_playersController.NextTurn();
        yield return StartCoroutine(TurnProcess());
    }

    private void FinalMoveOperation(Vector2Int mousePos)
    {
        Piece piece = _playersController.curPlayer.CurrentSelectedPiece;
		Vector2Int moveRange = new Vector2Int(Mathf.FloorToInt(piece.MovableTiles.GetLength(0) / 2f), Mathf.FloorToInt(piece.MovableTiles.GetLength(1) / 2f));

		Vector2Int piecePos = new Vector2Int(
            Mathf.RoundToInt(piece.transform.position.x), 
            Mathf.RoundToInt(piece.transform.position.y));

        Vector2Int finalMovePos = (mousePos - piecePos);

        finalMovePos.y *= -1;

        finalMovePos += moveRange;

		try
        {
            Debug.Log(finalMovePos);
			if (piece.MovableTiles[finalMovePos.x, finalMovePos.y] != 0)
			{
				Vector2Int localVec =
					new Vector2Int(Mathf.RoundToInt(Mathf.Clamp(curMap.transform.position.x, 0, Map.GetLength(0))),
					Mathf.RoundToInt(Mathf.Clamp(curMap.transform.position.y, 0, Map.GetLength(1))));

				piece.MoveTo(mousePos);
			}
		}
        catch (IndexOutOfRangeException)
        {
            Debug.Log("거긴 갈 수 없는 곳임");
        }
	}
}
