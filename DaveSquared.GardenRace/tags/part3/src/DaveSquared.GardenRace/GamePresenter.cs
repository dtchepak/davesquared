using System;

namespace DaveSquared.GardenRace
{
    public class GamePresenter
    {
        private readonly IGameView view;
        private readonly IGame game;
        private readonly IDieRoller roller;

        public GamePresenter(IGameView view, IGame game, IDieRoller roller)
        {
            this.view = view;
            this.game = game;
            this.roller = roller;
            view.RollClicked += view_RollClicked;
            view.SetCurrentPlayer(game.CurrentPlayer);
        }

        void view_RollClicked(object sender, EventArgs e)
        {
            var dieValue = roller.Roll();
            var player = game.CurrentPlayer;
            var startingSquare = game.GetSquareFor(player);
            game.Roll(dieValue);
            view.ShowRollResult(dieValue);
            view.SetCurrentPlayer(player);
            view.MovePlayerMarker(player, startingSquare, game.GetSquareFor(player));
            if (game.IsFinished)
            {
                view.DisableDieRolls();
                view.ShowWinner(player);
            }
        }
    }
}