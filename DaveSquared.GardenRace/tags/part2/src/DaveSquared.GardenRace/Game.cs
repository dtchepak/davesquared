using System.Linq;

namespace DaveSquared.GardenRace {
    public class Game {
        private const int defaultNumberOfPlayers = 1;
        private readonly int boardSize;
        private readonly int numberOfPlayers;
        private readonly int[] playerPositions;
        
        public int CurrentPlayer { get; private set; }
        public int CurrentSquare { get { return GetSquareFor(CurrentPlayer); } }
        public int NumberOfPlayers { get { return numberOfPlayers; } }
        
        public Game(int boardSize) : this(boardSize, defaultNumberOfPlayers) {}

        public Game(int boardSize, int numberOfPlayers) {
            this.boardSize = boardSize;
            this.numberOfPlayers = numberOfPlayers;
            this.playerPositions = new int[numberOfPlayers];
            this.CurrentPlayer = 1;
        }

        public bool IsFinished {
            get {
                 return playerPositions.Any(square => square >= boardSize);
            }
        }

        public void Roll(int dieValue) {
            playerPositions[CurrentPlayer - 1] += dieValue;
            nextPlayer();
        }

        private void nextPlayer() {
            CurrentPlayer++;
            if (CurrentPlayer > NumberOfPlayers) {
                CurrentPlayer = 1;
            }
        }

        public int GetSquareFor(int player) {
            return playerPositions[player - 1];
        }
    }
}