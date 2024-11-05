using Crogen.AgentFSM.Movement;
using UnityEngine;

namespace _01.Scripts.Agent
{
    public class ChessMovement : MonoBehaviour, IChessMovement
    {
        public Crogen.AgentFSM.Agent AgentBase { get; set; }

        public void Initialize(Crogen.AgentFSM.Agent agent)
        {
            AgentBase = agent;
        }

        public void MoveTo(Vector2Int dir)
        {
            if (AgentBase.isMyTurn == true)
            {
            }
        }
    }
}