using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

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
        context.Player.RollDice();
        context.SetState(new ScoringState());
    };

    public bool IsGameOn => true;
}

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

public class GameOverState : IGameState
{
    public NextTurnDelegate NextTurn => (context) =>
    {
        // No action needed for game over
    };

    public bool IsGameOn => false;
}

public class Strategy
{
    public string ChooseCategory(Dictionary<string, int> scores, int[] dice)
    {
        string bestCategory = string.Empty;
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
            Console.WriteLine("Press any key to play the next turn, or type 's' to stop the game.");
            var input = Console.ReadKey(intercept: true).KeyChar;
            if (input == 's')
            {
                Console.WriteLine("Game stopped by user.");
                break;
            }

            game.PlayNextTurn();

            if (game.CurrentState is RollingDiceState)
            {
                Console.WriteLine("Rolling dice...");
                Console.WriteLine("Dice values: " + string.Join(", ", game.DiceValues));
            }
            else if (game.CurrentState is ScoringState)
            {
                Console.WriteLine("Scoring...");
                Console.WriteLine("Current Scores:");
                foreach (var score in game.Scores)
                {
                    Console.WriteLine($"{score.Key}: {score.Value}");
                }
                Console.WriteLine($"Total Score: {game.TotalScore}");
            }

            if (!game.CurrentState.IsGameOn)
            {
                Console.WriteLine("Game over");
                break;
            }
        }
    }
}
 