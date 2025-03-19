using System;
using YatzySimple.Interfaces;

namespace YatzySimple.States
{
    public class GameOverState : IGameState
    {
        public NextTurnDelegate NextTurn => (context) =>
        {
            // No action needed for game over
        };

        public bool IsGameOn => false;
    }
}
