using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DaveSquared.GardenRace.Gui
{
    /// <summary>
    /// Interaction logic for GardenRaceView.xaml
    /// </summary>
    public partial class GardenRaceView : Window, IGameView
    {
        public GardenRaceView()
        {
            InitializeComponent();
            FillSquares();
            MoveToStartingPositions();
            var gameModel = new Game(64, 2);
            new GamePresenter(this, gameModel, new DieRoller());
        }

        private void MoveToStartingPositions()
        {
            MovePlayerMarker(1, 0, 0);
            MovePlayerMarker(2, 0, 0);
        }

        private void FillSquares()
        {                        
            for (var squareNumber=1; squareNumber <= 64; squareNumber++)
            {
                var square = new StackPanel();
                var squareLabel = new Label();                
                squareLabel.Content = squareNumber;
                square.Children.Add(squareLabel);
                gameBoard.Children.Add(square);
            }
        }

        public event EventHandler RollClicked;

        private void OnRollClicked()
        {
            EventHandler rollClickedHandler = RollClicked;
            if (rollClickedHandler != null) rollClickedHandler(this, EventArgs.Empty);
        }

        public void ShowRollResult(int dieFace)
        {
            rollResult.Content = "You rolled a " + dieFace;
        }

        public void SetCurrentPlayer(int player)
        {
            currentPlayer.Content = "Player " + player + "'s turn.";
        }

        public void MovePlayerMarker(int player, int fromSquare, int toSquare)        
        {            
            var markerForPlayer = GetMarkerForPlayer(player);
            markerForPlayer.Visibility = Visibility.Visible;
            
            var containerForMarker = (Panel) markerForPlayer.Parent;
            containerForMarker.Children.Remove(markerForPlayer);

            if (toSquare >= gameBoard.Children.Count) toSquare = gameBoard.Children.Count-1;
            var newSquare = (StackPanel) gameBoard.Children[toSquare];
            newSquare.Children.Add(markerForPlayer);            
        }

        private Shape GetMarkerForPlayer(int player)
        {
            if (player == 1) {
                return player1Marker;
            }
            if (player == 2)
            {
                return player2Marker;
            }
            throw new ArgumentOutOfRangeException();
        }

        public void ShowWinner(int winningPlayer)
        {
            MessageBox.Show("Player " + winningPlayer + " wins! Nice work!");
        }

        public void DisableDieRolls()
        {
            rollDieButton.IsEnabled = false;
        }

        private void rollDieButton_Click(object sender, RoutedEventArgs e)
        {
            OnRollClicked();
        }
    }

    internal class DieRoller : IDieRoller
    {
        Random random = new Random();
        public int Roll()
        {
            return random.Next(1, 6);
        }
    }
}
