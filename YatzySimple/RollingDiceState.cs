using System;
using YatzySimple.Interfaces;

namespace YatzySimple.States
{
    public class RollingDiceState : IGameState
    {
        public NextTurnDelegate NextTurn => (context) =>
        {
            context.RollDice();
            context.SetState(new ScoringState());
        };

        public bool IsGameOn => true;
    }
}
