using UnityEngine;

namespace Crogen.AgentFSM.Movement
{
    public interface IChessMovement
    {
        public Agent AgentBase { get; set; }
        public void Initialize(Agent agent);
        public void MoveTo(Vector2Int dir);
    }
}