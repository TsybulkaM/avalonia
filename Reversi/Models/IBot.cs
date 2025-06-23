using System.Threading.Tasks;

namespace Reversi.Models;

public enum BotType
{
    Greedy,
    MiniMax,
    Positional,
}

public interface IBot
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
    
    public static int GetEvaluationMatrixValue(int row, int col)
    {
        return EvaluationMatrix[row, col];
    }
    
    public Task MakeBestMove(GameBoard board);
}