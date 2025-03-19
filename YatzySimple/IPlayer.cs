public interface IPlayer
{
    int[] Dice { get; }
    void RollDice();
    void ScoreDice(GameContext context);
}
