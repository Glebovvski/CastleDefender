using Grid;
using Zenject;

namespace Models
{
    public class LoseModel
    {
        private GameGrid Grid { get; set; }

        [Inject]
        private void Construct(GameGrid grid)
        {
            Grid = grid;
        }


    }
}