using UnityEngine;

public class Board : MonoBehaviour
{
    public Square[][] squares;

    public bool CanMove(Vector2Int position)
        => squares[position.y][position.x] != null;
}
