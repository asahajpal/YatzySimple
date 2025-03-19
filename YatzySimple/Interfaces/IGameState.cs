using System;
using YatzySimple.Core;


namespace YatzySimple.Interfaces
{
    public delegate void NextTurnDelegate(GameContext context);
    public interface IGameState
    {
        NextTurnDelegate NextTurn { get; }
        bool IsGameOn { get; }
    }
}

