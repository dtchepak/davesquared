using System;
using Rhino.Mocks;
using Xunit;

namespace DaveSquared.GardenRace.Tests
{
    public class GamePresenterSpec
    {
        private IGameView fakeGameView;
        private IGame fakeGame;
        private IDieRoller fakeDieRoller;

        private void CreateGamePresenterAndDependencies()
        {
            CreateGameDependencies();
            new GamePresenter(fakeGameView, fakeGame, fakeDieRoller);
        }

        private void CreateGameDependencies()
        {
            fakeGameView = MockRepository.GenerateStub<IGameView>();
            fakeGame = MockRepository.GenerateMock<IGame>();
            fakeDieRoller = MockRepository.GenerateStub<IDieRoller>();
        }

        [Fact]
        public void Game_should_update_when_roll_die_is_clicked()
        {
            CreateGamePresenterAndDependencies();

            RaiseRollClickedEventOnView();
            fakeGame.AssertWasCalled(game => game.Roll(Arg<int>.Is.Anything));
        }

        [Fact]
        public void Game_should_roll_value_from_die_when_roll_die_is_clicked()
        {
            CreateGamePresenterAndDependencies();
            int dieFace = 3;
            fakeDieRoller.Stub(die => die.Roll()).Return(dieFace);

            RaiseRollClickedEventOnView();
            fakeGame.AssertWasCalled(game => game.Roll(dieFace));
        }

        [Fact]
        public void View_should_show_result_of_roll()
        {
            CreateGamePresenterAndDependencies();
            int dieFace = 2;
            fakeDieRoller.Stub(die => die.Roll()).Return(dieFace);
            RaiseRollClickedEventOnView();
            fakeGameView.AssertWasCalled(view => view.ShowRollResult(dieFace));
        }

        [Fact]
        public void View_should_show_current_player_when_game_is_created()
        {
            CreateGameDependencies();
            var currentPlayer = 1;
            fakeGame.Stub(game => game.CurrentPlayer).Return(currentPlayer);

            new GamePresenter(fakeGameView, fakeGame, fakeDieRoller);

            fakeGameView.AssertWasCalled(view => view.SetCurrentPlayer(currentPlayer));
        }

        [Fact]
        public void View_should_show_current_player_after_a_roll()
        {
            CreateGamePresenterAndDependencies();
            var player = 2;
            fakeGame.Stub(game => game.CurrentPlayer).Return(player);
            RaiseRollClickedEventOnView();
            fakeGameView.AssertWasCalled(view => view.SetCurrentPlayer(player));
        }

        [Fact]
        public void Should_update_players_position_after_roll()
        {
            CreateGamePresenterAndDependencies();
            var player = 1;
            var newSquare = 10;
            var oldSquare = 5;
            fakeGame.Stub(game => game.CurrentPlayer).Return(player);
            fakeGame.Stub(game => game.GetSquareFor(player)).Return(oldSquare);
            fakeGame.Stub(game => game.GetSquareFor(player)).Return(newSquare);
            RaiseRollClickedEventOnView();

            fakeGameView.AssertWasCalled(view => view.MovePlayerMarker(player, oldSquare, newSquare));
        }

        [Fact]
        public void Should_show_winner_when_game_is_finished()
        {
            CreateGamePresenterAndDependencies();
            int player = 3;
            fakeGame.Stub(game => game.CurrentPlayer).Return(player);
            fakeGame.Stub(game => game.IsFinished).Return(true);
            RaiseRollClickedEventOnView();
            fakeGameView.AssertWasCalled(view => view.ShowWinner(player));
        }

        [Fact]
        public void Should_disable_die_roll_when_game_is_finished()
        {
            CreateGamePresenterAndDependencies();
            fakeGame.Stub(game => game.IsFinished).Return(true);
            RaiseRollClickedEventOnView();
            fakeGameView.AssertWasCalled(view => view.DisableDieRolls());
        }

        private void RaiseRollClickedEventOnView()
        {
            fakeGameView.Raise(view => view.RollClicked += null, this, EventArgs.Empty);
        }
    }
}