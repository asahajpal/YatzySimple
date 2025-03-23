using System;
using YatzySimple.Core;


namespace YatzySimple.Interfaces
{
//public delegate void NextTurnDelegate(GameContext context);

    public interface IGameState
    {
        void NextTurn(GameContext context);
        bool IsGameOn { get; }
    }
}

