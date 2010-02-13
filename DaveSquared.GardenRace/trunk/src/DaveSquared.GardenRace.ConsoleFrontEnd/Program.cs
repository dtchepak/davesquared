using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaveSquared.GardenRace.ConsoleFrontEnd {
    class Program {
        private static readonly Random random = new Random();
                
        static void Main(string[] args) {
            Console.WriteLine("Welcome to Garden Race! It's like Snakes and Ladders, only without the copyright violation!");
            Console.WriteLine();
            
            const int numberOfSquares = 20;
            const int numberOfPlayers = 2;
            const bool interceptKey = true;
            
            Console.WriteLine("Creating a new game with " + numberOfSquares + " squares and " + numberOfPlayers + " players.");
            var game = new Game(numberOfSquares, numberOfPlayers);
            Console.WriteLine("Press any key to roll the die.");
            while (!game.IsFinished) {
                var currentPlayer = game.CurrentPlayer;
                Console.ReadKey(interceptKey);
                var dieRoll = GetDieValue();
                game.Roll(dieRoll);
                var currentState = (game.IsFinished) ? "You won the game!" : "Now on square " + game.GetSquareFor(currentPlayer);
                Console.WriteLine("Player " + currentPlayer + " rolled a " + dieRoll + ". " + currentState);
            }
            Console.WriteLine();
            Console.WriteLine("Press a key to exit.");
            Console.ReadKey(interceptKey);
        }

        static int GetDieValue() {
            return random.Next(1, 6);
        }
    }
}
