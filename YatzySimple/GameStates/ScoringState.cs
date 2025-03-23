using System;
using YatzySimple.Core;
using YatzySimple.Interfaces;

namespace YatzySimple.States
{

    public class ScoringState : IGameState
    {
        public bool IsGameOn { get; private set; } = true;

        public void NextTurn(GameContext context)
        {
            context.Player.ScoreDice(context);
            if (context.AllCategoriesScored())
            {
                IsGameOn = false;
            }
            else
            {
                context.TransitionToRollingDiceState();
            }
        }
    }
}
