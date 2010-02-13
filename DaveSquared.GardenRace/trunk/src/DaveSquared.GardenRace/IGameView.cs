using System;

namespace DaveSquared.GardenRace
{
    public interface IGameView
    {
        event EventHandler RollClicked;
        void ShowRollResult(int dieFace);
        void SetCurrentPlayer(int player);
        void MovePlayerMarker(int player, int fromSquare, int toSquare);
        void ShowWinner(int winningPlayer);
        void DisableDieRolls();
    }
}