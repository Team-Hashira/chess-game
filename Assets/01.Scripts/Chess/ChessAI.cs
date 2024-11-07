using System.Threading.Tasks;
using UnityEngine;
public struct MoveInfo
{
    public Piece piece;
    public int x;
    public int y;
    public int score;
}

public struct AIBoard
{

}

public struct AISquare
{

}

public struct AIPiece
{

}


public static class ChessAI
{
    public async static Task<MoveInfo> GetMoveInfo(Board board)
    {
        MoveInfo info = await CalculateMovement(board);
        return info;
    }

    public static async Task<MoveInfo> CalculateMovement(Board board)
    {
        //Tqlkfzz
        return new MoveInfo();
    }
}