using System;
using YatzySimple.Interfaces;

namespace YatzySimple.States
{

    public class ScoringState : IGameState
    {
        public NextTurnDelegate NextTurn => (context) =>
        {
            context.Player.ScoreDice(context);
            if (context.AllCategoriesScored())
            {
                context.SetState(new GameOverState());
            }
            else
            {
                context.SetState(new RollingDiceState());
            }
        };

        public bool IsGameOn => true;
    }
}
