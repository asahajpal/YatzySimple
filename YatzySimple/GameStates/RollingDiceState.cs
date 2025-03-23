using System;
using YatzySimple.Core;
using YatzySimple.Interfaces;

namespace YatzySimple.States
{
    public class RollingDiceState : IGameState
    {
        public void NextTurn(GameContext context)
        {
            // Implement the logic for the next turn in the rolling dice state
            context.RollDice();
            // Transition to the next state if needed
            context.TransitionToScoringState();
        }
        public bool IsGameOn => true;
    }
}
