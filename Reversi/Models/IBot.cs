using System.Threading.Tasks;

namespace Reversi.Models;

public enum BotType
{
    Greedy,
    Evaluation,
}

public interface IBot
{
    public Task MakeBestMove(GameBoard board);
}