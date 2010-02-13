namespace DaveSquared.GardenRace
{
    public interface IGame
    {
        void Roll(int dieValue);
        int CurrentPlayer { get; }
        bool IsFinished { get; }
        int GetSquareFor(int player);
    }
}