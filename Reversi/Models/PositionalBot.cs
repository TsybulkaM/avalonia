using System.Threading.Tasks;

namespace Reversi.Models;

public class PositionalBot : IBot
{
    
    public async Task MakeBestMove(GameBoard board)
    {
        await Task.Delay(700);
        
        var bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);

        for (var i = 0; i < GameBoard.BoardSize; i++)
        {
            for (var j = 0; j < GameBoard.BoardSize; j++)
            {
                if (board.GetCell(i, j) == CellState.Valid)
                {
                    var score = IBot.GetEvaluationMatrixValue(i, j);
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