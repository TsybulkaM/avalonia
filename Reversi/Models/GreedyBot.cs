using System;
using System.Threading.Tasks;

namespace Reversi.Models;

public class GreedyBot : IBot
{
    private bool IsWithinBounds(int row, int col)
    {
        return row is >= 0 and < GameBoard.BoardSize && col is >= 0 and < GameBoard.BoardSize;
    }
    
    private int CalculateFlippedPieces(GameBoard board, int row, int col)
    {
        int[] directions = [-1, 0, 1];
        var opponent = board.ChangePlayer(board.CurrentPlayer);
        var totalFlipped = 0;

        foreach (var drow in directions)
        {
            foreach (var dcol in directions)
            {
                // Skip the (0,0) direction
                if (drow == 0 && dcol == 0)
                    continue;

                var pieces = 0;
                var r = row + drow;
                var c = col + dcol;

                while (IsWithinBounds(r, c) && board.GetCell(r, c) == opponent)
                {
                    pieces++;
                    r += drow;
                    c += dcol;
                }

                if (pieces > 0 && IsWithinBounds(r, c) && board.GetCell(r, c) == board.CurrentPlayer)
                {
                    totalFlipped += pieces;
                }
            }
        }

        return totalFlipped;
    }
    
    public async Task MakeBestMove(GameBoard board)
    {
        await Task.Delay(1200);
        
        var bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);

        for (var i = 0; i < GameBoard.BoardSize; i++)
        {
            for (var j = 0; j < GameBoard.BoardSize; j++)
            {
                if (board.GetCell(i, j) == CellState.Valid)
                {
                    var score = CalculateFlippedPieces(board, i, j);
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
}