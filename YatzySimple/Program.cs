using System;
using System.Collections.Generic;

public delegate void NextTurnDelegate(GameContext context);

public interface IGameState
{
    NextTurnDelegate NextTurn { get; }
    bool IsGameOn { get; }
}

public class RollingDiceState : IGameState
{
    public NextTurnDelegate NextTurn => (context) =>
    {
        Console.WriteLine("Rolling dice...");
        context.Player.RollDice();
        context.SetState(new ScoringState());
    };

    public bool IsGameOn => true;
}

public class ScoringState : IGameState
{
    public NextTurnDelegate NextTurn => (context) =>
    {
        Console.WriteLine("Scoring...");
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

public class GameOverState : IGameState
{
    public NextTurnDelegate NextTurn => (context) =>
    {
        Console.WriteLine("Game over");
    };

    public bool IsGameOn => false;
}

public class GameContext
{
    private IGameState _state;
    public SimulatedPlayer Player { get; private set; }
    private Dictionary<string, int> _scores;

    public GameContext()
    {
        _state = new RollingDiceState();
        Player = new SimulatedPlayer();
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

    public IGameState CurrentState => _state;

    public void SetState(IGameState state)
    {
        _state = state;
    }

    public void PlayNextTurn()
    {
        _state.NextTurn(this);
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

    public void UpdateScore(string category, int score)
    {
        _scores[category] = score;
        Console.WriteLine($"{category} scored: {score}");
    }

    public Dictionary<string, int> GetScores()
    {
        return _scores;
    }
}

public class SimulatedPlayer
{
    private Random _random;
    private int[] _dice;
    private Strategy _strategy;

    public SimulatedPlayer()
    {
        _random = new Random();
        _dice = new int[5];
        _strategy = new Strategy();
    }

    public void RollDice()
    {
        for (int i = 0; i < 3; i++) // Roll dice up to 3 times
        {
            for (int j = 0; j < _dice.Length; j++)
            {
                _dice[j] = _random.Next(1, 7);
            }
            Console.WriteLine($"Roll {i + 1}: " + string.Join(", ", _dice));
        }
    }

    public void ScoreDice(GameContext context)
    {
         Console.WriteLine("Current Scores:");
        foreach (var score in context.GetScores())
        {
            Console.WriteLine($"{score.Key}: {score.Value}");
        }
        string category = _strategy.ChooseCategory(context.GetScores(), _dice);
        int newScore = CalculateScore(category);
        context.UpdateScore(category, newScore);
    }

    private int CalculateScore(string category)
    {
        int number = category switch
        {
            "Ones" => 1,
            "Twos" => 2,
            "Threes" => 3,
            "Fours" => 4,
            "Fives" => 5,
            "Sixes" => 6,
            _ => 0
        };

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
}

public class Strategy
{
    public string ChooseCategory(Dictionary<string, int> scores, int[] dice)
    {
        string bestCategory = null;
        int maxScore = 0;

        foreach (var category in scores.Keys)
        {
            if (scores[category] == 0) // Choose only unscored categories
            {
                int score = CalculatePotentialScore(category, dice);
                if (score > maxScore)
                {
                    maxScore = score;
                    bestCategory = category;
                }
            }
        }

        return bestCategory;
    }

    private int CalculatePotentialScore(string category, int[] dice)
    {
        int number = category switch
        {
            "Ones" => 1,
            "Twos" => 2,
            "Threes" => 3,
            "Fours" => 4,
            "Fives" => 5,
            "Sixes" => 6,
            _ => 0
        };

        int score = 0;
        foreach (int die in dice)
        {
            if (die == number)
            {
                score += number;
            }
        }
        return score;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        GameContext game = new GameContext();

        while (game.CurrentState.IsGameOn)
        {
            game.PlayNextTurn();
        }

        // play the last turn and conclude the game
        game.PlayNextTurn();
    }
}