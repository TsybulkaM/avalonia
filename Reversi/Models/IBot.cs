using System.Threading.Tasks;

namespace Reversi.Models;

public interface IBot
{
    public Task MakeBestMove(GameBoard board);
}