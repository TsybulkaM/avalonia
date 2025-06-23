using System;
using System.Threading.Tasks;

namespace Reversi.Models;

public class MiniMaxBot : IBot
{
    private const int MaxDepth = 3;

    public async Task MakeBestMove(GameBoard board)
    {
        await Task.Delay(500);

        (int row, int col) bestMove = (-1, -1);
        var bestScore = int.MinValue;

        for (var i = 0; i < GameBoard.BoardSize; i++)
        {
            for (var j = 0; j < GameBoard.BoardSize; j++)
            {
                if (board.GetCell(i, j) == CellState.Valid)
                {
                    var simulatedBoard = board.CloneBoard();
                    simulatedBoard.MakeMove(i, j);
                    var score = Minimax(simulatedBoard, MaxDepth, int.MinValue, int.MaxValue, false);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (i, j);
                    }
                }
            }
        }

        board.MakeMove(bestMove.row, bestMove.col);
    }

    private int Minimax(GameBoard board, int depth, int alpha, int beta, bool isMaximizing)
    {
        if (depth == 0 || board.GameOver)
        {
            return EvaluateBoard(board);
        }

        var bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (var i = 0; i < GameBoard.BoardSize; i++)
        {
            for (var j = 0; j < GameBoard.BoardSize; j++)
            {
                if (board.GetCell(i, j) == CellState.Valid)
                {
                    var simulatedBoard = board.CloneBoard();
                    simulatedBoard.MakeMove(i, j);

                    var score = Minimax(simulatedBoard, depth - 1, alpha, beta, !isMaximizing);

                    if (isMaximizing)
                    {
                        bestScore = Math.Max(bestScore, score);
                        alpha = Math.Max(alpha, bestScore);
                    }
                    else
                    {
                        bestScore = Math.Min(bestScore, score);
                        beta = Math.Min(beta, bestScore);
                    }

                    if (beta <= alpha)
                    {
                        return bestScore;
                    }
                }
            }
        }

        return bestScore;
    }

    private int EvaluateBoard(GameBoard board)
    {
        int score = 0;
        int mobility = board.GetValidMoves(board.CurrentPlayer).Count - board.GetValidMoves(board.ChangePlayer(board.CurrentPlayer)).Count;

        for (var i = 0; i < GameBoard.BoardSize; i++)
        {
            for (var j = 0; j < GameBoard.BoardSize; j++)
            {
                if (board.GetCell(i, j) == CellState.Black)
                {
                    score += IBot.GetEvaluationMatrixValue(i, j);
                }
                else if (board.GetCell(i, j) == CellState.White)
                {
                    score -= IBot.GetEvaluationMatrixValue(i, j);
                }
            }
        }

        return score + mobility * 10;
    }
}