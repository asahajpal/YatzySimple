using System;
using YatzySimple.Core;
using YatzySimple.Interfaces;

namespace YatzySimple.States
{
    public class GameOverState : IGameState
    {
       
        public void NextTurn(GameContext context)
        {
            // No action needed for game over
        }

        public bool IsGameOn => false;
    }
}
