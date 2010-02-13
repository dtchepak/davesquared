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
        
    }
}
