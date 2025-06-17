using System.Threading.Tasks;

namespace Reversi.Models;

public class EvaluationBot : IBot
{
    private static readonly int[,] EvaluationMatrix = new int[,]
    {
        { 120, -20,  20,   5,   5,  20, -20, 120 },
        { -20, -40,  -5,  -5,  -5,  -5, -40, -20 },
        {  20,  -5,  15,   3,   3,  15,  -5,  20 },
        {   5,  -5,   3,   3,   3,   3,  -5,   5 },
        {   5,  -5,   3,   3,   3,   3,  -5,   5 },
        {  20,  -5,  15,   3,   3,  15,  -5,  20 },
        { -20, -40,  -5,  -5,  -5,  -5, -40, -20 },
        { 120, -20,  20,   5,   5,  20, -20, 120 }
    };
    
    public async Task MakeBestMove(GameBoard board)
    {
        await Task.Delay(700);
        
        int bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);

        for (int i = 0; i < GameBoard.BoardSize; i++)
        {
            for (int j = 0; j < GameBoard.BoardSize; j++)
            {
                if (board.GetCell(i, j) == CellState.Valid)
                {
                    int score = EvaluateMove(board, i, j);
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
    
    private static int EvaluateMove(GameBoard board, int row, int col)
    {
        int score = EvaluationMatrix[row, col];
        
        // Add additional evaluation logic if needed
        // For example, consider the number of pieces flipped or potential future moves
        
        return score;
    }
}