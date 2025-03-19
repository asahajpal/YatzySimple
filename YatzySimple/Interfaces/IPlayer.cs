using YatzySimple.Core;

namespace YatzySimple.Interfaces
{
    public interface IPlayer
    {
        void RollDice(GameContext context);
        void ScoreDice(GameContext context);
    }
}
