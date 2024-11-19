using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public static class ChessAI
{
    private static int _weightThreshold = 100;
    private static System.Random _random = new System.Random();
    private static Dictionary<EPieceType, int> _pieceValue = new Dictionary<EPieceType, int>()
    {
        {EPieceType.Player, 10000},
        {EPieceType.Pawn, 100},
        {EPieceType.Bishop, 300},
        {EPieceType.Knight, 300},
        {EPieceType.Rook, 500},
        {EPieceType.Queen, 1000},
        {EPieceType.King, 10000},
    };

    public static void SetWeightThreshold(int weightThreshold) => _weightThreshold = weightThreshold;

    public class MinimaxResult
    {
        public PossibleMove possibleMove;
        public int score;

        public MinimaxResult(PossibleMove possibleMove = null, int score = 0)
        {
            this.possibleMove = possibleMove;
            this.score = score;
        }
    }

    public async static Task<PossibleMove> GetBestMove(Board board, int depth)
    {
        MinimaxResult result = await CalculateMovement(board, depth, true);
        return result.possibleMove;
    }

    public static async Task<MinimaxResult> CalculateMovement(Board board, int depth, bool playerTurn, int alpha = int.MinValue, int beta = int.MaxValue)
    {
        if (depth == 0)
        {
            return new MinimaxResult(null, EvaluateBoard(board));
        }

        List<PossibleMove> moveList = playerTurn ?
            board.GetAllPlayerPossibleMoves() :
            board.GetAllEnemiesPossibleMoves();

        PossibleMove bestMove = null;
        int bestScore = playerTurn ? int.MinValue : int.MaxValue;

        foreach (PossibleMove move in moveList)
        {
            int weight = CalculateWeight(board, move, playerTurn);
            if (weight < _weightThreshold)
            {
                continue;
            }

            board.MakeMove(move);
            MinimaxResult result = await CalculateMovement(board, depth - 1, !playerTurn, alpha, beta);
            board.UndoMove(move);

            if (playerTurn)
            {
                if (result.score > bestScore)
                {
                    bestScore = result.score;
                    bestMove = move;
                }
                alpha = Mathf.Max(alpha, bestScore);
            }
            else
            {
                if (result.score < bestScore)
                {
                    bestScore = result.score;
                    bestMove = move;
                }
                beta = Mathf.Min(beta, bestScore);
            }

            if (beta <= alpha)
            {
                break;
            }
        }
        return new MinimaxResult(bestMove, bestScore);
    }

    private static int CalculateWeight(Board board, PossibleMove move, bool playerTurn)
    {
        return _random.Next(0, 1000);
    }

    private static int EvaluateBoard(Board board)
    {
        return _random.Next(0, 1000);
    }
}