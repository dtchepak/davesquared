namespace DaveSquared.GardenRace {
    public class Game {        
        public int CurrentSquare;
        private readonly int boardSize;

        public Game(int boardSize) {
            this.boardSize = boardSize;
        }        
        public bool IsFinished {
            get { return CurrentSquare >= boardSize; }
        }
        public void Roll(int dieValue) {
            CurrentSquare += dieValue;
        }
    }
}