using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DaveSquared.GardenRace.Tests {


    public class GameSpec {
        private readonly Game newTenSquareGame = new Game(10);

        [Fact]
        public void Player_should_be_on_fourth_square_after_rolling_a_four() {            
            newTenSquareGame.Roll(4);
            Assert.Equal(4, newTenSquareGame.CurrentSquare);
        }

        [Fact]
        public void Player_should_be_on_eight_square_after_rolling_a_two_then_a_six() {           
            newTenSquareGame.Roll(2);
            newTenSquareGame.Roll(6);
            Assert.Equal(8, newTenSquareGame.CurrentSquare);
        }

        [Fact]
        public void Player_should_start_the_game_off_the_board() {
            Assert.Equal(0, newTenSquareGame.CurrentSquare);
        }

        [Fact]
        public void Game_should_finish_when_player_reaches_end_of_board() {
            newTenSquareGame.Roll(10);
            Assert.Equal(10, newTenSquareGame.CurrentSquare);
            Assert.True(newTenSquareGame.IsFinished);
        }

        [Fact]
        public void New_game_should_be_unfinished() {            
            Assert.False(newTenSquareGame.IsFinished);
        }

        [Fact]
        public void In_progress_game_should_be_unfinished() {
            newTenSquareGame.Roll(5);
            Assert.False(newTenSquareGame.IsFinished);
        }

        [Fact]
        public void Game_should_still_finish_when_player_overruns_last_square() {
            newTenSquareGame.Roll(12);
            Assert.True(newTenSquareGame.IsFinished);
        }

        [Fact]
        public void Should_be_able_to_create_2_player_game() {
            var twoPlayerGame = new Game(10, 2);
            Assert.Equal(2, twoPlayerGame.NumberOfPlayers);
        }
        
        [Fact]
        public void New_game_should_have_1_player_by_default() {
            var onePlayerGame = new Game(10);
            Assert.Equal(1, onePlayerGame.NumberOfPlayers);
        }

        [Fact]
        public void New_game_should_start_all_players_off_the_board() {
            var newThreePlayerGame = new Game(10, 3);
            var players = new[] {1, 2, 3};
            foreach (var player in players) {
                Assert.Equal(0, newThreePlayerGame.GetSquareFor(player));   
            }            
        }

        [Fact]
        public void Positions_should_be_correct_after_first_two_players_roll() {
            var threePlayerGame = new Game(10, 3);
            const int firstRoll = 3;
            const int secondRoll = 5;
            
            threePlayerGame.Roll(firstRoll);
            threePlayerGame.Roll(secondRoll);
            
            Assert.Equal(firstRoll, threePlayerGame.GetSquareFor(1));
            Assert.Equal(secondRoll, threePlayerGame.GetSquareFor(2));
            Assert.Equal(0, threePlayerGame.GetSquareFor(3));
        }
        
        [Fact]
        public void Current_player_for_new_game_should_be_player_1() {
            var newTwoPlayerGame = new Game(10, 2);
            Assert.Equal(1, newTwoPlayerGame.CurrentPlayer);
        }

        [Fact]
        public void After_first_players_roll_it_should_be_the_second_players_turn() {
            var newTwoPlayerGame = new Game(10, 2);
            newTwoPlayerGame.Roll(2);
            Assert.Equal(2, newTwoPlayerGame.CurrentPlayer);
        }

        [Fact]
        public void After_all_players_have_had_a_turn_it_should_be_first_players_turn_again() {
            var newThreePlayerGame = new Game(10, 3);
            newThreePlayerGame.Roll(1);
            newThreePlayerGame.Roll(1);
            newThreePlayerGame.Roll(1);
            Assert.Equal(1, newThreePlayerGame.CurrentPlayer);
        }

        [Fact]
        public void Check_results_after_two_rounds() {
            var threePlayerLargeGame = new Game(100, 3);            
            const int rounds = 2;
            const int player1AlwaysRolls = 1;
            const int player2AlwaysRolls = 2;
            const int player3AlwaysRolls = 3;

            for (var round=0; round<rounds; round++) {
                threePlayerLargeGame.Roll(player1AlwaysRolls);
                threePlayerLargeGame.Roll(player2AlwaysRolls);
                threePlayerLargeGame.Roll(player3AlwaysRolls);
            }

            Assert.Equal(1, threePlayerLargeGame.CurrentPlayer);
            Assert.Equal(player1AlwaysRolls * rounds, threePlayerLargeGame.GetSquareFor(1));
            Assert.Equal(player2AlwaysRolls * rounds, threePlayerLargeGame.GetSquareFor(2));
            Assert.Equal(player3AlwaysRolls * rounds, threePlayerLargeGame.GetSquareFor(3));
        }

        [Fact]
        public void Game_should_finish_as_soon_as_any_player_reaches_the_end() {
            var threePlayerGame = new Game(5, 3);
            threePlayerGame.Roll(1);
            threePlayerGame.Roll(6);
            Assert.True(threePlayerGame.IsFinished);
        }
    }
}
