using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    public Player curPlayer;
    [field:SerializeField]public TeamType CurTeam { get; private set; }
    private Dictionary<TeamType, Player> Players {get; set;}
    public bool positionPeekComplete;
    private int _currentTurn = 0;
    public int CurrentTurn
    {
        get => _currentTurn;
        private set
        {
            _currentTurn = value;
            curPlayer = Players[(TeamType)_currentTurn];
        }
    }

    [SerializeField] private LayerMask _whatIsPiece;
    public List<Piece> whitePiece;
    public List<Piece> blackPiece;

    //Event
    public event Action<Vector2Int> OnClientPosPickEvent;

    private void Awake()
    {
        Players = new Dictionary<TeamType, Player>();
        
        Players.Add(TeamType.White, new White());
        Players.Add(TeamType.Black, new Black());

        Players[TeamType.White].Init(whitePiece);
        Players[TeamType.Black].Init(blackPiece);

        //현재 플레이어 초기화
        CurrentTurn = 0;
    }

    public int NextTurn()
    {
        ++CurrentTurn;
        CurTeam = (TeamType)(CurrentTurn % Players.Count);
        return CurrentTurn;
    }
      
    private Vector2Int GetIntMousePosition()
    {
        return Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int pos = GetIntMousePosition();
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2Int.zero, 1, _whatIsPiece);

			if (hit.transform != null)
            {
                curPlayer.CurrentSelectedPiece = hit.transform.GetComponent<Piece>();
            }
            else
            {
                OnClientPosPickEvent?.Invoke(pos);
			}
        }
    }

	private void OnDrawGizmos()
	{
        Vector3 targetPos = new Vector3(GetIntMousePosition().x, GetIntMousePosition().y, 0);

        Gizmos.DrawSphere(targetPos, 0.3f);
	}
}
