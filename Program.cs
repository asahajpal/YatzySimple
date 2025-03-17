// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;

public delegate void NextTurnDelegate(GameContext context);

public interface IGameState
{
    NextTurnDelegate NextTurn { get; }
}

public class RollingDiceState : IGameState
{
    public NextTurnDelegate NextTurn => (context) =>
    {
        Console.WriteLine("Rolling dice...");
        context.RollDice();
        context.SetState(new ScoringState());
    };
}

public class ScoringState : IGameState
{
    public NextTurnDelegate NextTurn => (context) =>
    {
        Console.WriteLine("Scoring...");
        context.ScoreDice();
        if (context.AllCategoriesScored())
        {
            context.SetState(new GameOverState());
        }
        else
        {
            context.SetState(new RollingDiceState());
        }
    };
}

public class GameOverState : IGameState
{
    public NextTurnDelegate NextTurn => (context) =>
    {
        Console.WriteLine("Game over");
    };
}

public class GameContext
{
    private IGameState _state;
    private Random _random;
    private int[] _dice;
    private Dictionary<string, int> _scores;

    public GameContext()
    {
        _state = new RollingDiceState();
        _random = new Random();
        _dice = new int[5];
        _scores = new Dictionary<string, int>
        {
            { "Ones", 0 },
            { "Twos", 0 },
            { "Threes", 0 },
            { "Fours", 0 },
            { "Fives", 0 },
            { "Sixes", 0 }
        };
    }

    public void SetState(IGameState state)
    {
        _state = state;
    }

    public void PlayNextTurn()
    {
        _state.NextTurn(this);
    }

    public void RollDice()
    {
        for (int i = 0; i < _dice.Length; i++)
        {
            _dice[i] = _random.Next(1, 7);
        }
        Console.WriteLine("Dice rolled: " + string.Join(", ", _dice));
    }

    public void ScoreDice()
    {
        Console.WriteLine("Choose a category to score (Ones, Twos, Threes, Fours, Fives, Sixes):");
        string category = Console.ReadLine();
        int score = 0;

        switch (category)
        {
            case "Ones":
                score = CalculateScore(1);
                break;
            case "Twos":
                score = CalculateScore(2);
                break;
            case "Threes":
                score = CalculateScore(3);
                break;
            case "Fours":
                score = CalculateScore(4);
                break;
            case "Fives":
                score = CalculateScore(5);
                break;
            case "Sixes":
                score = CalculateScore(6);
                break;
            default:
                Console.WriteLine("Invalid category.");
                return;
        }

        _scores[category] = score;
        Console.WriteLine($"{category} scored: {score}");
    }

    private int CalculateScore(int number)
    {
        int score = 0;
        foreach (int die in _dice)
        {
            if (die == number)
            {
                score += number;
            }
        }
        return score;
    }

    public bool AllCategoriesScored()
    {
        foreach (var score in _scores.Values)
        {
            if (score == 0)
            {
                return false;
            }
        }
        return true;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        GameContext game = new GameContext();
        while (!(game is GameOverState))
        {
            game.PlayNextTurn();
        }
    }
}